using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using System.Collections.Generic;
using SuperSocket.ProtoBase;
using SuperSocket.ClientEngine;
using ProtoBuf;
using ProtoMessage;

namespace NetWork
{
    public class NetMgr : Singleton<NetMgr>
    {                
        EasyClient m_client = null;

        Dictionary<ushort, NetMsgHandler> NetMsgListeners = new Dictionary<ushort, NetMsgHandler>();
        MemoryStream _memoryStream = new MemoryStream();

        Queue<MsgPackageInfo> _q = new Queue<MsgPackageInfo>();

        public void Initialization()
        {
            m_client = new EasyClient();
            m_client.Connected += OnConnected;
            m_client.Closed += OnClosed;
            m_client.Error += OnError;
            m_client.Initialize<MsgPackageInfo>(new MsgReceiveFilter(), OnReceivePackage);
        }

        public void Uninstall()
        {
            m_client.Close();
        }

        public void ConnectToServer(string ip, int Port)
        {            
            Debug.LogErrorFormat("ConnectToServer({0},{1})", ip, Port);
            EndPoint endPoint = new DnsEndPoint(ip, Port);
            m_client.BeginConnect(endPoint);
        }

        public void OnConnected(object sender, EventArgs e)
        {
            Debug.Log("OnConnected");
            EnterLevel();
        }

        private void EnterLevel()
        {
            REQEnterLevel _req = new REQEnterLevel();
            _req.sceneId = 1;

            SendMessage(NetMessage.EnterLevel, _req);
        }

        public void OnClosed(object sender, EventArgs e)
        {
            Debug.Log("OnClosed");
        }

        public void OnError(object sender, EventArgs e)
        {
            Debug.LogErrorFormat("OnError");
        }

        public bool IsConnected()
        {
            return m_client != null && m_client.IsConnected;
        }

        public void SendMessage<T>(ushort msgType, T body) where T : IExtensible
        {
            Debug.LogErrorFormat("SendMessage({0})", msgType);

            if (!IsConnected())
                return;

            _memoryStream.SetLength(0L);
            _memoryStream.Position = 0;

            Serializer.Serialize<T>(_memoryStream, body);

            _memoryStream.Position = 0;
            //var a = Serializer.Deserialize<T>(_memoryStream);

            ushort _size = (ushort)(Convert.ToUInt16(_memoryStream.Length));

            byte[] _sizeBuffer = BitConverter.GetBytes(_size);
            byte[] _typeBuffer = BitConverter.GetBytes(msgType);
            byte[] _bodyBuffer = _memoryStream.ToArray();

            _memoryStream.Position = 0;
            _memoryStream.Write(_typeBuffer, 0, 2);
            _memoryStream.Write(_sizeBuffer, 0, 2);            
            _memoryStream.Write(_bodyBuffer, 0, _bodyBuffer.Length);

            byte[] _buffer = _memoryStream.ToArray();
            m_client.Send(_buffer);            
        }

        private void OnReceivePackage(MsgPackageInfo package)
        {
            Debug.LogErrorFormat("OnReceivePackage({0})", package.MessageType);

            _q.Enqueue(package);
        }

        public void RegisterMessage(ushort msg, NetMsgHandler handler)
        {
            if (null != handler)
            {
                NetMsgListeners[msg] = handler;
            }
            else
            {
                Debug.LogErrorFormat("RegisterMessage Net Message <{0}> Handler is null", msg);
            }
        }

        public void Update(float dt)
        {
            int i = 0;
            while(_q.Count > 0 && i < 10)
            {                
                MsgPackageInfo package = _q.Dequeue();
                NetMsgHandler func = null;
                if (NetMsgListeners.TryGetValue(package.MessageType, out func) && func != null)
                {
                    func(package.MessageType, package.Body);
                }
                ++i;
            }
        }
    }
}




