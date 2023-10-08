using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KinematicCharacterController;
using KinematicCharacterController.Examples;


public class LightBuffSystem : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    private List<GameObject> _lightList = new List<GameObject>();
    public bool isEnemy;
    public bool isAlly;
    public Image stateCheck;
    [Header("Buff Velocity")]
    private float _defaultVelocity = 6;
    private float _buffVelocity = 10;
    private float _debuffVelocity = 3;
    Color redColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    Color blueColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    Color pupleColor = new Color(1.0f, 0.0f, 1.0f, 1.0f);
    Color defaultColor = new Color(0, 0, 0, 1.0f);
    ExampleCharacterController exampleCharacterController;
    private void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        exampleCharacterController = GetComponent<ExampleCharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _lightList.Add(other.gameObject);
        if (other.CompareTag("Enemy"))
        {
            isEnemy = true;
        }else if (other.CompareTag("Ally"))
        {
            isAlly = true;
        }
        Debug.Log("현재 남은 light수 = " + _lightList.Count);
        Debug.Log("적 잔상 = " + isEnemy);
        Debug.Log("아군 잔상 = " + isAlly);
    }
    private void OnTriggerExit(Collider other)
    {
        _lightList.Remove(other.gameObject);
        isEnemy = IsTag<string>(_lightList, "Enemy");
        isAlly = IsTag<string>(_lightList, "Ally");
        Debug.Log("현재 남은 light수 = " + _lightList.Count);
        Debug.Log("적 잔상 = " + isEnemy);
        Debug.Log("아군 잔상 = " + isAlly);
    }
    private bool IsTag<T>(List<GameObject> Array,T tagname)
    {
        bool isTag = false;
        for (int i = 0; i < Array.Count; i++)
        {
            if (Array[i].CompareTag("Enemy"))
            {
                isTag = true;
                break;
            }
        }
        return isTag;
    }
    private void FixedUpdate()
    {
        StartCoroutine(Buff());
        Debug.Log("현재 최대 속도 = " + exampleCharacterController.MaxStableMoveSpeed);
    }
    //불러온 에셋에서 velocity를 변환하는 메소드를 찾아야함. 찾으면 바로 적용가능
    private IEnumerator Buff()
    {
        float time = 0;
        if (isEnemy && !isAlly)
        {
            while (true)
            {
                time += Time.deltaTime;
                exampleCharacterController.MaxStableMoveSpeed = _debuffVelocity;
                if (time >= 1.0f)
                {
                    time = 0;
                    exampleCharacterController.MaxStableMoveSpeed = _defaultVelocity;
                    break;
                }
                yield return null;
            }
        }
        if (!isEnemy && isAlly)
        {
            while (true)
            {
                time += Time.deltaTime;
                exampleCharacterController.MaxStableMoveSpeed = _buffVelocity;
                if (time >= 1.0f)
                {
                    time = 0;
                    exampleCharacterController.MaxStableMoveSpeed = _defaultVelocity;
                    break;
                }
                yield return null;
            }
        }
        if(isAlly && isEnemy)
        {
            yield return null;
        }
    }
}
