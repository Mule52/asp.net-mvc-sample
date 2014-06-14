using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Sample.Core.Models
{
    public class Role : IdentityRole
    {
        [JsonIgnore]
        public string Description { get; set; }
    }
}