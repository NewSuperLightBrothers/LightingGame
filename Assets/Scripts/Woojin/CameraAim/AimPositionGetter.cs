using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitInfo {
    public Vector3 hitPointWS;
    public Vector3 hitNormalWS;
    public string tag;

    public HitInfo(Vector3 point, Vector3 normal, string tag) {
        hitPointWS = point;
        hitNormalWS = normal;
        this.tag = tag;
    }
}

public class AimPositionGetter : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private int _maxIterations;
    [SerializeField] private Transform _gunEndPoint;
    private float _remainDistance;
    private int _iteration;
    public List<HitInfo> l_hitInfo;
    public LineRenderer bulletPath;

    private void LateUpdate() {
        _remainDistance = _maxDistance;
        _iteration = 0;
        l_hitInfo.Clear();

        CheckMirror(transform.position, transform.forward);
        UnfoldTarget();
    }

    private void CheckMirror(Vector3 position, Vector3 direction) {
        if (_iteration > _maxIterations) return; 
        if (Physics.Raycast(position, direction, out RaycastHit hit, _remainDistance)) {
            _remainDistance -= hit.distance;
            _iteration++;
            l_hitInfo.Add(new HitInfo(hit.point, hit.normal, hit.transform.tag));
            if (hit.transform.tag == "Mirror") {
                CheckMirror(hit.point, Vector3.Reflect(direction, hit.normal));
            }
        } else {
            l_hitInfo.Add(new HitInfo(position + direction * _remainDistance, Vector3.zero, "null"));
            _remainDistance = 0;
        }
    }

    private void UnfoldTarget() {
        _gunEndPoint.localPosition = (_maxDistance - _remainDistance) * Vector3.forward;

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (HitInfo v in l_hitInfo) {
            if (v == l_hitInfo[l_hitInfo.Count - 1]) {
                Gizmos.DrawSphere(v.hitPointWS, 0.1f);
                return;
            }
            Gizmos.DrawWireSphere(v.hitPointWS, 0.1f);
        }
    }
}
