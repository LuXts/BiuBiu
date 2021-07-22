using System;
using System.Net.Http;
using System.Threading;
using BiuBiuWpfClient.OnlineHub;
using BiuBiuWpfClient.Tools;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuWpfClient
{
    public class Initialization
    {
        public readonly static string GrpcIp = "127.0.0.1";

        public readonly static string GrpcPort = ":5001";

        public static IClientFilter ClientFilter;

        public static GrpcChannel GChannel;

        public static OnlineHubClient OnlineHub = new OnlineHubClient();

        public static readonly NLog.Logger Logger
            = NLog.LogManager.GetCurrentClassLogger();

        public static LiteDBDriven LiteDb = new LiteDBDriven();

        public static DataDriven DataDb = new DataDriven();

        public Initialization()
        {
            var httpClientHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpClientHandler.ServerCertificateCustomValidationCallback
                = HttpClientHandler
                    .DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(httpClientHandler);

            GChannel = GrpcChannel.ForAddress("https://" + GrpcIp + GrpcPort
                , new GrpcChannelOptions { HttpClient = httpClient });
        }
    }
}