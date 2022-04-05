using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;
using static PoorMessage;

public class ServerManager : MonoBehaviour, INetEventListener, INetLogger
{

    [Header("Références")]
    public GameObject networkPlayer;

    private NetManager _netServer;
    private NetPeer _ourPeer;
    private NetDataWriter _dataWriter;

    public Dictionary<int,NetPeer> _allClientsPeer = new Dictionary<int, NetPeer>();

    
    //#if UNITY_SERVER

    private int portNumber = 9050;

    public bool serverOnUnity;
    // Start is called before the first frame update
    void Start()
    {
        #if !UNITY_SERVER
        gameObject.SetActive(false);
#endif

        if (serverOnUnity)
        {
            gameObject.SetActive(true);
        }

        NetDebug.Logger = this;
        _dataWriter = new NetDataWriter();
        _netServer = new NetManager(this);
        _netServer.Start(portNumber);
        _netServer.BroadcastReceiveEnabled = true;
        _netServer.UpdateTime = 15;

        Debug.Log("Server");
    }

    // Update is called once per frame
    void Update()
    {
        _netServer.PollEvents();

    }

    public void FixedUpdate()
    {
        foreach(NetPeer client in _allClientsPeer.Values)
        {
            Debug.Log("send");
            _dataWriter.Reset();
            _dataWriter.Put("Send");
            _netServer.SendToAll(_dataWriter, DeliveryMethod.ReliableOrdered);
        }
    }

    void INetEventListener.OnConnectionRequest(ConnectionRequest request)
    {
        if(_netServer.ConnectedPeersCount < 5)
        {
            request.Accept();
        }
        else
        {
            request.Reject();
        }
        
    }

    void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        
    }

    void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        
    }

    void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        switch (reader.GetByte())
        {
            case (byte)MessageType.CreatePlayer:
                Debug.Log(reader.GetString());
                break;
            case (byte)MessageType.MoovePlayer:
                break;
        }
    }

    void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {

    }

    void INetEventListener.OnPeerConnected(NetPeer peer)
    {
        NetDataWriter writer = new NetDataWriter();
        Debug.Log("[SERVER] We have new peer " + peer.EndPoint);
        _allClientsPeer.Add(peer.Id,peer);
        _dataWriter.Reset();
        _dataWriter.Put((byte)MessageType.ClientConnected);
        peer.Send(_dataWriter,DeliveryMethod.ReliableOrdered);

    }

    void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[SERVER] We have Lost peer " + disconnectInfo);
    }


    void INetLogger.WriteNet(NetLogLevel level, string str, params object[] args)
    {
        
    }

    public void StopServer()
    {
        _netServer.Stop();
    }

    //#endif
}

