namespace WowDin.Backstage.Models
{
    public class APIResult
    {
        public APIResult(APIStatus status, string msg, object result)
        {
            Status = status;
            Msg = msg;
            Result = result;
        }

        public APIStatus Status { get; set; }
        public string Msg { get; set; }
        public object Result { get; set; }
    }

    public enum APIStatus
    {
        Success = 20000,
        Fail = 40000
    }
}
