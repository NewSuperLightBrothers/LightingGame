using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageManager : MonoBehaviour
{
    public GameObject camArm;
    public Button fireButton;
    public GameObject jumpButton;
    public RectTransform joystickForeGround;
    public Canvas inputCanvas;
    public GameObject player;
    [SerializeField] bool isOwner;

    private void SetObj(GameObject player,bool isOwner)
    {
        if (isOwner)
        {
            inputCanvas.gameObject.SetActive(true);
            jumpButton.gameObject.SetActive(true);
            joystickForeGround.gameObject.SetActive(true);
            fireButton.gameObject.SetActive(true);
            Debug.Log("인풋매니저 = " + player.GetComponentInChildren<InputManager>().transform);
            GameObject camPredfab = GameObject.Instantiate(camArm, player.GetComponentInChildren<PlayerManager>().transform);
            player.GetComponentInChildren<InputManager>().joystickForeground = joystickForeGround;
            player.GetComponentInChildren<InputManager>().cameraAnchor = camPredfab.transform;
        }
    }
    private void Start()
    {
        Debug.Log("접근");
        SetObj(player,isOwner);
    }
}