using SmartBooks.ApplicationCore.DomainServices;
using SmartBooks.ApplicationCore.Models;
using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Services;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.Enums;
using SmartBooks.Domains.SaccoEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.SaccoServices
{
    public class MemberService
    {
        private readonly Logger logger;
        private readonly CompanyService companyService;
        private readonly IMemberRepository<Member> memberRepository;
        private readonly IRepository<Attachment> attachmentRepository;
        private readonly IRepository<Address> addressRepository;
        private readonly IRepository<MemberAttachment> memberAttRepository;
        private readonly IRepository<NextOfKin> nextOfKinRepository;
        private readonly IRepository<MemberApproval> memberApprovalRepository;

        public MemberService(
            Logger loger,
            CompanyService companySer,
            IRepository<Address> addressRepo,
            IMemberRepository<Member> memberRepo,
            IRepository<NextOfKin> nextOfKinRepo,
            IRepository<Attachment> attachmentRepo,
            IRepository<MemberAttachment> memberAttRepo,
            IRepository<MemberApproval> memberApprovalRepo)
        {
            logger = loger;
            memberRepository = memberRepo;
            addressRepository = addressRepo;
            memberAttRepository = memberAttRepo;
            nextOfKinRepository = nextOfKinRepo;
            attachmentRepository = attachmentRepo;
            memberApprovalRepository = memberApprovalRepo;
            companyService = companySer;
        }

        public async Task<Member> GetMemberAsync(int id)
        {
            return await memberRepository.GetByIdAsync(id);
        }

        public async Task<Member> GetDetailedMemberAsync(int id)
        {
            return await memberRepository.GetDetailedMember(id);
        }
        public async Task<Member> GetMemberByUserIdAsync(int userId)
        {
            return await memberRepository
                .FirstOrDefaultAsync(e => e.ApplicationUserId == userId);
        }

        public async Task<List<Member>> GetMembersAsync(int companyId)
        {
            return await memberRepository
                .ListAsync(new MembersSpecification(companyId));
        }

        public async Task<List<Member>> GetDetailedMembersAsync(int companyId)
        {
            return await memberRepository
                .ListAsync(new MembersSpecification(companyId, true));
        }

        public async Task<MemberAttachment> GetMemberAttachmentAsync(int id)
        {
            return await memberRepository.GetDetailedMemberAttachment(id);
        }

        public async Task<Attachment> GetAttachmentByIdAsync(int id)
        {
            return await attachmentRepository.GetByIdAsync(id);
        }

        public async Task<Member> Add(
            Member member,
            int userId,
           string userFullName,
           DateTimeOffset dateTimeOffset)
        {
            if (member.CompanyId == 0)
                throw new Exception("An error occured while saving member");
            Company company = await companyService.GetCompanyAsync(member.CompanyId);
            if (company == null)
                throw new Exception("Invalid company");

            if (member.CurrencyId == 0)
            {
                //if member currency is not specified use company currency
                member.CurrencyId = company.CurrencyId;
            }

            if (string.IsNullOrWhiteSpace(member.FirstName) 
                && string.IsNullOrWhiteSpace(member.LastName))
                throw new Exception("Name cannot be blank");

            var homeAddress = new Address();
            addressRepository.Add(homeAddress);
            var permanentAddress = new Address();
            addressRepository.Add(permanentAddress);

            await addressRepository.SaveChangesAsync();

            member.HomeAddressId = homeAddress.Id;
            member.PermanentAddressId = permanentAddress.Id;

            memberRepository.Add(member);
            await memberRepository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = member.CompanyId,
                CreatedBy = userId,
                CreatedByName = userFullName,
                CreatedOn = dateTimeOffset,
                RecordId = member.Id,
                RecordType = RecordType.Member,
                SerializedRecord = logger.SeliarizeObject(member)
            });
            return member;
        }
        
        public async Task<Member> Update(
           MemberModel model,
           int userId,
           string userFullName,
           DateTimeOffset dateTimeOffset,
           bool systemGenerated = false)
        {

            if (string.IsNullOrWhiteSpace(model.OtherNames))
                throw new Exception("First name cannot be blank");

            if (string.IsNullOrWhiteSpace(model.IndentificationNo))
                throw new Exception("ID/Passport number cannot be blank");

            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
                throw new Exception("Phone number cannot be blank");

            Member member = await GetMemberAsync(model.Id);
            if (member == null)
                throw new Exception("Member not found");

            if (memberRepository.DuplicateIdNumber(member.Id, model.IndentificationNo, member.CompanyId))
                throw new Exception($"Updating member ID/Passport number with {member.IndentificationNo} would create a duplicate record");

            member.Gender = model.Gender;
            member.IndentificationNo = model.IndentificationNo;
            member.MaritalStatus = model.MaritalStatus;
            member.FirstName = model.OtherNames;
            member.PhoneNumber = model.PhoneNumber;
            member.LastName = model.Surname;
            member.NearestTown = model.NearestTown;
            member.Occupation = model.Occupation;
            member.OccupationType = model.OccupationType;
            member.LearntAboutUs = model.LearntAboutUs;
            member.Title = model.Title;

            if (member.HomeAddressId.HasValue)
            {
                UpdateAddress(model, member.HomeAddressId.Value);
            }
            else
            {
                var address = new Address();
                addressRepository.Add(address);
                member.HomeAddress = address;
            }
                

            if (member.PermanentAddressId.HasValue)
            {
                UpdateAddress(model, member.PermanentAddressId.Value);
            }
            else
            {
                var address = new Address();
                addressRepository.Add(address);
                member.PermanentAddress = address;
            }
                

            SaveNextOfKin(model, member.CompanyId, userId, userFullName, dateTimeOffset);
            await memberRepository.Update(member);
            if (!systemGenerated)
            {
                await logger.Log(new AuditLog
                {
                    ActionType = ActionType.Update,
                    CompanyId = member.CompanyId,
                    CreatedBy = member.ApplicationUserId,
                    CreatedByName = $"{member.FirstName} {member.LastName}",
                    CreatedOn = dateTimeOffset,
                    RecordId = member.Id,
                    RecordType = RecordType.LedgerAccount,
                    SerializedRecord = logger.SeliarizeObject(member)
                });
            }

            return member;
        }


        private async void UpdateAddress(MemberModel model, int addressId)
        {
            Address address = await addressRepository
                    .GetByIdAsync(addressId);
            address.CountryId = model.PermanentAddress?.Country;
            address.City = model.PermanentAddress?.County;
            address.PostalCode = model.PermanentAddress?.District;
            address.Location = model.PermanentAddress?.Location;
            address.PoBox = model.PermanentAddress?.Village;

            await addressRepository.Update(address);
        }

        private async void SaveNextOfKin(
            MemberModel model,
            int companyId,
            int userId,
            string userFullName,
            DateTimeOffset dateTimeOffset)
        {
            if (model.NextOfKins.Count == 0)
                return;
            foreach (var nok in model.NextOfKins)
            {
                if (nok.Id > 0)
                {
                    var nk = await nextOfKinRepository.GetByIdAsync(nok.Id);
                    if (nk == null) continue;
                    nk.CareOf = nok.CareOf;
                    nk.Contacts = nok.Contacts;
                    nk.IsMinor = nok.IsMinor;
                    nk.Name = nok.Name;
                    nk.Percentage = nok.Percentage;
                    nk.Relation = nok.Relation;
                    await nextOfKinRepository.Update(nk);
                }
                else
                {
                    var nk = new NextOfKin
                    {
                        CareOf = nok.CareOf,
                        Contacts = nok.Contacts,
                        IsMinor = nok.IsMinor,
                        Name = nok.Name,
                        Percentage = nok.Percentage,
                        Relation = nok.Relation,
                        CompanyId = companyId,
                        CreatedBy = userId,
                        CreatedByName = userFullName,
                        CreatedOn = dateTimeOffset,
                        MemberId = model.Id
                    };
                    nextOfKinRepository.Add(nk);
                }
            }
        }

        /// <summary>
        /// Save member attachment
        /// </summary>
        /// <param name="member">Member Id</param>
        /// <param name="attachment">Attachment object</param>
        /// <param name="attachmentType">Type of attachment</param>
        /// <returns></returns>
        public async Task<Member> SaveAttachment(
            Member member,
            Attachment attachment,
            AttachmentType attachmentType)
        {
            MemberAttachment memberAttachment = new MemberAttachment
            {
                MemberId = member.Id,
                Attachment = attachment,
                AttachmentType = attachmentType
            };
            memberAttRepository.Add(memberAttachment);
            await memberAttRepository.SaveChangesAsync();
            return await GetDetailedMemberAsync(member.Id);
        }

        public async Task<bool> DeleteAttachmentAsync(MemberAttachment memberAttachment)
        {
            Attachment attachment = await attachmentRepository
                .GetByIdAsync(memberAttachment.AttachmentId);
            attachmentRepository.Delete(attachment);
            memberAttRepository.Delete(memberAttachment);
            await memberAttRepository.SaveChangesAsync();
            return true;
        }

        public async Task<Member> ApproveMember(MemberApproval memberApproval)
        {
            Member member = await GetMemberByUserIdAsync(memberApproval.MemberId);
            if (member == null)
                throw new Exception("Member not found");
            if (string.IsNullOrWhiteSpace(member.IndentificationNo))
                throw new Exception("Member national ID number required to generate member number");

            if (member.MemberStatus > MemberStatus.Active)
            {
                return await GetDetailedMemberAsync(memberApproval.MemberId);
            }
            if (!string.IsNullOrEmpty(member.MemberNumber))
            {
                return await GetDetailedMemberAsync(memberApproval.MemberId);
            }
            SaccoSettings saccoSettings = await companyService
                .GetSaccoSettingsAsync(member.CompanyId);

            if (string.IsNullOrWhiteSpace(member.MemberNumber))
            {
                member.MemberNumber = GetMemberNumber(member, saccoSettings.NextMemberNumber);
            }
            member.MemberStatus = MemberStatus.Approved;
            await memberRepository.Update(member);
            saccoSettings.NextMemberNumber += 1;
            await companyService.UpdateSaccoSettings(saccoSettings);
            memberApprovalRepository.Add(memberApproval);

            await memberApprovalRepository.SaveChangesAsync();
            return await GetDetailedMemberAsync(memberApproval.MemberId);
        }

        public async Task<Member> PutMemberOnHold(MemberApproval memberApproval)
        {
            Member member = await GetMemberByUserIdAsync(memberApproval.MemberId);
            if (member == null)
                throw new Exception("Member not found");

            member.MemberStatus = MemberStatus.OnHold;
            await memberRepository.Update(member);
            memberApprovalRepository.Add(memberApproval);

            await memberApprovalRepository.SaveChangesAsync();
            return await GetDetailedMemberAsync(memberApproval.MemberId);
        }

        public async Task<Member> RejectMember(MemberApproval memberApproval)
        {
            Member member = await GetMemberByUserIdAsync(memberApproval.MemberId);
            if (member == null)
                throw new Exception("Member not found");

            member.MemberStatus = MemberStatus.Rejected;
            await memberRepository.Update(member);
            memberApprovalRepository.Add(memberApproval);

            await memberApprovalRepository.SaveChangesAsync();
            return await GetDetailedMemberAsync(memberApproval.MemberId);
        }

        private string GetMemberNumber(Member member, int currentNumber)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.Month.ToString().PadLeft(2, '0'));
            sb.Append((DateTime.Now.Year - 2000).ToString());
            sb.Append(member.IndentificationNo.Substring(0, 1));
            sb.Append(member.IndentificationNo.Substring(member.IndentificationNo.Length - 1, 1));
            sb.Append((currentNumber + 1).ToString().PadLeft(4, '0'));
            return sb.ToString();
        }
    }
}
