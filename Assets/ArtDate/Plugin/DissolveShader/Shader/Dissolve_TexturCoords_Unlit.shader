//***recoded by 3Fun,2014.09.22
//***
Shader "Dissolve/Dissolve_TexturCoords_Unlit" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		
		_Amount ("Amount", Range (0, 1)) = 0.5
		_StartAmount("StartAmount", float) = 0.1
		_Illuminate ("Illuminate", Range (0, 1)) = 0.5
		_Tile("Tile", float) = 1
		_DissColor ("DissColor", Color) = (1,1,1,1)
		_ColorAnimate ("ColorAnimate", vector) = (1,1,1,1)
		
		_DissolveSrc ("DissolveSrc", 2D) = "white" {}
		_DissolveSrcBump ("DissolveSrcBump", 2D) = "white" {}
		_Distortion("Distortion",Range(0,1))=0.5

	}
	SubShader { 
		Tags { "RenderType"="Transparent" }
		
		Pass{
			Tags { "LightMode" = "Always" } 			
			cull off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#pragma target 3.0
			#include "UnityCG.cginc"


			sampler2D _MainTex;	
			float4 _MainTex_ST;
			sampler2D _DissolveSrc;
			sampler2D _DissolveSrcBump;
			float4 _DissolveSrcBump_ST;
			float _Distortion;

			fixed4 _Color;
			half4 _DissColor;
			
			half _Amount;
			static half3 Color = float3(1,1,1);
			half4 _ColorAnimate;
			half _Illuminate;
			half _Tile;
			half _StartAmount;



			struct Output {
				float4 pos : SV_POSITION;
				float4 uv_MainTex : TEXCOORD;
				
			};

			Output vert (appdata_base v) {
				Output o;
				o.pos=mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv_MainTex.xy=TRANSFORM_TEX(v.texcoord,_MainTex);
				o.uv_MainTex.zw=TRANSFORM_TEX(v.texcoord,_DissolveSrcBump);
				return o;
			}
			
			float4 frag (Output IN):COLOR{
				
				float2 uv=IN.uv_MainTex.xy;
				fixed4 col=tex2D(_MainTex, uv)*_Color;
				float ClipTex = tex2D (_DissolveSrc, IN.uv_MainTex.zw).r ;
				float ClipAmount = ClipTex - _Amount;
				float Clip = 0;
				float3 DematBump =  UnpackNormal(tex2D (_DissolveSrcBump,IN.uv_MainTex.zw/_Tile));
				
				if (_Amount > 0)
				{
					if (ClipAmount <0)
					{
						Clip = 1; //clip(-0.1);
					
					}
					 else{
					
						if (ClipAmount < _StartAmount)
						{
							if (_ColorAnimate.x == 0)
								Color.x = _DissColor.x;
							else
								Color.x = ClipAmount/_StartAmount;
				          
							if (_ColorAnimate.y == 0)
								Color.y = _DissColor.y;
							else
								Color.y = ClipAmount/_StartAmount;
				          
							if (_ColorAnimate.z == 0)
								Color.z = _DissColor.z;
							else
								Color.z = ClipAmount/_StartAmount;
								
							
							col.rgb  = lerp(col.rgb,(col.rgb *(Color.x+Color.y+Color.z)* Color*(Color.x+Color.y+Color.z))/(1 - _Illuminate),ClipAmount/_StartAmount);
							//col.rgb  = lerp(col.rgb,(col.rgb *(Color.x+Color.y+Color.z)* Color*(Color.x+Color.y+Color.z)),ClipAmount/_StartAmount);
							col.a=col.a*lerp(0,1,ClipAmount/_StartAmount);
						}
					 }
				 }

				 
				if (Clip == 1)
				{
				clip(-0.1);
				}
				
				return col;
			}
			ENDCG
		}
	}

	//FallBack "Specular"
}
