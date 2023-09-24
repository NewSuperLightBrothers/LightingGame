using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRay : MonoBehaviour
{
    public float maxDistance;
    public float d = 0;
    public List<Vector3> l_hitPositionWS;
    public int maxIterations;
    private int i = 0;

    private void LateUpdate() {
        d = maxDistance;
        i = 0;
        l_hitPositionWS.Clear();
        CheckMirror(transform.position, transform.forward);
        
    }

    private void CheckMirror(Vector3 position, Vector3 direction) {
        if (i > maxIterations) return; 
        if (Physics.Raycast(position, direction, out RaycastHit hit, d)) {
            d -= hit.distance;
            i++;
            l_hitPositionWS.Add(hit.point);
            if (hit.transform.tag == "Mirror") {
                CheckMirror(hit.point, Vector3.Reflect(direction, hit.normal));
            }
        } else {
            l_hitPositionWS.Add(position + direction * d);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (Vector3 v in l_hitPositionWS) {
            Gizmos.DrawWireSphere(v, 0.1f);
        }
    }
}
