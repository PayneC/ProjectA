using UnityEngine;
using SuperSocket.ProtoBase;
using System.IO;
using System;

namespace NetWork
{
    public class MsgReceiveFilter : IReceiveFilter<MsgPackageInfo>
    {
        public int LeftBufferSize { get; private set; }
        public IReceiveFilter<MsgPackageInfo> NextReceiveFilter { get; private set; }
        public FilterState State { get; private set; }

        public MsgPackageInfo Filter(BufferList data, out int rest)
        {
            BufferStream _buffer = new BufferStream();
            _buffer.Initialize(data);
            ushort _type = _buffer.ReadUInt16(true);
            ushort _size = _buffer.ReadUInt16(true);

            rest = data.Total - _size - 4;


            MsgPackageInfo requestInfo = null;
            
            
            byte[] bytes = new byte[_size];
            bool _canRead = _buffer.CanRead;
            _buffer.Read(bytes, 0, _size);
            //
            MemoryStream _deserialize = new MemoryStream();
            _deserialize.SetLength(0L);
            _deserialize.Position = 0;
            _deserialize.Write(bytes, 0, _size);
            _deserialize.Position = 0;
            requestInfo = new MsgPackageInfo(_type, _deserialize);

            return requestInfo;
        }

        public void Reset()
        {
            NextReceiveFilter = null;
            State = FilterState.Normal;
        }
    }
}