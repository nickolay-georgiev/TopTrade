namespace TopTrade.Web.ViewModels.Administration
{
    public class AdministratorDashboardViewModel
    {
        public int TotalUserThisMonth { get; set; }

        public int TotalUsersWithDeletedAccounts { get; set; }

        public int TotalAccountManagers { get; set; }

        public decimal TotalProfitFromFees { get; set; }
    }
}
