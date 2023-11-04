using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEmission : MonoBehaviour
{
    [SerializeField] private EObjectColorType objectColor;
    [SerializeField] private MeshRenderer _meshRenderer;
    private Material _meshMaterial;
    private Vector2 _textureOffset;

    float delta = 1 / 8f;
    float gauge = 100;


    /*
     * Y축 0.25이상, 공차 1/8을 갖고 마젠타->파랑->시안->초록->노랑->빨강 순으로 색 팔레트 이동
     * X축 0.75이하, 공차 1/8을 갖고 7단계의 색 감소 이동
     * X축 0.875는 하양 검정 팔레트, Y축으로 올라갈수록 8단계로 색이 어두워짐 (최대 어둡기 0.875, 더 어둡게 하고 싶으면 이미션 끄면 됩니다.
     */



    void Start()
    {
        _meshMaterial = _meshRenderer.material;
        _textureOffset = new();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gauge -= 0.5f;


        /*
        if (_textureOffset.x > 0)
        {
            _textureOffset.x -= delta;
            print("!!");
        }*/


        

        _meshMaterial.mainTextureOffset = _textureOffset;
        _meshRenderer.material = _meshMaterial;
        




    }
}
