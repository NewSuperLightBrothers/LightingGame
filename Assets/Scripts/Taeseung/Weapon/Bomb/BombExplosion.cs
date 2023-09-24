using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    [SerializeField] private float _maxExplosionSize;
    [SerializeField] private float _explosionTime;
    [SerializeField] Animator _explosionAnimation;

    private bool _isMaxsize = false;
    private float _time = 0;
    private float _anitime = 0;

    private void Start()
    {
        _explosionAnimation.Play(0);
        _anitime = _explosionAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        //_explosionAnimation.GetCurrentAnimatorClipInfo(0)[0].clip;
    }

    private void FixedUpdate()
    {
        _time += Time.deltaTime;

        if (_time > _anitime) Destroy(this.gameObject);

 
    }


}
