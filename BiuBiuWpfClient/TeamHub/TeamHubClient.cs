using System;
using System.Threading.Tasks;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.TeamHub;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuWpfClient.TeamHub
{
    public class TeamHubClient : ITeamHubReceiver
    {
        private ITeamHub client;

        public async Task ConnectAsync(GrpcChannel grpcChannel, ulong teamId)
        {
            client = await StreamingHubClient
                .ConnectAsync<ITeamHub, ITeamHubReceiver>(grpcChannel, this);
            await client.JoinAsync(new TeamInfo() { TeamId = teamId });
        }

        public event Action<Message> SMEvent;

        public event Action<UserInfo> AUEvent;

        public event Action<UserInfo> DUEvent;

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
            SMEvent?.Invoke(message);
        }

        public void AddUser(UserInfo userInfo)
        {
            AUEvent?.Invoke(userInfo);
        }

        public void DelUser(UserInfo userInfo)
        {
            DUEvent?.Invoke(userInfo);
        }
    }
}