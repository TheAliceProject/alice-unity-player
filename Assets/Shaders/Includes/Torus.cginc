#ifndef TORUS_INCLUDE
#define TORUS_INCLUDE

#include "TorusVertex.cginc"

sampler2D _MainTex;

struct Input
{
    float2 uv_MainTex;
};

half _Glossiness;
half _Metallic;
fixed4 _Color;

void vert (inout appdata_full v, out Input o)
{
    v.vertex = TorusVertex(v.vertex, v.normal);
    UNITY_INITIALIZE_OUTPUT(Input,o);
    
}

void surf (Input IN, inout SurfaceOutputStandard o)
{
    // Albedo comes from a texture tinted by color
    fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = c.rgb;
    // Metallic and smoothness come from slider variables
    o.Metallic = _Metallic;
    o.Smoothness = _Glossiness;
    o.Alpha = c.a;
}

#endif //TORUS_INCLUDE