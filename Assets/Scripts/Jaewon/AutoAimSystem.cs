using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KinematicCharacterController;

public class AutoAimSystem : MonoBehaviour
{
    private Vector3 _currentAim;
    private Vector3 _enemyAim;
    [SerializeField] private float _rayLength = 10.0f;
    [SerializeField] private float _autoAimLength = 10.0f;
    [SerializeField] private float _autoAimAngle = 10.0f;
    [SerializeField] private float _followSpeed = 1.0f;
    [SerializeField] private float _autoAimPower = 4f;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camArm;
    [SerializeField] private float _autoAimCriticalPoint = 10.0f;
    private GameObject _targetEnemy;

    private void Start()
    {
        Debug.Log("오토에임 시작");
    }
    private void FixedUpdate()
    {
        #region DEBUG LINE
        Vector3 characterPos = this.transform.position;
        Vector3 currentAim = this.transform.forward;
        RaycastHit hit;
        //레이 테스트용
        Debug.DrawLine(characterPos, characterPos + currentAim * 10, Color.red);
        if (Physics.Raycast(characterPos, currentAim, out hit, _rayLength))
        {
            // 레이와 충돌한 오브젝트를 감지했을 때 실행할 코드
            //Debug.Log("레이와 충돌한 오브젝트: " + hit.collider.gameObject.name);
        }
        #endregion
    }
    private void TargetEnemy()
    {
        List<GameObject> targetList = new List<GameObject>();
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        _currentAim = _player.transform.forward;
        float atLeast = 100;
        GameObject atLeastObj = null;
        for (int i = 0; i < enemyList.Length; i++)
        {
            float dis = (_player.transform.position - enemyList[i].transform.position).magnitude;
            Vector3 enemyVec = enemyList[i].transform.position - _player.transform.position;
            float vecAngle = Vector3.Angle(enemyVec, _currentAim);
            Debug.Log("각도 = " + vecAngle);
            if(dis< _autoAimLength && vecAngle< _autoAimAngle)
            {
                targetList.Add(enemyList[i]);
                Debug.Log("적 인식, 적 이름 = " + enemyList[i].name);
            }
        }
        if (targetList.Count > 0)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                if((targetList[i].transform.position - _player.transform.position).magnitude < atLeast)
                {
                    atLeastObj = targetList[i];
                    atLeast = (targetList[i].transform.position - _player.transform.position).magnitude;
                }
            }
            _targetEnemy = atLeastObj;
            Debug.Log(_targetEnemy.name);
        }
    }
    public void AutoAim()
    {
        TargetEnemy();
        if (_targetEnemy != null)
        {
            if ((_targetEnemy.transform.position - _player.transform.position).magnitude > _autoAimLength || Vector3.Angle(_targetEnemy.transform.position - _player.transform.position, _player.transform.forward) > _autoAimAngle)
            {
                _targetEnemy = null;
            }
        }
        if (_targetEnemy != null)
        {
            Debug.Log("코루틴 시작");
            StopAllCoroutines();
            StartCoroutine(FollowEnemy());
        }
        else if (_targetEnemy == null)
        {
            Debug.Log("타겟 없음");
        }
    }
    private IEnumerator FollowEnemy()
    {
        while (true)
        {
            Vector3 currentPlayerForward = _player.transform.forward;
            Vector3 currentEnemyOrient = _targetEnemy.transform.position - _player.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemyOrient);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, _autoAimPower);
            float angleDif = Quaternion.Angle(this.transform.rotation, targetRotation);
            Debug.Log(angleDif);
            if(angleDif < _autoAimCriticalPoint)
            {
                Debug.Log("에임 도달");
                break;
            }
            yield return null;
        }
    }
}
