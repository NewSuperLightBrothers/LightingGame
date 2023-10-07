using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBuffSystem : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    private List<GameObject> _lightList = new List<GameObject>();
    public bool isEnemy;
    public bool isAlly;
    private void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
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
        if (isAlly || isEnemy)
        {
            StartCoroutine(Buff());
        }
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
                this.rb.velocity *= 0.5f;
                if (time >= 1.0f)
                {
                    this.rb.velocity /= 0.5f;
                    break;
                }
                Debug.Log("속도 감소");
                yield return null;
            }
        }
        if (!isEnemy && isAlly)
        {
            while (true)
            {
                time += Time.deltaTime;
                this.rb.velocity *= 1.5f;
                if (time >= 1.0f)
                {
                    this.rb.velocity /= 1.5f;
                    break;
                }
                Debug.Log("속도 증가");
                yield return null;
            }
        }
        if(isAlly && isEnemy)
        {
            yield return null;
            Debug.Log("중첩 상태");
        }
    }
}
