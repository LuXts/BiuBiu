using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.OnlineHub;
using BiuBiuWpfClient.Login;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuWpfClient.OnlineHub
{
    public class OnlineHubClient : IOnlineHubReceiver
    {
        public Dictionary<ulong, UserInfo> OnlineUserDictionary = new Dictionary<ulong, UserInfo>();

        private IOnlineHub client;

        public async Task<UserInfo> ConnectAsync(GrpcChannel grpcChannel, UserInfo userInfo)
        {
            client = await StreamingHubClient.ConnectAsync<IOnlineHub, IOnlineHubReceiver>(grpcChannel, this);

            var userInfos = await client.JoinAsync(userInfo);

            int count = 0;

            foreach (var user in userInfos)
            {
                if (user.UserId == userInfo.UserId)
                {
                    count++;
                }
                else
                {
                    OnlineUserDictionary.Add(user.UserId, user);
                }

                if (count >= 2)
                {
                    return null;
                }
            }

            return new UserInfo();
        }

        public event Action<UserInfo> OJEvent;

        public void OnJoin(UserInfo user)
        {
            Initialization.Logger.Debug(user.UserId);
            if (user.UserId != AuthenticationTokenStorage.UserId)
            {
                OnlineUserDictionary[user.UserId] = user;
                OJEvent?.Invoke(user);
            }
        }

        public event Action<UserInfo> OLEvent;

        public void OnLeave(UserInfo user)
        {
            if (OnlineUserDictionary.TryGetValue(user.UserId, out user))
            {
                OnlineUserDictionary.Remove(user.UserId, out user);
                OLEvent?.Invoke(user);
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