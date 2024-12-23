using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace DemoApp.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(string username,string role);
    }
}
