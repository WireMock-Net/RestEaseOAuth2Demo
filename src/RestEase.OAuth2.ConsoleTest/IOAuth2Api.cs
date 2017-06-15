using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestEase.OAuth2.ConsoleTest
{
    public interface IOAuth2Api
    {
        [Post("oauth2/authorize")]
        Task<OAuth2Model> AuthorizeAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("oauth2/token")]
        Task<OAuth2Model> RefreshTokenAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}