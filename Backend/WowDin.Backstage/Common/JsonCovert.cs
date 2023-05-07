namespace WowDin.Frontstage.Common
{
    public static class JsonCovert
    {
        public static T Deserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize(object source)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(source);
        }
    }
}
