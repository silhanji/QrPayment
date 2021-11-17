using System;
using System.Collections.Generic;
using NUnit.Framework;
using QrPayment;

namespace QrPaymentTests
{
    [TestFixture]
    public class CzechIBANFactoryTests
    {
        public static IEnumerable<string> ValidIBANs = new[]
        {
            "CZ6508000000192000145399",
            "CZ6907101781240000004159",
        };
        public static IEnumerable<string> InvalidIBANs = new[]
        {
            "",
            "CZ3",
            "CZ6508000000192000145394",
            "GB82WEST12345698765432",
        };

        [TestCaseSource(typeof(CzechIBANFactoryTests), nameof(ValidIBANs))]
        public void ValidIBANTest(string representation)
        {
            var factory = new CzechIBANFactory();
            var iban = factory.CreateIBAN(representation);
            
            Assert.That(iban.Representation, Is.EqualTo(representation));
        }

        [TestCaseSource(typeof(CzechIBANFactoryTests), nameof(InvalidIBANs))]
        public void InvalidIBANTest(string representation)
        {
            var factory = new CzechIBANFactory();
            Assert.Throws<ArgumentException>(() => factory.CreateIBAN(representation));
        }
    }
}