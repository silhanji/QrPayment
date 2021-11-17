namespace QrPayment
{
    public class CzechIBANFactory : IBANFactory
    {
        protected override bool IsValid(string countryCode, string checkDigits, string nationalPart)
        {
            if (! base.IsValid(countryCode, checkDigits, nationalPart))
                return false;

            if (countryCode != "CZ")
                return false;

            if (nationalPart.Length != 20)
                return false;

            foreach (var c in nationalPart)
            {
                if (c is < '0' or > '9')
                    return false;
            }

            var accountPrefix = nationalPart[4..10];
            var accountNumber = nationalPart[10..];

            if (! IsCzechChecksumValid(accountPrefix))
                return false;
            if (! IsCzechChecksumValid(accountNumber))
                return false;

            return true;
        }

        private bool IsCzechChecksumValid(string accountNumber)
        {
            int[] weights = { 1, 2, 4, 8, 5, 10, 9, 7, 3, 6  };

            long sum = 0;
            for (int charIndex = 0; charIndex < accountNumber.Length; charIndex++)
            {
                var weight = weights[charIndex];
                var c = accountNumber[accountNumber.Length - charIndex - 1];
                var digit = c - '0';
                sum += weight * digit;
            }

            var rem = sum % 11;
            return rem == 0;
        }
    }
}