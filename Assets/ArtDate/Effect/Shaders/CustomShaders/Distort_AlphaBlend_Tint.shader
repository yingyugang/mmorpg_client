Shader "Custom/Distort_AlphaBlend_Tint" {
Properties {
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("Main Texture", 2D) = "white" {}
 _DispMap ("Displacement Map (RG)", 2D) = "white" {}
 _MaskTex ("Mask(R)", 2D) = "white" {}
 _DispScrollSpeedX  ("Map Scroll Speed X", Float) = 0
 _DispScrollSpeedY  ("Map Scroll Speed Y", Float) = 0
 _DispX  ("Displacement Strength X", Float) = 0
 _DispY  ("Displacement Strength Y", Float) = 0.2
 _DistortStrength  ("Distort Strength", Float) =1.0
 _EdgeStrength  ("Edge Strength", Float) =1.0
 _EdgeShininess("Edge Shininess",Float)=2.0
}

Category {
 Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
 Blend SrcAlpha One
 Cull Off Lighting Off ZWrite Off
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

         uniform sampler2D _MainTex;
         uniform float4 _MainTex_ST;
		 uniform sampler2D _DispMap;
		 uniform float4 _DispMap_ST;
		 uniform sampler2D _MaskTex;
		 uniform float4 _MaskTex_ST;
         uniform half _DispScrollSpeedX;
         uniform half _DispScrollSpeedY;

		 uniform half _DispX;
         uniform half _DispY;
         uniform half _DistortStrength;
         uniform half _EdgeStrength;
		 uniform half _EdgeShininess;

         uniform fixed4 _TintColor;

         
         struct appdata_t {
             float4 vertex : POSITION;
             fixed4 color : COLOR;
			 float2 texcoord : TEXCOORD0;
             
         };

         struct v2f {
             float4 vertex : POSITION;
             fixed4 color : COLOR;
             float4 texcoord : TEXCOORD0;
			 
         }; 
         
         v2f vert (appdata_t v)
         {
             v2f o;
             o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
             o.color = v.color;
             o.texcoord.xy =TRANSFORM_TEX(v.texcoord,_MainTex);
			 o.texcoord.zw =TRANSFORM_TEX(v.texcoord,_DispMap);
             return o;
         }
         fixed4 frag (v2f i) : COLOR
         {
		     //scroll displacement map.
			 half2 mapoft = half2(_Time.y*_DispScrollSpeedX, _Time.y*_DispScrollSpeedY);

			 //get displacement color
			 half4 dispColor = tex2D(_DispMap, i.texcoord.zw + mapoft);

			 //get uv oft
             half2 uvoft = i.texcoord.xy;

		     uvoft.x +=  dispColor.r  * _DispX;
			 uvoft.y +=  dispColor.g  * _DispY;


			 //apply displacement
			 fixed4 mainColor = tex2D(_MainTex, uvoft);

			 //get mask;
			 fixed4 mask = tex2D(_MaskTex, i.texcoord.xy*_MaskTex_ST.xy+_MaskTex_ST.zw);

             float4 finalCol=_DistortStrength*i.color * _TintColor * mainColor * mask.r+_EdgeStrength*pow(float4(mask.rgb,1),_EdgeShininess)*_TintColor;
             return finalCol;
         }
         ENDCG
     }
 }   
}
}