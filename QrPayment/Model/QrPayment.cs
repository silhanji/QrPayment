using System;
using System.Text;

namespace QrPayment.Model
{
    public class QrPayment
    {
        public IBAN Account { get; }
        
        public MoneyAmount? Amount { get; }
        
        public string? VariableSymbol { get; }
        
        public string? ConstantSymbol { get; }
        
        public string? Message { get; }

        public QrPayment(
            IBAN account, 
            MoneyAmount? amount = null, 
            string? variableSymbol = null, 
            string? constantSymbol = null, 
            string? message = null)
        {
            Account = account;

            if (amount is not null && amount.Amount < 0)
                throw new ArgumentException("amount has to be greater or equal to 0");
            Amount = amount;

            if (variableSymbol is not null)
            {
                if(variableSymbol.Length > 10)
                    throw new ArgumentException("variableSymbol can have at most 10 characters");
                if (variableSymbol.Contains('*'))
                    throw new ArgumentException("variableSymbol can not contain asterisk [*]");
            }
            VariableSymbol = variableSymbol;

            if (constantSymbol is not null)
            {
                if(constantSymbol.Length > 10)
                    throw new ArgumentException("constantSymbol can have at most 10 characters");
                if (constantSymbol.Contains('*'))
                    throw new ArgumentException("constantSymbol can not contain asterisk [*]");
            } 
            ConstantSymbol = constantSymbol;

            if (message is not null)
            {
                if(message.Length > 60)
                    throw new ArgumentException("message can have at most 60 characters");
                if (message.Contains('*'))
                    throw new ArgumentException("message can not contain asterisk [*]");
            } 
            Message = message;
        }

        public string CreatePaymentString()
        {
            var paymentStringBuilder = new StringBuilder();

            string header = "SPD*";
            string protocolVersion = "1.0";

            paymentStringBuilder.Append(header);
            paymentStringBuilder.Append(protocolVersion);
            
            paymentStringBuilder.Append($"*ACC:{Account.Representation}");

            if (Amount is not null)
            {
                paymentStringBuilder.Append($"*AM:{Amount.Amount}");
                paymentStringBuilder.Append($"*CC:{Amount.Currency.ToString()}");
            }

            if (VariableSymbol is not null)
            {
                paymentStringBuilder.Append($"X-VS:{VariableSymbol}");
            }

            if (ConstantSymbol is not null)
            {
                paymentStringBuilder.Append($"X-KS:{ConstantSymbol}");
            }

            if (Message is not null)
            {
                paymentStringBuilder.Append($"*MSG:{Message}");
            }

            return paymentStringBuilder.ToString();
        }
    }
}