using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class RelativeButtonShow : MonoBehaviour
{
    public Transform button; // 버튼 오브젝트의 Transform 컴포넌트

    private bool isbuttonActive = false; // 버튼이 활성화되었는지 여부

    private void Start()
    {

    }

    private void Update()
    {
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // 가장 최근 터치 입력(손가락)을 사용

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // 터치가 시작될 때 버튼을 활성화
                    isbuttonActive = true;
                    button.gameObject.SetActive(true);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // 터치가 종료되면 버튼을 비활성화
                    isbuttonActive = false;
                    button.gameObject.SetActive(false);
                    break;
            }
        }
    }
}