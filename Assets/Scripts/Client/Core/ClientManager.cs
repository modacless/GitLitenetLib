using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using System.Net;
using System.Net.Sockets;
using static PoorMessage;
using LiteNetLib.Utils;

public class ClientManager : MonoBehaviour, INetEventListener
{
    [Header("Références")]
    public GameObject networkPlayer;

    private NetManager _netClient;
    private NetPeer _ourPeer;
    private NetDataWriter _dataWriter;


    EventBasedNetListener listener = new EventBasedNetListener();

    public bool serverOnUnity;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_SERVER
        gameObject.SetActive(false);
#endif
        if (serverOnUnity)
        {
            gameObject.SetActive(false);
        }
        _dataWriter = new NetDataWriter();
        _netClient = new NetManager(listener);
        _netClient.UnconnectedMessagesEnabled = true;
        _netClient.UpdateTime = 15;
        _netClient.Start();
        _netClient.Connect("localhost", 9050, "");

        Debug.Log("Start Client");
    }

    // Update is called once per frame
    void Update()
    {
        _ourPeer = _netClient.FirstPeer;
        _netClient.PollEvents();

        if(_ourPeer != null && _ourPeer.ConnectionState == ConnectionState.Connected)
        {
            
        }

    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        Debug.Log(reader.GetString());
        switch (reader.GetByte())
        {
            case (byte) MessageType.ClientConnected:
                Debug.Log("[CLIENT] We connected to " + peer.EndPoint);
                MessageType createMessage = MessageType.CreatePlayer;
                CreatePlayer createPlayer = new CreatePlayer();
                createPlayer.pseudo = "Player";
                _dataWriter.Reset();
                _dataWriter.Put((byte)createMessage);
                _dataWriter.Put(createPlayer);

                peer.Send(_dataWriter, DeliveryMethod.ReliableOrdered);
                Debug.Log("Send");
                break;
        }

    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        
    }

    public void OnPeerConnected(NetPeer peer)
    {
      
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        
    }


}
