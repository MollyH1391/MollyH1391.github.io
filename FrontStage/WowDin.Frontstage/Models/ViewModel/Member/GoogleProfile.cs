using Newtonsoft.Json;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class GoogleProfile
    {
        [JsonProperty("resourceName")]
        public string ResourceName { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("names")]
        public Name[] Names { get; set; }

        [JsonProperty("emailAddresses")]
        public Emailaddress[] EmailAddresses { get; set; }

        public class Name
        {
            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("familyName")]
            public string FamilyName { get; set; }

            [JsonProperty("givenName")]
            public string GivenName { get; set; }

            [JsonProperty("displayNameLastFirst")]
            public string DisplayNameLastFirst { get; set; }

            [JsonProperty("unstructuredName")]
            public string UnstructuredName { get; set; }
        }

        public class Metadata
        {
            [JsonProperty("primary")]
            public bool Primary { get; set; }

            [JsonProperty("source")]
            public Source Source { get; set; }
        }

        public class Source
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public class Emailaddress
        {
            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }

}
