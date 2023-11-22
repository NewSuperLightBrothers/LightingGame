using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroSystem : MonoBehaviour
{
    [SerializeField]
    private float _gyroSpeed = 0.5f;
    private void Start()
    {
        Input.gyro.enabled = true;
    }
    private void Update()
    {
        transform.Rotate(Input.gyro.rotationRateUnbiased.x* _gyroSpeed, Input.gyro.rotationRateUnbiased.y * _gyroSpeed, Input.gyro.rotationRateUnbiased.z * _gyroSpeed);
    }
}
