using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawSphere : MonoBehaviour
{
    public float capsuleRadius;
    private void Update() {
        float capsuleRadius = Mathf.Min(transform.localScale.x, transform.localScale.z) * 0.5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        // Draw the capsule
        Gizmos.DrawWireSphere(transform.position, capsuleRadius);
    }
}
