using System.Collections.Generic;
using BiuBiuShare.GrouFri;
using MagicOnion;
using Microsoft.AspNetCore.SignalR;

namespace BiuBiuShare
{
    public interface IGroFriService:IService<IGroFriService>
    {
        UnaryResult<int> AddFriend(ulong senderId ,ulong receiverId,string invitationMessage);
        UnaryResult<int> AddGroup(ulong senderId ,ulong groupId,string invitationMessage);
        UnaryResult<int> InviteUserToGroup(ulong senderId ,ulong receiverId,ulong groupId,string invitationMessage);
        UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId);
        UnaryResult<List<GroupInvitation>> GetFriendInvitation(ulong userId);
        UnaryResult<bool> ReplyFriendRequest(ulong requestId,string replyResult);
        UnaryResult<bool> ReplyGroupInvitation(ulong invitationId,string replyResult);
        UnaryResult<bool> ReplyGroupIRequest(ulong requestId,string replyResult);
        UnaryResult<bool> ExitGroup(ulong userId,ulong groupId); 
         UnaryResult<bool> DissolveGroup(ulong userId, ulong groupId);
        UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId );
        UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId,ulong memberId,ulong groupId);

    }
}
