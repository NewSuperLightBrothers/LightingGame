using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponInterface
{
    /// <summary>
    /// 리로딩
    /// </summary>
    public void Reloading();

    /// <summary>
    /// 총알 발사, 베기 등 공격을 시작할려고 할 때 쓰는 함수
    /// </summary>
    public void StartAttack();

}
