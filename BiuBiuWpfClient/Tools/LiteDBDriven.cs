using System.IO;
using LiteDB;

namespace BiuBiuWpfClient.Tools
{
    public class LiteDBDriven
    {
        private LiteDatabase db = new LiteDatabase("./MyData.db");

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