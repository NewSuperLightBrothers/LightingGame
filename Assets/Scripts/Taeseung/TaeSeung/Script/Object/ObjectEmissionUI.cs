using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectEmissionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvasObject;
    [SerializeField]
    private Transform _progressbarparent;
    [SerializeField]
    private Transform _progressbar;
    [SerializeField]
    private Dictionary<Color,MeshRenderer> _progressbarRenderer;


    private void Start()
    {
        _canvasObject = this.gameObject;
    }

    public GameObject InstantiateUI(Transform parent)
    {

        return Instantiate(_canvasObject, parent);
    }

    public void AddProgressbar(Color color, float Emissionmagnitude)
    {
        GameObject newprogressbar = Instantiate(_progressbar.gameObject, this.transform);
        MeshRenderer newmeshRenderer = newprogressbar.GetComponentInChildren<MeshRenderer>();
        newmeshRenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, Emissionmagnitude));
        _progressbarRenderer[color] = newmeshRenderer;
    }

    public void SetProgrssbarFill(Color color, float newgauge, float entiregauge)
    {
        Vector3 scale = _progressbarRenderer[color].transform.localScale;
        scale.z = (newgauge / entiregauge) * 50;
        _progressbarRenderer[color].transform.localScale = scale;
    }


    public void SetProgressbarColor(Color color, float Emissionmagnitude)
    {
        _progressbarRenderer[color].material.SetColor("_EmissionColor", color * Mathf.Pow(2, Emissionmagnitude));
    }

}
