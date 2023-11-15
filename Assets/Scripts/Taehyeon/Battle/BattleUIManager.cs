using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class BattleUIManager : Singleton<BattleUIManager>
{
    [SerializeField] private TMP_Text _curTimeText;
    [SerializeField] private TMP_Text redScore;
    [SerializeField] private TMP_Text blueScore;
    
    private void Update()
    {
        ShowCurTime();
        // _curTimeText.text = BattleManager.Instance.curPlayTime.Value.ToString("F1");
    }

    private void ShowCurTime()
    {
        float min = (int)BattleManager.Instance.curPlayTime.Value / 60;
        float sec = BattleManager.Instance.curPlayTime.Value % 60;
        
        _curTimeText.text = $"{min.ToString("F0")}:{sec.ToString("F0")}";
    }

    public void UpdateScore(int red, int blue)
    {
        redScore.text = red.ToString();
        blueScore.text = blue.ToString();
    }
}
