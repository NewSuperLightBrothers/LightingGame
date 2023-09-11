using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    private void Update() {
        transform.position = _target.transform.position;
        transform.rotation = _target.transform.rotation;
    }
}
