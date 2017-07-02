Shader "Custom/Multiply_Unlit"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_Color("Tint Color",COLOR)=(1,1,1,1)
		
	}
	
	SubShader
	{
		LOD 300

		Tags
		{
			"Queue" = "Transparent+500"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull off
			Lighting Off
			ZWrite off
			ZTest Less
			Fog { Mode Off }
			//Offset -1, -1
			Blend DstColor OneMinusSrcAlpha
			//Blend one OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float4 _Color;

			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				return o;
			}

			half4 frag (v2f IN) : COLOR
			{
				half4 col = tex2D(_MainTex, IN.texcoord) * IN.color*_Color;
				col.rgb = lerp(half3(0.0, 0.0, 0.0), col.rgb, col.a);
				return col/float4(0.99,0.99,0.99,0.99);
				//return col;
			}
			ENDCG
		}
	}
	
}
