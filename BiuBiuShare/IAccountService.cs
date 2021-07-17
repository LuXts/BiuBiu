using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuShare
{
    public interface IAccountService : IService<IAccountService>
    {
        UnaryResult<SignInResponse> CommonSignInAsync(string signInId
            , string password);

        UnaryResult<SignInResponse> AdministrantSignInAsync(
            string signInId, string password);
    }
}