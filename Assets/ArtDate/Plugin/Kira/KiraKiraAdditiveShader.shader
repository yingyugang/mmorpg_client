Shader "Custom/KiraKiraAdditiveShader" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_TintColor2 ("Tint Color2", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_WaveScale ("Wave Scale",Float) = 1
		_WaveOffset ("Wave Offset",Float) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		
		Pass
		{
			Blend SrcAlpha One
			//AlphaTest Greater .01
			ColorMask RGB
			Cull Off Lighting Off ZWrite Off
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _TintColor;
			fixed4 _TintColor2;
			float _WaveScale;
			float _WaveOffset;
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;//TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				float t = frac((_Time.y+_WaveOffset) * _WaveScale);
				//t = sin(t * _WaveScale + _WaveOffset) * 0.5 + 0.5;
				t = (t>0.5?1-t:t) * 2;
				//t = smoothstep(t,0,1);
				fixed4 tint = lerp(_TintColor,_TintColor2,t);
				return 2.0 * tint * tex2D(_MainTex, i.texcoord);
			}

			ENDCG
		}
		
	} 
}
