using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introFollowCam : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles +  new Vector3(0, speed, 0) * Time.deltaTime);
    }
}

