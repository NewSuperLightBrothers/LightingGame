using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRay : MonoBehaviour
{
    public Transform hitTransformWS;
    public float maxDistance;
    public Transform[] hitMirrors;

    private void LateUpdate() {
        CheckMirror(transform.position, transform.forward);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance)) {
            hitTransformWS.position = hit.point;
            return;
        }
        hitTransformWS.position = transform.position + transform.forward * maxDistance;
        
    }

    private void CheckMirror(Vector3 position, Vector3 direction) {
        if (Physics.Raycast(position, direction, out RaycastHit hit, maxDistance)) {
            hitTransformWS.position = hit.point;
            if (hit.transform.tag == "Mirror") {
                CheckMirror(hit.point, Vector3.Reflect(direction, hit.normal));
            }
        }
    }
}
