Shader "LightweightPipeline/Alice/Standard Torus Multipass Depth "
{
    Properties
    {
        // Specular vs Metallic workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        _Color("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex("Albedo", 2D) = "white" {}

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _Parallax("Height Scale", Range(0.005, 0.08)) = 0.02
        _ParallaxMap("Height Map", 2D) = "black" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _DetailMask("Detail Mask", 2D) = "white" {}

        _DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
        _DetailNormalMapScale("Scale", Float) = 1.0
        _DetailNormalMap("Normal Map", 2D) = "bump" {}

        [Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        // torus
        [HideInInspector] _RingRadiusDelta("__ringDelta", Float) = 0.0
        [HideInInspector] _CrossRadiusDelta("__crossDelta", Float) = 0.0
    }

    
    SubShader
    {
        // Lightweight Pipeline tag is required. If Lightweight pipeline is not set in the graphics settings
        // this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
        // material work with both Lightweight Pipeline and Builtin Unity Pipeline
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "LightweightPipeline" "IgnoreProjector" = "True"}
        LOD 300

        HLSLINCLUDE

        #include "LWRP/ShaderLibrary/InputSurfacePBR.hlsl"

        struct VertexInputTorus
        {
            float4 vertex     : POSITION;
            float3 normal       : NORMAL;
            float2 texcoord     : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };



        float _RingRadiusDelta;
        float _CrossRadiusDelta;

        float4 TorusVertex(float4 vertex, float3 normal) {
            float3 centerNorm = vertex.xyz;
            centerNorm.y = 0;
            centerNorm = normalize(centerNorm);

            vertex.xyz += (centerNorm * _RingRadiusDelta) + (normal * _CrossRadiusDelta);

            return vertex;
        } 
        ENDHLSL

        Pass
        {
            Name "Depth Fill"
            Tags{"LightMode" = "SRPDefaultUnlit"}
 
            Cull [_Cull]
            ZTest LEqual
            ZWrite On
            ColorMask 0
 
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
 
            #pragma multi_compile_instancing
           
            #pragma vertex vert
            #pragma fragment frag
 
            #include "LWRP/ShaderLibrary/Core.hlsl"
 
            struct VertexInput
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
            struct VertexOutput
            {
                float4 position : POSITION;
            };
 
            VertexOutput vert (VertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(v);
 
                o.position = TransformObjectToHClip(v.vertex.xyz);
 
                return o;
            }
 
            void frag (VertexOutput IN) {}
            ENDHLSL
        }

        // ------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
            // Lightmode matches the ShaderPassName set in LightweightPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Lightweight Pipeline
            Tags{"LightMode" = "LightweightForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            // All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _OCCLUSIONMAP

            #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature _GLOSSYREFLECTIONS_OFF
            #pragma shader_feature _SPECULAR_SETUP

            // -------------------------------------
            // Lightweight Pipeline keywords
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _VERTEX_LIGHTS
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma multi_compile _ FOG_LINEAR FOG_EXP2
            #pragma multi_compile _ _SHADOWS_ENABLED

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex LitPassVertexTorus
            #pragma fragment LitPassFragment

            
            #include "LWRP/ShaderLibrary/LightweightPassLit.hlsl"

            

            LightweightVertexOutput LitPassVertexTorus(LightweightVertexInput v)
            {
                v.vertex = TorusVertex(v.vertex, v.normal); 
                return LitPassVertex(v);
            }
            ENDHLSL
        }

        Pass
        {
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma vertex ShadowPassVertexTorus
            #pragma fragment ShadowPassFragment

            #include "LWRP/ShaderLibrary/LightweightPassShadow.hlsl"

            VertexOutput ShadowPassVertexTorus(VertexInput v)
            {
                v.position = TorusVertex(v.position, v.normal); 
                return ShadowPassVertex(v);
            }

            ENDHLSL
        }

        Pass
        {
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertexTorus
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "LWRP/ShaderLibrary/InputSurfacePBR.hlsl"
            #include "LWRP/ShaderLibrary/LightweightPassDepthOnly.hlsl"

            VertexOutput DepthOnlyVertexTorus(VertexInputTorus IN)
            {
                VertexInput v = (VertexInput)0;
                v.position = TorusVertex(IN.vertex, IN.normal);
                v.texcoord = IN.texcoord;
                return DepthOnlyVertex(v);
            }

            ENDHLSL
        }

        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Tags{"LightMode" = "Meta"}

            Cull Off

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex LightweightVertexMeta
            #pragma fragment LightweightFragmentMeta

            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature EDITOR_VISUALIZATION

            #pragma shader_feature _SPECGLOSSMAP

            #include "LWRP/ShaderLibrary/InputSurfacePBR.hlsl"
            #include "LWRP/ShaderLibrary/LightweightPassMetaPBR.hlsl"

            ENDHLSL
        }

    }
    FallBack "Hidden/InternalErrorShader"
    CustomEditor "LightweightStandardGUI"
}