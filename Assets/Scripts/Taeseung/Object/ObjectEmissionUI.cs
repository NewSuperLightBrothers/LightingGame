using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

    public void PanelColor(Color color)
    {
        _progressbarparent.parent.GetComponent<Image>().color = color;
    }

    public void SetForwardVector(Vector3 vector)
    {
        this.transform.forward = vector;
    }


    public void AddFirseBar(Color color, float Emissionmagnitude)
    {
        _progressbarRenderer = new();
        MeshRenderer defaultRenderer = _progressbar.GetComponent<MeshRenderer>();
        defaultRenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, Emissionmagnitude));
        _progressbarRenderer.Add(color,defaultRenderer);
    }

    public void AddProgressbar(Color color, float Emissionmagnitude)
    {
        GameObject newprogressbar = Instantiate(_progressbarparent.gameObject, _progressbarparent.parent);
        MeshRenderer newmeshRenderer = newprogressbar.GetComponentInChildren<MeshRenderer>();
        newmeshRenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, Emissionmagnitude));
        _progressbarRenderer.Add(color, newmeshRenderer);
    }

    public void SetProgrssbarFill(Color color, float newgauge, float entiregauge)
    {
        Vector3 scale = _progressbarRenderer[color].transform.localScale;
        if (scale.z > 0) scale.z = (newgauge / entiregauge) * 50;
        else scale.z = 0;
        _progressbarRenderer[color].transform.localScale = scale;
    }

    public void SetProgressbarColor(Color color, float Emissionmagnitude)
    {
        _progressbarRenderer[color].material.SetColor("_EmissionColor", color * Mathf.Pow(2, Emissionmagnitude));
    }

    public void SetTurnUI(bool turn)
    {
        this.gameObject.SetActive(turn);
    }

}
