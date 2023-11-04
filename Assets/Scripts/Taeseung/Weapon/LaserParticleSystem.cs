using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserParticleSystem : MonoBehaviour
{
    public GameObject particlePrefab;
    public List<ParticleSystem> l_particleSystem;

    [SerializeField] private ELaserGunType _gunType;


    private void Update()
    {
        if (IsParticleStop() && _gunType == ELaserGunType.Bullet)
        {
            Destroy(this.gameObject);
        }
    }


    public void ParticlePlay()
    {
        for (int i = 0; i < l_particleSystem.Count; i++) {
            l_particleSystem[i].Play();
        }
    }

    public void ParticleStop()
    {
        for (int i = 0; i < l_particleSystem.Count; i++)
        {
            l_particleSystem[i].Stop();
        }
    }

    public bool IsParticleStart()
    {
        return l_particleSystem[0].isPlaying;
    }

    public bool IsParticleStop()
    {
        return l_particleSystem[0].isStopped;
    }

    public (GameObject, LaserParticleSystem) ParticleInstantiate()
    {
        GameObject newObject = Instantiate(particlePrefab);
        LaserParticleSystem newParticleSystem = newObject.GetComponent<LaserParticleSystem>();
        return (newObject, newParticleSystem);
    }

    public (GameObject,LaserParticleSystem) ParticleInstantiate(Vector3 location, Quaternion rotation)
    {
        GameObject newObject = Instantiate(particlePrefab, location, rotation, null);
        LaserParticleSystem newParticleSystem = newObject.GetComponent<LaserParticleSystem>();
        return (newObject, newParticleSystem);
    }

    public void ParticleColorSetting(Color color)
    {
        foreach(ParticleSystem i in l_particleSystem)
        {
            ParticleSystem.MainModule mainModule = i.main;
            mainModule.startColor = color;
        }
    }


    public void ParticleDestroy(GameObject particle)
    {
        Destroy(particle);
    }


}
