using System;
using NUnit.Framework;
using QrPayment.Model;

namespace QrPaymentTests
{
    [TestFixture]
    public class QrPaymentTests
    {
        [Test]
        public void CreatePaymentString_Valid_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var amount = new MoneyAmount(1000, Currency.Eur);
            var variableSymbol = "420";
            var constantSymbol = "69";
            var message = "Monthly payment";

            var payment = new Payment(iban, amount,variableSymbol, constantSymbol, message);
            string expectedPaymentString =
                "SPD*1.0*ACC:CZ6508000000192000145399*AM:1000*CC:EUR*X-VS:420*X-KS:69*MSG:Monthly payment";

            var paymentString = payment.CreatePaymentString();
            Assert.That(paymentString, Is.EqualTo(expectedPaymentString));
        }

        [Test]
        public void Ctor_InvalidAmount_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var amount = new MoneyAmount(-10, Currency.Czk);

            Assert.Throws<ArgumentException>(() => new Payment(iban, amount));
        }

        [Test]
        public void Ctor_InvalidConstantSymbol1_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var constantSymbol = "12*34";

            Assert.Throws<ArgumentException>(() => new Payment(iban, constantSymbol: constantSymbol));
        }
        
        [Test]
        public void Ctor_InvalidConstantSymbol2_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var constantSymbol = "12A34";

            Assert.Throws<ArgumentException>(() => new Payment(iban, constantSymbol: constantSymbol));
        }

        [Test]
        public void Ctor_ConstantSymbolToLong_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var constantSymbol = "01234567899";

            Assert.Throws<ArgumentException>(() => new Payment(iban, constantSymbol: constantSymbol));
        }

        [Test]
        public void Ctor_InvalidVariableSymbol1_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var variableSymbol = "12*34";

            Assert.Throws<ArgumentException>(() => new Payment(iban, variableSymbol: variableSymbol));
        }
        
        [Test]
        public void Ctor_InvalidVariableSymbol2_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var variableSymbol = "12A34";

            Assert.Throws<ArgumentException>(() => new Payment(iban, variableSymbol: variableSymbol));
        }

        [Test]
        public void Ctor_VariableSymbolToLong_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var variableSymbol = "01234567899";

            Assert.Throws<ArgumentException>(() => new Payment(iban, variableSymbol: variableSymbol));
        }
        
        [Test]
        public void Ctor_InvalidMessage_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var message = "Something about *";

            Assert.Throws<ArgumentException>(() => new Payment(iban, message: message));
        }

        [Test]
        public void Ctor_MessageToLong_Test()
        {
            var iban = (IBAN)new CzechBankAccount("19", "2000145399", "0800");
            var message = new string('a', 61);

            Assert.Throws<ArgumentException>(() => new Payment(iban, message: message));
        }
    }
}