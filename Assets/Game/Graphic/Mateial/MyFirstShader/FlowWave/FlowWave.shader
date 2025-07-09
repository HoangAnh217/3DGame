Shader "Unlit/Flows"
{
    Properties
    {
        _MainTexture("Main Texture", 2D) = "white" {}
        _FLowText("Flow Texture",2D) ="white" {}
        _UVText("Uv Texture",2D) ="white" {}
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
            float4 _MainTexture_ST;

            sampler2D _FLowText;
            float4 _FLowText_ST;

            sampler2D _UVText;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTexture);
                o.uv.zw = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 uv = tex2D(_UVText,i.uv.xy);
                fixed4 flow = tex2D(_FLowText,uv.rg);
                fixed4 col = tex2D(_MainTexture,i.uv.xy);
                return flow;
            }
            ENDCG
        }
    }
}
