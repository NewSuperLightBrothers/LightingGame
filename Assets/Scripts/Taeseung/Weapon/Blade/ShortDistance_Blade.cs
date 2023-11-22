using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortDistance_Blade : ShortDistanceWeaponManager, WeaponInterface
{
    [SerializeField] private int _bladeAniMotionCount;
    [SerializeField] private Collider _bladeCollider;
    private int count = 0;

    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // taehyeon : 이후 이 부분에 bullet의 endpoint를 전달해야 함
                StartAttack(Vector3.zero);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("헉!");   
    }



    public void StartAttack(Vector3 endPoint)
    {
        // taehyeon : 동적으로 카메라가 바라보고 있는 중앙 위치에 endPoint를 계산하여 동적으로 전달해야 함
        
        count++;
        if (count == _bladeAniMotionCount)
            count = 1;
        SD_weaponAttackAnimation.GetValue("BladeAttack").SetInteger("AnimationValue", count);
    }

    public int GetWeaponGauge() => _weaponRemainGauge;
    public void SetWeaponGauge(int newval) => _weaponRemainGauge += newval;

    public void Reloading()
    {
        throw new System.NotImplementedException();
    }
}
