using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestCameraMove : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    private Vector3 aa;

    private void Start()
    {
        // 자이로스코프 초기화
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        // 디바이스의 자이로스코프 데이터를 가져옴
        Vector3 gyroRotationRate = Input.gyro.rotationRate;

        // 자이로스코프 데이터를 이용하여 회전 방향 계산
        Vector3 rotation = new Vector3(gyroRotationRate.x, gyroRotationRate.y, gyroRotationRate.z);

        // 회전 방향에 따라 오브젝트를 회전시킴
        transform.Rotate(rotation * _rotateSpeed * Time.deltaTime);
    }



}
