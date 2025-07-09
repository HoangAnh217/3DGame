Shader "Custom/DissolveShaderURP"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0, 0, 1)
        _Metallic ("Metallic", Range(0, 1)) = 0.5
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Metallic;
            float _Smoothness;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // Lấy màu từ texture và nhân với màu
                half4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Tính toán ánh sáng
                half3 albedo = texColor.rgb;
                half alpha = texColor.a;

                // Tạo output lighting model
                SurfaceData surface;
                InitializeStandardLitSurface(surface);
                surface.Albedo = albedo;
                surface.Metallic = _Metallic;
                surface.Smoothness = _Smoothness;

                // Gán alpha (cho phép transparency nếu cần)
                surface.Alpha = alpha;

                return UniversalFragmentPBR(surface);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/Lit"
}
