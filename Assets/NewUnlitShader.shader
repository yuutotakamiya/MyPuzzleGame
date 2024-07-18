Shader "Custom/WaveShader"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _WaveScale("Wave Scale", Float) = 0.1
        _WaveSpeed("Wave Speed", Float) = 1.0
    }
        SubShader
        {
            Tags {"Queue" = "Geometry"}
            LOD 100

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
                float4 _MainTex_ST;
                float4 _Color;
                float _WaveScale;
                float _WaveSpeed;

                v2f vert(appdata v)
                {
                    v2f o;
                    float wave = sin(v.vertex.x * _WaveScale + _Time.y * _WaveSpeed) * _WaveScale;
                    v.vertex.y += wave;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                    return col;
                }
                ENDCG
            }
        }
}
