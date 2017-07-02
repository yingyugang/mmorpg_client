Shader "Custom/UVScrollMaskAdditiveTint" {
Properties {
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("Main Texture", 2D) = "white" {}
 _MaskTex ("Mask (R)", 2D) = "white" {}
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

         sampler2D _MainTex;
         float4 _MainTex_ST;
         sampler2D _MaskTex;
		 float4 _MaskTex_ST;
         fixed4 _TintColor;
         
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
             o.texcoord.zw =TRANSFORM_TEX(v.texcoord,_MaskTex);
             return o;
         }
         fixed4 frag (v2f i) : COLOR
         {
             fixed4 maskColor = tex2D(_MaskTex,i.texcoord.zw);

             fixed4 mainColor = tex2D(_MainTex, i.texcoord.xy);
             return 2.0f * i.color * _TintColor * mainColor * maskColor;
         }
         ENDCG
     }
 }   
}
}