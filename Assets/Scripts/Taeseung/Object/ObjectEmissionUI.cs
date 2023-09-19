using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ObjectEmissionUI : MonoBehaviour
{
    [SerializeField] private GameObject _canvasObject;
    [SerializeField] private Transform _progressbarparent;
    [SerializeField] private Transform _progressbar;
    [SerializeField] private Dictionary<Color,MeshRenderer> _d_progressbarRenderer;


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


    public void AddFirstBar(Color color, float emissionMagnitude)
    {
        _d_progressbarRenderer = new();
        MeshRenderer defaultRenderer = _progressbar.GetComponent<MeshRenderer>();
        defaultRenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionMagnitude));
        _d_progressbarRenderer.Add(color,defaultRenderer);
    }

    public void AddProgressbar(Color color, float emissionMagnitude)
    {
        GameObject newProgressbar = Instantiate(_progressbarparent.gameObject, _progressbarparent.parent);
        MeshRenderer newMeshRenderer = newProgressbar.GetComponentInChildren<MeshRenderer>();
        newMeshRenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionMagnitude));
        _d_progressbarRenderer.Add(color, newMeshRenderer);
    }

    public void SetProgressbarFill(Color color, float newGauge, float entireGauge)
    {
        Vector3 scale = _d_progressbarRenderer[color].transform.localScale;
        if (scale.z > 0) scale.z = (newGauge / entireGauge) * 50;
        else scale.z = 0;
        _d_progressbarRenderer[color].transform.localScale = scale;
    }

    public void SetProgressbarColor(Color color, float emissionMagnitude)
    {
        _d_progressbarRenderer[color].material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionMagnitude));
    }

    public void SetTurnUI(bool turn)
    {
        this.gameObject.SetActive(turn);
    }

}
