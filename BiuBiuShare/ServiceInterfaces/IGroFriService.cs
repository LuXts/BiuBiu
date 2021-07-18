using System.Collections.Generic;
using BiuBiuShare.GrouFri;
using MagicOnion;

namespace BiuBiuShare.ServiceInterfaces
{
    public interface IGroFriService : IService<IGroFriService>
    {
        UnaryResult<bool> AddFriend(ulong senderId, ulong receiverId, string invitationMessage);

        UnaryResult<bool> AddGroup(ulong senderId, ulong groupId, string invitationMessage);

        UnaryResult<bool> InviteUserToGroup(ulong senderId, ulong receiverId, ulong groupId, string invitationMessage);

        UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId);

        UnaryResult<List<GroupInvitation>> GetGroupInvitation(ulong userId);

        UnaryResult<List<GroupRequest>> GetGroupRequest(ulong useId);

        UnaryResult<bool> ReplyFriendRequest(ulong requestId, bool replyResult);

        UnaryResult<bool> ReplyGroupInvitation(ulong invitationId, bool replyResult);

        UnaryResult<bool> ReplyGroupIRequest(ulong requestId, bool replyResult);

        UnaryResult<bool> ExitGroup(ulong userId, ulong groupId);

        UnaryResult<bool> DissolveGroup(ulong userId, ulong groupId);

        UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId);

        UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong memberId, ulong groupId);
    }
}