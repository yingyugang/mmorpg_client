// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BowShader"
{
	Properties
	{
	    _curlR ("curlR", Float) = 5.0 
		_MainTex ("Texture", 2D) = "white" {}
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
			#pragma multi_compile_fog
			
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
			float _curlR; 
			
			v2f vert (appdata v)
			{  
			    float4 position=v.vertex;
				if(_curlR>0){
					float theta= v.vertex.x /_curlR;
  					float tx = _curlR * sin(theta);		
					float ty = v.vertex.y;
  					float tz = _curlR * (1.0 - cos(theta))+v.vertex.z;
  					position = float4(tx, ty, tz, 1.0);
  				}
				v2f o;
				o.vertex = UnityObjectToClipPos(position);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;

			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
