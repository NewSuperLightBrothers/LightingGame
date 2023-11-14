using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Logger = Utils.Logger;

public class Target : NetworkBehaviour
{
    // host color will be plus score, client color will be minus score
    public NetworkVariable<int> score;
    public int threshold = 3;

    public ParticleSystem particleSystem;
    public MeshRenderer meshRenderer;
    
    public Material redMat;
    public Material blueMat;
    public Material defaultMat;

    private ParticleSystem.MainModule mainModule;
    
    private void Awake()
    {
        score.OnValueChanged += OnScoreChanged;
        mainModule = particleSystem.main;
    }

    private void OnScoreChanged(int previousvalue, int newvalue)
    {
        // if (newvalue > 0 && previousvalue <= 0)
        // {
        //     Utils.Logger.Log("host target");
        //     SetMatRed();
        // }
        // else if (newvalue < 0 && previousvalue >= 0)
        // {
        //     Utils.Logger.Log("client target");
        //     SetMatBlue();
        // }
        // else
        // {
        //     Utils.Logger.Log("median target");
        //     SetMatDefault();
        // }
    
        Logger.Log("target score changed / new : " + newvalue + " / prev : " + previousvalue);
        if(newvalue > 0) SetMatRed();
        else if(newvalue < 0) SetMatBlue();
        else SetMatDefault();
    }

    public void SetScore(int delta)
    {
        if(!IsServer) return;
        
        score.Value += delta;
        score.Value = Mathf.Clamp(score.Value, -threshold, threshold);
    }

    private void SetMatRed()
    {
        mainModule.startColor = new Color(0.9843137f, 0.2313726f, 0.145098f, 0.7176471f);
        meshRenderer.material = redMat;
    }

    private void SetMatBlue()
    {
        mainModule.startColor = new Color(0, 0.3607843f, 1, 0.7176471f);
        meshRenderer.material = blueMat;
    }
    
    private void SetMatDefault()
    {
        mainModule.startColor = new Color(0.8784314f, 0.8784314f, 0.8784314f, 0.7176471f);
        meshRenderer.material = defaultMat;
    }
}
