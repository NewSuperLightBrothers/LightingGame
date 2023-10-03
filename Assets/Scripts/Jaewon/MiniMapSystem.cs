using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapSystem : MonoBehaviour
{
    private Transform _camtr;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _yPos =10;
    [SerializeField] private float _adjustmentValue = 1;
    private void Awake()
    {
        _camtr = GetComponent<Transform>();
        _yPos = 10;
    }
    private void FixedUpdate()
    {
        this._camtr.position = new Vector3(_player.transform.position.x, _yPos, _player.transform.position.z);
    }
    public void ZoomIn()
    {
        _yPos = _yPos > 5 ? _yPos - _adjustmentValue : 5;
    }
    public void ZoomOut()
    {
        _yPos = _yPos < 15 ? _yPos + _adjustmentValue : 15;
    }
}