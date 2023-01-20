using SmartBooks.Domains.Enums;
using System.Collections.Generic;

namespace SmartBooks.ApplicationCore.Models
{
    public class MemberModel
    {
        public MemberModel()
        {
            NextOfKins = new List<NextOfKinModel>();
        }
        public int Id { get; set; }
        public string MemberNumber { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IndentificationNo { get; set; }
        public int PassportCopyId { get; set; }
        public int SignatureId { get; set; }
        public int? HomeAddressId { get; set; }
        public int? PermanentAddressId { get; set; }
        public string NearestTown { get; set; }
        public string Occupation { get; set; }
        public OccupationType OccupationType { get; set; }
        public LearntAboutUs LearntAboutUs { get; set; }
        public AddressModel HomeAddress { get; set; }
        public AddressModel PermanentAddress { get; set; }

        public List<NextOfKinModel> NextOfKins { get; set; }
    }

    public class AddressModel
    {
        public int Id { get; set; }
        public string Village { get; set; }
        public string Location { get; set; }
        public string District { get; set; }
        public string County { get; set; }
        public int Country { get; set; }
    }
    
    public class MemberApprovalModel
    {
        public int MemberId { get; set; }
        public string MessageToMember { get; set; }
        public string Comments { get; set; }
    }

    public class NextOfKinModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string Contacts { get; set; }
        public bool IsMinor { get; set; }
        public string CareOf { get; set; }
        public decimal Percentage { get; set; }
    }
}
