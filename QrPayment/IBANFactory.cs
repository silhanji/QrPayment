using System;
using System.Numerics;
using System.Text;

namespace QrPayment
{
    public class IBANFactory
    {
        public IBAN CreateIBAN(string representation)
        {
            if (representation.Length < 5)
                throw new ArgumentException("Representation is not a valid IBAN, not enough characters");
            
            var countryCode = representation[..2];
            var checkDigits = representation[2..4];
            var nationalPart = representation[4..];

            if (! IsValid(countryCode, checkDigits, nationalPart))
                throw new ArgumentException("Representation is not a valid IBAN");

            return new IBAN(countryCode, checkDigits, nationalPart);
        }

        protected bool IsValid(string countryCode, string checkDigits, string nationalPart)
        {
            return IsIBANChecksumValid(countryCode, checkDigits, nationalPart);
        }

        private bool IsIBANChecksumValid(string countryCode, string checksum, string nationalPart)
        {
            string reordered = $"{nationalPart}{countryCode}{checksum}";
            var interpolatedBuilder = new StringBuilder();
            foreach (var c in reordered)
            {
                if (c is >= '0' and <= '9')
                {
                    interpolatedBuilder.Append(c);
                }
                else if(c is >= 'A' and <= 'Z')
                {
                    int value = c - 'A' + 10;
                    interpolatedBuilder.Append(value);
                }
                else
                {
                    // IBAN contains invalid characters
                    return false;
                }
            }

            var interpolated = interpolatedBuilder.ToString();

            var bigNumber = BigInteger.Parse(interpolated);
            var ninetySeven = new BigInteger(97);
            var remainder = BigInteger.Remainder(bigNumber, ninetySeven);

            return remainder.IsOne;
        }
    }
}