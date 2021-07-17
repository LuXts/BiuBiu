using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using MagicOnion;

namespace BiuBiuServer.Tests
{
    public class GroFriDatabasesDrivenTest : IGroFriDatabaseDriven
    {
        public UnaryResult<int> WriteFriendRequest(ulong senderId, ulong receiverId, string invitation)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<int> WriteGroupInvitation(ulong senderId, ulong receiver, ulong groupId, string invitation)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<int> WriteGroupRequest(ulong senderId, ulong groupId, string invitation)
        {
            throw new System.NotImplementedException();
        }
    }
}