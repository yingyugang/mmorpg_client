Shader "Custom/FogView" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainTex1 ("Addition Texture)", 2D) = "white" {}
		_MainTex2 ("Background",2D) = "white" {}
		_MainTex3 ("Front",2D) = "white" {}
		_Alpha("Alpha", Range (0.0, 1.0)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			Ztest Always
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _MainTex1;
			sampler2D _MainTex2;
			sampler2D _MainTex3;
			float _Alpha;
			
			struct Output {
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
		
			Output vert(appdata_full i)
			{
				Output output;
				output.pos = mul(UNITY_MATRIX_MVP,i.vertex);
				output.uv = i.texcoord;
				return output;
			}
		
			float4 frag(Output op):COLOR
			{
				half4 c = tex2D (_MainTex, op.uv);
//				op.uv.y -= _Time * 10;
				half4 c1 = tex2D(_MainTex1, op.uv);
				half4 c2 = tex2D(_MainTex2, op.uv);
				half4 c3 = tex2D(_MainTex3, op.uv);
				c.a = 1 - c.r;
				c.a = min(c.a,c1.a);
				c.rgb = c1.rgb;
				c.a = c.a * _Alpha;
				c.a = clamp(c.a,0.0,1.0);
				half4 result = lerp(c3,c,1- c3.a);
				result = lerp(result,c2,1- result.a);
				return result;
			}
			ENDCG
		}
		
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		void surf (Input IN, inout SurfaceOutput o) {
//			half4 c = tex2D (_MainTex, IN.uv_MainTex);
//			o.Albedo = c.rgb;
//			o.Alpha = 1.0-c.a;
//		}
		
	} 
	FallBack "Diffuse"
}
