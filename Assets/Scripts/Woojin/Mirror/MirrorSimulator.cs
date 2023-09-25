using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSimulator : MonoBehaviour
{
    public new Camera camera;
    public int iteration;
    [SerializeField] private int _maxIteration;
    [SerializeField] private GameObject _mirrorCameraPrefab;

    public Renderer mirror;


    private void Awake() {
        camera = GetComponent<Camera>();
    }

    private void Update() {
        //Debug.Log(mirror.gameObject.name + " is rendered: " + GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), mirror.bounds));
    }

}
