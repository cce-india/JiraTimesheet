using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class AvatarUrls
    {
        [JsonProperty("16x16")]
        public string Size16 { get; set; }

        [JsonProperty("48x48")]
        public string Size48 { get; set; }
    }
}