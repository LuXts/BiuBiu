using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuShare
{
    public interface IAccountService : IService<IAccountService>
    {

        // 参数：电话号码、工号  密码
        // 输出：返回结果和令牌
        UnaryResult<SignInResponse> CommonSignInAsync(string signInId
            , string password);

        UnaryResult<SignInResponse> AdministrantSignInAsync(
            string signInId, string password);
    }
}