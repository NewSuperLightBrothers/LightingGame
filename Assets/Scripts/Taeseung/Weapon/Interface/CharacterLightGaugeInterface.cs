using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterLightGaugeInterface
{
    /// <summary>
    /// 총알이 날아갈 궤적, 수류탄이 날아갈 궤적, 검의 공격 범위 체크 등 공격 범위를 계산할 때 쓰는 함수
    /// </summary>
    public int getCharacterCurrentGauge();
    public int getCharacterMaxGauge();
}
