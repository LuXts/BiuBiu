using LiteDB;
using System.IO;

namespace BiuBiuWpfClient.Tools
{
    public class LiteDBDriven
    {
        public class SaveInfo
        {
            public string ID { get; set; }

            public string Account { get; set; }

            public string Password { get; set; }

            public string ServiceIp { get; set; }

            public bool CheckAccount { get; set; }

            public bool CheckPassword { get; set; }
        }

        private static LiteDatabase db = new LiteDatabase("./MyData.db");
        private static ILiteCollection<SaveInfo> siCollection;

        private static SaveInfo results;

        public LiteDBDriven()
        {
            siCollection = db.GetCollection<SaveInfo>("SaveInfo");
            siCollection.EnsureIndex(x => x.ID, true);
        }

        public string GetIp()
        {
            results = siCollection.FindOne(x => x.ID == "17761");

            if (results is null)
            {
                SaveInfo save = new SaveInfo()
                {
                    ID = "17761",
                    Account = "",
                    Password = "",
                    ServiceIp = "127.0.0.1"
                };
                siCollection.Insert(save);
                results = save;
                return "127.0.0.1";
            }
            else
            {
                return results.ServiceIp;
            }
        }

        public void SetIp(string Ip)
        {
            results.ServiceIp = Ip;
            siCollection.Update(results);
        }

        public string GetAccount()
        {
            if (!(results is null))
            {
                return results.Account;
            }
            else
            {
                results.Account = "";
                return "";
            }
        }

        public void SetAccount(string account)
        {
            results.Account = account;
            siCollection.Update(results);
        }

        public string GetPassword()
        {
            if (!(results is null))
            {
                return results.Password;
            }
            else
            {
                results.Password = "";
                return "";
            }
        }

        public void SetPassword(string password)
        {
            results.Password = password;
            siCollection.Update(results);
        }

        public bool GetCheckAccount()
        {
            if (!(results is null))
            {
                return results.CheckAccount;
            }
            else
            {
                results.CheckAccount = false;
                return false;
            }
        }

        public void SetCheckAccount(bool checkAccount)
        {
            results.CheckAccount = checkAccount;
            siCollection.Update(results);
        }

        public bool GetCheckPassword()
        {
            if (!(results is null))
            {
                return results.CheckAccount;
            }
            else
            {
                results.CheckAccount = false;
                return false;
            }
        }

        public void SetCheckPassword(bool checkPassword)
        {
            results.CheckPassword = checkPassword;
            siCollection.Update(results);
        }

        public byte[] LoadImage(ulong imageId)
        {
            var file
                = db.FileStorage.FindById("$/images/" + imageId.ToString() +
                                          ".image");
            if (file is null)
            {
                return new byte[0];
            }
            else
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public void UploadImage(ulong imageId, Stream stream)
        {
            db.FileStorage.Upload("$/images/" + imageId.ToString() + ".image"
                , imageId.ToString() + ".image", stream);
        }
    }
}