Shader "Alice/Transparent Torus"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        // Depth Pass
        Pass {
            ColorMask 0
            ZWrite On
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Includes/TorusVertex.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {
                float4 position : POSITION;
            };

            VertexOutput vert (VertexInput v)
            {
                v.vertex = TorusVertex(v.vertex, v.normal);
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(v);

                o.position = UnityObjectToClipPos(v.vertex.xyz);

                return o;
            }

            void frag (VertexOutput IN) {}

            ENDCG
        }
    
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "Includes/Torus.cginc"
        
        ENDCG
    }
    FallBack "Diffuse"
}
