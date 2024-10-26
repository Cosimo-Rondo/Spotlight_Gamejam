Shader "CustomRenderTexture/Mask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
        _CanvasSize("CanvasSize", Vector) = (1920, 1080, 0, 0)
        _CanvasPivot("CanvasPivot", Vector) = (0, 0, 0, 0)
        _Alpha ("Alpha", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Mask"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            float4 _MainTex_ST;
            sampler2D   _MainTex;
            float4 _CanvasSize;
            float4 _CanvasPivot;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(float4 sp:VPOS) : COLOR
            {
                float2 uv = (sp.xy - _CanvasPivot.xy * _ScreenParams.xy) / _CanvasSize.xy + float2(0.5, 0.5);
                float4 color = tex2D(_MainTex, uv * _MainTex_ST.xy + _MainTex_ST.zw) * _Color;
                color.a *= _Alpha;
                return color;
            }
            ENDCG
        }
    }
}
