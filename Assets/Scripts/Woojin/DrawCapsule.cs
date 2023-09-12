using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DrawCapsule : MonoBehaviour {
    public float capsuleRadius;

    public Vector3 a, b;

    private void Update() {
        float capsuleRadius = Mathf.Min(transform.localScale.x, transform.localScale.z) * 0.5f;
        a = transform.position - (capsuleRadius - transform.localScale.y) * Vector3.up;
        b = transform.position + (capsuleRadius - transform.localScale.y) * Vector3.up;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        // Draw the capsule
        Gizmos.DrawWireSphere(a, capsuleRadius);
        Gizmos.DrawWireSphere(b, capsuleRadius);
        Gizmos.DrawLine(a + Vector3.forward * capsuleRadius, b + Vector3.forward * capsuleRadius);
        Gizmos.DrawLine(a + Vector3.right * capsuleRadius, b + Vector3.right * capsuleRadius);
        Gizmos.DrawLine(a - Vector3.forward * capsuleRadius, b - Vector3.forward * capsuleRadius);
        Gizmos.DrawLine(a - Vector3.right * capsuleRadius, b - Vector3.right * capsuleRadius);
    }
}