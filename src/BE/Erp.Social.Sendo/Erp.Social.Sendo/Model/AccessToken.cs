namespace Erp.Social.Sendo.Model
{
    public class AccessToken
    {
        public int id { get; set; }

        public int did { get; set; }

        public string token { get; set; }

        public DateTime? expires { get; set; }

        public string? success { get; set; }

        public string? error { get; set; }

        public  int? status_code { get; set; }

        public string? params_one{ get; set; }

        public int? track_id { get; set; }
    }
}
