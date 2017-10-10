namespace Redack.ServiceLayer.Models
{
    public class TokenResponse : Model
    {
        public string Access { get; set; }
        public string Refresh { get; set; }
    }
}