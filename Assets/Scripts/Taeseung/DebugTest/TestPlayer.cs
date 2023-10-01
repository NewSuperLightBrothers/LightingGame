using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPlayer : Singleton<TestPlayer>
{
    public float testHP;
    public GameObject Gun;
    public BombShootingManager bomb;

    public int bombmode = 0;

    public void CreateBomb()
    {
        if (bombmode == 0)
        {
            Gun.SetActive(false);
            bomb.enabled = true;
            bombmode = 1;
        }
        else if(bombmode == 1)
        {
            Gun.SetActive(true);
            bomb.enabled = false;
            bombmode = 0;
        }

    }



}
