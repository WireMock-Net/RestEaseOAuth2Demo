using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestEase.OAuth2.ConsoleTest
{
    public interface IOAuth2Api
    {
        [Post("/oauth2/access")]
        Task<OAuth2Model> AuthenticateAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}
