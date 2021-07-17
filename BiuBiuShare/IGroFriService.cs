using MagicOnion;

namespace BiuBiuShare
{
    public interface IGroFriService:IService<IGroFriService>
    {
        UnaryResult<int> AddFriend(ulong senderId ,ulong receiverId,string invitationMessage);
        UnaryResult<int> AddGroup(ulong senderId ,ulong groupId,string invitationMessage);
        UnaryResult<int> InviteUserToGroup(ulong senderId ,ulong receiverId,ulong groupId,string invitationMessage);
    }
}
