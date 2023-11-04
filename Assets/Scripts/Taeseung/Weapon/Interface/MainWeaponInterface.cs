using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MainWeaponInterface 
{
    /// <summary>
    /// 총알이 날아갈 궤적, 수류탄이 날아갈 궤적, 검의 공격 범위 체크 등 공격 범위를 계산할 때 쓰는 함수
    /// </summary>
    public void CheckAttackRange();

    /// <summary>
    /// 게이지 셋팅
    /// </summary>
    /// <param name="newval"></param>
    public void SetWeaponGauge(int newval);

    /// <summary>
    /// 게이지량 가져오기
    /// </summary>
    /// <returns></returns>
    public int GetWeaponGauge();

    public int GetBulletCount();
    public int GetBulletMaxCount();
}
