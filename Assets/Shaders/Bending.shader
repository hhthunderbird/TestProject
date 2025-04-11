Shader "Custom/Bending"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (0,0,0,0)
        // _HorizontalCurvature ("Horizontal Curvature", Range(-0.005, 0.005)) = 0.001
        // _VerticalCurvature ("Vertical Curvature", Range(-0.005, 0.005)) = 0.001
        _CurvatureOffset ("Curvature Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            float4 _Tint;
            float _HorizontalCurvature;
            float _VerticalCurvature;
            float _Origin;

            v2f vert (appdata v)
            {
                v2f o;
                
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                float distanceFromCamera = worldPos.z - _Origin;
                float distanceFactor = distanceFromCamera * distanceFromCamera;
                worldPos.y -= _VerticalCurvature * distanceFactor;
                worldPos.x -= _HorizontalCurvature * distanceFactor;
                
                o.vertex = mul(UNITY_MATRIX_VP, worldPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Tint;
            }
            ENDCG
        }
    }
}