Shader "Custom/SpriteMultiplyCameraRGB"
{
    Properties
    {
        _Color("Tint", Color) = (1,1,1,1)
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _CameraMultiply("Camera Multiply", Color) = (1,1,1,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float2 myVector;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _CameraMultiply;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 cameraColor = _CameraMultiply;
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= cameraColor.rgb;
                col *= i.color;

                return col;
                
            }
            ENDCG
        }
    }
}
