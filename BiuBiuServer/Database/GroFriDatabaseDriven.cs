using BiuBiuServer.Interfaces;
using BiuBiuShare;
using MagicOnion;

namespace BiuBiuServer.Database
{
    public class GroFriDatabaseDriven:IGroFriDatabaseDriven
    {
        //TODO:函数功能：输入申请者ID，被添加者ID，申请信息，写入数据库中；输出：返回是否成功写入 tip:当同一个人邀请信息重复发送时，不重复写入,返回失败
        public async UnaryResult<int> WriteFriendRequest(ulong senderId, ulong receiverId, string invitation)
        {
            throw new System.NotImplementedException();
        }
        //TODO:实现某人邀请某人加入某群组数据写入，输入邀请者ID，被邀请者ID，邀请备注信息，写入数据库中；输出：返回是否成功写入 tip:邀请人必须为群主，不可重复发送
        public async UnaryResult<int> WriteGroupInvitation(ulong senderId, ulong receiver, ulong groupId, string invitation)
        {
            throw new System.NotImplementedException();
        }
        //TODO：实现某人申请加入某群聊数据写入 输入申请者ID，群组ID，申请信息 tip:不可重复写入
        public async UnaryResult<int> WriteGroupRequest(ulong senderId, ulong groupId, string invitation)
        {
            throw new System.NotImplementedException();
        }
    }
}