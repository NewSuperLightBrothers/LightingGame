using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DynamicController;

public class DynamicCharacterController : MonoBehaviour
{
    public GameObject root;
    public Transform orientation;
    private Rigidbody rootRigidbody;
    private CapsuleCollider rootCollider;
    public float monoballRadius;
    private Vector3 velocityIS, velocityOS, velocityTS, velocityWS;

    public float jumpSpeed;
    public float gravity;

    bool isGroundDetect;
    bool isGrounded = true;

    public float maxGroundAngle;

    public float groundMoveMultiplier;
    public float airMoveMultiplier;
    private float moveMultiplier;

    RaycastHit hit;
    float hDistance, hDistanceOld;
    private Vector3 hNormal;
    private float hVelocity;

    private float maxDistance = 1.4f;
    private float neutralDistance;

    private bool enableSphereCast = true;

    public float maxGroundVelocity;

    public float groundSpring, groundDamper;

    private Vector3 rayDirectionWS = Vector3.down;
    
    private void Awake() {
        rootRigidbody = root.GetComponent<Rigidbody>();
        rootCollider = root.GetComponent<CapsuleCollider>();
        neutralDistance = rootCollider.center.y - root.transform.position.y - monoballRadius;
        hDistance = neutralDistance;
        hDistanceOld = neutralDistance;
    }

    private void FixedUpdate() {
        orientation.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);

        velocityIS = DynamicInputManager.instance.dynamicInputData.velocityIS;
        velocityOS = velocityIS.x * Vector3.right + velocityIS.y * Vector3.forward;
        velocityTS = orientation.rotation * velocityOS;

        Vector3 groundRayOrigin = rootRigidbody.position + (neutralDistance + monoballRadius) * Vector3.up;
    
        if (enableSphereCast) {
            isGrounded = Physics.SphereCast(groundRayOrigin, monoballRadius, rayDirectionWS, out hit, maxDistance);
        } else {
            isGrounded  = false;
        }

        if (isGrounded) {
            hNormal = Vector3.up;
            hDistance = Vector3.Distance(groundRayOrigin, hit.point + hit.normal * monoballRadius);

            if (Vector3.Angle(hit.normal, Vector3.up) <= maxGroundAngle) {
                hNormal = hit.normal;
            } else {
                if (Physics.Raycast(groundRayOrigin + velocityTS * Time.fixedDeltaTime, rayDirectionWS, out RaycastHit h, hDistance + monoballRadius) && Vector3.Angle(h.normal, Vector3.up) <= maxGroundAngle) {
                    hNormal = h.normal;
                } else {
                    hNormal = Vector3.ProjectOnPlane(hit.normal, Vector3.up).normalized;
                    rootRigidbody.AddForce(Vector3.down * 20f);
                }
            }

            velocityWS = Vector3.ProjectOnPlane(velocityTS, hNormal).normalized * velocityTS.magnitude;
        }
        rootRigidbody.AddForce(velocityWS * 10f);

    }

    private IEnumerator IJumpDelay() {
        enableSphereCast = false;
        yield return new WaitForSeconds(0.2f);
        enableSphereCast = true;
        yield return null;
    }
}
