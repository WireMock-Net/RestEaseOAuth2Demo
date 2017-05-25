using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestEase.OAuth2.ConsoleTest
{
    public interface IOAuth2Api
    {
        [Post]
        Task<OAuth2Model> AuthenticateAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}