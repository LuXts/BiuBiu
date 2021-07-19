﻿using MessagePack;

namespace BiuBiuShare.GrouFri
{
    [MessagePackObject(true)]
    public class GroupRequest
    {
        public ulong RequestId { get; set; }
        public ulong SenderId { get; set; }//申请者ID
        public ulong GroupId { get; set; }
        public string RequestMessage { get; set; }
        public string RequestResult { get; set; }
    }
}