using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoAimSystem : MonoBehaviour
{
    private Vector3 currentAim;
    private Vector3 enemyAim;
    [SerializeField]
    private float rayLength = 10.0f;
    [SerializeField]
    private float autoAimLength = 10.0f;
    [SerializeField]
    private float autoAimAngle = 10.0f;
    [SerializeField]
    private float followSpeed = 1.0f;
    private GameObject targetEnemy;
 
    private void FixedUpdate()
    {
        Vector3 characterPos = transform.position;
        Vector3 currentAim = transform.forward;
        RaycastHit hit;
        //���� �׽�Ʈ��
        Debug.DrawLine(characterPos, characterPos + currentAim * 10, Color.red);
        if (Physics.Raycast(characterPos, currentAim, out hit, rayLength))
        {
            // ���̿� �浹�� ������Ʈ�� �������� �� ������ �ڵ�
            //Debug.Log("���̿� �浹�� ������Ʈ: " + hit.collider.gameObject.name);
        }
    }
    private void TargetEnemy()
    {
        List<GameObject> targetList = new List<GameObject>();
        GameObject[] enemysList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemysList.Length; i++)
        {
            float dis = (transform.position - enemysList[i].transform.position).magnitude;
            Vector3 enemyVec = enemysList[i].transform.position - this.transform.position;
            float vecAngle = Vector3.Angle(enemyVec, currentAim);
            if(dis<autoAimLength && vecAngle< autoAimAngle)
            {
                targetList.Add(enemysList[i]);
                Debug.Log("�� �ν�, �� �̸� = " + enemysList[i].name);
            }
        }
        if (targetList.Count > 0)
        {
            targetEnemy = targetList[0];
            Debug.Log(targetEnemy.name);
        }
    }
    public void AutoAim()
    {
        TargetEnemy();
        if (targetEnemy != null)
        {
            Vector3 lookEnemy = targetEnemy.transform.position - this.transform.position;
            this.transform.LookAt(targetEnemy.transform);
            //GameObject.DestroyImmediate(targetEnemy);
        }
        else if (targetEnemy == null)
        {
            Debug.Log("Ÿ�� ����");
        }
    }
    public void FollowTarget()
    {
        if (targetEnemy != null)
        { 
        Vector3 dir = targetEnemy.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * followSpeed);
        }
    }
}
