// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SpriteMask" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MaskTex0 ("Mask0",2D) = "white"{}
		_MaskColor0 ("Mask Color0",Color) = (1.0,1.0,1.0,1.0)
		_UVSpeed_X0 ("UVSpeed_X0",float) = 0.1
		_UVSpeed_Y0 ("UVSpeed_Y0",float) = 0.1
		_MaskTex1 ("Mask1",2D) = "white"{}
		_MaskColor1 ("Mask Color1",Color) = (1.0,1.0,1.0,1.0)
		_UVSpeed_X1 ("UVSpeed_X1",float) = 0.1
		_UVSpeed_Y1 ("UVSpeed_Y1",float) = 0.1
	}
	SubShader {
	    Lighting Off 
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off 
        Fog { Mode Off } 
        Pass {  
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 position_in_world_space : TEXCOORD1;
            };
            
			sampler2D _MainTex;
			sampler2D _MaskTex0;
			float4 _MaskColor0;
			float _UVSpeed_X0;
			float _UVSpeed_Y0;
			sampler2D _MaskTex1;
			float4 _MaskColor1;
			float _UVSpeed_X1;
			float _UVSpeed_Y1;
            uniform float4 _MainTex_ST;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                o.position_in_world_space =  mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : COLOR
            {
            	
				float4 baseColor = tex2D( _MainTex, i.texcoord ); 
				float x = i.texcoord.x + _Time * _UVSpeed_X0;
				float y = i.texcoord.y + _Time * _UVSpeed_Y0;
				float2 tex0 = float2(x,y);
				float4 maskColor0 = tex2D(_MaskTex0,tex0);
				x = i.texcoord.x + _Time * _UVSpeed_X1;
				y = i.texcoord.y + _Time * _UVSpeed_Y1;
				float2 tex1 = float2(x,y);
				float4 maskColor1 = tex2D(_MaskTex1,tex1);
				
				baseColor = baseColor + maskColor0 + maskColor1;
  				return baseColor;   
            }
            ENDCG 
        }
	} 
	FallBack "Diffuse"
}
