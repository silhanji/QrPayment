using System;

namespace QrPayment
{
    public sealed class IBAN : IEquatable<IBAN>
    {
        public string Representation => $"{CountryCode}{CheckDigits}{NationalPart}";
        
        public string CountryCode { get; }
        
        public string CheckDigits { get; }
        
        public string NationalPart { get; }

        internal IBAN(string countryCode, string checkDigits, string nationalPart)
        {
            CountryCode = countryCode;
            CheckDigits = checkDigits;
            NationalPart = nationalPart;
        }

        public bool Equals(IBAN? other)
        {
            if (other is null)
                return false;

            return Representation == other.Representation;
        }

        public override bool Equals(object? obj)
        {
            if (obj is IBAN other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CountryCode, CheckDigits, NationalPart);
        }
        
        public static bool operator ==(IBAN iban1, IBAN iban2)
        {
            return iban1.Equals(iban2);
        }

        public static bool operator !=(IBAN iban1, IBAN iban2)
        {
            return ! iban1.Equals(iban2);
        }
    }
}