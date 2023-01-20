using SmartBooks.ApplicationCore.Repositories;
using SmartBooks.Domains.Entities;
using System.Text.RegularExpressions;

namespace SmartBooks.ApplicationCore.Helpers
{
    public static class GeneralHelpers
    {
        public static int GetNumberFromString(this string subjectString)
        {
            int.TryParse(Regex.Match(subjectString, @"\d+").Value, out int returnValue);
            return returnValue;
        }

        public static string GetReferenceNumber(this DocumentSetting documentSetting)
        {
            if (!documentSetting.AutoGenerateReference) return "";
            string provisionalRef = $"{documentSetting.ReferencePrefix}{documentSetting.NextReferenceNo}";
            if (documentSetting.ReferenceLength > provisionalRef.Length)
            {
                int paddingLen = documentSetting.ReferenceLength - documentSetting.ReferencePrefix.Length;
                return $"{documentSetting.ReferencePrefix}{documentSetting.NextReferenceNo.ToString().PadLeft(paddingLen, '0')}";
            }
            return provisionalRef;
        }

        public static string GetLedgerAccountName(
            this string accountName,
            int companyId,
            ILedgerAccountsRepository<LedgerAccount> ledgerRepository)
        {
            string accName = accountName;
            bool accNameExist = true;
            int count = 0;
            while (accNameExist)
            {
                accNameExist = ledgerRepository.AccountNameExists(accName, companyId);
                if (accNameExist)
                {
                    count += 1;
                    accName = $"{accName}_{count}";
                }
            }
            return accName;
        }

        public static string GetCleanLedgerAccountName(this string accountName)
        {
            if (string.IsNullOrWhiteSpace(accountName)) return "";

            if (accountName.Contains('_'))
            {
                int index = accountName.LastIndexOf('_');
                return accountName.Substring(0, accountName.Length - (accountName.Length - index));

            } else
            {
                return accountName;
            }
        }
    }
}
