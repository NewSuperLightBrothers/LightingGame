using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Button gotoMainButton;

    private void Awake()
    {
        gotoMainButton.onClick.AddListener(() =>
        {
            Application.Quit(0);
        });
    }
}
