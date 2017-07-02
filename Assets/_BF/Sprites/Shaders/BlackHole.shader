// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/BlackHole" 
{
    Properties {
		_MainTex ("Base Texture", 2D) = "white" {} 
		_Color("Blend Color", Color) = (1, 1, 1 ,1)
		_Pos0("Pos0",Vector) = (0,0,0,1)
		_Pos("Pos",Vector) = (0,0,0,1)
    } 

    SubShader {
        Lighting Off 
//        Cull Off
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
			float4 _Pos0;
			float4 _Pos;
			float4 _Color;
            uniform float4 _MainTex_ST;
            float _Time0;
            
            v2f vert (appdata_t v)

            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
              	o.position_in_world_space =  mul(unity_ObjectToWorld, v.vertex);
              	
              	float x = o.vertex.x;
              	if(x > 0)
              	{
			      	o.vertex.x = lerp(o.vertex.x,0,_Time);
					o.vertex.y = lerp(o.vertex.y,0,_Time);
              	}
              	
      			
              	
                
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                
                return o;
            }
            fixed4 frag (v2f i) : COLOR
            {
            	float4 baseColor = tex2D(_MainTex, i.texcoord) * _Color;
            	float dist = i.position_in_world_space.y - _Pos0.y;
            	float mind = 0.1;
//            	if(i.position_in_world_space.x-floor(i.position_in_world_space.x)>0.5)
//            	{
//            		mind += (1 - (i.position_in_world_space.x-floor(i.position_in_world_space.x)))/10;
//            	}else{
//            		mind += (i.position_in_world_space.x-floor(i.position_in_world_space.x))/10;
//            	}
            	if(dist<0 || distance(i.position_in_world_space,float2(_Pos.x,7)) > 7)
            	{
            		baseColor.a = 0;
            	}
            	else if(dist<mind )
            	{
	          		baseColor =  float4(0.3,1,1,baseColor.a);
            	}
				return baseColor;
            }
            ENDCG 
        }
    }   
    Fallback "Diffuse" 

}
