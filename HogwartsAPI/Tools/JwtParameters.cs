using System.Runtime.CompilerServices;

namespace HogwartsAPI.Tools
{
    public class JwtParameters
    {
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }
        public string JwtKey { get; set; }
    }
}
