using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audios;
    private bool isStart = false;

    private void Update()
    {
        if (audios.isPlaying && isStart == false)
            isStart = true;

        if (isStart && !audios.isPlaying)
            Destroy(this.gameObject);
    }
}
