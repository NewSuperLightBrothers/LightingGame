using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audios;
    private bool istart = false;

    // Update is called once per frame
    void Update()
    {
        if (audios.isPlaying && istart == false)
            istart = true;

        if (istart && !audios.isPlaying)
            Destroy(this.gameObject);
    }


}
