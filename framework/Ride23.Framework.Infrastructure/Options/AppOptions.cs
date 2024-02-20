using System.ComponentModel.DataAnnotations;

namespace Ride23.Framework.Infrastructure.Options
{
    public class AppOptions : IOptionsRoot
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "Ride23.WebAPI";
    }
}
