namespace anyhelp.Service.Models
{
    public class TokenDetail
    {
      
        public string mobilenumber { get; set; }

        public int nbf { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
    }
}