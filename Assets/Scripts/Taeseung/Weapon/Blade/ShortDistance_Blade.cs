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
                StartAttack();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        print("헉!");   
    }



    public void CheckAttackRange()
    {
        throw new System.NotImplementedException();
    }

    public void StartAttack()
    {
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
