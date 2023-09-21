using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRay : MonoBehaviour
{
    public Transform hitTransformWS;
    public float maxDistance;
    public Transform[] hitMirrors;

    private void LateUpdate() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance)) {
            hitTransformWS.position = hit.point;
            return;
        }
        hitTransformWS.position = transform.position + transform.forward * maxDistance;
        
    }
}
