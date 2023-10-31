using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class ObjectEmissionSystem : MonoBehaviour
{
    [SerializeField] private EObjectColorType _objectTeamColor1;
    [SerializeField] private EObjectColorType _objectTeamColor2;
    [SerializeField] private float _objectEmissionStrength;

    public List<int> dictionkey;

    struct ObjectEmissionData
    {
        public MeshRenderer meshRenderer;
        public Color color;
        public short gauge;
    }
    private Dictionary<int, ObjectEmissionData> _dictionary = new();

    private GameObject[] _gameObjects;
    private EObjectColorType _notChoiceTeamColor;
    private EObjectColorType _mixChoiceTeamColor;


    private void Awake()
    {
        (_notChoiceTeamColor,_mixChoiceTeamColor) = NoChoiceColor(_objectTeamColor1, _objectTeamColor2);
        InitializeLightObjectProcess("LightObject1", _objectTeamColor1, _objectTeamColor2, true);
        InitializeLightObjectProcess("LightObject2", _objectTeamColor2, _objectTeamColor1, true);
        InitializeLightObjectProcess("LightObject3", _mixChoiceTeamColor, _notChoiceTeamColor, false);

    }



    public void TakeObjectInfo(int objectID)
    {
        ObjectEmissionData searchObjData= new();
        print(_dictionary.TryGetValue(objectID, out searchObjData));

    }


    private void InitializeLightObjectProcess(string FindObjectstr, EObjectColorType color1, EObjectColorType color2, bool ColorAssoiciation)
    {
        List<Color> l_colors = AvailableColor(color1, color2, ColorAssoiciation);
        _gameObjects = GameObject.FindGameObjectsWithTag(FindObjectstr);
        AddNewLightObject(_gameObjects, l_colors);
    }


    private List<Color> AvailableColor(EObjectColorType teamColor, EObjectColorType anotherColor, bool Assoication)
    {
        List<Color> l_colors = new();
        short sTC = ((short)teamColor);
        short aTC = ((short)anotherColor);

        if (Assoication)
        {
            if (sTC - aTC < 0 && sTC + aTC == 1) MakeAvailableColor(Color.green, Color.cyan, ref l_colors);
            else if (sTC - aTC < 0 && sTC + aTC == 2) MakeAvailableColor(Color.blue, Color.cyan, ref l_colors);
            else if (sTC - aTC < 0 && sTC + aTC == 3) MakeAvailableColor(Color.blue, Color.magenta, ref l_colors);
            else if (sTC - aTC > 0 && sTC + aTC == 1) MakeAvailableColor(Color.red, Color.magenta, ref l_colors);
            else if (sTC - aTC > 0 && sTC + aTC == 2) MakeAvailableColor(Color.red, Color.yellow, ref l_colors);
            else if (sTC - aTC > 0 && sTC + aTC == 3) MakeAvailableColor(Color.green, Color.yellow, ref l_colors);
            else Debug.LogError("Wrong Team Color Setting");
        }
        else
        {
            if(Mathf.Abs(sTC - aTC) == 5) MakeAvailableColor(Color.cyan, Color.red, ref l_colors);
            else if(Mathf.Abs(sTC - aTC) == 3) MakeAvailableColor(Color.magenta, Color.green, ref l_colors);
            else if(Mathf.Abs(sTC - aTC) == 1) MakeAvailableColor(Color.yellow, Color.blue, ref l_colors);
            else Debug.LogError("Wrong Team Color Setting");
        }

        return l_colors;
    }


    private void MakeAvailableColor(Color pureColor, Color mixColor, ref List<Color> l_colors)
    {
        l_colors.Add(pureColor);
        l_colors.Add(pureColor);
        l_colors.Add(mixColor);
    }

    private (EObjectColorType, EObjectColorType) NoChoiceColor(EObjectColorType teamColor1, EObjectColorType teamColor2){

        short sTC1 = (short)teamColor1;
        short sTC2 = (short)teamColor2;

        return ((EObjectColorType)(3 - (sTC1 + sTC2)), (EObjectColorType)sTC1 + sTC2 + 2);
    }

    private void AddNewLightObject(GameObject[] _gameObjects, List<Color> l_colors)
    {
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            ObjectEmissionData objData = new();
            objData.color = l_colors[Random.Range(0, l_colors.Count)];
            objData.gauge = 100;
            objData.meshRenderer = _gameObjects[i].GetComponentInChildren<MeshRenderer>(true);
            objData.meshRenderer.material.SetColor("_EmissionColor", objData.color * Mathf.Pow(2, _objectEmissionStrength));
            dictionkey.Add(_gameObjects[i].transform.GetInstanceID());
            _dictionary.Add(_gameObjects[i].transform.GetInstanceID(), objData);
        }
    }

}
