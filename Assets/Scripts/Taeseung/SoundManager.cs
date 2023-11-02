using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> audios;

    private Dictionary<string, AudioSource> dictionaryaudios;
    private bool isStart = false;



    private void Update()
    {




        if (audios[0].isPlaying && isStart == false)
            isStart = true;

        if (isStart && !audios[0].isPlaying)
            Destroy(this.gameObject);
     }



}