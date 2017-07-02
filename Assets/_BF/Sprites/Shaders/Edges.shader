// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Edges" {
    Properties {
		_MainTex ("Base Texture", 2D) = "white" {} 
		_Color("Blend Color", Color) = (1, 1, 1 ,1)
		_TexSize("Texture Size",Vector) = (1,1,1,1)
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
                float4 position_in_world_space : TEXCOORD1;
            };
            
			sampler2D _MainTex;
			float2 _TexSize;
			float4 _Color;
            uniform float4 _MainTex_ST;
            
            float4 dip_filter(float3x3 _filter , sampler2D _image, float2 _xy, float2 texSize)
			{
			    //纹理坐标采样的偏移
			     float2 _filter_pos_delta0[3] = { float2(-1.0 , -1.0) , float2(0,-1.0), float2(1.0 , -1.0) };
			     float2 _filter_pos_delta1[3] = { float2( 0.0 , -1.0) , float2(0, 0.0), float2(1.0 ,  0.0) };
			     float2 _filter_pos_delta2[3] = { float2( 1.0 , -1.0) , float2(0, 1.0), float2(1.0 ,  1.0) };

			     //最终的输出颜色
			     float4 final_color = float4(0.0,0.0,0.0,0.0);
			     //对图像做滤波操作
			     
				 for(int j = 0 ; j < 3 ; j ++)
				 {
				    //计算采样点，得到当前像素附近的像素的坐标
				      float2 _xy_new = float2(_xy.x + _filter_pos_delta0[j].x,_xy.y + _filter_pos_delta0[j].y);            float2 _uv_new = float2(_xy_new.x/texSize.x , _xy_new.y/texSize.y);
				    //采样并乘以滤波器权重，然后累加
				      final_color += tex2D( _image, _uv_new ) * _filter[0][j];
				 } 
			      for(int j = 0 ; j < 3 ; j ++)
				 {
				    //计算采样点，得到当前像素附近的像素的坐标
				      float2 _xy_new = float2(_xy.x + _filter_pos_delta1[j].x,_xy.y + _filter_pos_delta1[j].y);            float2 _uv_new = float2(_xy_new.x/texSize.x , _xy_new.y/texSize.y);
				    //采样并乘以滤波器权重，然后累加
				      final_color += tex2D( _image, _uv_new ) * _filter[1][j];
				 } 
				  for(int j = 0 ; j < 3 ; j ++)
				 {
				    //计算采样点，得到当前像素附近的像素的坐标
				      float2 _xy_new = float2(_xy.x + _filter_pos_delta2[j].x,_xy.y + _filter_pos_delta2[j].y);            float2 _uv_new = float2(_xy_new.x/texSize.x , _xy_new.y/texSize.y);
				    //采样并乘以滤波器权重，然后累加
				      final_color += tex2D( _image, _uv_new ) * _filter[2][j];
				 } 
			     return final_color;
			}

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
            	float2  intXY = float2(i.texcoord.x * _TexSize.x , i.texcoord.y * _TexSize.y);
   				float3x3 _pencil_fil = float3x3 (-0.5 ,-1.0 , 0.0 ,-1.0 ,  0.0 , 1.0 ,-0.0  , 1.0 , 0.5 );   
                float4 delColor =  dip_filter(_pencil_fil , _MainTex , intXY, _TexSize);
   				float  deltaGray = 0.3 * delColor.x  + 0.59 * delColor.y  + 0.11* delColor.z;                 
  				if(deltaGray < 0.0) deltaGray = -1.0 * deltaGray;
  				deltaGray = 1.0 - deltaGray;
  				float4 c1 = tex2D( _MainTex, i.texcoord ); 
  				float4 c2 = float4(_Color.xyz,abs(delColor.a));
  				if(c2.a>0.5)
  					c2.a = _Color.a;
				else
					c2.a = 0;
  				return lerp(c2,c1,1-c2.a);   
            }
            ENDCG 
        }
    }   
    Fallback "Diffuse" 

}
