using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.UserHub;
using Grpc.Net.Client;
using MagicOnion.Client;
using System;
using System.Threading.Tasks;

namespace BiuBiuWpfClient.Userhub
{
    public class UserHubClient : IUserHubReceiver
    {
        private IUserHub client;

        public async Task ConnectAsync(GrpcChannel grpcChannel, ulong userId)
        {
            client = await StreamingHubClient.ConnectAsync<IUserHub, IUserHubReceiver>(grpcChannel, this);
            await client.JoinAsync(new UserInfo() { UserId = userId });
        }

        public Task DisposeAsync()
        {
            return client.DisposeAsync();
        }

        public Task WaitForDisconnect()
        {
            return client.WaitForDisconnect();
        }

        public event Action<Message> SMEvent;

        public event Action<FriendRequest> SFTEvent;

        public event Action<TeamInvitation> SGIEvent;

        public event Action<TeamRequest> SGREvent;

        public void SendMessage(Message message)
        {
            SMEvent?.Invoke(message);
        }

        public void SendFriendRequest(FriendRequest request)
        {
            SFTEvent?.Invoke(request);
        }

        public void SendGroupInvitation(TeamInvitation invitation)
        {
            SGIEvent?.Invoke(invitation);
        }

        public void SendGroupRequest(TeamRequest request)
        {
            SGREvent?.Invoke(request);
        }
    }
}