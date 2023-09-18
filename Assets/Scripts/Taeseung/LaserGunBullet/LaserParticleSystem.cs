using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserParticleSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particlePrefab;
    public List<ParticleSystem> l_particleSystem;

    [SerializeField]
    private ELaserGunType gunType;


    private void Update()
    {
        if (isparticleStop() && gunType == ELaserGunType.Bullet)
        {
            Destroy(this.gameObject);
        }
    }


    public void particlePlay()
    {
        for (int i = 0; i < l_particleSystem.Count; i++) {
            l_particleSystem[i].Play();
         }
    }

    public void particleStop()
    {
        for (int i = 0; i < l_particleSystem.Count; i++)
        {
            l_particleSystem[i].Stop();
        }
    }

    public bool isparticleStart()
    {
        return l_particleSystem[0].isPlaying;
    }

    public bool isparticleStop()
    {
        return l_particleSystem[0].isStopped;
    }

    public (GameObject, LaserParticleSystem) particleInstantiate()
    {
        GameObject newObject = Instantiate(particlePrefab);
        LaserParticleSystem newParticleSystem = newObject.GetComponent<LaserParticleSystem>();
        return (newObject, newParticleSystem);
    }

    public (GameObject,LaserParticleSystem) particleInstantiate(Vector3 location, Quaternion rotation)
    {
        GameObject newObject = Instantiate(particlePrefab, location, rotation, null);
        LaserParticleSystem newParticleSystem = newObject.GetComponent<LaserParticleSystem>();
        return (newObject, newParticleSystem);
    }

    public void particleDestroy(GameObject particle)
    {
        Destroy(particle);
    }


}
