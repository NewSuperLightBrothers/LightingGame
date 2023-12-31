using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectEmissionSystem : MonoBehaviour
{
    [SerializeField] private EObjectColorType _objectTeamColor1;
    [SerializeField] private EObjectColorType _objectTeamColor2;
    [SerializeField] private float _objectEmissionStrength;


    private GameObject[] _gameObjects;
    private EObjectColorType _notChoiceTeamColor;
    private EObjectColorType _mixChoiceTeamColor;


    private void Awake()
    {
        (_notChoiceTeamColor,_mixChoiceTeamColor) = ObjectData.NoChoiceColor(_objectTeamColor1, _objectTeamColor2);
        InitializeLightObjectProcess("LightObject1", _objectTeamColor1, _objectTeamColor2, true);
        InitializeLightObjectProcess("LightObject2", _objectTeamColor2, _objectTeamColor1, true);
        InitializeLightObjectProcess("LightObject3", _mixChoiceTeamColor, _notChoiceTeamColor, false);

        //NetworkObject c = new();
       // ulong k = c.NetworkObjectId;
    }



    public bool TakeObjectLight(GameObject gObject, EObjectColorType colortype)
    {
        ObjectEmission searchObjData;

        if(gObject.TryGetComponent<ObjectEmission>(out searchObjData))
        {
            short gauge = -1;

            if (searchObjData.gauge.TryGetValue(colortype, out gauge) && gauge > 0 && ObjectData.IsAssociationLightColor(colortype,searchObjData.objectColorType.Value))
            {
                //searchObjData.color -= ObjectData.d_objectColor[searchObjData.objectColorType.Value] / 100f;
                searchObjData.gauge[colortype] -= 1;
                //searchObjData.meshRenderer.material.SetColor("_EmissionColor", searchObjData.color * Mathf.Pow(2, _objectEmissionStrength));
                return true;
            }

            else
                return false;
        }
        return false;

    }


    private void InitializeLightObjectProcess(string FindObjectstr, EObjectColorType color1, EObjectColorType color2, bool ColorAssoiciation)
    {
        List<Color> l_colors = AvailableColor(color1, color2, ColorAssoiciation);
        _gameObjects = GameObject.FindGameObjectsWithTag(FindObjectstr);
        AddNewLightObject(_gameObjects, l_colors, color1);
    }


    private List<Color> AvailableColor(EObjectColorType teamColor, EObjectColorType anotherColor, bool Assoication)
    {
        List<Color> l_colors = new();
        short sTC = ((short)teamColor);
        short aTC = ((short)anotherColor);

        if (Assoication)
        {
            Color MixColor = ObjectData.d_objectColor[teamColor] + ObjectData.d_objectColor[_notChoiceTeamColor];
            MakeAvailableColor(ObjectData.d_objectColor[teamColor], MixColor, ref l_colors, 2,1);
        }
        else
        {
            if(Mathf.Abs(sTC - aTC) == 5) MakeAvailableColor(Color.cyan, Color.red, ref l_colors, 1, 2);
            else if(Mathf.Abs(sTC - aTC) == 3) MakeAvailableColor(Color.magenta, Color.green, ref l_colors, 1, 2);
            else if(Mathf.Abs(sTC - aTC) == 1) MakeAvailableColor(Color.yellow, Color.blue, ref l_colors, 1, 2);
            else Debug.LogError("Wrong Team Color Setting");
        }

        return l_colors;
    }


    private void MakeAvailableColor(Color pureColor, Color mixColor, ref List<Color> l_colors, short w1, short w2)
    {
        for(int i=0; i<w1; i++)
            l_colors.Add(pureColor);

        for(int j=0; j<w2; j++)
            l_colors.Add(mixColor);
    }


    private void AddNewLightObject(GameObject[] _gameObjects, List<Color> l_colors, EObjectColorType colorType)
    {
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            ObjectEmission objData = _gameObjects[i].AddComponent<ObjectEmission>();

            //objData.color = l_colors[Random.Range(0, l_colors.Count)];
            //objData.maxgauge = 100;
            objData.gauge = new();
            //foreach(EObjectColorType j in ObjectData.DivideColorList(colorType))  objData.gauge[j] = objData.maxgauge;

            //objData.objectColorType = colorType;
 
           // objData.meshRenderer = _gameObjects[i].GetComponentInChildren<MeshRenderer>(true);
           // objData.meshRenderer.material.SetColor("_EmissionColor", objData.color * Mathf.Pow(2, _objectEmissionStrength));


        }
    }

}
