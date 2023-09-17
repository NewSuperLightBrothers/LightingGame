using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//weapon���� �ֻ��� Ŭ����
public abstract class LaserGunWeaponSystem : MonoBehaviour
{
    [SerializeField]
    protected LaserTeamType Team;
    [SerializeField]
    protected float _emissionstrength;
    protected Color _materialcolor;

    private void Awake()
    {
        TakeTeamInfo();
        getTeamColor();
    }

    protected Color getTeamColor()
    {
        if (Team == LaserTeamType.Red) _materialcolor = Color.red;
        else if (Team == LaserTeamType.Blue) _materialcolor = Color.blue;
        else if (Team == LaserTeamType.Green) _materialcolor = Color.green;

        return _materialcolor;
    }
    protected abstract void SetObjectTeamColor(Color color, float emissionstrength);


    //������ �ܺ� ������ ���� �� ������ �޾ƿ� �� �ִ� �Լ�
    private void TakeTeamInfo() { }


}



//LaserGunWeaponSystem/LaserGunWeaponShootingSystem/LaserGunManager/LaserGunShootingManager
public abstract class LaserGunWeaponShootingSystem : LaserGunWeaponSystem
{
    [SerializeField]
    protected Guninfo _guninfo;

    //���ݻ�Ÿ�
    [SerializeField]
    protected float _distance;
    [SerializeField]
    //������
    protected Vector3 _direction;


    //���� ���� źâ ������ ����
    protected float _currentbulletcount = 0;
    //cooltime ����
    protected float _cooltimeinterval = 0f;
    //��� ������ �ƴ��� Ȯ��
    protected bool _isshoot = true;


    //�ѱ��� ����(�Ǵ� firepoint�κ���) ������ ray
    protected Ray _ray;
    //ray�� hit�� ��ü��
    protected RaycastHit[] _hits;

    protected void Start()
    {
        //��Ÿ� �ʱ�ȭ
        _distance = Vector3.Distance(_guninfo.firepoint.position, _guninfo.endpoint.position);
        print(_distance);

        //���� ���� �ʱ�ȭ
        _direction = _guninfo.endpoint.position - _guninfo.firepoint.position;
    }

    
    protected abstract void BulletFire();
    protected abstract void BulletRayCast();

}