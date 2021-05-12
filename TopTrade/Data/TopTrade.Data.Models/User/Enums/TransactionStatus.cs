namespace TopTrade.Data.Models.User.Enums
{
    public enum TransactionStatus
    {
        Pending = 1,           // The payment is pending.See pending_reason for more information.
        Approved = 2,         // The payment has been completed, and the funds have been added successfully to your account balance.
        Completed = 3,         // The payment has been completed, and the funds have been added successfully to your account balance.
        Canceled = 4, // A reversal has been canceled. For example, you won a dispute with the customer, and the funds for the transaction that was reversed have been returned to you.
    }
}
