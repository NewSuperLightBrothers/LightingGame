using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraMove : MonoBehaviour
{

    [SerializeField]
    private float RotateSpeed;

    void Start()
    {
        // ���̷ν����� �ʱ�ȭ
        Input.gyro.enabled = true;
    }
void Update()
    {
        // ����̽��� ���̷ν����� �����͸� ������
        Vector3 gyroRotationRate = Input.gyro.rotationRate;

        // ���̷ν����� �����͸� �̿��Ͽ� ȸ�� ���� ���
        Vector3 rotation = new Vector3(gyroRotationRate.x, gyroRotationRate.y, -gyroRotationRate.z);

        // ȸ�� ���⿡ ���� ������Ʈ�� ȸ����Ŵ
        transform.Rotate(rotation * RotateSpeed * Time.deltaTime);
    }



}
