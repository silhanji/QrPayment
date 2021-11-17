using System;

namespace QrPayment
{
    public sealed class IBAN
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
    }
}