Shader "Custom/GlassMaskShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _ClearTex("Clear Texture", 2D) = "white"{}
        _BlurTex("Blue Texture", 2D) = "white"{}
        _BlurScale("Blur Scale", Range(0, 1)) = 0
        _SampleOffset("Sample Offset", Vector) = (0, 0, 100, 0.479)
    }
        SubShader
    {
        Tags{
            "RenderPipeline" = "UniversalRenderPipeline"
            "RenderType" = "Opaque"
        }

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

        CBUFFER_START(UnityPerMaterial)

        half4 _MainTex_ST;
        half4 _ClearTex_ST;
        half4 _ClearTex_TexelSize;
        half4 _BlurTex_ST;
        float _BlurScale;
        float4 _SampleOffset;

        CBUFFER_END

        TEXTURE2D(_ClearTex);
        SAMPLER(sampler_ClearTex);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_BlurTex);
        SAMPLER(sampler_BlurTex);

        struct a2v
        {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD;
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 worldPos : TEXCOORD1;
        };

        ENDHLSL

        Pass{
            Tags{ "LightMode" = "Universal2D" }
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            v2f vert(a2v v)
            {
                v2f o;
                o.uv = v.texcoord;
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                return o;
            }

            half4 frag(v2f i) :SV_TARGET
            {
                float2 size = _ClearTex_TexelSize.zw / _SampleOffset.z;
                float2 uv = 0.5 * (i.worldPos.xy + _SampleOffset.xy) / size + float2(_SampleOffset.w, 0.5);
                half4 main_tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                half4 clear_tex = SAMPLE_TEXTURE2D(_ClearTex, sampler_ClearTex, uv);
                half4 blur_tex = SAMPLE_TEXTURE2D(_BlurTex, sampler_BlurTex, uv);
                return half4((1 - _BlurScale) * clear_tex.rgb + _BlurScale * blur_tex.rgb, main_tex.a);
            }

            ENDHLSL
        }
    }
}
