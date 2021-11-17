using System;
using NUnit.Framework;
using QrPayment;
using QrPayment.Model;

namespace QrPaymentTests
{
    [TestFixture]
    public class CzechBankAccountTests
    {
        [Test]
        public void Ctor_Valid_Test()
        {
            Ctor_TestMethod(
                accountPrefix: "234",
                accountNumber: "658794",
                bankNumber: "2010",
                expected: "234-658794/2010");
        }

        [Test]
        public void Ctor_NoPrefix_Test()
        {
            Ctor_TestMethod(
                accountPrefix:string.Empty,
                accountNumber: "658794",
                bankNumber: "2010",
                expected: "658794/2010");
        }

        [Test]
        public void Ctor_LeadingZeros_Test()
        {
            Ctor_TestMethod(
                accountPrefix: "00234",
                accountNumber: "000658794",
                bankNumber: "2010",
                expected: "234-658794/2010");
        }

        [Test]
        public void Ctor_InvalidPrefix_Test()
        {
            Ctor_InvalidData_TestMethod(
                accountPrefix: "1234234",
                accountNumber: "658794",
                bankNumber: "2010");
        }

        [Test]
        public void Ctor_InvalidAccountNumber_Test()
        {
            Ctor_InvalidData_TestMethod(
                accountPrefix: "234",
                accountNumber: "12345658794",
                bankNumber: "2010");
        }

        [Test]
        public void Ctor_EmptyAccountNumber_Test()
        {
            Ctor_InvalidData_TestMethod(
                accountPrefix: "234",
                accountNumber: "0000",
                bankNumber: "2010");
        }

        [Test]
        public void Ctor_InvalidBankNumber_Test()
        {
            Ctor_InvalidData_TestMethod(
                accountPrefix: "234",
                accountNumber: "658794",
                bankNumber: "300");
        }

        [Test]
        public void Cast_CzechBankAccountToIBAN_Test()
        {
            var bankAccount = new CzechBankAccount("19", "2000145399", "0800");
            var iban = (IBAN)bankAccount;

            Assert.That(iban, Is.Not.Null);
        }

        [Test]
        public void Cast_IBANToCzechBankAccount_Test()
        {
            var ibanFactory = new CzechIBANFactory();
            var iban = ibanFactory.CreateIBAN("CZ6508000000192000145399");

            var bankAccount = (CzechBankAccount)iban;
            
            Assert.That(bankAccount, Is.Not.Null);
        }
        
        private void Ctor_TestMethod(string accountPrefix, string accountNumber, string bankNumber, string expected)
        {
            var bankAccount = new CzechBankAccount(accountPrefix, accountNumber, bankNumber);
            var bankAccountStr = bankAccount.ToString();
            Assert.That(bankAccountStr, Is.EqualTo(expected));
        }

        private void Ctor_InvalidData_TestMethod(string accountPrefix, string accountNumber, string bankNumber)
        {
            Assert.Throws<ArgumentException>(() => new CzechBankAccount(accountPrefix, accountNumber, bankNumber));
        }
    }
}