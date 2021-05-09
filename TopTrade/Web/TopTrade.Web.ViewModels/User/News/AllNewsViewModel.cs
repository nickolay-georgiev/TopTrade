namespace TopTrade.Web.ViewModels.User
{
    using System;

    public class AllNewsViewModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? PublishedAt { get; set; }
    }
}
