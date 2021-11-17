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

        public string CalculateChecksum(string countryCode, string nationalPart)
        {
            int remainder = ChecksumAlgorithm(countryCode, "00", nationalPart);
            int checksum = 98 - remainder;
            return $"{checksum}";
        }
        
        protected virtual bool IsValid(string countryCode, string checkDigits, string nationalPart)
        {
            return IsIBANChecksumValid(countryCode, checkDigits, nationalPart);
        }

        private bool IsIBANChecksumValid(string countryCode, string checksum, string nationalPart)
        {
            int remainder = ChecksumAlgorithm(countryCode, checksum, nationalPart);
            return remainder == 1;
        }

        private int ChecksumAlgorithm(string countryCode, string checksum, string nationalPart)
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
                    throw new ArgumentException($"IBAN contains invalid character: [{c}]");
                }
            }

            var interpolated = interpolatedBuilder.ToString();

            var bigNumber = BigInteger.Parse(interpolated);
            var ninetySeven = new BigInteger(97);
            var remainder = BigInteger.Remainder(bigNumber, ninetySeven);

            return (int)remainder;
        }
    }
}