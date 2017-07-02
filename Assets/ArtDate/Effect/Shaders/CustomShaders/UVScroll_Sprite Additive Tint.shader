Shader "Custom/UVScrollSpriteAdditiveTint" {
Properties {
 _TintMainColor ("Tint Main Color", Color) = (1,1,1,1)
 _MainTex ("Main Texture", 2D) = "white" {}
 _TintScrollColor("Tint Scroll Color", Color) = (0.5,0.5,0.5,0.5)
 _ScrollTex ("Scroll Texture", 2D) = "white" {}
 _DisplaceMap("Displace Map", 2D) = "white" {}
 _DistortIntensity("Distort Intensity",Float)=0.5
 _ScrollSpeedX("Scroll Speed X",Float)=0
 _ScrollSpeedY("Scroll Speed Y",Float)=0
}

Category {
 Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
 Blend SrcAlpha OneMinusSrcAlpha
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

         sampler2D _MainTex;
         float4 _MainTex_ST;
         sampler2D _ScrollTex;
		 float4 _ScrollTex_ST;
         sampler2D _DisplaceMap;
		 float4 _DisplaceMap_ST;
         fixed4 _TintMainColor;
         fixed4 _TintScrollColor;
         half _ScrollSpeedX;
         half _ScrollSpeedY;
         half _DistortIntensity;
         
         struct appdata_t {
             float4 vertex : POSITION;
             fixed4 color : COLOR;
             float2 texcoord : TEXCOORD0;
         };

         struct v2f {
             float4 vertex : POSITION;
             fixed4 color : COLOR;
             float4 texcoord : TEXCOORD0;
             float2 texcoord1: TEXCOORD1;
         };
         
         v2f vert (appdata_t v)
         {
             v2f o;
             o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
             o.color = v.color;
             o.texcoord.xy =TRANSFORM_TEX(v.texcoord,_MainTex);
             o.texcoord.zw =TRANSFORM_TEX(v.texcoord,_ScrollTex);
             o.texcoord1=TRANSFORM_TEX(v.texcoord,_DisplaceMap);
             return o;
         }
         fixed4 frag (v2f i) : COLOR
         {
         
         	fixed4 norm=tex2D(_DisplaceMap,i.texcoord1);	
             fixed4 scrollColor = tex2D(_ScrollTex,i.texcoord.zw+_Time.y*float2(_ScrollSpeedX,_ScrollSpeedY)+norm.xy*_DistortIntensity);

             fixed4 mainColor = tex2D(_MainTex, i.texcoord.xy);
             fixed4 col;
             col.rgb= i.color.rgb * _TintMainColor.rgb * mainColor.rgb+2*scrollColor.rgb*_TintScrollColor.rgb;
             col.a=i.color.a * _TintMainColor.a * mainColor.a;
             return col;
         }
         ENDCG
     }
 }   
}
}