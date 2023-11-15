using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BattleUIManager : Singleton<BattleUIManager>
{
    [SerializeField] private TMP_Text _curTimeText;
    [SerializeField] private TMP_Text redScore;
    [SerializeField] private TMP_Text blueScore;
    
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TMP_Text _resultText;
    
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
    
    public void ShowResult()
    {
        _resultPanel.SetActive(true);

        EObjectColorType WinnerColor;
        if(BattleManager.Instance.redTeamScore.Value > BattleManager.Instance.blueTeamScore.Value)
            WinnerColor = EObjectColorType.Red;
        else if(BattleManager.Instance.redTeamScore.Value < BattleManager.Instance.blueTeamScore.Value)
            WinnerColor = EObjectColorType.Blue;
        else
            WinnerColor = EObjectColorType.Black;

        if (WinnerColor == EObjectColorType.Black)
        {
            _resultText.text = "DRAW..";
            return;
        }
        
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < playerObjects.Length; i++)
        {
            if (playerObjects[i].GetComponent<NetworkObject>().IsOwner)
            {
                if(playerObjects[i].GetComponent<NCharacter>().teamColor.Value == WinnerColor)
                    _resultText.text = "WIN!!";
                else
                    _resultText.text = "LOSE..";
                return;
            }
            
        }
    }
}
