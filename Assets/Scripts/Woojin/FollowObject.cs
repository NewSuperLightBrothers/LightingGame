using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _orientation;

    private void Update() {
        transform.position = _target.transform.position;
        transform.rotation = _target.transform.rotation;
        float angle = transform.rotation.eulerAngles.y;
        _orientation.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
