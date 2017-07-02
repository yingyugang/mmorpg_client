Shader "Custom/ChangeColor" 
{
    Properties {
		_MainTex ("Base Texture", 2D) = "white" {} 
		_Color("Blend Color", Color) = (1, 1, 1 ,1)
    } 

    SubShader {
    	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
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
            };
			sampler2D _MainTex;
			float4 _Color;
            uniform float4 _MainTex_ST;
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
            fixed4 frag (v2f i) : COLOR
            {
            	float4 baseColor = tex2D(_MainTex, i.texcoord);
            	float3 lerpColor = lerp(_Color.xyz,baseColor.xyz,1-_Color.a);
				return float4(lerpColor,baseColor.a);
            }
            ENDCG 
        }
    }   
    Fallback "Diffuse" 

}