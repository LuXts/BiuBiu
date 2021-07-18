using System;

namespace BiuBiuShare.Tool
{
    public class IdManagement
    {
        private static uint _index = 0;
        private static uint _typeId = 0;
        private static ulong _timeStampId = 0;
        private static uint _indexId = 0;
        private static uint _expandId = 0;
        private const uint _timeStapIdBits = 44;
        private const uint _typeIdBits = 8;
        private const uint _indexIdBits = 10;
        private const uint _expandIdBits = 2;
        private const uint _idBits = 64;

        public static ulong TimeGen()
        {
            return (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }

        public static ulong GenerateId(uint dataType)
        {
            _timeStampId = TimeGen() << (int)(_idBits - _timeStapIdBits);
            _typeId = dataType << (int)(_idBits - _timeStapIdBits - _typeIdBits);
            _indexId = _index << (int)(_idBits - _timeStapIdBits - _typeIdBits - _indexIdBits);
            _index = (_index + 1) % (uint)(Math.Pow(2, _indexIdBits));
            return _timeStampId + _typeId + _indexId + _expandId;
        }
    }
}