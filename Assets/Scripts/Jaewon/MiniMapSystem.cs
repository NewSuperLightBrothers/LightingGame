using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapSystem : MonoBehaviour
{
    Transform tr;
    [SerializeField] private GameObject player;
    [SerializeField] private float _yPos =10;
    [SerializeField] private float _adjustmentValue = 1;
    private void Awake()
    {
        tr = GetComponent<Transform>();
        _yPos = 10;
    }
    private void FixedUpdate()
    {
        this.tr.position = new Vector3(player.transform.position.x, _yPos, player.transform.position.z);
    }
    public void ZoonIn()
    {
        _yPos = _yPos > 5 ? _yPos - _adjustmentValue : 5;
    }
    public void ZoonOut()
    {
        _yPos = _yPos < 15 ? _yPos + _adjustmentValue : 15;
    }
}
