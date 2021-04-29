namespace TopTrade.Data.Models.User.Enums
{
    public enum TransactionStatus
    {
        Completed = 1,         // The payment has been completed, and the funds have been added successfully to your account balance.
        Pending = 2,           // The payment is pending.See pending_reason for more information.
        Canceled_Reversal = 3, // A reversal has been canceled. For example, you won a dispute with the customer, and the funds for the transaction that was reversed have been returned to you.
        Denied = 4,            // You denied the payment.This happens only if the payment was previously pending because of possible reasons described for the pending_reason variable or the Fraud_Management_Filters_x variable.
        Failed = 5,            // The payment has failed. This happens only if the payment was made from your customer’s bank account.
        Created = 6,           // A German ELV payment is made using Express Checkout.
        Expired = 7,           // This authorization has expired and cannot be captured.
        Refunded = 8,          // You refunded the payment.
        Reversed = 9,          // A payment was reversed due to a chargeback or other type of reversal.The funds have been removed from your account balance and returned to the buyer. The reason for the reversal is specified in the ReasonCode element.
        Processed = 10,        // A payment has been accepted.
    }
}
