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
    [SerializeField] private GameObject player;
    [SerializeField] private float _autoAimCriticalPoint = 10.0f;
    private GameObject _targetEnemy;

    private void FixedUpdate()
    {
        #region DEBUG LINE
        Vector3 characterPos = this.transform.position;
        Vector3 currentAim = this.transform.forward;
        RaycastHit hit;
        //���� �׽�Ʈ��
        Debug.DrawLine(characterPos, characterPos + currentAim * 10, Color.red);
        if (Physics.Raycast(characterPos, currentAim, out hit, _rayLength))
        {
            // ���̿� �浹�� ������Ʈ�� �������� �� ������ �ڵ�
            //Debug.Log("���̿� �浹�� ������Ʈ: " + hit.collider.gameObject.name);
        }
        #endregion
    }
    private void TargetEnemy()
    {
        List<GameObject> targetList = new List<GameObject>();
        GameObject[] enemysList = GameObject.FindGameObjectsWithTag("Enemy");
        _currentAim = player.transform.forward;
        float atLeast = 100;
        GameObject atLeastObj = null;
        for (int i = 0; i < enemysList.Length; i++)
        {
            float dis = (player.transform.position - enemysList[i].transform.position).magnitude;
            Vector3 enemyVec = enemysList[i].transform.position - player.transform.position;
            float vecAngle = Vector3.Angle(enemyVec, _currentAim);
            Debug.Log("���� = " + vecAngle);
            if(dis< _autoAimLength && vecAngle< _autoAimAngle)
            {
                targetList.Add(enemysList[i]);
                Debug.Log("�� �ν�, �� �̸� = " + enemysList[i].name);
            }
        }
        if (targetList.Count > 0)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                if((targetList[i].transform.position - player.transform.position).magnitude < atLeast)
                {
                    atLeastObj = targetList[i];
                    atLeast = (targetList[i].transform.position - player.transform.position).magnitude;
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
            if ((_targetEnemy.transform.position - player.transform.position).magnitude > _autoAimLength || Vector3.Angle(_targetEnemy.transform.position - player.transform.position, player.transform.forward) > _autoAimAngle)
            {
                _targetEnemy = null;
            }
        }
        if (_targetEnemy != null)
        {
            Debug.Log("�ڷ�ƾ ����");
            StopAllCoroutines();
            StartCoroutine(FollowEnemy());
        }
        else if (_targetEnemy == null)
        {
            Debug.Log("Ÿ�� ����");
        }
    }
    IEnumerator FollowEnemy()
    {
        while (true)
        {
            Vector3 currentPlayerForward = this.transform.forward;
            Vector3 currentEnemyOrient = _targetEnemy.transform.position - this.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemyOrient);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, _autoAimPower);
            float angleDif = Quaternion.Angle(this.transform.rotation, targetRotation);
            Debug.Log(angleDif);
            if(angleDif < _autoAimCriticalPoint)
            {
                Debug.Log("���� ����");
                break;
            }
            yield return null;
        }
    }
}
