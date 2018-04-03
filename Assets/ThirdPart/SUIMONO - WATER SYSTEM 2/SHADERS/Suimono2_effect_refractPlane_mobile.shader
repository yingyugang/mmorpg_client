// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Suimono2/effect_refractPlane_mobile" {


Properties {
	_MasterScale ("Master Scale", Float) = 1.0
	
	_BlurSpread ("Blur Spread", Range (0.0, 0.125)) = 0.001
	_BlurRamp ("Blur Ramp", 2D) = "" {}

	_underFogStart ("Underwater Fog Start", Range(0.0,1.0)) = 0.0
	_underFogStretch ("Underwater Fog Stretch", Range(0.0,0.02)) = 0.0
	
	//_underTex ("Underwater Fog Texture 2", 2D) = "" {}
	
	_TestHeight1 ("Test Normal 1", 2D) = "" {}
	_TestHeight2 ("Test Normal 1", 2D) = "" {}
	
	//refraction
	_RefrStrength ("Refraction Strength", Range(0.0,1.0)) = 0.0
    _RefrSpeed ("Refraction Speed", Float) = 0.5
	_AnimSpeed ("Animation Speed", Float) = 1.0

	_DepthAmt ("Depth Amount", Float) = 0.1
	_DiffuseColor ("Diffuse Color", Color) = (0.5, 0.5, 1.0, 1.0)
	
	_DepthColor ("Depth Over Tint", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorR ("Depth Color 1(r)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorG ("Depth Color 2(g)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorB ("Depth Color 3(b)", Color) = (0.25,0.25,0.5,1.0)

}




Category {
	Tags { "Queue"="Transparent+500" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Mode Global }//Fog { Color (0,0,0,0) }
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

			//sampler2D _MainTex;
			float4 _DepthColorB;
			
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
			
			//float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.color.a = 0.5;
				o.texcoord = v.texcoord;
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;
			
			half4 frag (v2f i) : COLOR
			{

				_DepthColorB.a *= 0.75;
				return 2.0f * i.color * _DepthColorB;// * tex2D(_MainTex, i.texcoord);
			}
			ENDCG 
		}
	} 	
	


}
}






















