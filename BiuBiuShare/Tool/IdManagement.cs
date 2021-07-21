using System;

namespace BiuBiuShare.Tool
{
    public enum IdType : uint
    {
        UserId = 1
        , TeamId = 2
        , ModifyId = 3
        , FriendRequestId = 4
        , TeamRequestId = 5
        , TeamInvitationId = 6
        , MessageId = 7
        , IconId = 8
        , FriendRelationId = 9
        , TeamRelationId = 10
    }

    public class IdManagement
    {
        private static uint _index = 0;
        private static uint _typeId = 0;
        private static ulong _timestampId = 0;
        private static uint _indexId = 0;
        private static uint _expandId = 0;
        private const uint _timestampIdBits = 44;
        private const uint _typeIdBits = 8;
        private const uint _indexIdBits = 10;
        private const uint _expandIdBits = 2;
        private const uint _idBits = 64;

        public static ulong TimeGen()
        {
            return (ulong)new DateTimeOffset(DateTime.UtcNow)
                .ToUnixTimeMilliseconds();
        }

        public static ulong GenerateId(IdType dataType)
        {
            _timestampId = TimeGen() << 20;
            _typeId = (uint)dataType << 12;
            _indexId = _index << 2;
            _index = (_index + 1) % 1024;
            return _timestampId + _typeId + _indexId + _expandId;
        }

        public static ulong GenTsById(ulong Id)
        {
            return Id >> (int)(_idBits - _timestampIdBits);
        }

        public static ulong GenIdByTs(ulong timestamp)
        {
            return timestamp << (int)(_idBits - _timestampIdBits);
        }

        public static IdType GenerateIdTypeById(ulong id)
        {
            id = id >> (int)(_expandIdBits + _indexIdBits);
            id = id % (ulong)Math.Pow(2, _typeIdBits);
            return (IdType)id;
        }

        public static string GenerateStrById(ulong id)
        {
            ulong oldTime = id >> 20;

            ulong newTime = TimeGen();

            DateTime dtStart
                = TimeZone.CurrentTimeZone.ToLocalTime(
                    new DateTime(1970, 1, 1));
            ulong lTime = oldTime * 10000;
            TimeSpan toNow = new TimeSpan((long)lTime);
            DateTime targetDt = dtStart.Add(toNow);
            if (newTime - oldTime >= 3600 * 1000 * 24)
            {
                return targetDt.ToString("MM-dd");
            }
            else
            {
                return targetDt.ToString("t");
            }
        }
    }
}