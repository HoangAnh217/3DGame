Shader "Unlit/MyFirstShader"
{
    Properties
    {
        _Color("Test Color", Color) = (1,1,1,1)
        _MainTexture("Main Texture", 2D) = "white" {}
        _NoiseMap("Noise Map", 2D) = "white" {}
        _RevealValue("Reveal Value", Range(0,1)) = 0
        _Feather("_Feather Value", float) = 0
        _ErodeColor("Erode Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
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
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTexture;
            sampler2D _NoiseMap;
            float4 _MainTexture_ST;
            float4 _NoiseMap_ST;
            float _RevealValue;
            float _Feather;
            float4 _ErodeColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTexture);
                o.uv.zw = TRANSFORM_TEX(v.uv, _NoiseMap);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTexture, i.uv.xy);
                fixed4 mask = tex2D(_NoiseMap, i.uv.zw);
                 float2 uv = i.uv;

                float gradient = (uv.x + uv.y) * 0.5;

                fixed4 te = fixed4(gradient, gradient, gradient, 1.0);
                float revealAmountTop = step(mask.r, _RevealValue + _Feather);
                float revealAmountBottom = step(mask.r, _RevealValue - _Feather);
                float revealDifference = revealAmountTop - revealAmountBottom;

                float3 finalCol = lerp(col.rgb, _ErodeColor.rgb, revealDifference);
                return fixed4(finalCol.rgb, col.a * revealAmountTop);
            }
            ENDCG
        }
    }
}
