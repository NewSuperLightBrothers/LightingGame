using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectEmissionSystem : NetworkBehaviour
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
    }

    private void Start()
    {
        
    }


    public bool TakeObjectLight(GameObject gObject, EObjectColorType colortype)
    {
        ObjectEmission searchObjData;

        if(gObject.TryGetComponent<ObjectEmission>(out searchObjData))
        {
            if (searchObjData.gauges[((short)colortype)] > 0  && ObjectData.IsAssociationLightColor(colortype,searchObjData.objectColorType.Value))
            {
                Color color1 = new Color(searchObjData.r.Value, searchObjData.g.Value, searchObjData.b.Value, 1);
                Color color2 = ObjectData.d_objectColor[searchObjData.objectColorType.Value] / 100f;
                color1 -= color2;

                searchObjData.r.Value -= color1.r;
                searchObjData.g.Value -= color1.g;
                searchObjData.b.Value -= color1.b;

                searchObjData.gauges[(short)(colortype)] -= 1;

                gObject.GetComponentInChildren<MeshRenderer>(true).material.SetColor("_EmissionColor", color1 * Mathf.Pow(2, _objectEmissionStrength));
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
            //  Color MixColor = ObjectData.d_objectColor[teamColor] + ObjectData.d_objectColor[_notChoiceTeamColor];
            EObjectColorType colortype = (EObjectColorType)(((short)teamColor) + ((short)_notChoiceTeamColor) + 2);
            Color MixColor = ObjectData.d_objectColor[colortype];

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
            for (int j = 0; j < 8; j++) objData.gauges.Add(0);

            //N color 정보 주입 
            Color color = l_colors[Random.Range(0, l_colors.Count)];
            objData.r = new(color.r);
            objData.g = new(color.g);
            objData.b = new(color.b);

            //N color 타입 정보 주입
            objData.objectColorType = new(ObjectData.d_objectColorType[color]);

            //N maxgauge정보 주입
            objData.maxgauge = 100;

            foreach (EObjectColorType j in ObjectData.DivideColorList(objData.objectColorType.Value)) objData.gauges[((short)j)] = objData.maxgauge;
    

            _gameObjects[i].GetComponentInChildren<MeshRenderer>(true).material.SetColor("_EmissionColor", color * Mathf.Pow(2, _objectEmissionStrength));


        }
    }

}
