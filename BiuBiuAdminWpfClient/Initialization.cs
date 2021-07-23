using System;
using System.Net.Http;
using System.Threading;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuAdminWpfClient
{
    public class Initialization
    {
        public readonly static string GrpcIp = "192.168.100.15";

        public readonly static string GrpcPort = ":5001";

        public static IClientFilter ClientFilter;

        public static GrpcChannel GChannel;

        public static readonly NLog.Logger Logger
            = NLog.LogManager.GetCurrentClassLogger();

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