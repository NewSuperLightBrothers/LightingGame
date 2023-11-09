#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

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

void DepthSobel_float(float2 UV, float thickness, out float Out) {
    float2 sobel = 0;

    [unroll] for(int i = 0; i < 9; i++) {
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + l_sobelSamplePointCS[i] * thickness);
        sobel += depth * float2(l_sobelXCS[i], l_sobelYCS[i]);
    }
    Out = length(sobel);
}


#endif