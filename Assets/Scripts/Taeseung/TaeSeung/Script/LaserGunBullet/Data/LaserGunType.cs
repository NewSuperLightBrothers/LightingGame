using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격 타입 (타입에 따라 파티클 처리 방식이 달라지기 때문, LaserParticleSystem에서 각 파티클마다 설정)
public enum LaserGunType
{
    // Start is called before the first frame update
    Beam,       //빔
    Bullet,     //탄알
    Blade,      //근접공격
}


//팀 타입 (타입에 따라 무기 색상 변경)
public enum LaserTeamType
{
    Red,
    Green,
    Blue
}