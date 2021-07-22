using BiuBiuWpfClient.OnlineHub;
using BiuBiuWpfClient.Tools;
using Grpc.Net.Client;
using MagicOnion.Client;
using System.Net.Http;

namespace BiuBiuWpfClient
{
    public class Initialization
    {
        public static string GrpcIp = "127.0.0.1";

        public readonly static string GrpcPort = ":5001";

        public static IClientFilter ClientFilter;

        public static GrpcChannel GChannel;

        public static OnlineHubClient OnlineHub = new OnlineHubClient();

        public static readonly NLog.Logger Logger
            = NLog.LogManager.GetCurrentClassLogger();

        public static LiteDBDriven LiteDb = new LiteDBDriven();

        public static DataDriven DataDb = new DataDriven();

        public static void Init()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback
                = HttpClientHandler
                    .DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(httpClientHandler);
            Initialization.Logger.Debug("https://" + GrpcIp + GrpcPort);
            GChannel = GrpcChannel.ForAddress("https://" + GrpcIp + GrpcPort
                , new GrpcChannelOptions { HttpClient = httpClient });
        }
    }
}