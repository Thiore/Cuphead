Shader "Unlit/FXOverlay"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DustTex("FX Texture", 2D) = "white" {}
        _DustFrame("Dust Frame", float) = 0
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
            float4 _MainTex_ST;
            float4 _DustTex_ST;
            float _DustFrame;

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
                dustUV.y += _DustFrame / 127.0;
                fixed4 dustColor = tex2D(_DustTex, dustUV);

                return (baseColor*0.9) * dustColor;
            }
            ENDCG
        }
    }
}
