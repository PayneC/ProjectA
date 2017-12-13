using UnityEngine;
using System.IO;
using SuperSocket.ProtoBase;

namespace NetWork
{
    public class MsgPackageInfo : IPackageInfo
    {        
        public ushort MessageType;
        public Stream Body { get; private set; }

        public MsgPackageInfo(ushort _type, Stream _body)
        {
            MessageType = _type;
            Body = _body;
        }
    }
}


