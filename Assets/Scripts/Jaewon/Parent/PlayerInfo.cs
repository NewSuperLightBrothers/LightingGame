using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�÷��̾��� ������ �ް�,�����մϴ�. �ʱⰪ�� Default�� �ʱ�ȭ �ص�.
public abstract class PlayerInfo
{
    #region ������
    protected GameObject player;

    protected float _playerHp;
    protected float _playerSpd;
    protected float _playerDfn;
    protected float _playerAtk;
    protected ELaserTeamType _team;
    #endregion

    //�÷��̾��� ������ ��ųʸ��� �����մϴ�. ���ڿ��� �ռ� �������� ������, float�� �������� ���Դϴ�. �⺻���� ���� 100�Դϴ�.
    protected Dictionary<string,float> _playerStat =  new Dictionary<string, float>();

    //���� �ʱ�ȭ�մϴ�. �⺻���� ��� 100�̸�, �⺻ ���� Red�Դϴ�.
    #region �ʱ�ȭ
    private void Awake()
    {
        DefaultStat(100,100,100,100);
        _team = ELaserTeamType.Red;
    }
    #endregion

    //��Ʈ��ũ ����� ���� �÷��̾� ������ ����� �޼ҵ带 �߻�ȭ�Ͽ����ϴ�.
    #region ���� ����
    protected abstract GameObject GetPlayerPrefab();
    protected Dictionary<string, float> DefaultStat(float hp, float spd, float dfn, float atk)
    {
        _playerStat.Add("_playerHp", hp);
        _playerStat.Add("_playerSpd", spd);
        _playerStat.Add("_playerDfn", dfn);
        _playerStat.Add("_playerAtk", atk);
        return _playerStat;
    }
    #endregion
}
