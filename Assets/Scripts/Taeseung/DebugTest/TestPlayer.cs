using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPlayer : MonoBehaviour
{
    public float testHP;
    [SerializeField] private GameObject _bomb;

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            GameObject temp = Instantiate(bomb);
            //temp.transform.position = (transform.position + transform.forward * 2);
        }*/
    }
}
