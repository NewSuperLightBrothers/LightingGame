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
    ExampleCharacterController exampleCharacterController;

    private List<GameObject> _lightList = new List<GameObject>();

    public bool isEnemy;
    public bool isAlly;
    
    public Image stateCheck;

    #region 버프/디버프 변수값(SerializeField)
    [SerializeField] private float _defaultVelocity = 6;
    [SerializeField] private float _buffVelocity = 10;
    [SerializeField] private float _debuffVelocity = 3;
    [SerializeField] private float _defaultJump = 10;
    [SerializeField] private float _buffJump = 15;
    [SerializeField] private float _debuffJump = 6;
    #endregion

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        exampleCharacterController = GetComponent<ExampleCharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        #region LightAreaIn
        if (other.gameObject.layer == 7)
        {
            _lightList.Add(other.gameObject);
            if (other.CompareTag("Enemy"))
            {
                isEnemy = true;
            }
            else if (other.CompareTag("Ally"))
            {
                isAlly = true;
            }
            Debug.Log("현재 남은 light수 = " + _lightList.Count);
            Debug.Log("적 잔상 = " + isEnemy);
            Debug.Log("아군 잔상 = " + isAlly);
        }
        #endregion LightAreaIn
    }
    private void OnTriggerExit(Collider other)
    {
        #region LightAreaOut
        if (other.gameObject.layer == 7)
        {
            _lightList.Remove(other.gameObject);
            isEnemy = IsTag(_lightList, "Enemy");
            isAlly = IsTag(_lightList, "Ally");
            Debug.Log("현재 남은 light수 = " + _lightList.Count);
            Debug.Log("적 잔상 = " + isEnemy);
            Debug.Log("아군 잔상 = " + isAlly);
        }
        #endregion LightAreaOut
    }
    private bool IsTag(List<GameObject> Array,string tagname)
    {
        bool isTag = false;
        for (int i = 0; i < Array.Count; i++)
        {
            if (Array[i].CompareTag(tagname))
            {
                isTag = true;
                break;
            }
        }
        return isTag;
    }
    private void FixedUpdate()
    {
        if (isAlly || isEnemy)
        {
            StartCoroutine(Buff());
            Debug.Log("현재 최대 속도 = " + exampleCharacterController.MaxStableMoveSpeed);
        }
    }
    private IEnumerator Buff()
    {
        float time = 0;
        #region Debuff
        if (isEnemy && !isAlly)
        {
            while (true)
            {
                stateCheck.GetComponent<Image>().color = new Color(0, 0, 1, 1);
                time += Time.deltaTime;
                exampleCharacterController.MaxStableMoveSpeed = _debuffVelocity;
                exampleCharacterController.JumpUpSpeed = _debuffJump;
                if (time >= 1.0f)
                {
                    time = 0;
                    exampleCharacterController.MaxStableMoveSpeed = _defaultVelocity;
                    exampleCharacterController.JumpUpSpeed = _defaultJump;
                    break;
                }
                yield return null;
            }
        }
        #endregion Debuff
        #region Buff
        if (!isEnemy && isAlly)
        {
            while (true)
            {
                stateCheck.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                time += Time.deltaTime;
                exampleCharacterController.MaxStableMoveSpeed = _buffVelocity;
                exampleCharacterController.JumpUpSpeed = _buffJump;
                if (time >= 1.0f)
                {
                    time = 0;
                    exampleCharacterController.MaxStableMoveSpeed = _defaultVelocity;
                    exampleCharacterController.JumpUpSpeed = _defaultJump;
                    break;
                }
                yield return null;
            }
        }
        #endregion Buff
        #region Together
        if (isAlly && isEnemy)
        {
            yield return null;
            stateCheck.GetComponent<Image>().color = new Color(1, 0, 1, 1);
        }
        #endregion Together
    }
}
