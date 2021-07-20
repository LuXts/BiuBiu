using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.TeamHub;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuServer.TeamHub
{
    public class TeamHubClient : ITeamHubReceiver
    {
        private ITeamHub client;

        public async Task ConnectAsync(GrpcChannel grpcChannel, ulong teamId)
        {
            client = await StreamingHubClient.ConnectAsync<ITeamHub, ITeamHubReceiver>(grpcChannel, this);
            await client.JoinAsync(new TeamInfo() { TeamId = teamId });
        }

        public void ServerSendMessage(Message message)
        {
            client.SendMessage(message);
        }

        public void ServerAddUser(UserInfo userInfo)
        {
            client.AddUser(userInfo);
        }

        public void ServerDelUser(UserInfo userInfo)
        {
            client.DelUser(userInfo);
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

        public void AddUser(UserInfo userInfo)
        {
        }

        public void DelUser(UserInfo userInfo)
        {
        }
    }
}