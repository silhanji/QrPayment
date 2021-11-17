using System;
using System.Collections.Generic;
using NUnit.Framework;
using QrPayment;

namespace QrPaymentTests
{
    [TestFixture]
    public class IBANFactoryTests
    {
        public static IEnumerable<string> ValidIBANs = new[]
        {
            "CZ6508000000192000145399",
            "CZ6907101781240000004159",
            "GB82WEST12345698765432"
            
        };
        public static IEnumerable<string> InvalidIBANs = new[]
        {
            "",
            "CZ3",
            "CZ6508000000192000145394"
        };

        [TestCaseSource(typeof(IBANFactoryTests), nameof(ValidIBANs))]
        public void ValidIBANTest(string representation)
        {
            var factory = new IBANFactory();
            var iban = factory.CreateIBAN(representation);
            
            Assert.That(iban.Representation, Is.EqualTo(representation));
        }

        [TestCaseSource(typeof(IBANFactoryTests), nameof(InvalidIBANs))]
        public void InvalidIBANTest(string representation)
        {
            var factory = new IBANFactory();
            Assert.Throws<ArgumentException>(() => factory.CreateIBAN(representation));
        }
    }
}