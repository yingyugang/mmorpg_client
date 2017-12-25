// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QFX/SF_VFX/AnimatedShield" {
    	Properties{
		[HDR]_Color("Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_Scale ("Scale", Range(0.1,500.0)) = 3.0
		_Speed ("Speed", Range(-50,50.0)) = 1.0
		_Params("Params", Float) = (0.5,4,0.1,0)
	}
		Category{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha One
			Cull Off Lighting Off ZWrite Off

			SubShader {
				Pass {

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag

					#include "UnityCG.cginc"

					fixed4 _Color;
					fixed4 _MaskColor;
                    sampler2D _MainTex;
					half _Scale;
					half _Speed;
					fixed4 _Params;

					struct appdata_t {
						float4 position : POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f {
						float4 position : SV_POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					float4 _MainTex_ST;

					v2f vert(appdata_t v)
					{
						v2f o;
						o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
						o.position = UnityObjectToClipPos(v.position);
						o.color = v.color;
						return o;
					}

					float4 frag(v2f i) : SV_Target
					{
			        	half2 uv = i.texcoord;
						
						float2 center = float2(0.5, 0.5);

						float x = (center.x-uv.x)*_Params.w;
						float y = (center.y-uv.y);
							
						float r = -sqrt(_Params.x *x*x + _Params.y * y*y);
						float z = 1 + 0.5*cos((r+_Time.y*_Speed)/0.013);
						
						if (r < _Params.z )
							discard;

						float2 nuv = i.texcoord + z;

						float4 mainTex = tex2D(_MainTex, nuv);

						float4 color = 2.0f * i.color * mainTex;
						color.a = saturate(color.a);
						color *= _Color;

						return color;
					}
					ENDCG
				}
			}
		}
}