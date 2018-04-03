// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Suimono2/effect_refractPlane" {


Properties {
	_MasterScale ("Master Scale", Float) = 1.0
	
	_BlurSpread ("Blur Spread", Range (0.0, 0.125)) = 0.001
	_BlurRamp ("Blur Ramp", 2D) = "" {}

	_underFogStart ("Underwater Fog Start", Range(0.0,1.0)) = 0.0
	_underFogStretch ("Underwater Fog Stretch", Range(0.0,0.02)) = 0.0
	
	//_underTex ("Underwater Fog Texture 2", 2D) = "" {}
	
	_TestHeight1 ("Test Normal 1", 2D) = "" {}
	_TestHeight2 ("Test Normal 1", 2D) = "" {}
	
	//refraction
	_RefrStrength ("Refraction Strength", Range(0.0,1.0)) = 0.0
    _RefrSpeed ("Refraction Speed", Float) = 0.5
	_AnimSpeed ("Animation Speed", Float) = 1.0

	_DepthAmt ("Depth Amount", Float) = 0.1
	_DiffuseColor ("Diffuse Color", Color) = (0.5, 0.5, 1.0, 1.0)
	
	_DepthColor ("Depth Over Tint", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorR ("Depth Color 1(r)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorG ("Depth Color 2(g)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorB ("Depth Color 3(b)", Color) = (0.25,0.25,0.5,1.0)


}



Subshader 
{ 








//HEIGHT FOG
Tags {"Queue"= "Transparent+2"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off



CGPROGRAM
#pragma surface surf BlinnPhong vertex:vert noambient

struct Input {
	float3 viewDir;
	float4 pos;
	float4 screenPos;	
};

void vert (inout appdata_full v, out Input o) {
	//o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	//#if UNITY_UV_STARTS_AT_TOP
	//	float scale = -1.0;
	//#else
	//	float scale = 1.0;
	//#endif
	//o.pos = mul(_Object2World, v.vertex)
	o.pos = UnityObjectToClipPos(v.vertex); ;
	o.viewDir.xzy = ObjSpaceViewDir(v.vertex);
	o.screenPos = ComputeScreenPos(o.pos); 

}

float4 _DepthColor;
sampler2D _CameraDepthTexture;
float _DepthAmt;
float4 _DepthColorB;


void surf (Input IN, inout SurfaceOutput o) {

	//CALCULATE DEPTH FOG
	//float4 DepthFade = float4(1.0,0.0,0.0,0.0);
	//float4 edgeBlendFactors = float4(0.0, 0.0, 0.0, 0.0);
	half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	//depth = LinearEyeDepth(depth);
	//edgeBlendFactors = saturate(DepthFade * (depth-IN.screenPos.w));		
		
	//float4 depthColor = (_DepthColorB - (DepthFade.w) * float4(0.15, 0.03, 0.01, 0.0));

	//depthColor.a *= (edgeBlendFactors.y);
	//depthColor.a = edgeBlendFactors.x * _DepthColorB.a;
	
	//float2 scrollUV1 = IN.uv_underTex;
	//float2 scrollUV2 = IN.uv_underTex2;
	//scrollUV2.x *= -0.15;
	//half4 ftex1 = tex2D(_underTex, scrollUV1);
	//half4 ftex2 = tex2D(_underTex2, scrollUV2);
	//o.Albedo = _DepthColorB.rgb + ftex1.r - ftex2.g;
	//o.Alpha = ftex2.g*2.0 * _DepthAmt;
	//o.Alpha = 0.0;
	
	//half4 c = tex2D (_MainTex, IN.uv_MainTex);
	
	float d = length(IN.viewDir);
	float l = saturate((1.0 - -24.0) / (5.0 - -24.0) / clamp(1.0 / 0.35 - 130, 1, 100.0));
	//float l = saturate((d - _FogNear) / (_FogFar - _FogNear) / clamp(IN.pos.y / _FogAltScale + 1, 1, _FogThinning));
	o.Albedo = lerp(_DepthColorB.rgb*0.25, half3(1,0,0), 0.0);
	o.Emission = o.Albedo;
	o.Alpha = d * 0.5;//0.0;//l;//l;//1.0;//c.a * _Color.a;
	//o.Alpha = 0.0;
  }
ENDCG          
















// -----------------------
//   UNDERWATER FOG 
// -----------------------
GrabPass {
	Tags {"Queue" = "Transparent+2"}
	//Name "UnderGrab1"
}

Pass{
	Tags {"Queue"= "Transparent+2"}
	Fog {Mode Off}
	Cull Back
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off

		
	CGPROGRAM

	#pragma target 3.0
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos          : POSITION;
		float4 uvgrab       : TEXCOORD0;
		float4 screenPos	: TEXCOORD1;
		float2 uv           : TEXCOORD2;
		float4 uvs          : TEXCOORD3;
		//float4 uv_underTex  : TEXCOORD4;
		//float4 interpolatedRay : TEXCOORD1;
	};
	

      
	
	v2f vert (appdata_full v)
	{
		v2f o;
		//o.pos = mul(_Object2World, v.vertex);
		o.pos = UnityObjectToClipPos(v.vertex);  
		//o.pos = mul(_Object2World, v.vertex); 
		#if UNITY_UV_STARTS_AT_TOP
		float scale = -1.0;
		#else
		float scale = 1.0;
		#endif
		
		o.uv = v.texcoord.xy;
		
		o.uvgrab.xy = (float2(o.pos.x, o.pos.y * scale) + o.pos.w) * 0.5;
		o.uvgrab.zw = o.pos.zw;

		o.screenPos = ComputeScreenPos(o.pos);
		
		o.uvs.xy = (float2(o.pos.x, o.pos.y * scale) + (o.pos.w)) * 0.5;
		o.uvs.z = o.pos.z;
		o.uvs.w = o.pos.w;
		
		//o.screenPos.w = v.vertex.z;
		
		return o;
	} 
	
	//sampler2D _underTex;
	sampler2D _GrabTexture;
	float4 _GrabTexture_TexelSize;
	float _BlurSpread;
	
	sampler2D _CameraDepthTexture;
	sampler2D _BlurRamp;
	
	float4 _DepthColor;
	float4 _DepthColorR;
	float4 _DepthColorG;
	float4 _DepthColorB;
	
	float4 _Test;
	float _underFogStart;
	float _underFogStretch;
	float _DepthAmt;
	
	//uniform float4 _Y;
	//uniform float4x4 _FrustumCornersWS;
	//uniform float4 _CameraWS;
	
	half4 frag( v2f i ) : COLOR
	{
	
	
	//GET ORIGINAL BACKGROUND
	half4 origCol = half4(tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab)).rgb, 1);
	//origCol = _DepthColorB;
	
	
	//CALCULATE BLUR RAMP
	//float4 DepthFade = _Test;//float4(1.0,(1.0 * 0.1),0.0,0.2);

	float4 edgeBlendFactors = float4(0.0, 0.0, 0.0, 0.0);
	half dpth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
	dpth = LinearEyeDepth(dpth);
	
	float4 DepthFade = float4(_underFogStretch,1.0,0.0,_underFogStart); //!!!! there are issues here under DX11 !!!!!
	edgeBlendFactors = saturate(DepthFade * (dpth-i.screenPos.w));		
	
	float depthAmt = tex2D(_BlurRamp, float2(0.0, 0.5)) - (DepthFade.w * float4(0.15, 0.08, 0.01, 0.0)).a;
	
	depthAmt *= (edgeBlendFactors.y);
	depthAmt = edgeBlendFactors.x * depthAmt;

	float depthPos = depthAmt-DepthFade.w;
	
	//if (depthPos > 1.0) depthPos = 1.0;
	//if (depthPos < 0.0) depthPos = 0.0;

	
	half depthViz = tex2D(_BlurRamp, float2(depthPos, 0.5)).r;
	
	

	//BLUR TEXTURE
	half4 oCol = origCol * 0.16;

	//BLUR MASK
	half4 xCol = half4(0.0,0.0,0.0,0.0);


	//CALCULATE FINAL COLOR
	half4 xCol2 = lerp(half4(-3.0,-3.0,-3.0,1.0),half4(6.0,6.0,6.0,1.0),xCol.r);
	if (xCol2.r > 1.0) xCol2.r = 1.0;
	if (xCol2.r < 0.0) xCol2.r = 0.0;
	
	//calculate blur scroll texture
	//float2 scrollUV2 = i.uv_underTex;
	//scrollUV2.x *= -0.15;
	//half4 ftex2 = tex2D(_underTex, i.uv);
	
	
	
	//swap blurred image based on blur ramp
	half4 mCol = lerp(origCol,oCol,xCol2.r);
	mCol = _DepthColorB;
	//origCol.rgb *= (1.0-(ftex2.g*0.25));
	half4 rCol = lerp(mCol,origCol,depthViz);
	rCol.a = _DepthColorB.a;
	
	
	if (rCol.a > 1.0) rCol.a = 1.0;
	if (rCol.a < 0.0) rCol.a = 0.0;
	
	if (depthAmt >= 0.8) rCol.a = 0.0;

	//rCol.a = 0.0;
	//rCol.a = edgeBlendFactors.x;
	
	if (rCol.a > 1.0) rCol.a = 1.0;
	if (rCol.a < 0.0) rCol.a = 0.0;
	return rCol;
	

		
	}
	

	
	ENDCG
} 




















// -----------------------
//   UNDERWATER BLURRING 
// -----------------------
GrabPass {
	Tags {"Queue" = "Transparent+2"}
	//Name "UnderGrab2"
}

Pass{
	Tags {"Queue"= "Transparent+2"}
	Cull Back
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off
		
	CGPROGRAM
	#pragma target 3.0
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos          : POSITION;
		float4 uvgrab       : TEXCOORD0;
		float4 screenPos	: TEXCOORD1;
		float2 uv           : TEXCOORD2;
		float4 uvs          : TEXCOORD3;
	};
	

		
	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		
		#if UNITY_UV_STARTS_AT_TOP
		float scale = -1.0;
		#else
		float scale = 1.0;
		#endif
		
		o.uv = v.texcoord.xy;
		
		o.uvgrab.xy = (float2(o.pos.x, o.pos.y * scale) + o.pos.w) * 0.5;
		o.uvgrab.zw = o.pos.zw;

		o.screenPos = ComputeScreenPos(o.pos);
		
		o.uvs.xy = (float2(o.pos.x, o.pos.y * scale) + (o.pos.w)) * 0.5;
		o.uvs.z = o.pos.z;
		o.uvs.w = o.pos.w;
		
		return o;
	} 
	
	sampler2D _GrabTexture;
	float4 _GrabTexture_TexelSize;
	float _BlurSpread;
	float4 _DepthColorR;
	
	sampler2D _CameraDepthTexture;
	sampler2D _BlurRamp;
	
	
	half4 frag( v2f i ) : COLOR
	{
	
	
	//GET ORIGINAL BACKGROUND
	half4 origCol = half4(tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab)).rgb, 1);
		
	
	
	//CALCULATE BLUR RAMP
	float4 DepthFade = float4(1.0,(1.5 * 0.01),0.0,0.0);
	float4 edgeBlendFactors = float4(0.0, 0.0, 0.0, 0.0);
	half dpth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
	dpth = LinearEyeDepth(dpth);
	edgeBlendFactors = saturate(DepthFade * (dpth-i.screenPos.w));		
		
	float depthAmt = tex2D(_BlurRamp, float2(0.0, 0.5)) - (DepthFade.w * float4(0.15, 0.03, 0.01, 0.0)).a;
	
	depthAmt *= (edgeBlendFactors.y);
	depthAmt = edgeBlendFactors.x * depthAmt;
	
	float depthPos = depthAmt-DepthFade.w;
	
	if (depthPos > 1.0) depthPos = 1.0;
	if (depthPos < 0.0) depthPos = 0.0;
	half depthViz = tex2D(_BlurRamp, float2(depthPos, 0.5)).r;
	
	

	//BLUR TEXTURE
	half4 oCol = origCol * 0.16;
	
	float depth = _BlurSpread;
	
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x-5.0*depth, i.uvgrab.y-5.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.025;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x-4.0*depth, i.uvgrab.y-4.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.05;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x-3.0*depth, i.uvgrab.y-3.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.09;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x-2.0*depth, i.uvgrab.y-2.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.12;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x-1.0*depth, i.uvgrab.y-1.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.15;
	
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x+1.0*depth, i.uvgrab.y+1.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.15;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x+2.0*depth, i.uvgrab.y+2.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.12;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x+3.0*depth, i.uvgrab.y+3.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.09;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x+4.0*depth, i.uvgrab.y+4.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.05;
	oCol += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x+5.0*depth, i.uvgrab.y+5.0*depth, i.uvgrab.z,i.uvgrab.w))) * 0.025;
	
	
	//BLUR MASK
	half4 xCol = half4(0.0,0.0,0.0,0.0);
	xCol += tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x, i.uvs.y, i.uvs.z,i.uvs.w))) * 1.0;

	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x-8.0*depth, i.uvs.y-8.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x-6.0*depth, i.uvs.y-6.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x-4.0*depth, i.uvs.y-4.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x-2.0*depth, i.uvs.y-2.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x-1.0*depth, i.uvs.y-1.0*depth, i.uvs.z,i.uvs.w))) * 1;
	
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x+1.0*depth, i.uvs.y+1.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x+2.0*depth, i.uvs.y+2.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x+4.0*depth, i.uvs.y+4.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x+6.0*depth, i.uvs.y+6.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol *= tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uvs.x+8.0*depth, i.uvs.y+8.0*depth, i.uvs.z,i.uvs.w))) * 1;
	xCol*2.0;
	xCol.a = 1.0;


	//CALCULATE FINAL COLOR
	half4 xCol2 = lerp(half4(-3.0,-3.0,-3.0,1.0),half4(6.0,6.0,6.0,1.0),xCol.r);
	if (xCol2.r > 1.0) xCol2.r = 1.0;
	if (xCol2.r < 0.0) xCol2.r = 0.0;
	
	//swap blurred image based on blur ramp
	half4 mCol = lerp(origCol,oCol,xCol2.r);
	half4 rCol = lerp(mCol,origCol,depthViz);
	rCol.a = 1.0;
	
	rCol.rgb *= _DepthColorR.rgb;
	return rCol;
	}
	

	
	ENDCG
} 



























