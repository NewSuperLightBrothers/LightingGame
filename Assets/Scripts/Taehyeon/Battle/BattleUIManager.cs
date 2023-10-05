using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _curTimeText;

    private void Update()
    {
        _curTimeText.text = BattleManager.Instance.curPlayTime.Value.ToString(CultureInfo.InvariantCulture);
    }
}
