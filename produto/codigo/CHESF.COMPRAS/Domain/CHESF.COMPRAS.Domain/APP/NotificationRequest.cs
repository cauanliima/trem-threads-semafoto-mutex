namespace CHESF.COMPRAS.Domain.APP
{
    public class NotificationRequest
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string[] Tags { get; set; }
        public bool Silent { get; set; }
    }
}