using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using BiuBiuShare.GrouFri;
using MagicOnion;
using System.Collections.Generic;

namespace BiuBiuServer.Tests
{
    public class GroFriDatabasesDrivenTest : IGroFriDatabaseDriven
    {
        public async  UnaryResult<bool> DeleteFriend(ulong sponsorId, ulong targetId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> DeleteMemberFromGroup(ulong sponsorId, ulong targetId, ulong groupId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> DissolveGroup(ulong sponsorId, ulong groupId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ExitGroup(ulong sponsorId, ulong groupId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<List<FriendRequest>> GetFriendRequest(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<List<GroupInvitation>> GetGroupInvitation(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ReplyFriendRequest(ulong requestId, string replyResult)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ReplyGroupInvitation(ulong invitationId, string replyResult)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ReplyGroupRequest(ulong requestId, string replyResult)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<int> WriteFriendRequest(ulong friendRequestId,ulong senderId, ulong receiverId, string invitation)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<int> WriteGroupInvitation(ulong groupInvitatiobId, ulong senderId, ulong receiver, ulong groupId, string invitation)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<int> WriteGroupRequest(ulong groupRequestId,ulong senderId, ulong groupId, string invitation)
        {
            throw new System.NotImplementedException();
        }
    }
}