// ------------------------
//    RENDER REFRACTION
// ------------------------
 
GrabPass {
	Tags {"Queue" = "Transparent+2"}
	//Name "UnderGrab3"
}



Pass{
	Tags {"Queue"= "Transparent+2"}
	Cull Back
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite On
		
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
	
	struct v2f { 
		float4 pos          : POSITION;
		float4 uvgrab       : TEXCOORD0;
		float2 uv           : TEXCOORD1;
		float4 screenPos    : TEXCOORD2;
		float4 uvs          : TEXCOORD3;
		float4 uv_TestHeight1      : TEXCOORD4;
		float4 uv_TestHeight2      : TEXCOORD5;
	};
	
	
	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);   
		#if UNITY_UV_STARTS_AT_TOP
		float scale = -1.0;
		#else
		float scale = 1.0;
		#endif

		o.screenPos = ComputeScreenPos(o.pos);
		
		o.uv_TestHeight1 = v.texcoord;
		o.uv_TestHeight2 = v.texcoord;
		
		o.uv = v.texcoord.xy;
		
		o.uvgrab.xy = (float2(o.pos.x, o.pos.y * scale) + o.pos.w) * 0.5;
		o.uvgrab.zw = o.pos.zw;
		
		o.uvs.xy = (float2(o.pos.x, o.pos.y * scale) + (o.pos.w)) * 0.5;
		o.uvs.z = o.pos.z;
		o.uvs.w = o.pos.w;
		
		return o;
	}
	sampler2D _CameraDepthTexture;
	float _RefrSpeed, _RefrStrength,_AnimSpeed;
	sampler2D _GrabTexture;
	float4 _GrabTexture_TexelSize;
	float _MasterScale;
	
	float _DepthAmt;
	
	sampler2D _TestHeight1;
	sampler2D _TestHeight2;
	float _WaveAmt;
	float _WaveStretch;
	float _NormalAmt;
	
	
	half4 frag( v2f i ) : COLOR
	{

		_RefrSpeed *= (_AnimSpeed*0.1);
		
		float2 effectUVs = i.uv;
		effectUVs.y += _Time * (_RefrSpeed*0.5);
		effectUVs.x *= (_MasterScale * 2.0);
		effectUVs.y *= (_MasterScale * 2.0);
		float3 normal1 = UnpackNormal(tex2D(_TestHeight1, effectUVs));

		effectUVs = i.uv;
		effectUVs.y -= _Time * (_RefrSpeed*0.5);
		effectUVs.x *= (_MasterScale * 3.0);
		effectUVs.y *= (_MasterScale * 3.0);
		float3 normal2 = UnpackNormal(tex2D(_TestHeight2, effectUVs));
   		
   	

		//grab original background
		half4 oCol = half4(tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab)).rgb, 1);
		
		//calculate mask
		float4 DepthFade = float4(1.0,0.5,0.0,0.0);
		float4 edgeBlendFactors = float4(0.0, 0.0, 0.0, 0.0);
		half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
		half depth1 = LinearEyeDepth(depth);
		half depth2 = Linear01Depth(depth);
		edgeBlendFactors = saturate(DepthFade * (depth1-i.screenPos.w)); //z		
		half4 mCol;
		mCol = half4(edgeBlendFactors.y,edgeBlendFactors.y,edgeBlendFactors.y,1.0);
		
		//distort
		float3 combinedNormal = normalize(normal1 * normal2);
		float2 offset = combinedNormal.xy * (_RefrStrength*500.0) * _GrabTexture_TexelSize.xy * (_MasterScale);
		i.uvgrab.xy = (offset * i.uvgrab.z) + i.uvgrab.xy;
		half4 rCol = half4(tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab)).rgb, 1.0);
		
		//calculate second UVs
		float3 combinedNormal2 = normalize(normal1 * normal2);
		float2 offset2 = combinedNormal2.xy * (_RefrStrength*500.0) * _GrabTexture_TexelSize.xy * (_MasterScale);
		i.uvs.xy = (offset2 * i.uvs.z) + i.uvs.xy;
		i.uvs.zw *= 1.0;
		
		//Sample Depth
		half drefr = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uvs)).r);
		half dref = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r);

		//If Depth doesn't match, switch in refracted background
		//half colMask = 1.0-((dref*10.0)-(drefr*10.0));
		//if (colMask >= 0.999)
		//	oCol = rCol;
		rCol.a = 1.0;
		
		return rCol;
	}

	ENDCG
}










            












}
FallBack "Diffuse"
}



































