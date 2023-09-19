using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _textUI;

    public static TestUI testUI;

    private void Start()
    {
        testUI = this;
    }

    public void setText(string newText)
    {
       _textUI.text = newText;
    }
}
