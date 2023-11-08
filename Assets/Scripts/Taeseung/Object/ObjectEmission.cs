using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectEmission : NetworkBehaviour
{
    public struct NetworkColor:INetworkSerializable
    {
        public NetworkVariable<float> r;
        public NetworkVariable<float> g;
        public NetworkVariable<float> b;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            throw new System.NotImplementedException();
        }
    }

      public Dictionary<EObjectColorType, short> gauge;
      public NetworkVariable<EObjectColorType> objectColorType;
      public NetworkVariable<NetworkColor> color;
      public NetworkVariable<short> maxgauge;
}
