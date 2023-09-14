using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private SphereCollider groundCollider;
    [SerializeField] private CapsuleCollider wallCollider;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.rigidbody);
    }
    private void OnCollisionStay(Collision other) {
        
    }
    private void OnCollisionExit(Collision other) {
        
    }
}
