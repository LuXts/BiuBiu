using System.Runtime.CompilerServices;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    public interface IGroFriDatabaseDriven
    {
        UnaryResult<int> WriteFriendRequest(ulong senderId, ulong receiverId, string invitation);
        UnaryResult<int> WriteGroupRequest(ulong senderId, ulong groupId, string invitation);
        UnaryResult<int> WriteGroupInvitation(ulong senderId, ulong receiver, ulong groupId, string invitation);
    }
}