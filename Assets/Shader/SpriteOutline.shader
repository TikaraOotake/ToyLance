Shader "Unlit/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.01
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 원본 텍스처 샘플링
                fixed4 mainColor = tex2D(_MainTex, i.uv);

                // 외곽선 샘플링
                float outlineAlpha = 0;
                float2 up = float2(0, _MainTex_TexelSize.y * _OutlineThickness);
                float2 right = float2(_MainTex_TexelSize.x * _OutlineThickness, 0);

                outlineAlpha = max(outlineAlpha, tex2D(_MainTex, i.uv + up).a);
                outlineAlpha = max(outlineAlpha, tex2D(_MainTex, i.uv - up).a);
                outlineAlpha = max(outlineAlpha, tex2D(_MainTex, i.uv + right).a);
                outlineAlpha = max(outlineAlpha, tex2D(_MainTex, i.uv - right).a);
                
                // 외곽선 색상 적용 (원본이 투명한 부분에만)
                fixed4 outlineColor = _OutlineColor;
                outlineColor.a *= saturate(outlineAlpha - mainColor.a);
                
                // 원본 색상과 외곽선 색상 합성
                return lerp(i.color * mainColor, outlineColor, outlineColor.a);
            }
            ENDCG
        }
    }

}
