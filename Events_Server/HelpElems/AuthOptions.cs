using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server.HelpElems
{
    public class AuthOptions
    {
        public const string ISSUER = "EventsServer";
        public const string AUDIENCE = "EventsCient";
        const string KEY = "super_sercetKey!321";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
