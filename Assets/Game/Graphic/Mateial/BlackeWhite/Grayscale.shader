﻿Shader "Custom/GrayScaleWithAlpha"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Lấy màu từ texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Kiểm tra alpha (pixel trong suốt)
                if (col.a == 0)
                {
                    discard; // Bỏ qua pixel trong suốt
                }

                // Tính giá trị grayscale
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));

                // Gán lại màu với alpha giữ nguyên
                return fixed4(gray, gray, gray, col.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
    