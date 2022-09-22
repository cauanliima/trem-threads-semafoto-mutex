using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CHESF.COMPRAS.Domain.APP
{
    public class DeviceInstallation
    {
        [Required] public string InstallationId { get; set; }

        [Required] public string Platform { get; set; }

        [Required] public string PushChannel { get; set; }

        public IList<string> Tags { get; set; }
    }
}