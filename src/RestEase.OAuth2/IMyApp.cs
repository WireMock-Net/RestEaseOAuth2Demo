using System.Threading.Tasks;
using RestEase;

namespace RestEaseOAuth2Test
{
    public interface IMyApp
    {
        [Get("helloworld")]
        [Header("Authorization", "Bearer")]
        Task<string> GetHelloAsync();
    }
}