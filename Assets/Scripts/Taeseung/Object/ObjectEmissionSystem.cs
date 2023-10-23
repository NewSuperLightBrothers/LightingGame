using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEmissionSystem : MonoBehaviour
{
    [SerializeField] private EObjectColorType _objectTeamColor1;
    [SerializeField] private EObjectColorType _objectTeamColor2;

    private List<int> _randomList = new(4);
    private Dictionary<int, MeshRenderer> _dictionary = new();
    private GameObject[] _gameObjects;


    private void Awake()
    {
        _gameObjects = GameObject.FindGameObjectsWithTag("LightObject1");

        //List가 0이면 무색, 1이면 순수색, 2면 혼합색1, 3이면 혼합색2 
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            int num = Random.Range(0, 4);
            if (num == 0 && _randomList[num]>3)
            {
               
                
            }
            else
            {

            }

        }


        _gameObjects = GameObject.FindGameObjectsWithTag("LightObject2");

        for (int i = 0; i < _gameObjects.Length; i++)
        {
            int num = Random.Range(0, _randomList.Count);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }


}
