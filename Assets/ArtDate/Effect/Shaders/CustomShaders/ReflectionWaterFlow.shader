// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D



Shader "Custom/ReflectionWaterFlow" {
	
Properties {
	_MainTex ("Base", 2D) = "white" {}
	_Normal("Normal", 2D) = "bump" {}
	_ReflectionTex("_ReflectionTex", 2D) = "black" {}
	_DirectionUv("Wet scroll direction (2 samples)", Vector) = (1.0,1.0, 2.65,-0.03)
	_TexAtlasTiling("Tex atlas tiling", Vector) = (1.88,0.02,-5.9,2.45)
	_ReflectIntensity("Reflect Intensity",Float)=0.23	
	_SampleDistance("Sample Distance",Float)=0.07
}



SubShader {
	Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="true"}

	LOD 200 

	Pass {
	    Cull back
	    blend srcAlpha oneMinusSrcAlpha
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
		#pragma fragmentoption ARB_precision_hint_fastest 
		#include "UnityCG.cginc"
		
		half4 _DirectionUv;
		half4 _TexAtlasTiling;
		half _ReflectIntensity;
		half _SampleDistance;

		sampler2D _MainTex;
		sampler2D _Normal;		
		sampler2D _ReflectionTex;
		
		float4 _MainTex_ST;
		#ifdef LIGHTMAP_ON
		// float4 unity_LightmapST;	
		// sampler2D unity_Lightmap;
		#endif
		
		
		struct v2f_full
		{
			half4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
			half4 normalScrollUv : TEXCOORD1;	
			half4 screen : TEXCOORD2;
			
			#ifdef LIGHTMAP_ON
				half2 uvLM : TEXCOORD4;
			#endif	
		};
		
		
		
		v2f_full vert (appdata_full v) 
		{
			v2f_full o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.uv.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
			
			#ifdef LIGHTMAP_ON
				o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			#endif
			
			o.normalScrollUv.xyzw = v.texcoord.xyxy * _TexAtlasTiling + _Time.xxxx * _DirectionUv;
						
			
			o.screen = ComputeScreenPos(o.pos);
				
			return o; 
		}
				
		fixed4 frag (v2f_full i) : COLOR0 
		{
			half3 nrml = UnpackNormal(tex2D(_Normal, i.normalScrollUv.xy));
			nrml += UnpackNormal(tex2D(_Normal, i.normalScrollUv.zw));
			
			nrml.xy *= _SampleDistance;
										
			fixed4 rtRefl = tex2D (_ReflectionTex, (i.screen.xy / i.screen.w) + nrml.xy);
			
						
			fixed4 tex = tex2D (_MainTex, i.uv.xy + nrml.xy * 0.05);
		
			#ifdef LIGHTMAP_ON
				fixed3 lm = ( DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM)));
				tex.rgb *= lm;
			#endif	
			
			tex.rgb += _ReflectIntensity * rtRefl.rgb;
			
			return tex;	
		}	
		

	
		ENDCG
	}
} 


//FallBack "AngryBots/Fallback"
}
