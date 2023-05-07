using Newtonsoft.Json;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class LINELoginResource
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        // 這邊跟一般的TokenResponse不同，多了使用者的Id Token
        [JsonProperty("id_token")]
        public string IDToken { get; set; }
    }
}
