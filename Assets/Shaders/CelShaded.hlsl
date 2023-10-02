#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
struct EdgeConstants {
    float shadowAttenuation;
    float distanceAttenuation;
    float diffuse;
    float specular;
    float specularOffset;
    float rim;
    float rimOffset;

};
struct SurfaceVariables {
    float smoothness;
    float shininess;
    float rimThreshold;
    float3 normal;
    float3 view;
    EdgeConstants ec;
};


float4 CalculateCelShading(Light l, SurfaceVariables s) {
    float sA = smoothstep(0, s.ec.shadowAttenuation, l.shadowAttenuation);
    float dA = smoothstep(0, s.ec.distanceAttenuation, l.distanceAttenuation);
    float attenuation = sA * dA;

    float diffuse = saturate(dot(s.normal, l.direction));
    diffuse *= attenuation;

    float3 h = SafeNormalize(l.direction + s.view);
    float specular = saturate(dot(s.normal, h));
    specular = pow(specular, s.shininess);
    specular *= diffuse * s.smoothness;

    float rim = 1 - dot(s.view, s.normal);
    rim *= pow(diffuse, s.rimThreshold);

    diffuse = smoothstep(0, s.ec.diffuse, diffuse);
    specular = s.smoothness * smoothstep((1 - s.smoothness) * s.ec.specular + s.ec.specularOffset, s.ec.specular + s.ec.specularOffset, specular);
    rim = s.smoothness * smoothstep(s.ec.rim - 0.5f * s.ec.rimOffset, s.ec.rim + 0.5f * s.ec.rimOffset, rim);

    return float4(l.color * (diffuse + max(specular, rim)), diffuse + max(specular, rim));
}
#endif

void LightingCelShaded_float(float Smoothness, float RimThreshold, float3 Position, float3 Normal, float3 View, 
    float EdgeDiffuse, float EdgeSpecular, float EdgeSpecularOffset,
    float EdgeDistanceAttenuation, float EdgeShadowAttenuation, float EdgeRim, float EdgeRimOffset, out float4 Color) {
    SurfaceVariables s;
    s.normal = Normal;
    s.view = SafeNormalize(View);
    s.smoothness = Smoothness;
    s.shininess = exp2(10 * Smoothness + 1);
    s.rimThreshold = RimThreshold;

    s.ec.diffuse = EdgeDiffuse;
    s.ec.specular = EdgeSpecular;
    s.ec.specularOffset = EdgeSpecularOffset;
    s.ec.distanceAttenuation = EdgeDistanceAttenuation;
    s.ec.shadowAttenuation = EdgeShadowAttenuation;
    s.ec.rim = EdgeRim;
    s.ec.rimOffset = EdgeRimOffset;

#if SHADOWS_SCREEN
    float4 clipPos = TransformWorldToHClip(Position);
    float4 shadowCoord = ComputeScreenPos(clipPos);
#else
    float4 shadowCoord = TransformWorldToShadowCoord(Position);
#endif

    Light light = GetMainLight();
    light.shadowAttenuation = MainLightRealtimeShadow(shadowCoord);
    Color = CalculateCelShading(light, s);

    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; i++) {
        light = GetAdditionalLight(i, Position, 1);
        Color += CalculateCelShading(light,s);
    }
}
#endif