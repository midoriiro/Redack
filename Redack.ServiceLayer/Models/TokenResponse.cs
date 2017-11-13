namespace Redack.ServiceLayer.Models
{
    public class TokenResponse : BaseModel
    {
        public string Access { get; set; }
        public string Refresh { get; set; }
    }
}