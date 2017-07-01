// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Suimono2/effect_refraction_mobile" {
Properties {
	_MaskMap ("MaskMap", 2D) = "" {}
	_HeightMap ("HeightMap", 2D) = "" {}
	_DepthColor ("Depth Color", Color) = (0.5, 0.5, 0.5, 1)
    _Strength ("Refraction Strength", range (0,150)) = 25.0
    _Overlay ("Overlay Strength", range (0.0,1.0)) = 1.0
    _Brightness ("Brightness Strength", range (1.0,1.8)) = 1.0
}



Category {
	Tags { "IgnoreProjector"="True" "RenderType"="Transparent" "Queue"= "Transparent+900"}
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off
	Cull Off
	//AlphaTest Greater .01
	ColorMask RGB
	//Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	// ---- Fragment program cards
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_particles
			
			#include "UnityCG.cginc"

			sampler2D _MaskMap;
			float4 _DepthColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD1;
				#endif
			};
			
			float4 _MaskMap_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MaskMap);
				return o;
			}

			//sampler2D _CameraDepthTexture;
			//float _InvFade;
			
			half4 frag (v2f i) : COLOR
			{
			_DepthColor.a = 1.0;

			i.color.a *= 0.1;
			half3 mask = tex2D(_MaskMap, i.texcoord);
			
			i.color.rgb *= (1.0+i.color.a);
			
			return i.color * (mask.r-mask.g);
			}
			ENDCG 
		}
	} 	
	
	// ---- Dual texture cards
	SubShader {
		Pass {
			SetTexture [_MaskMap] {
				constantColor [_TintColor]
				combine constant * primary
			}
			SetTexture [_MaskMap] {
				combine texture * previous DOUBLE
			}
		}
	}
	
	// ---- Single texture cards (does not do color tint)
	SubShader {
		Pass {
			SetTexture [_MaskMap] {
				combine texture * primary
			}
		}
	}
}



}
