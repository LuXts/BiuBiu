using BiuBiuServer.Services;
using Grpc.Net.Client;
using System.Net.Http;

namespace BiuBiuServer
{
    public class Initialization
    {
        public readonly static string GrpcAddress
            = "https://127.0.0.1:5001";

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

            GChannel = GrpcChannel.ForAddress(GrpcAddress
                , new GrpcChannelOptions { HttpClient = httpClient });
            for (uint i = 0; i < 2000; i++)
            {
                TalkService.PortList.AddLast(i);
            }
        }
    }
}