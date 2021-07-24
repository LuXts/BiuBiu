using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.UserHub;
using Grpc.Net.Client;
using MagicOnion.Client;
using System.Threading.Tasks;

namespace BiuBiuServer.Userhub
{
    public class UserHubClient : IUserHubReceiver
    {
        private IUserHub client;

        public async Task ConnectAsync(GrpcChannel grpcChannel, ulong userId)
        {
            client = await StreamingHubClient.ConnectAsync<IUserHub, IUserHubReceiver>(grpcChannel, this);
            await client.JoinAsync(new UserInfo() { UserId = userId });
        }

        public void ServerSendMessage(Message message)
        {
            client.SendMessage(message);
        }

        public void ServerSendFriendRequest(FriendRequest request)
        {
            client.SendFriendRequest(request);
        }

        public void ServerSendGroupInvitation(TeamInvitation invitation)
        {
            client.SendGroupInvitation(invitation);
        }

        public void ServerSendGroupRequest(TeamRequest request)
        {
            client.SendGroupRequest(request);
        }

        public Task DisposeAsync()
        {
            return client.DisposeAsync();
        }

        public Task WaitForDisconnect()
        {
            return client.WaitForDisconnect();
        }

        public void SendMessage(Message message)
        {
        }

        public void SendFriendRequest(FriendRequest request)
        {
        }

        public void SendGroupInvitation(TeamInvitation invitation)
        {
        }

        public void SendGroupRequest(TeamRequest request)
        {
        }
    }
}