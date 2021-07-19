using System.Threading.Tasks;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.SignIn;
using BiuBiuShare.TalkInfo;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuShare.UserHub
{
    public class UserHubClient : IUserHubReceiver
    {
        private IUserHub client;

        public async Task ConnectAsync(GrpcChannel grpcChannel, ulong userId)
        {
            client = await StreamingHubClient.ConnectAsync<IUserHub, IUserHubReceiver>(grpcChannel, this);
            await client.JoinAsync(new UserInfo() { UserId = userId });
        }

        public void SendMessage(Message message)
        {
            client.SendMessage(message);
        }

        public void SendFriendRequest(FriendRequest request)
        {
            client.SendFriendRequest(request);
        }

        public void SendGroupInvitation(TeamInvitation invitation)
        {
            client.SendGroupInvitation(invitation);
        }

        public void SendGroupRequest(TeamRequest request)
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
    }
}