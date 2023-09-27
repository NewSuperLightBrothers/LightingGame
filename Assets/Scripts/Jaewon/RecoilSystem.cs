using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilSystem : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private IEnumerator ReCoil(float vertiacalForce , float horizontalForce)
    {
        while (true)
        {
            Vector3 _currentForward = player.transform.forward;
            yield return null;
        }
    }
}
