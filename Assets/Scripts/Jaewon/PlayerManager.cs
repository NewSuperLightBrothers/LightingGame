using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.Mathematics;
public class PlayerManager : MonoBehaviour
{
    public GameObject camArm;
    public GameObject playerablePlayer;
    public float playerAtk;
    public float playerHp;
    public float playerSpd;
    public Color teamColor;
    private void OnEnable()
    {
        PlayerStats player = new PlayerStats();
        playerablePlayer = player.playerablePlayer = this.gameObject;
        playerHp = player.playerHp = 100;
        playerSpd = player.playerSpd = 100;
        playerAtk = player.playerAtk = 100;
        teamColor = player.teamColor = Color.red;
        Debug.Log(playerHp);
    }
}