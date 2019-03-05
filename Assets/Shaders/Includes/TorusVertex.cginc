#ifndef TORUS_VERTEX_INCLUDE
#define TORUS_VERTEX_INCLUDE

UNITY_INSTANCING_BUFFER_START(Props)
    UNITY_DEFINE_INSTANCED_PROP(float, _RingRadiusDelta)
    UNITY_DEFINE_INSTANCED_PROP(float, _CrossRadiusDelta)
UNITY_INSTANCING_BUFFER_END(Props)

float4 TorusVertex(float4 vertex, float3 normal) {
    float3 centerNorm = vertex.xyz;
    centerNorm.y = 0;
    centerNorm = normalize(centerNorm);

    vertex.xyz += (centerNorm * UNITY_ACCESS_INSTANCED_PROP(Props, _RingRadiusDelta)) + (normal * UNITY_ACCESS_INSTANCED_PROP(Props, _CrossRadiusDelta));

    return vertex;
} 

#endif //TORUS_VERTEX_INCLUDE