using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib.Utils;
using System;

public struct CreatePlayer : INetSerializable
{
    public string pseudo;
    public void Deserialize(NetDataReader reader)
    {
        pseudo = reader.GetString();
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(pseudo);
    }
}

public class PoorMessage : MonoBehaviour
{
    [Serializable]
    public enum MessageType : byte
    {
        ClientConnected = 0,
        CreatePlayer = 1,
        MoovePlayer = 2
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
