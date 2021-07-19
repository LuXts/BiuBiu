using System;
using System.Collections.Generic;
using System.Numerics;
using Grpc.Net.Client;
using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.OnlineHub;
using MagicOnion.Client;

namespace BiuBiuTerminalClient
{
    public class OnlineHubClient : IOnlineHubReceiver
    {
        private Dictionary<ulong, UserInfo> users = new Dictionary<ulong, UserInfo>();

        private IOnlineHub client;

        public async Task<UserInfo> ConnectAsync(GrpcChannel grpcChannel, UserInfo userInfo)
        {
            client = await StreamingHubClient.ConnectAsync<IOnlineHub, IOnlineHubReceiver>(grpcChannel, this);

            var userInfos = await client.JoinAsync(userInfo);
            foreach (var info in userInfos)
            {
                Console.WriteLine("User List:{0},{1}", info.UserId, info.DisplayName);
            }

            return users[userInfo.UserId];
        }

        public void OnJoin(UserInfo user)
        {
            Console.WriteLine("OnJoin");
            Console.WriteLine(user.UserId);
            Console.WriteLine(user.DisplayName);
            users[user.UserId] = user;
        }

        public void OnLeave(UserInfo user)
        {
            Console.WriteLine("OnLeave");
            Console.WriteLine(user.UserId);
            Console.WriteLine(user.DisplayName);
            if (users.TryGetValue(user.UserId, out user))
            {
                users.Remove(user.UserId, out user);
            }
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