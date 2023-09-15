using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEmissionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasObject;
    [SerializeField]
    private Transform _progresstransform;
    [SerializeField]
    private List<MeshRenderer> _progressmesh;
    [SerializeField]
    private ObjectEmissionManager _objectemission;
   

    // Start is called before the first frame update
    private void Start()
    {
       ObjectColorType colortype =  _objectemission.getColortype();
       Initializecolor(colortype);


    }

    private void Initializecolor(ObjectColorType colortype)
    {

        if (colortype == ObjectColorType.Red) _progressmesh[0].material.SetColor("_EmissionColor", Color.red);
        else if (colortype == ObjectColorType.Green) _progressmesh[0].material.SetColor("_EmissionColor", Color.green);
        else if (colortype == ObjectColorType.Blue) _progressmesh[0].material.SetColor("_EmissionColor", Color.blue);
        else if (colortype == ObjectColorType.Cyan)
        {
            _progressmesh.Add(Instantiate(_progresstransform).GetComponentInChildren<MeshRenderer>());
            _progressmesh[0].material.SetColor("_EmissionColor", Color.green);
            _progressmesh[1].material.SetColor("_EmissionColor", Color.blue);
        }
        else if (colortype == ObjectColorType.Magenta)
        {
            _progressmesh.Add(Instantiate(_progresstransform).GetComponentInChildren<MeshRenderer>());
            _progressmesh[0].material.SetColor("_EmissionColor", Color.red);
            _progressmesh[1].material.SetColor("_EmissionColor", Color.blue);
        }
        else if (colortype == ObjectColorType.Yellow)
        {
            _progressmesh.Add(Instantiate(_progresstransform).GetComponentInChildren<MeshRenderer>());
            _progressmesh[0].material.SetColor("_EmissionColor", Color.red);
            _progressmesh[1].material.SetColor("_EmissionColor", Color.green);
        }
        else if (colortype == ObjectColorType.White)
        {
            _progressmesh[0].material.SetColor("_EmissionColor", Color.blue);
        }

    }


    public void setObjectEmission(ObjectEmissionManager objectemission)
    {
        _objectemission = objectemission;
    }


}
