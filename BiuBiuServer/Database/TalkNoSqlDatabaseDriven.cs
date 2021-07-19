using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BiuBiuServer.Interfaces;
using BiuBiuShare.SignIn;
using BiuBiuShare.TalkInfo;
using MagicOnion;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BiuBiuServer.Database
{
    public class TalkNoSqlDatabaseDriven : ITalkNoSqlDatabaseDriven
    {
        // TODO: 正式上线的时候把 test 数据库换掉
        private readonly IMongoDatabase _database
            = MySqlDriven.GetNoSqlClient().GetDatabase("test");

        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IGridFSBucket _bucket;

        public TalkNoSqlDatabaseDriven()
        {
            _collection = _database.GetCollection<BsonDocument>("Message");
            // TODO: 正式上线的时候把 FileTest.dat 换掉
            _bucket = new GridFSBucket(_database
                , new GridFSBucketOptions
                {
                    BucketName = "FileTest.dat"
                    ,
                    WriteConcern = WriteConcern.WMajority
                    ,
                    ReadPreference = ReadPreference.Secondary
                });
        }

        // MessageResponse 和 BsonDocument 转换接口

        private static BsonDocument MessageToBsonDocument(
            Message message)
        {
            var item = new BsonDocument()
            {
                {"MessageId", message.MessageId.ToString()}
                , {"SourceId", message.SourceId.ToString()}
                , {"TargetId", message.TargetId.ToString()}
                , {"Type", message.Type}
                , {"Data", message.Data}
            };
            return item;
        }

        private static Message BsonDocumentToMessage(
            BsonDocument document)
        {
            var temp = new Message()
            {
                MessageId = Convert.ToUInt64(document["MessageId"].AsString)
                ,
                Data = document["Data"].AsString
                ,
                SourceId = Convert.ToUInt64(document["SourceId"].AsString)
                ,
                TargetId = Convert.ToUInt64(document["TargetId"].AsString)
                ,
                Type = document["Type"].AsString
            };
            return temp;
        }

        public async UnaryResult<MessageResponse> AddMessageAsync(Message message)
        {
            // HACK: 这里用了比较硬的编码
            var item = MessageToBsonDocument(message);
            // HACK: 这里的处理非常生硬
            try
            {
                await _collection.InsertOneAsync(item);
                return new MessageResponse(message) { Success = true };
            }
            catch (Exception e)
            {
                Initialization.Logger.Error(e.Message);
                return MessageResponse.Failed;
            }
        }

        public async UnaryResult<MessageResponse> GetMessagesAsync(
            Message message)
        {
            var filter
                = Builders<BsonDocument>.Filter.Eq("MessageId"
                    , message.MessageId.ToString());
            var document = await _collection.Find(filter).FirstAsync();
            // HACK: 这里的处理非常生硬
            try
            {
                var temp = BsonDocumentToMessage(document);
                return new MessageResponse(temp);
            }
            catch (Exception e)
            {
                Initialization.Logger.Error(e.Message);
                return MessageResponse.Failed;
            }
        }

        public async UnaryResult<MessageResponse> SendDataMessage(Message message
            , uint port)
        {
            // HACK: 这里的处理非常生硬

            try
            {
                TcpListener listener
                    = new TcpListener(IPAddress.Any, (int)port);
                listener.Start();

                var client = await listener.AcceptTcpClientAsync();
                NetworkStream ns = client.GetStream();
                if (ns.DataAvailable)
                {
                    await _bucket.UploadFromStreamAsync(
                        message.MessageId.ToString() + ".tar", ns);

                    ns.Close();
                    client.Close();
                    listener.Stop();
                    return new MessageResponse(message) { Success = true };
                }
                else
                {
                    ns.Close();
                    client.Close();
                    listener.Stop();
                    return MessageResponse.Failed;
                }
            }
            catch (Exception e)
            {
                Initialization.Logger.Error(e.Message);
                return MessageResponse.Failed;
            }
        }

        public async UnaryResult<MessageResponse> GetDataMessage(Message message
            , uint port)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, (int)port);
                listener.Start();

                var client = await listener.AcceptTcpClientAsync();
                NetworkStream ns = client.GetStream();

                var filter = Builders<GridFSFileInfo>.Filter.And(
                    Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename
                        , message.MessageId.ToString() + ".tar"));
                var cursor = await _bucket.Find(filter).FirstOrDefaultAsync();
                await _bucket.DownloadToStreamAsync(cursor.Id, ns);
                ns.Close();
                client.Close();
                listener.Stop();
                return new MessageResponse(message) { Success = true };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return MessageResponse.Failed;
            }
        }

        public async UnaryResult<List<Message>> GetMessagesRecordAsync(
            ulong targetId, ulong startTime, ulong endTime)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("TargetId", targetId.ToString()) &
                         filterBuilder.Gt("MessageId", startTime.ToString()) &
                         filterBuilder.Lt("MessageId", endTime.ToString());

            var cursor = await _collection.Find(filter).ToCursorAsync();

            List<Message> list = new List<Message>();
            foreach (var document in cursor.ToEnumerable())
            {
                list.Add(BsonDocumentToMessage(document));
            }

            return list;
        }

        public async UnaryResult<List<Message>>
            GetChatMessagesRecordAsync(ulong sourceId, ulong targetId
                , ulong startTime, ulong endTime)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter
                = ((filterBuilder.Eq("TargetId", targetId.ToString()) &
                    filterBuilder.Eq("SourceId", sourceId.ToString())) |
                   (filterBuilder.Eq("SourceId", targetId.ToString()) &
                    filterBuilder.Eq("TargetId", sourceId.ToString()))) &
                  filterBuilder.Gt("MessageId", startTime.ToString()) &
                  filterBuilder.Lt("MessageId", endTime.ToString());

            var cursor = await _collection.Find(filter).ToCursorAsync();

            List<Message> list = new List<Message>();
            foreach (var document in cursor.ToEnumerable())
            {
                list.Add(BsonDocumentToMessage(document));
            }

            return list;
        }
    }
}