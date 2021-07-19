using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.SignIn;
using BiuBiuShare.TalkInfo;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuShare.TeamHub
{
    public class TeamHubClient : ITeamHubReceiver
    {
        private ITeamHub client;

        public async Task ConnectAsync(GrpcChannel grpcChannel, ulong teamId)
        {
            var client = StreamingHubClient.Connect<ITeamHub, ITeamHubReceiver>(grpcChannel, this);
            await client.JoinAsync(new TeamInfo() { TeamId = teamId });
        }

        public void SendMessage(Message message)
        {
            client.SendMessage(message);
        }

        public void AddUser(UserInfo userInfo)
        {
            client.AddUser(userInfo);
        }

        public void DelUser(UserInfo userInfo)
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
    }
}