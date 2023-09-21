using Unity.Collections;
using Unity.Netcode;


public struct NetworkString : INetworkSerializable
{
    private FixedString32Bytes _string;

    public NetworkString(string inputString)
    {
        _string = inputString;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _string);
    }

    public override string ToString()
    {
        return _string.ToString();
    }

    public static implicit operator string(NetworkString s) => s.ToString();

    public static implicit operator NetworkString(string s) =>
        new() { _string = new FixedString32Bytes(s) };
}
