Shader "Custom/MyShader"
{
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        [Toggle] _Invincible ("Invincible enable", float)=0
        _IColor ("Invincible Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        fixed4 _Color;
        bool _Invincible;
        fixed4 _IColor;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex)*_Color.a;
            if(_Invincible || _Color.b==0){
                c=c+abs(cos(_Time.w))*_IColor;
            }
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Transparent/VertexLit"
}
