// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QFX/SF_VFX/DynamicLighting" {
	Properties{
		[HDR]_Color("Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_DistortionTex("Distortion Texture", 2D) = "white" {}
		_Params("Params", Float) = (0.2, 0.5, 0, 0)
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
                    sampler2D _MainTex;
                    sampler2D _DistortionTex;
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
						float2 distTextcoord : TEXCOORD1;
					};

					float4 _MainTex_ST;
					float4 _DistortionTex_ST;

					v2f vert(appdata_t v)
					{
						v2f o;
						o.position = UnityObjectToClipPos(v.position);
						o.color = v.color;
						o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
						o.distTextcoord = TRANSFORM_TEX(v.texcoord, _DistortionTex);
						return o;
					}

					float4 frag(v2f i) : SV_Target
					{
						half4 distortTex = tex2D(_DistortionTex, i.distTextcoord.xy + _Params.x * _Time.y) * 2 - 1;
						
						half4 mainTex = tex2D(_MainTex, i.texcoord.y + distortTex.y * _Params.y);
						half4 color = 2.0f * i.color * _Color * mainTex;

						float alpha = tex2D(_DistortionTex, i.distTextcoord.xy).a;
				
						float pos = i.texcoord.x + sin(_Params.z * _Time.y);
						clip(pos);

						return color;
					}
					ENDCG
				}
			}
		}
}
