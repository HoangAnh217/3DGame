Shader "Custom/ToonEnemy"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 0, 0, 1) // Màu chính của enemy
        _RimColor ("Rim Light Color", Color) = (1, 1, 1, 1) // Màu sáng ở viền
        _RimIntensity ("Rim Intensity", Range(0, 1)) = 0.5 // Độ mạnh viền sáng
        _LightIntensity ("Light Intensity", Range(0, 1)) = 0.5 // Cường độ ánh sáng
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            float4 _MainColor;
            float4 _RimColor;
            float _RimIntensity;
            float _LightIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Ánh sáng chính (Fake Light)
                float lightFactor = saturate(dot(i.normal, float3(0,1,0)));
                lightFactor = step(_LightIntensity, lightFactor); // Làm sắc nét toon shading

                // Hiệu ứng ánh sáng viền (Rim Light)
                float rim = 1.0 - saturate(dot(i.viewDir, i.normal));
                rim = smoothstep(1.0 - _RimIntensity, 1.0, rim); // Làm mềm vùng viền sáng

                // Kết hợp màu chính + Rim Light
                fixed4 col = _MainColor * lightFactor + _RimColor * rim;
                return col;
            }
            ENDCG
        }
    }
}
