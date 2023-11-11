#ifndef FLIP_UV_INLUDED
#define FLIP_UV_INCLUDED

void FlipUv_float(float2 uv, out float2 Out) {
#if UNITY_UV_STARTS_AT_TOP
if (_MainTex_TexelSize.y < 0)
        uv.y = 1-uv.y;
#endif
Out = uv;
}

#endif