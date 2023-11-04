using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KinematicCharacterController;
using KinematicCharacterController.Examples;

public enum State{
    Red = 0,
    Green = 1,
    Blue = 2
}
public class LightBuffSystem : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    ExampleCharacterController exampleCharacterController;
    PlayerManager playerManager;

    private List<GameObject> _lightList = new List<GameObject>();

    public bool isEnemy;
    public bool isAlly;
    private bool isOnArea;
    Color Neutrality = new Color(1, 1, 1, 1);

    //인덱스 0은 아군, 인덱스 1은 적군입니다. 인덱스 2는 중립(아군도 적군도 아닌 상태)
    private int[] _stateCheck = new int[] { 0, 0, 0 };

    //public Image stateCheck;

    #region 변수값(SerializeField)
    [SerializeField] private float _defaultVelocity = 6;
    [SerializeField] private float _buffVelocity = 10;
    [SerializeField] private float _debuffVelocity = 3;
    [SerializeField] private float _defaultJump = 10;
    [SerializeField] private float _buffJump = 15;
    [SerializeField] private float _debuffJump = 6;
    [SerializeField] private float _buffTime=1.0f;
    #endregion
    private void Start()
    {
        exampleCharacterController = GetComponent<ExampleCharacterController>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerManager = this.gameObject.GetComponent<PlayerManager>();
        Debug.Log(playerManager.teamColor);
    }
    #region LateUpdate
    //RGB동시에 고려하니까 뭔가 말이안되는 부분이 있어서 같은 팀 영역에 들어갈때만 버프를 받도록 하였습니다.
    //디버프, 버프 메소드는 일단 둘다 만들었고요.
    //상태 리스트에서 값을 비교하여 현재 받을 버프상태를 정합니다.
    private void LateUpdate()
    {
        Buff();
        DeBuff();
    }
    #endregion LateUpdate
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            //현재 팀의 컬러와 대상의 컬러를 비교합니다.
            if (other.CompareTag("Red"))
            {
                _stateCheck[(int)State.Red]++;
                Debug.Log("빨강 진입");
            }
            else if (other.CompareTag("Blue"))
            {
                _stateCheck[(int)State.Blue]++;
                Debug.Log("파랑 진입");
            }
            else if (other.CompareTag("Green"))
            {
                _stateCheck[(int)State.Green]++;
            }
            GetIsOnArea();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            //현재 팀의 컬러와 대상의 컬러를 비교합니다.
            if (other.CompareTag("Red"))
            {
                _stateCheck[(int)State.Red]--;
            }
            else if (other.CompareTag("Green"))
            {
                _stateCheck[(int)State.Green]--;
            }
            else if (other.CompareTag("Blue"))
            {
                _stateCheck[(int)State.Blue]--;
            }
            GetIsOnArea();
        }
    }

    private Color GetState()
    {
        Color _state;
        if(_stateCheck[(int)State.Red] != 0 && _stateCheck[(int)State.Green] == 0 && _stateCheck[(int)State.Blue] == 0)
        {
            _state = Color.red;
        }
        else if (_stateCheck[(int)State.Red] == 0 && _stateCheck[(int)State.Green] != 0 && _stateCheck[(int)State.Blue] == 0)
        {
            _state = Color.green;
        }
        else if (_stateCheck[(int)State.Red] == 0 && _stateCheck[(int)State.Green] == 0 && _stateCheck[(int)State.Blue] != 0)
        {
            _state = Color.blue;
        }
        else
        {
            _state = Neutrality;
        }
        return _state;
    }
    private void GetIsOnArea()
    {
        if (_stateCheck == new int[] { 0, 0, 0 })
        {
            isOnArea = false;
        }
        else isOnArea = true;
    }
    #region Buff
    private void Buff()
    {
        if (playerManager.teamColor == GetState() && isOnArea)
        {
            StartCoroutine(EndBuff());
        }
        else if(playerManager.teamColor == GetState() && !isOnArea)
        {
            Debug.Log("영역 밖");
        }
    }
    private IEnumerator EndBuff()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            exampleCharacterController.MaxStableMoveSpeed = _buffVelocity;
            exampleCharacterController.JumpUpSpeed = _buffJump;
            if (time >= _buffTime)
            {
                exampleCharacterController.MaxStableMoveSpeed = _defaultVelocity;
                exampleCharacterController.JumpUpSpeed = _defaultJump;
                time = 0;
                break;
            }
            yield return null;
        }
    }
    #endregion Buff
    #region DeBuff
    private void DeBuff()
    {
        if (playerManager.teamColor != GetState() && isOnArea)
        {
            if(GetState() == Neutrality)
            {
                return;
            }
            StartCoroutine(EndDeBuff());
        }
        else if (playerManager.teamColor != GetState() && !isOnArea)
        {
            return;        
        }
    }
    private IEnumerator EndDeBuff()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            exampleCharacterController.MaxStableMoveSpeed = _debuffVelocity;
            exampleCharacterController.JumpUpSpeed = _debuffJump;
            if (time >= _buffTime)
            {
                exampleCharacterController.MaxStableMoveSpeed = _defaultVelocity;
                exampleCharacterController.JumpUpSpeed = _defaultJump;
                time = 0;
                break;
            }
            yield return null;
        }
    }
    #endregion Buff
}
