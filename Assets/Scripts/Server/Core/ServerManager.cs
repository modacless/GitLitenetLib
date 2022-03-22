using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;

public class ServerManager : INetEventListener, INetLogger
{

    private NetManager _netServer;
    private NetPeer _ourPeer;
    private NetDataWriter _dataWriter;
    
    //#if UNITY_SERVER


    // Start is called before the first frame update
    void Start()
    {
        NetDebug.Logger = this;
        _dataWriter = new NetDataWriter();
        _netServer = new NetManager(this);
        _netServer.Start(5000);
        _netServer.BroadcastReceiveEnabled = true;
        _netServer.UpdateTime = 15;
    }

    // Update is called once per frame
    void Update()
    {
        _netServer.PollEvents();
    }

    void INetEventListener.OnConnectionRequest(ConnectionRequest request)
    {
        
    }

    void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        
    }

    void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        
    }

    void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {

    }

    void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {

    }

    void INetEventListener.OnPeerConnected(NetPeer peer)
    {

    }

    void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {

    }


    void INetLogger.WriteNet(NetLogLevel level, string str, params object[] args)
    {
        throw new System.NotImplementedException();
    }
    //#endif
}

