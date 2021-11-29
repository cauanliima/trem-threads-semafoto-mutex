using System.Collections.Generic;

namespace CHESF.COMPRAS.Domain.APP
{
    public class NotificationRequest
    {
        public NotificationRequest()
        {
            Tags = new List<string>();
        }
        public string Text { get; set; }
        public string Action { get; set; }
        public IList<string> Tags { get; set; }
        public bool Silent { get; set; }
    }
}