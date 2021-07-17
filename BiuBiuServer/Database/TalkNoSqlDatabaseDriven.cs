using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using BiuBiuServer.Interfaces;
using BiuBiuShare.Response;
using MagicOnion;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BiuBiuServer.Database
{
    public class TalkNoSqlDatabaseDriven : ITalkNoSqlDatabaseDriven
    {
        private readonly IMongoDatabase _database
            = MySqlDriven.GetNoSqlClient().GetDatabase("test");

        private readonly IMongoCollection<BsonDocument> _collection;

        public TalkNoSqlDatabaseDriven()
        {
            _collection = _database.GetCollection<BsonDocument>("Message");
        }

        public async UnaryResult<bool> AddMessageAsync(MessageResponse message)
        {
            // HACK: 这里用了比较硬的编码
            var item = new BsonDocument()
            {
                {"MessageId", message.MessageId.ToString()}
                , {"SourceId", message.SourceId.ToString()}
                , {"TargetId", message.TargetId.ToString()}
                , {"Type", message.Type}
                , {"Data", message.Data}
            };
            // HACK: 这里的处理非常生硬
            try
            {
                await _collection.InsertOneAsync(item);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // TODO: 打日志
                return false;
            }
        }

        public async UnaryResult<MessageResponse> GetMessagesAsync(
            ulong messageId)
        {
            var filter
                = Builders<BsonDocument>.Filter.Eq("MessageId"
                    , messageId.ToString());
            var document = await _collection.Find(filter).FirstAsync();
            // HACK: 这里的处理非常生硬
            try
            {
                var temp = new MessageResponse()
                {
                    MessageId
                        = Convert.ToUInt64(document["MessageId"].AsString)
                    ,
                    Data = document["Data"].AsString
                    ,
                    SourceId
                        = Convert.ToUInt64(document["SourceId"].AsString)
                    ,
                    Success = true
                    ,
                    TargetId
                        = Convert.ToUInt64(document["TargetId"].AsString)
                    ,
                    Type = document["Type"].AsString
                };
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // TODO: 打日志
                return MessageResponse.Failed;
            }
        }

        public async UnaryResult<bool> SendDataMessage(MessageResponse message
            , int port)
        {
            // HACK: 这里的处理非常生硬
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                var client = listener.AcceptTcpClient();
                NetworkStream ns = client.GetStream();

                var bucket = new GridFSBucket(_database
                    , new GridFSBucketOptions
                    {
                        BucketName = "FileTest.dat"
                        ,
                        WriteConcern = WriteConcern.WMajority
                        ,
                        ReadPreference = ReadPreference.Secondary
                    });
                if (ns.DataAvailable)
                {
                    bucket.UploadFromStream(
                        message.MessageId.ToString() + ".tar", ns);

                    ns.Close();
                    client.Close();
                    listener.Stop();
                    return true;
                }
                else
                {
                    ns.Close();
                    client.Close();
                    listener.Stop();
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async UnaryResult<bool> GetDataMessage(MessageResponse message
            , int port)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                var client = listener.AcceptTcpClient();
                NetworkStream ns = client.GetStream();

                IGridFSBucket bucket = new GridFSBucket(_database
                    , new GridFSBucketOptions
                    {
                        BucketName = "FileTest.dat"
                        ,
                        WriteConcern = WriteConcern.WMajority
                        ,
                        ReadPreference = ReadPreference.Secondary
                    });
                var filter = Builders<GridFSFileInfo>.Filter.And(
                    Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename
                        , message.MessageId.ToString() + ".tar"));
                var cursor = await bucket.Find(filter).FirstOrDefaultAsync();
                bucket.DownloadToStream(cursor.Id, ns);
                ns.Close();
                client.Close();
                listener.Stop();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}