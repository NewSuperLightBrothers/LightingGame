using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textUI;

    public static TestUI testUI;

    private void Start()
    {
        testUI = this;

    }


    // Update is called once per frame
    public void setText(string newtext)
    {
       textUI.text = newtext;
    }
}
