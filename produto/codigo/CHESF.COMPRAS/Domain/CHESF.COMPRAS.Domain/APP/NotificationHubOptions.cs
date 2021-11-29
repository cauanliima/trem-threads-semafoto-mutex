using System.ComponentModel.DataAnnotations;

namespace CHESF.COMPRAS.Domain.APP
{
    public class NotificationHubOptions
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}