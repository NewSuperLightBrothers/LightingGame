using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserParticleSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ParticlePrefab;
    public List<ParticleSystem> l_particlesystem;
    [SerializeField] private LaserGunType _guntype;


    private void Update()
    {
        if (isparticleStop() && _guntype == LaserGunType.Bullet)
        {
            Destroy(this.gameObject);
        }
    }


    public void particleplay()
    {
        for (int i = 0; i < l_particlesystem.Count; i++) {
            l_particlesystem[i].Play();
         }
    }

    public void particlestop()
    {
        for (int i = 0; i < l_particlesystem.Count; i++)
        {
            l_particlesystem[i].Stop();
        }
    }

    public bool isparticleStart()
    {
        return l_particlesystem[0].isPlaying;
    }

    public bool isparticleStop()
    {
        return l_particlesystem[0].isStopped;
    }

    public (GameObject, LaserParticleSystem) particleInstantiate()
    {
        GameObject newobject = Instantiate(ParticlePrefab);
        LaserParticleSystem newparticlesystem = newobject.GetComponent<LaserParticleSystem>();
        return (newobject, newparticlesystem);
    }

    public (GameObject,LaserParticleSystem) particleInstantiate(Vector3 Location, Quaternion Rotation)
    {
        GameObject newobject = Instantiate(ParticlePrefab, Location, Rotation, null);
        LaserParticleSystem newparticlesystem = newobject.GetComponent<LaserParticleSystem>();
        return (newobject, newparticlesystem);
    }

    public void particleDestroy(GameObject particle)
    {
        Destroy(particle);
    }


}
