namespace Autransoft.ApplicationCore.AppSettings.Integration
{
    public class StatusInvest
    {
        public string Id { get; set; }
        public string URL { get; set; }
        public Routes Routes { get; set; }
    }

    public class Routes
    {
        public string AdvancedSearch { get; set; }
    }
}