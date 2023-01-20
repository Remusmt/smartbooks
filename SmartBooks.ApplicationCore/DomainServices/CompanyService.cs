using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.ApplicationCore.Specifications;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SaccoEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.ApplicationCore.DomainServices
{
    public class CompanyService
    {
        private readonly IRepository<Bank> bankRepository;
        private readonly IRepository<Company> companyRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<Currency> currencyRepository;
        private readonly IRepository<CompanyDefaults> companyDefaultsRepository;
        private readonly IRepository<SaccoSettings> saccoSettingsRepository;
        public CompanyService(
            IRepository<Bank> bankRepo,
            IRepository<Company> companyRepo,
            IRepository<Country> countryRepo,
            IRepository<Currency> currencyRepo,
            IRepository<SaccoSettings> saccoSettingsRepo,
            IRepository<CompanyDefaults> companyDefaultsRepo
            )
        {
            bankRepository = bankRepo;
            companyRepository = companyRepo;
            countryRepository = countryRepo;
            currencyRepository = currencyRepo;
            saccoSettingsRepository = saccoSettingsRepo;
            companyDefaultsRepository = companyDefaultsRepo;
        }

        public async Task<Company> Register(
            string companyName,
            int countryId,
            DateTimeOffset createdOn
            )
        {
            Country country = await countryRepository.GetByIdAsync(countryId);

            if (country == null) throw new Exception("Invalid country.");
            if (!country.CurrencyId.HasValue) throw new Exception("Selected country has no currency defined.");

            Company company = await companyRepository
                .FirstOrDefaultAsync(e => e.CompanyName == companyName);

            if (company != null) return company;

            company = new Company
            {
                CompanyName = companyName,
                CountryId = country.Id,
                CurrencyId = country.CurrencyId.Value,
                CreatedOn = createdOn
            };
            companyRepository.Add(company);
            await companyRepository.SaveChangesAsync();
            // Define defaults
            CompanyDefaults companyDefaults = new CompanyDefaults
            {
                CompanyId = company.Id,
                DefaultCurrency = company.CurrencyId,
                AllowPostingToParentAccount = true,
                CreatedOn = createdOn,
                UseAccountNumbers = false,
                DefaultCodeLength = 4
            };
            companyDefaultsRepository.Add(companyDefaults);
            await companyRepository.SaveChangesAsync();

            return company;
        }

        public async Task<Company> GetCompanyAsync(int companyId)
        {
            return await companyRepository.GetByIdAsync(companyId);
        }

        public async Task<CompanyDefaults> GetCompanyDefaultsAsync(int companyId)
        {
            return await companyDefaultsRepository
                .FirstOrDefaultAsync(e => e.CompanyId == companyId);
        }

        public async Task<SaccoSettings> GetSaccoSettingsAsync(int companyId)
        {
            return await saccoSettingsRepository
                .FirstOrDefaultAsync(e => e.CompanyId == companyId);
        }
        public async Task UpdateCompanyDefaults(CompanyDefaults companyDefaults)
        {
            await companyDefaultsRepository.Update(companyDefaults);
        }

        public async Task UpdateSaccoSettings(SaccoSettings saccoSettings)
        {
            await saccoSettingsRepository.Update(saccoSettings);
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            return await countryRepository.ListAllAsync();
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            return await currencyRepository.ListAllAsync();
        }

        public async Task<List<Bank>> GetBanksAsync(int countryId)
        {
            return await bankRepository.ListAsync(new BankSpecification(countryId));
        }
    }
}
