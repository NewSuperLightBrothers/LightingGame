using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectEmission : NetworkBehaviour
{
    public List<short> gauges = new();
    public NetworkVariable<EObjectColorType> objectColorType = new();
    public NetworkVariable<float> r = new();
    public NetworkVariable<float> g = new();
    public NetworkVariable<float> b = new();
    public short maxgauge;


      

}
