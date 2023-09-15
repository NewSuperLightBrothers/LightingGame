using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEmissionManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private MeshRenderer _meshrenderer;
    [SerializeField]
    private ObjectColorType _colortype;
    [SerializeField]
    private float _gauge;
    [SerializeField]
    private float _takeweight;
    [SerializeField]
    private float _emissionstrength;

    
    [SerializeField]
    private Transform ObjectEmissionUIPrefab;
    [SerializeField]
    private ObjectEmissionUI objectemissionuiclass;


    private Color _Initialcolor;
    private Dictionary<ObjectColorType, float> _colorInitialvalue;
    private Dictionary<ObjectColorType, int> _check;

    void Start()
    {
        _Initialcolor = _meshrenderer.material.GetColor("_EmissionColor");
        SetColorInitialize();
        _meshrenderer.material.SetColor("_EmissionColor", _Initialcolor);

    }



    private void SetColorInitialize() {

        _colorInitialvalue = new();
        _check = new();

        if (_colortype == ObjectColorType.Red)
        {
            _Initialcolor = Color.red; //* _emissionInitialvalue;
            _colorInitialvalue.Add(ObjectColorType.Red, _Initialcolor.r);
            _check.Add(ObjectColorType.Red, 0);
        }
        else if (_colortype == ObjectColorType.Green)
        {
            _Initialcolor = Color.green; //* _emissionInitialvalue;
            _colorInitialvalue.Add(ObjectColorType.Green, _Initialcolor.g);
            _check.Add(ObjectColorType.Green, 0);
        }
        else if (_colortype == ObjectColorType.Blue)
        {
            _Initialcolor = Color.blue; //* _emissionInitialvalue;
            _colorInitialvalue.Add(ObjectColorType.Blue, _Initialcolor.b);
            _check.Add(ObjectColorType.Blue,0) ;
        }
        else if (_colortype == ObjectColorType.Magenta)
        {
            _Initialcolor = Color.magenta; //* _emissionInitialvalue;
            _colorInitialvalue.Add(ObjectColorType.Red, _Initialcolor.r);
            _colorInitialvalue.Add(ObjectColorType.Blue, _Initialcolor.b);
            _check.Add(ObjectColorType.Red, 0);
            _check.Add(ObjectColorType.Blue, 0);
        }
        else if (_colortype == ObjectColorType.Yellow)
        {
            _Initialcolor = Color.yellow; //* _emissionInitialvalue;
            _colorInitialvalue.Add(ObjectColorType.Red, _Initialcolor.r);
            _colorInitialvalue.Add(ObjectColorType.Green, _Initialcolor.g);
            _check.Add(ObjectColorType.Red, 0);
            _check.Add(ObjectColorType.Green, 0);
        }
        else if (_colortype == ObjectColorType.Cyan)
        {
            _Initialcolor = Color.cyan; //* _emissionInitialvalue;
            _colorInitialvalue.Add(ObjectColorType.Green, _Initialcolor.g);
            _colorInitialvalue.Add(ObjectColorType.Blue, _Initialcolor.b);
            _check.Add(ObjectColorType.Green, 0);
            _check.Add(ObjectColorType.Blue, 0);
        }
        else if (_colortype == ObjectColorType.White)
        {
            _Initialcolor = Color.white;// * _emissionInitialvalue;
            _colorInitialvalue.Add(ObjectColorType.Red, _Initialcolor.r);
            _colorInitialvalue.Add(ObjectColorType.Green, _Initialcolor.g);
            _colorInitialvalue.Add(ObjectColorType.Blue, _Initialcolor.b);
            _check.Add(ObjectColorType.Red, 0);
            _check.Add(ObjectColorType.Green, 0);
            _check.Add(ObjectColorType.Blue, 0);
        }
        }
    
    public bool takeLightEnergy(ObjectColorType colortype)
    {
        float tempcolorval = 0;
        int tempcheck = 0;
        if (_check.TryGetValue(colortype, out tempcheck)) {
            if (tempcheck <= (_gauge / _takeweight))
            {
                _check[colortype]++;

                if (colortype == ObjectColorType.Red)
                {
                    if (_colorInitialvalue.TryGetValue(colortype, out tempcolorval))
                        _Initialcolor.r -= (tempcolorval / _gauge) * _takeweight;

                }
                else if (colortype == ObjectColorType.Green)
                {
                    if (_colorInitialvalue.TryGetValue(colortype, out tempcolorval))
                        _Initialcolor.g -= (tempcolorval / _gauge) * _takeweight;
                }

                else if (colortype == ObjectColorType.Blue)
                {
                    if (_colorInitialvalue.TryGetValue(colortype, out tempcolorval))
                        _Initialcolor.b -= (tempcolorval / _gauge) * _takeweight;

                }
                _meshrenderer.material.SetColor("_EmissionColor", _Initialcolor);
                return true;
            }
            else
            {
                print("빨거 다 빨았다 이자식아");
                return false;
            }
        }
        else
        {
            print("못빤다 이자식아");
            return false;
        }
    }



    public ObjectColorType getColortype() => _colortype;
    public float getGuage() => _gauge;
    public void setGauge(float gauge) => _gauge = gauge;
    public float getWeight() => _takeweight;


}
