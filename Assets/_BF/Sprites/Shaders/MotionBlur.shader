Shader "Custom/MotionBlur" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass{
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
			uniform sampler2D _AccTex;
			uniform float accPercent;
			
            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
			
			struct Output{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			Output vert(appdata_t i)
			{
				Output o;
				o.pos = mul(UNITY_MATRIX_MVP,i.vertex);
				o.uv = i.texcoord;
				accPercent = clamp(accPercent,0,1);
				return o;
			}
			
			float4 frag(Output i):COLOR
			{
				float4 c = tex2D(_MainTex,i.uv);
				float4 acc = tex2D(_AccTex,i.uv);
				c = lerp(c,acc,1-accPercent);
				return c;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
