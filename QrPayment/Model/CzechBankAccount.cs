using System;

namespace QrPayment.Model
{
    public class CzechBankAccount
    {
        public string AccountPrefix { get; }
        
        public string AccountNumber { get; }
        
        public string BankNumber { get; }

        public CzechBankAccount(string accountPrefix, string accountNumber, string bankNumber)
        {
            if (accountPrefix.Length > 6)
                throw new ArgumentException("Account prefix to large");
            if (accountNumber.Length > 10)
                throw new ArgumentException("Account number to large");
            if (bankNumber.Length != 4)
                throw new ArgumentException("Bank number has incorrect number of digits");

            accountPrefix = RemoveLeadingZeros(accountPrefix);
            accountNumber = RemoveLeadingZeros(accountNumber);

            if (accountNumber.Length == 0)
                throw new ArgumentException("Account number can not be 0");
            
            AccountPrefix = accountPrefix;
            AccountNumber = accountNumber;
            BankNumber = bankNumber;

            static string RemoveLeadingZeros(string input)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] != '0')
                        return input[i..];
                }

                return string.Empty;
            }
        }

        public override string ToString()
        {
            return AccountPrefix == string.Empty 
                ? $"{AccountNumber}/{BankNumber}" 
                : $"{AccountPrefix}-{AccountNumber}/{BankNumber}";
        }

        public static explicit operator IBAN(CzechBankAccount bankAccount)
        {
            var ibanFactory = new CzechIBANFactory();

            string paddedPrefix = bankAccount.AccountPrefix.PadLeft(6, '0');
            string paddedAccountNumber = bankAccount.AccountNumber.PadLeft(10, '0');
            
            string countryCode = "CZ";
            string nationalPart = $"{bankAccount.BankNumber}{paddedPrefix}{paddedAccountNumber}";

            var checksum = ibanFactory.CalculateChecksum(countryCode, nationalPart);
            if (checksum.Length < 2)
                checksum = $"0{checksum}";
            
            return ibanFactory.CreateIBAN($"{countryCode}{checksum}{nationalPart}");
        }

        public static explicit operator CzechBankAccount(IBAN iban)
        {
            if (iban.CountryCode != "CZ")
                throw new InvalidOperationException($"Can not convert IBAN: [{iban}] to Czech Bank Account");

            var bankNumber = iban.NationalPart[..4];
            var accountPrefix = iban.NationalPart[4..10];
            var accountNumber = iban.NationalPart[10..];

            return new CzechBankAccount(accountPrefix, accountNumber, bankNumber);
        }
    }
}