using System;

public struct RelayJoinData
{
    public string joinCode;
    public string ipv4Address;
    public ushort port;
    public Guid allocationID;
    public byte[] allocationIDBytes;
    public byte[] connectionData;
    public byte[] hostConnectionData;
    public byte[] key;
}