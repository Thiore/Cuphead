Shader "Unlit/FXOverlay"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DustTex("FX Texture", 2D) = "white" {}
        _Dust2Tex("FX2 Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
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
            sampler2D _DustTex;
            sampler2D _Dust2Tex;
            float4 _MainTex_ST;
            float4 _DustTex_ST;
            float4 _Dust2Tex_ST;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_MainTex, i.uv);

                float2 dustUV = i.uv;
                fixed4 dustColor = tex2D(_DustTex, dustUV);
               
                float2 dust2UV = i.uv;
                fixed4 dust2Color = tex2D(_Dust2Tex, dust2UV);

                if (dust2Color.a < 0.9)
                {
                    baseColor.rgb = (1 / dust2Color.a) * 0.8;
                }

                baseColor.rgb *= dustColor.rgb;

                baseColor.rgb *= dustColor.a;

                return baseColor;
            }
            ENDCG
        }
    }
}
