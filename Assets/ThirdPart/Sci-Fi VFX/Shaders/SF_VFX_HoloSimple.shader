// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QFX/SF_VFX/HoloSimple" {
    	Properties{
			[HDR]_Color("Color", Color) = (0.5,0.5,0.5,0.5)
			_MainTex("Main Texture", 2D) = "white" {}
	    	[Toggle(ORIENTATION)]
        	_Orientation ("Orientation", Float) = 0
		}

		Category{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha One
			Cull Off Lighting Off ZWrite Off

			SubShader {
				Pass {
					CGINCLUDE
						float hash( float n ) { return frac(sin(n)*753.5453123); }
					ENDCG

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma shader_feature ORIENTATION

					#include "UnityCG.cginc"

					fixed4 _Color;
                    sampler2D _MainTex;
					float4 _MainTex_ST;

					struct appdata_t {
						fixed4 pos : POSITION;
						fixed4 color : COLOR;
						float2 uv : TEXCOORD0;
					};

					struct v2f {
						fixed4 pos : SV_POSITION;
						fixed4 color : COLOR;
						float2 uv : TEXCOORD0;
					};


					v2f vert(appdata_t v)
					{
						v2f o;
						o.pos = UnityObjectToClipPos(v.pos);
						o.uv = TRANSFORM_TEX(v.uv,_MainTex);
						o.color = v.color;
						return o;
					}

					float4 frag(v2f i) : SV_Target
					{
						float main = tex2D(_MainTex, i.uv);

						float o;
						#ifdef ORIENTATION
							o = i.pos.x;
						#else
							o = i.pos.y;
						#endif

						float h = hash(o * _Time.y);

						return h * main * _Color * 0.7;
					}
					ENDCG
				}
			}
		}
}