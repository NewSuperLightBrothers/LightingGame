#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

#include "DecodeDepthNormals.hlsl"

//TEXTURE2D(_DepthNormalsTexture); SAMPLER(sampler_DepthNormalsTexture);

static float2 l_sobelSamplePointCS[9] = {
    float2(-1, 1), float2(0, 1), float2(1, 1),
    float2(-1, 0), float2(0, 0), float2(1, 0),
    float2(-1, -1), float2(0, -1), float2(1, -1),
};

static float l_sobelXCS[9] = {
    1, 0, -1,
    2, 0, -2,
    1, 0, -1,
};

static float l_sobelYCS[9] = {
    1, 2, 1,
    0, 0, 0,
    -1, -2, -1,
};

void DepthSobel_float(float2 uv, float thickness, out float Out) {
    float2 sobel = 0;

    [unroll] for(int i = 0; i < 9; i++) {
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv + l_sobelSamplePointCS[i] * thickness);
        sobel += depth * float2(l_sobelXCS[i], l_sobelYCS[i]);
    }
    Out = length(sobel);
}

void GetDepthAndNormal(float2 uv, out float depth, out float3 normal) {
    float4 coded = SAMPLE_TEXTURE2D(_DepthNormalsTexture, sampler_DepthNormalsTexture, uv);
    DecodeDepthNormal(coded, depth, normal);
}

void CalculateDepthNormal_float(float2 uv, out float depth, out float3 normal) {
    GetDepthAndNormal(uv, depth, normal);
    normal = normal * 2 - 1;
}


void DepthSobelNormal_float(float2 uv, float thickness, out float3 Out) {
    float2 sobelX = 0;
    float2 sobelY = 0;
    float2 sobelZ = 0;

    [unroll] for(int i = 0; i < 9; i++) {
        float depth;
        float3 normal;
        GetDepthAndNormal(uv + l_sobelSamplePointCS[i] * thickness, depth, normal);
        float2 kernel = float2(l_sobelXCS[i], l_sobelYCS[i]);

        sobelX += normal.x * kernel;
        sobelY += normal.y * kernel;
        sobelZ += normal.z * kernel;
    }

    Out = float3(length(sobelX), length(sobelY), length(sobelZ));
}


#endif