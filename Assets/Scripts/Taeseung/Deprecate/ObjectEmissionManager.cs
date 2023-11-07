using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectEmissionManager : MonoBehaviour
{
    [Header("OBJECT LIGHT INFO")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private EObjectColorType _colorType;
    [SerializeField] private float _gauge;
    [SerializeField] private float _emissionStrength;

    [Space]
    [Header("OBJECT UI")]
    [SerializeField] private ObjectEmissionUI _emissionUI;


    private Color _initialColor;
    private Dictionary<EObjectColorType, float> _d_colorInitialValue;
    private Dictionary<EObjectColorType, int> _d_check;

    private void Start()
    {

        _initialColor = _meshRenderer.material.GetColor("_EmissionColor");
        GameObject UI = _emissionUI.InstantiateUI(this.transform);
        _emissionUI = UI.GetComponent<ObjectEmissionUI>();
        UI.SetActive(false);

        SetColorInitialize();
        _meshRenderer.material.SetColor("_EmissionColor", _initialColor * Mathf.Pow(2, _emissionStrength));
    }



    private void SetColorInitialize() {

        _d_colorInitialValue = new();
        _d_check = new();

        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 thisPosition = this.transform.position;
        Vector3 face = thisPosition - playerPosition;

        if (_colorType == EObjectColorType.Red)
        {
            _initialColor = Color.red; //* _emissionInitialValue;
            _d_colorInitialValue.Add(EObjectColorType.Red, _initialColor.r);
            _d_check.Add(EObjectColorType.Red, 0);
            _emissionUI.AddFirstBar(Color.red, 1);
           
        }
        else if (_colorType == EObjectColorType.Green)
        {
            _initialColor = Color.green; //* _emissionInitialValue;
            _d_colorInitialValue.Add(EObjectColorType.Green, _initialColor.g);
            _d_check.Add(EObjectColorType.Green, 0);
            _emissionUI.AddFirstBar(Color.green, 1);
        }
        else if (_colorType == EObjectColorType.Blue)
        {
            _initialColor = Color.blue; //* _emissionInitialValue;
            _d_colorInitialValue.Add(EObjectColorType.Blue, _initialColor.b);
            _d_check.Add(EObjectColorType.Blue,0) ;
            _emissionUI.AddFirstBar(Color.blue, 1);
        }
        else if (_colorType == EObjectColorType.Magenta)
        {
            _initialColor = Color.magenta; //* _emissionInitialValue;
            _d_colorInitialValue.Add(EObjectColorType.Red, _initialColor.r);
            _d_colorInitialValue.Add(EObjectColorType.Blue, _initialColor.b);
            _d_check.Add(EObjectColorType.Red, 0);
            _d_check.Add(EObjectColorType.Blue, 0);
            _emissionUI.AddFirstBar(Color.red, 1);
            _emissionUI.AddProgressbar(Color.blue, 1);
        }
        else if (_colorType == EObjectColorType.Yellow)
        {
            _initialColor = Color.yellow; //* _emissionInitialValue;
            _d_colorInitialValue.Add(EObjectColorType.Red, _initialColor.r);
            _d_colorInitialValue.Add(EObjectColorType.Green, _initialColor.g);
            _d_check.Add(EObjectColorType.Red, 0);
            _d_check.Add(EObjectColorType.Green, 0);

            _emissionUI.AddFirstBar(Color.red, 1);
            _emissionUI.AddProgressbar(Color.green, 1);
        }
        else if (_colorType == EObjectColorType.Cyan)
        {
            _initialColor = Color.cyan; //* _emissionInitialValue;
            _d_colorInitialValue.Add(EObjectColorType.Green, _initialColor.g);
            _d_colorInitialValue.Add(EObjectColorType.Blue, _initialColor.b);
            _d_check.Add(EObjectColorType.Green, 0);
            _d_check.Add(EObjectColorType.Blue, 0);
            _emissionUI.AddFirstBar(Color.green, 1);
            _emissionUI.AddProgressbar(Color.blue, 1);
        }
        else if (_colorType == EObjectColorType.White)
        {
            _initialColor = Color.white;// * _emissionInitialValue;
            _d_colorInitialValue.Add(EObjectColorType.Red, _initialColor.r);
            _d_colorInitialValue.Add(EObjectColorType.Green, _initialColor.g);
            _d_colorInitialValue.Add(EObjectColorType.Blue, _initialColor.b);
            _d_check.Add(EObjectColorType.Red, 0);
            _d_check.Add(EObjectColorType.Green, 0);
            _d_check.Add(EObjectColorType.Blue, 0);
            _emissionUI.AddFirstBar(Color.red, 1);
            _emissionUI.AddProgressbar(Color.green, 1);
            _emissionUI.AddProgressbar(Color.blue, 1);
        }


        _emissionUI.PanelColor(_initialColor);
        _emissionUI.SetForwardVector(face);
     }
   


    public bool takeLightEnergy(EObjectColorType colorType)
    {
        float tempColorVal = 0;
        int tempCheck = 0;

        if (!_emissionUI.gameObject.activeSelf)
            _emissionUI.gameObject.SetActive(true);


        if (_d_check.TryGetValue(colorType, out tempCheck)) {
            if (tempCheck < (_gauge))
            {
                _d_check[colorType]++;

                if (colorType == EObjectColorType.Red)
                {
                    if (_d_colorInitialValue.TryGetValue(colorType, out tempColorVal))
                        _initialColor.r -= (tempColorVal / _gauge);

                    _emissionUI.SetProgressbarFill(Color.red, _initialColor.r, 1);

                }
                else if (colorType == EObjectColorType.Green)
                {
                    if (_d_colorInitialValue.TryGetValue(colorType, out tempColorVal))
                        _initialColor.g -= (tempColorVal / _gauge);

                    _emissionUI.SetProgressbarFill(Color.green, _initialColor.g, 1);
                }

                else if (colorType == EObjectColorType.Blue)
                {
                    if (_d_colorInitialValue.TryGetValue(colorType, out tempColorVal))
                        _initialColor.b -= (tempColorVal / _gauge);

                    _emissionUI.SetProgressbarFill(Color.blue, _initialColor.b, 1);

                }
                _meshRenderer.material.SetColor("_EmissionColor", _initialColor * Mathf.Pow(2, _emissionStrength));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    public void TurnOffUI() => _emissionUI.SetTurnUI(false);
    public EObjectColorType getColortype() => _colorType;
    public float getGuage() => _gauge;
    public void setGauge(float gauge) => _gauge = gauge;

}
