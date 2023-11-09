#ifndef FLIP_UV_INLUDED
#define FLIP_UV_INCLUDED

void FlipUVInGame_float(float4 UV, out float4 Out) {
Out = UV;
#if UNITY_UV_STARTS_AT_TOP
    Out.y = -UV.y;
#endif
}

#endif