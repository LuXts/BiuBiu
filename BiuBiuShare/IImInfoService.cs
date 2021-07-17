using System.Collections.Generic;
using BiuBiuShare.ImInfos;
using MagicOnion;

namespace BiuBiuShare
{
    public interface IImInfoService : IService<IImInfoService>
    {
        // 获取用户信息
        UnaryResult<UserInfo> GetUserInfo(ulong userId);

        // 获取修改队列的用户信息
        UnaryResult<UserInfo> GetModifyQueueInfo(ulong userId);

        // 设置用户信息 因为 UserInfo 里面已经包含了ID所以不需要给ID了
        UnaryResult<bool> SetUserInfo(UserInfo userInfo);

        // 修改用户密码
        UnaryResult<bool> SetUserPassword(ulong userId, string password);

        // 获取群组信息
        UnaryResult<TeamInfo> GetTeamInfo(ulong teamId);

        // 根据 ID 修改群组信息，因为 TeamInfo 里面已经包括了 ID 所以不需要传递 ID 了
        UnaryResult<bool> SetTeamInfo(TeamInfo teamInfo);

        // 根据 ID 获取群成员信息
        UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId);
    }
}