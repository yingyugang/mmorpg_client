// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Suimono2/water_pro" {


Properties {

	_Tess ("Tessellation", Float) = 4.0
    _minDist ("TessMin", Range(-180.0, 0.0)) = 10.0
    _maxDist ("TessMax", Range(20.0, 500.0)) = 25.0
    _Displacement ("Displacement", Range(0, 8.0)) = 0.3
    _MaskAmt ("Mask Strength", Range(1, 8.0)) = 1.0
    _ShoreAmt ("Shore Strength", Range(0, 2.0)) = 1.0
    //_ShoreAmtDk ("Shore Strength Wet", Range(0, 2.0)) = 1.0  
         
    //_WaveMaskTex ("Wave Mask", 2D) = "gray" {}
    _WaveLargeTex ("Wave Large", 2D) = "gray" {}
   	_WaveHeight ("Wave Height", Range(0, 20.0)) = 0.0
   	_WaveShoreHeight ("Wave Shore Height", Range(0, 8.0)) = 0.0
   	_WaveScale ("Wave Scale", Range(0, 1.0)) = 0.25
	
	_CenterHeight ("Center Height", Float) = 0.0
	_MaxVariance ("Maximum Variance", Float) = 3.0

	_HighColor ("High Color", Color) = (1.0, 0.0, 0.0, 1.0)
	_LowColor ("Low Color", Color) = (0.0, 1.0, 0.0, 0.1)
		
	//_WaveAmt ("Wave Amount", Float) = 0.1
	_Surface1 ("Surface Distortion 1", 2D) = "" {}
	_Surface2 ("Surface Distortion 2", 2D) = "" {}
	_WaveRamp ("Wave Ramp", 2D) = "" {}
	
	_RefrStrength ("Refraction Strength (0.0 - 25.0)", Float) = 25.0
    _RefrSpeed ("Refraction Speed (0.0 - 0.5)", Float) = 0.5
    _RefrScale ("Refraction Scale", Float) = 0.5
	//_AnimSpeed ("Animation Speed (0.0 - 1.0)", Range(0.0,1.0)) = 1.0
	
	_SpecScatterWidth ("Specular Width", Range(1.0,10.0)) = 2.0
	_SpecScatterAmt ("Specular Scatter", Range(0.0001,0.05)) = 0.02
	_SpecColorH ("Hot Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_SpecColorL ("Reflect Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	
	_DynReflColor ("Reflection Dynamic", Color) = (1.0, 1.0, 1.0, 0.5)
	_ReflDist ("Reflection Distance", Float) = 1000
	_ReflBlend ("Reflection Blend", Range(0.002,0.1)) = 0.01
	_ReflBlur ("Reflection Blur", Range (0.0, 0.125)) = 0.01
	_ReflectionTex ("Reflection", 2D) = "" {}

	_DepthAmt ("Depth Amount", Float) = 0.1
	
	_DepthColor ("Depth Over Tint", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorR ("Depth Color 1(r)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorG ("Depth Color 2(g)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorB ("Depth Color 3(b)", Color) = (0.25,0.25,0.5,1.0)
	_DepthRamp ("Depth Color Ramp", 2D) = "" {}
	
	_BlurSpread ("Blur Spread", Range (0.0, 0.125)) = 0.01
	_BlurRamp ("Blur Ramp", 2D) = "" {}
	
	_FoamHeight ("Foam Height", Float) = 5.0
	_HeightFoamAmount ("Height Foam Amount", Range (0.0, 1.0)) = 1.0
	_HeightFoamSpread ("Height Foam Spread", Float) = 2.0
	
	_FoamSpread ("Foam Spread", Range (0.0, 1.0)) = 0.5
	_FoamColor ("Foam Color", Color) = (1,1,1,1)
	_FoamRamp ("Foam Ramp", 2D) = "" {}
	_FoamOverlay ("Foam Overlay (RGB)", 2D) = "" {}
	_FoamTex ("Foam Texture (RGB)", 2D) = "" {}

	_EdgeSpread ("Edge Spread", Range (0.04,5.0)) = 10.0
	_EdgeColor ("Edge Color", Color) = (1,1,1,1)
	
	_BumpStrength ("Normal Strength", Float) = 0.9
	_ReflectStrength ("Reflection Strength", Float) = 1.0
		
	_CubeTex ("Cubemap", CUBE) = "" {}
	_CubeMobile ("Cubemap Mobile", CUBE) = "" {}
    
	_MasterScale ("Master Scale", Float) = 1.0
	_UnderReflDist ("Under Reflection", Float) = 1.0
	_UnderColor ("Underwater Color", Color) = (0.25,0.25,0.5,1.0)
	
	_WaveTex ("_WaveTex", 2D) = "gray" {}
	_FlowMap ("_FlowMap", 2D) = "gray" {}
	_FlowScale ("Flowmap Scale", Range(0.1,10.0)) = 0.0

	_TideColor ("Tide Color", Color) = (0.0,0.0,0.2,1.0)
	_TideAmount ("Tide Amount", Range(0.0,1.0)) = 1.0
	_TideSpread ("Tide Spread", Range(0.02,1.0)) = 0.4

	_WaveMap ("_WaveMap", 2D) = "gray" {}
}



Subshader 
{ 













// ---------------------------------
//   DARK TIDE LINE
// ---------------------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
Fog {Mode Off}

CGPROGRAM
#pragma target 3.0
//#pragma surface surf Lambert vertex:vert noambient
#pragma surface surf Lambert addshadow vertex:disp nolightmap noambient 
#include <UnityCG.cginc>
#pragma glsl


struct Input {
	float2 uv_FoamTex;
	float2 uv_FoamOverlay;
	float2 uv_Surface1;
	float2 uv_FlowMap;
	float2 uv_WaveTex;
	float4 pos;
	float4 screenPos;
	float fade;
	float4 color : Color;
	
	//float4 ref;
	//float3 worldRefl;
    //INTERNAL_DATA
};


float _FoamHeight;
float _HeightFoamAmount;
float _HeightFoamSpread;
	

sampler2D _WaveRamp;
sampler2D _CameraDepthTexture;
sampler2D _DepthRamp;
float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;

float4 _SpecColorH;
float _SpecScatterWidth;
float _ReflDist;
float _ReflBlend;
float _ReflectStrength;
sampler2D _ReflectionTex;
samplerCUBE _CubeTex;
float4 _DynReflColor;
float _SpecScatterAmt;
float4 _SpecColorL;

float _MasterScale = 1.0;
float4 _FoamColor;
float4 _EdgeColor;
sampler2D _FoamTex;
float _EdgeSpread;
float _FoamSpread;
sampler2D _FoamRamp;
sampler2D _FoamOverlay;

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMap;
sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;

float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;

float _TimeX;
float _TimeY;

sampler2D _WaveTex;
sampler2D _FlowMap;

float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;
float4 _TideColor;
float _TideAmount;
float _TideSpread;

void disp (inout appdata_full v, out Input o) {

	//raise vertex
	v.vertex.xyz += v.normal*(_TideAmount);

	//
	o.uv_FoamTex = v.texcoord;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.screenPos = ComputeScreenPos(o.pos);
	
	// Convert to world position
	float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
	float diff = worldPos.y - _FoamHeight;
	float cFactor = saturate(diff/(2*_HeightFoamSpread) + 0.5);
	//lerp by factor
	o.fade = lerp(0.0, 1.0, cFactor);


}




void surf (Input IN, inout SurfaceOutput o) {

	
	//float4 foamFade = float4(1.0,_FoamSpread,0.0,0.0);
	//float4 edgeBlendFactors = float4(1.0, 0.0, 0.0, 0.0);
	half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	depth = LinearEyeDepth(depth);
	//edgeBlendFactors = saturate(foamFade * (depth-IN.screenPos.w));		
	//float4 foamSpread = (_FoamColor - (foamFade.w) * float4(0.15, 0.03, 0.01, 0.0));
	//foamSpread.a = (edgeBlendFactors.y);
	//foamSpread.a = 1.0-foamSpread.a;
	//if (foamSpread.a > 1.0){
	//	foamSpread.a=1.0;
	//}
	//if (foamSpread.a < 0.0){
	//	foamSpread.a=0.0;
	//}
	//half fspread = foamSpread.a;
	
	
	float4 foamFade2 = float4(1.0,_TideSpread,0.0,0.0);
	float4 edgeBlendFactors2 = float4(1.0, 0.0, 0.0, 0.0);
	edgeBlendFactors2 = saturate(foamFade2 * (depth-IN.screenPos.w));		
	float4 foamSpread2 = ((foamFade2.w) * float4(0.15, 0.03, 0.01, 0.0));
	foamSpread2.a = (edgeBlendFactors2.y);
	foamSpread2.a = 1.0-foamSpread2.a;
	if (foamSpread2.a > 1.0){
		foamSpread2.a=1.0;
	}
	if (foamSpread2.a < 0.0){
		foamSpread2.a=0.0;
	}
	half fspread2 = foamSpread2.a;


	
	//edge
	o.Albedo = (_TideColor.rgb);
	
	o.Alpha = fspread2*1.4;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	o.Alpha = (1.0-o.Alpha)*_TideColor.a;
	
	
	o.Specular = 0.0;
	o.Gloss = 0.0;
	o.Emission = 0.0;
	

			
}
ENDCG





















// ---------------------------------
//   WATER DEPTH 
// ---------------------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
Fog {Mode Off}

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl


//struct appdata_full {
//	float4 vertex : POSITION;
//	float4 tangent : TANGENT;
//	float3 normal : NORMAL;
//	float2 texcoord : TEXCOORD0;
//};

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
float _Phase;
//float _TimeX;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
float _ShoreAmt;
float _TimeX;
float _TimeY;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;


struct Input {
	float4 pos;
	float4 screenPos;	
	float2 uv_Surface1;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
};

void disp (inout appdata_full v){

	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
}






sampler2D _WaveRamp;
sampler2D _CameraDepthTexture;
sampler2D _DepthRamp;
float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;
float _SpecScatterWidth;



void surf (Input IN, inout SurfaceOutput o) {

	// calculate depth fogging
	float4 DepthFade = float4(1.0,(_DepthAmt * 0.01),0.0,0.0);
	float4 edgeBlendFactors = float4(0.0, 0.0, 0.0, 0.0);
	half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	depth = LinearEyeDepth(depth);
	edgeBlendFactors = saturate(DepthFade * (depth-IN.screenPos.w));		
		
	float4 depthColor = (tex2D(_DepthRamp, float2(0.0, 0.5)) - (DepthFade.w * float4(0.15, 0.03, 0.01, 0.0)));
	float depthAmt = depthColor.a;
	
	depthAmt *= (edgeBlendFactors.y);
	depthAmt = edgeBlendFactors.x * depthAmt;
	
	float depthPos = depthAmt-DepthFade.w;
	
	if (depthPos > 1.0) depthPos = 1.0;
	if (depthPos < 0.0) depthPos = 0.0;
	depthColor = tex2D(_DepthRamp, float2(depthPos, 0.5));
	
	half4 setColor = _DepthColorB * depthColor.b;
	setColor += _DepthColorG * depthColor.g;
	setColor += _DepthColorR * depthColor.r;
	
	//ASSIGN DEPTH COLOR AND ALPHA
	o.Albedo = setColor.rgb;
	
	o.Alpha = _DepthColorB.a * depthColor.b;
	o.Alpha += _DepthColorG.a * depthColor.g;
	o.Alpha += _DepthColorR.a * depthColor.r;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	o.Alpha *= (1.0 - (depthColor.a));


	

	//calculate distance mask
	float mask1 = ((IN.screenPos.w - lerp(60.0,20.0,(_DepthAmt/25.0)))*0.025);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w + 60.0)*0.002);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;
	
	

	o.Alpha += (mask1*_DepthColorB.a);
	o.Albedo = lerp(o.Albedo,_DepthColorB,mask1);
	
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN));
	half3 N = N1;
	
	half heightInt = (_WaveHeight/5.0);
	if (heightInt > 1.0) heightInt = 1.0;
	N += UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	o.Normal = lerp(half3(0,0,1),half3(N.x,N.y,N.z),lerp(0.1,(0.25*(_Displacement/8.0)),_BumpStrength));

	o.Specular = 0.0;
	o.Gloss = 0.0;


	//surface overcolor
	o.Albedo = lerp(o.Albedo,_DepthColor.rgb,_DepthColor.a);
	o.Alpha = lerp(o.Alpha,1.0,_DepthColor.a);
}

ENDCG



















//-------------------
//    COLOR HEIGHTS
//-------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
Fog {Mode Linear}

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#include <UnityCG.cginc>
#pragma glsl


//struct appdata_full {
//	float4 vertex : POSITION;
//	float4 tangent : TANGENT;
//	float3 normal : NORMAL;
//	float2 texcoord : TEXCOORD0;
//};

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
//float _ShoreAmt;
float _TimeX;
float _TimeY;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;




struct Input {
	float2 uv_FoamTex;
	float2 uv_FoamOverlay;
	float2 uv_Surface1;
	float4 screenPos;
	float4 color : Color;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
};



void disp (inout appdata_full v){

	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
}


float _FoamHeight;
float _HeightFoamAmount;
float _HeightFoamSpread;
	
float4 _SpecColorL;
float _SpecScatterAmt;
sampler2D _WaveRamp;
float4 _FoamColor;
float4 _EdgeColor;
sampler2D _FoamTex;
sampler2D _CameraDepthTexture;
float _EdgeSpread;
float _FoamSpread;
sampler2D _FoamRamp;
sampler2D _FoamOverlay;
float4 _HighColor;
float4 _LowColor;
float4 _DepthColorG;

float _CenterHeight;
float _MaxVariance;


void surf (Input IN, inout SurfaceOutput o) {



	//calculate distance mask
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w - 350.0)*0.003);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;

	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN))*1.07;
	half3 N2 = UnpackNormal(tex2D(_Surface1, uvN*3.0)) + (UnpackNormal(tex2D(_Surface2, uvN*3.0))*0.1);
	half3 N3 = UnpackNormal(tex2D(_Surface1, uvN*0.8))*1.2;
	half3 N4 = UnpackNormal(tex2D(_Surface1, uvN*0.2))*1.12;
	
	half heightInt = (_WaveHeight/10.0);
	if (heightInt > 1.0) heightInt = 1.0;
	half3 N6 = UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	
	half3 N = lerp(N1,(N2*N3),mask2);
	N = lerp(N,N4,mask3);
	N = lerp(half3(0,0,1),half3(N.x,N.y,N.z),lerp(0.0,1.0,_BumpStrength));
	
	N += N6;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	
	
	//
	half3 texwave = tex2D(_WaveMap, IN.uv_FlowMap).rgb;
	half3 texflow = tex2D(_FlowMap, IN.uv_FlowMap).rgb;
	//o.Gloss = lerp((texwave.r),0.0,texflow.r)*(_WaveHeight/5.0);
	
	o.Alpha = _LowColor.a;
	o.Alpha += half3(lerp((1.0-_CenterHeight),1.0,((texwave.r*(_WaveHeight/5.0))*(_MaxVariance))),0,0).r * _HighColor.a;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	
	o.Albedo = lerp(_LowColor.rgb,_HighColor.rgb,half3(lerp((1.0-_CenterHeight),1.0,(texwave.r*(_MaxVariance))),0,0).r);
	//
	
	if (o.Albedo.r > 1.0) o.Albedo.r = 1.0;
	if (o.Albedo.r < 0.0) o.Albedo.r = 0.0;
	if (o.Albedo.g > 1.0) o.Albedo.g = 1.0;
	if (o.Albedo.g < 0.0) o.Albedo.g = 0.0;
	if (o.Albedo.b > 1.0) o.Albedo.b = 1.0;
	if (o.Albedo.b < 0.0) o.Albedo.b = 0.0;
	
	
	_SpecColor = lerp(_DepthColorG*0.75,half4(0,0,0,0),0.0);
	o.Specular = 0.1;//_SpecScatterAmt;//0.1;
	o.Gloss = 0.0;//mask2*1.4;
	if (o.Gloss > 1.0) o.Gloss = 1.0;
	
	

}
ENDCG













//-------------------
//    BACK GLOW
//-------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
Fog {Mode Off} 


CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#include <UnityCG.cginc>
#pragma glsl


//struct appdata_full {
//	float4 vertex : POSITION;
//	float4 tangent : TANGENT;
//	float3 normal : NORMAL;
//	float2 texcoord : TEXCOORD0;
//};

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
//float _ShoreAmt;
float _TimeX;
float _TimeY;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;


struct Input {
	float2 uv_FoamTex;
	float2 uv_FoamOverlay;
	float2 uv_Surface1;
	float4 screenPos;
	float4 color : Color;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
};



void disp (inout appdata_full v){

	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
}


float _FoamHeight;
float _HeightFoamAmount;
float _HeightFoamSpread;

sampler2D _WaveRamp;
float4 _FoamColor;
float4 _EdgeColor;
sampler2D _FoamTex;
sampler2D _CameraDepthTexture;
float _EdgeSpread;
float _FoamSpread;
sampler2D _FoamRamp;
sampler2D _FoamOverlay;
float4 _HighColor;
float4 _LowColor;
float4 _DepthColorR;
float4 _UnderColor;

float _CenterHeight;
float _MaxVariance;

	

void surf (Input IN, inout SurfaceOutput o) {



	//calculate distance mask
	float mask1 = ((IN.screenPos.w + 90.0)*0.005);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w - 350.0)*0.003);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;

	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN))*1.07;
	half3 N2 = UnpackNormal(tex2D(_Surface1, uvN*3.0)) + (UnpackNormal(tex2D(_Surface2, uvN*3.0))*0.1);
	half3 N3 = UnpackNormal(tex2D(_Surface1, uvN*0.8))*1.2;
	half3 N4 = UnpackNormal(tex2D(_Surface1, uvN*0.2))*1.12;
	
	half heightInt = (_WaveHeight/10.0);
	if (heightInt > 1.0) heightInt = 1.0;
	half3 N6 = UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	
	half3 N = lerp(N1,(N2*N3),mask2);
	N = lerp(N,N4,mask3);
	N = lerp(half3(0,0,1),half3(N.x,N.y,N.z),lerp(0.0,1.0,_BumpStrength));
	
	N += N6;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	_SpecColor = _UnderColor*1.5;//_DepthColorR*1.5;
	o.Specular = 0.075;
	o.Gloss = half3(lerp(0.6,1.0,(N6.y*(8))),0,0).r;// * (mask1);
	if (o.Gloss > 1.0) o.Gloss = 1.0;
	if (o.Gloss < 0.0) o.Gloss = 0.0;
	
	
	//o.Albedo = half3(1,0,0);
	o.Albedo = half3(0.5,0.5,0.5);
	o.Alpha = 0.0;
	
	//
	half3 texwave = tex2D(_WaveMap, IN.uv_FlowMap).rgb;
	half3 texflow = tex2D(_FlowMap, IN.uv_FlowMap).rgb;
	o.Gloss = lerp((texwave.r),0.0,texflow.r)*(_WaveHeight/5.0);
	
	
}
ENDCG

















//-----------------------
//      EDGE FOAM
//-----------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
Fog {Mode Off}

CGPROGRAM
#pragma target 3.0
//#pragma surface surf Lambert vertex:vert noambient
#pragma surface surf Lambert addshadow vertex:disp nolightmap noambient 
#include <UnityCG.cginc>
#pragma glsl


struct Input {
	float2 uv_FoamTex;
	float2 uv_FoamOverlay;
	float2 uv_Surface1;
	float2 uv_FlowMap;
	float2 uv_WaveTex;
	float4 pos;
	float4 screenPos;
	float fade;
	float4 color : Color;
	
	//float4 ref;
	//float3 worldRefl;
    //INTERNAL_DATA
};


float _FoamHeight;
float _HeightFoamAmount;
float _HeightFoamSpread;
	

sampler2D _WaveRamp;
sampler2D _CameraDepthTexture;
sampler2D _DepthRamp;
float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;

float4 _SpecColorH;
float _SpecScatterWidth;
float _ReflDist;
float _ReflBlend;
float _ReflectStrength;
sampler2D _ReflectionTex;
samplerCUBE _CubeTex;
float4 _DynReflColor;
float _SpecScatterAmt;
float4 _SpecColorL;

float _MasterScale = 1.0;
float4 _FoamColor;
float4 _EdgeColor;
sampler2D _FoamTex;
float _EdgeSpread;
float _FoamSpread;
sampler2D _FoamRamp;
sampler2D _FoamOverlay;

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMap;
sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;

float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;

float _TimeX;
float _TimeY;

sampler2D _WaveTex;
sampler2D _FlowMap;

float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;


void disp (inout appdata_full v, out Input o) {


	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
	
	//
	o.uv_FoamTex = v.texcoord;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.screenPos = ComputeScreenPos(o.pos);
	
	// Convert to world position
	float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
	float diff = worldPos.y - _FoamHeight;
	float cFactor = saturate(diff/(2*_HeightFoamSpread) + 0.5);
	//lerp by factor
	o.fade = lerp(0.0, 1.0, cFactor);


}




void surf (Input IN, inout SurfaceOutput o) {



	float4 foamFade = float4(1.0,_FoamSpread,0.0,0.0);
	float4 edgeBlendFactors = float4(1.0, 0.0, 0.0, 0.0);
	half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	depth = LinearEyeDepth(depth);
	edgeBlendFactors = saturate(foamFade * (depth-IN.screenPos.w));		
	float4 foamSpread = (_FoamColor - (foamFade.w) * float4(0.15, 0.03, 0.01, 0.0));
	foamSpread.a = (edgeBlendFactors.y);
	foamSpread.a = 1.0-foamSpread.a;
	if (foamSpread.a > 1.0){
		foamSpread.a=1.0;
	}
	if (foamSpread.a < 0.0){
		foamSpread.a=0.0;
	}
	//half fspread = foamSpread.a;
	
	
	float4 foamFade2 = float4(1.0,_EdgeSpread,0.0,0.0);
	float4 edgeBlendFactors2 = float4(1.0, 0.0, 0.0, 0.0);
	edgeBlendFactors2 = saturate(foamFade2 * (depth-IN.screenPos.w));		
	float4 foamSpread2 = (_FoamColor - (foamFade2.w) * float4(0.15, 0.03, 0.01, 0.0));
	foamSpread2.a = (edgeBlendFactors2.y);
	foamSpread2.a = 1.0-foamSpread2.a;
	if (foamSpread2.a > 1.0){
		foamSpread2.a=1.0;
	}
	if (foamSpread2.a < 0.0){
		foamSpread2.a=0.0;
	}
	half fspread2 = foamSpread2.a;
	
	
	//fspread += (IN.fade * _HeightFoamAmount);
	//fspread += IN.color.g;
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	//calculate distance mask
	float mask1 = ((IN.screenPos.w - 400.0)*0.01);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w - 350.0)*0.003);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;

	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN))*1.07;
	half3 N2 = UnpackNormal(tex2D(_Surface1, uvN*3.0)) + (UnpackNormal(tex2D(_Surface2, uvN*3.0))*0.1);
	half3 N3 = UnpackNormal(tex2D(_Surface1, uvN*0.8))*1.2;
	half3 N4 = UnpackNormal(tex2D(_Surface1, uvN*0.2))*1.12;
	
	half heightInt = (_WaveHeight/10.0);
	if (heightInt > 1.0) heightInt = 1.0;
	half3 N6 = UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	
	half3 N = lerp(N1,(N2*N3),mask2);
	N = lerp(N,N4,mask3);
	N = lerp(half3(0,0,1),half3(N.x,N.y,N.z),lerp(0.0,1.0,_BumpStrength));
	
	N += N6;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	

	//add shore wave foam
 	half4 getflowmap = tex2D(_FlowMap,IN.uv_FlowMap);// * 2.0f - 1.0f;
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	half4 waveTex0 = (tex2D(_WaveTex, float2(IN.uv_WaveTex.x, IN.uv_WaveTex.y) + flowmap * phase0));
	half4 waveTex1 = (tex2D(_WaveTex, float2(IN.uv_WaveTex.x, IN.uv_WaveTex.y) + flowmap * phase1));
	half4 waveTex = lerp( waveTex0, waveTex1, f );

	half waveFoam = (1.0-waveTex.b)*(getflowmap.r*0.5);
	
	half4 tex = tex2D(_FlowMap,IN.uv_FlowMap);
	_HeightFoamSpread = lerp(_HeightFoamSpread,_HeightFoamSpread*0.5,tex.r);	
	_HeightFoamSpread = lerp(_HeightFoamSpread,1.0,tex.g);
		
	//Add HeightFoam
	half3 texwave = tex2D(_WaveMap, IN.uv_FlowMap).rgb;
	half3 texflow = tex2D(_FlowMap, IN.uv_FlowMap).rgb;
	half fspread = (half3(lerp((1.0-_FoamHeight),1.0,((texwave.r*(_WaveHeight/5.0))*(_HeightFoamSpread))),0,0).r)*_HeightFoamAmount;
	half fspread3 = foamSpread.a;
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	half4 foamTexture = tex2D(_FoamTex, IN.uv_FoamTex);
	half4 foamTextureb = tex2D(_FoamTex, half2(IN.uv_FoamTex.x*0.1,IN.uv_FoamTex.y*0.1));
	half4 foamTexturec = tex2D(_FoamTex, half2(IN.uv_FoamTex.x*2.5,IN.uv_FoamTex.y*2.5));
	half4 foamOverlay = tex2D(_FoamOverlay, IN.uv_FoamOverlay);
	
	o.Albedo = _FoamColor * foamOverlay.rgb * 2.0;
	
	half alphaCalc = 0.0;


	//height foam
	alphaCalc += lerp(0.0,foamTexture.b,tex2D(_FoamRamp, float2(1.0-fspread, 0.5)).b);
	alphaCalc += lerp(0.0,foamTexture.r,tex2D(_FoamRamp, float2(1.0-fspread, 0.5)).r);
	alphaCalc += lerp(0.0,foamTexture.g,tex2D(_FoamRamp, float2(1.0-fspread, 0.5)).g);
	if (alphaCalc > 1.0) alphaCalc = 1.0;
	alphaCalc *= _FoamColor.a;
	alphaCalc *= tex2D(_FoamRamp, float2(1.0-fspread, 0.5)).a * 2.0;
	if (alphaCalc >= 0.99) alphaCalc = 0.99;
	if (alphaCalc < 0.0) alphaCalc = 0.0;
	o.Alpha = 0.0;
	if (alphaCalc > 0.0 && alphaCalc < 1.0) o.Alpha = alphaCalc;
	
	
	
	
	//edge foam
	half alphaCalc2 = 0.0;
	alphaCalc2 = lerp(0.0,foamTexture.b,tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).b);
	alphaCalc2 += lerp(0.0,foamTexture.r,tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).r);
	alphaCalc2 += lerp(0.0,foamTexture.g,tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).g);
	if (alphaCalc2 > 1.0) alphaCalc2 = 1.0;
	alphaCalc2 *= _FoamColor.a;
	alphaCalc2 *= tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).a * 2.0;
	if (alphaCalc2 >= 0.99) alphaCalc2 = 0.99;
	if (alphaCalc2 < 0.0) alphaCalc2 = 0.0;
	
	o.Alpha += alphaCalc2;

	

	
	
	
	//edge line
	half edgePos = ((fspread2) * _EdgeColor.a);
	o.Albedo *= (1.0 - edgePos);
	o.Albedo += (_EdgeColor.rgb * 2.0 * edgePos);
	o.Alpha += edgePos*1.4;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	
	
	
	o.Specular = 0.0;
	o.Gloss = 0.0;
	o.Emission = 0.0;
	
	
	//
	half alph = ((foamTexture.a*1.0) + (foamTexturec.a*0.5) + (foamTextureb.a*0.5));
	if (alph > 1.0) alph = 1.0;
	o.Alpha *= alph + foamTextureb.a;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	o.Alpha = lerp(o.Alpha,0.0,mask1);
	
	
	
	
	//new
	//o.Albedo = IN.color.rgb;
	//o.Alpha = 1.0;
	
	//o.Alpha = 0.0;
			
}
ENDCG












// ------------------------
//    RENDER REFRACTION
// ------------------------
 
GrabPass {
	Tags {"Queue" = "Transparent"}
	Name "SurfaceGrab"
}



Pass{
	Tags {"Queue"= "Transparent"}
	Cull Back
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off
	Fog {Mode Off}
	
	//ZTest Always
	
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
		float4 uv_Surface1      : TEXCOORD4;
		//float4 uv_Surface2      : TEXCOORD5;
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
		
		o.uv = v.texcoord.xy;
		
		o.uvgrab.xy = (float2(o.pos.x, o.pos.y * scale) + o.pos.w) * 0.5;
		o.uvgrab.zw = o.pos.zw;
		
		o.uvs.xy = (float2(o.pos.x, o.pos.y * scale) + (o.pos.w)) * 0.5;
		o.uvs.z = o.pos.z;
		o.uvs.w = o.pos.w;
		
		return o;
	}
	sampler2D _CameraDepthTexture;
	float _RefrSpeed, _RefrStrength,_RefrScale;
	sampler2D _GrabTexture;
	float4 _GrabTexture_TexelSize;
	float _MasterScale;
	
	//float _DepthAmt;
	
	sampler2D _Surface1;
	sampler2D _Surface2;
	float _WaveAmt;
	//float _NormalAmt;
	float4 _DepthColor;

	
	
	half4 frag( v2f i ) : COLOR
	{

		_RefrSpeed *= (1.0);//0.1
		float2 effectUVs = i.uv;
		effectUVs.y += _Time * (_RefrSpeed);//0.5
		//_RefrScale = 1.0;
		effectUVs.x *= (_MasterScale * _RefrScale);
		effectUVs.y *= (_MasterScale * _RefrScale);
		float3 normal1 = UnpackNormal(tex2D(_Surface1, effectUVs));

		effectUVs = i.uv;
		effectUVs.y -= _Time * (_RefrSpeed);//0.5
		effectUVs.x *= (_MasterScale * _RefrScale);
		effectUVs.y *= (_MasterScale * _RefrScale);
		float3 normal2 = UnpackNormal(tex2D(_Surface2, effectUVs));
   		
   	

		//grab original background
		//half4 oCol = half4(0,0,0,0);
		//half4 oCol = half4(0,0,0,0);//(tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab)).rgb, 1);
		half4 oCol = half4(tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab)).rgb, 1);
		
		//if (sCol.a == 0.0) oCol = sCol;
		
		
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
		
		half4 useGrab = i.uvgrab;
		float2 offset = combinedNormal.xy * _RefrStrength * _GrabTexture_TexelSize.xy * _MasterScale;
		useGrab.xy = (offset * useGrab.z) + useGrab.xy;
		
		half4 xCol;
		xCol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(useGrab)).rgba;

		half4 rCol = half4(xCol.r,xCol.g,xCol.b,xCol.a);
		
		
		//calculate second UVs
		float3 combinedNormal2 = normalize(normal1 * normal2);
		float2 offset2 = combinedNormal2.xy * _RefrStrength * _GrabTexture_TexelSize.xy * _MasterScale;
		i.uvs.xy = (offset2 * i.uvs.z) + i.uvs.xy;
		i.uvs.zw *= 1.0;
		
		//Sample Depth
		half drefr = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uvs)).r);
		half dref = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r);

		//If Depth doesn't match, switch in refracted background
		half colMask = 1.0-((dref*10.0)-(drefr*10.0));
		if (colMask >= 0.99)
			oCol = rCol;
		
		//oCol.rgb *= 3.0;
		//oCol.a = 0.0;
		//if (oCol.r > 1.0) oCol.r = 1.0;
		//if (oCol.g > 1.0) oCol.g = 1.0;
		//if (oCol.b > 1.0) oCol.b = 1.0;
		
		oCol.a = 1.0;//-rCol.a;
		//if (sCol.r == 1.0){
			//oCol.rgb = half3(0,0,0);
		//}
		
		//oCol.rgb = lerp(oCol.rgb,half3(0,0,0),_SetDistort);
		//oCol.a = lerp(oCol.a,0.0,_SetDistort);
		
		return oCol;
	}

	ENDCG
}











// -----------------------
//   UNDERWATER BLURRING 
// -----------------------
GrabPass {
	Tags {"Queue" = "Transparent"}
	Name "BlurGrab"
}

Pass{
	Tags {"Queue"= "Transparent"}
	Cull Back
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off
	Fog {Mode Off}
	
	
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
	
	return rCol;
	}
	

	
	ENDCG
} 






































// ---------------------------------
//   SURFACE OPACITY
// ---------------------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite On
Fog {Mode Linear}

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl


//struct appdata_full {
//	float4 vertex : POSITION;
//	float4 tangent : TANGENT;
//	float3 normal : NORMAL;
//	float2 texcoord : TEXCOORD0;
//};

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
float _Phase;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
//float _ShoreAmt;
float _TimeX;
float _TimeY;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;


struct Input {
	float4 screenPos;	
	float2 uv_Surface1;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
};

void disp (inout appdata_full v){

	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
}


float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float _ReflBlend;
float _ReflDist;


void surf (Input IN, inout SurfaceOutput o) {

	//calculate distance mask
	float mask = (((IN.screenPos.w + _ReflDist))*(_ReflBlend));
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w - (200.0-(200.0*_DepthColor.a)))*0.005);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;
	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN));
	//half3 N2 = UnpackNormal(tex2D(_Surface2, uvN2));
	half3 N = N1;

	half heightInt = (_WaveHeight/5.0);
	if (heightInt > 1.0) heightInt = 1.0;
	N += UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	//Set surface opacity
	o.Albedo = _DepthColor.rgb*2.0;
	o.Alpha = mask3;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	
	
	//calculate wave height shading
 	half4 getflowmap = tex2D(_FlowMap,IN.uv_FlowMap);// * 2.0f - 1.0f;
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;//tex2D( _NoiseMap, IN.uv_NoiseMap).r;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	half4 waveTex0 = (tex2D(_WaveTex, float2(IN.uv_WaveTex.x, IN.uv_WaveTex.y) + flowmap * phase0));
	half4 waveTex1 = (tex2D(_WaveTex, float2(IN.uv_WaveTex.x, IN.uv_WaveTex.y) + flowmap * phase1));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	o.Albedo = half3(0,0,0);//lerp(_DepthColorR.rgb,half3(0,0,0),waveTex.b);//getflowmap.rgb*2.0*waveTex.b;
	o.Alpha += waveTex.b*getflowmap.r;//(getflowmap.g + getflowmap.r);
	o.Alpha = 0.0;

}

ENDCG



























// ------------------------------
//   MIRROR REFLECTION 
// ------------------------------
Tags {"Queue"= "Transparent"}
Cull Back
//Blend SrcAlpha OneMinusSrcAlpha
Blend OneMinusDstColor One
ZWrite On
Fog {Mode Linear}

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl


//struct appdata_full {
//	float4 vertex : POSITION;
//	float4 tangent : TANGENT;
//	float3 normal : NORMAL;
//	float2 texcoord : TEXCOORD0;
//};

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
//float _TimeX;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
//float _ShoreAmt;
float _TimeX;
float _TimeY;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;



struct Input {
    float4 pos;
	float4 screenPos;
	float4 ref;
	float2 uv_Surface1;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
	float3 worldRefl;
    INTERNAL_DATA
};


void disp (inout appdata_full v){

	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
}






sampler2D _WaveRamp;
sampler2D _CameraDepthTexture;
sampler2D _DepthRamp;
float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;
//float _BumpStrength;
float4 _SpecColorH;
float _SpecScatterWidth;
float _ReflDist;
float _ReflBlend;
float _ReflectStrength;
sampler2D _ReflectionTex;
samplerCUBE _CubeTex;
samplerCUBE _CubeMobile;
float4 _DynReflColor;
float useReflection;


void surf (Input IN, inout SurfaceOutput o) {


	//calculate distance mask
	float mask = (((IN.screenPos.w + _ReflDist))*(_ReflBlend));
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
	float mask1 = ((IN.screenPos.w - 800.0)*0.0005);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w - 10.0)*0.005);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;

	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	float2 uvN2 = IN.uv_Surface1;
	uvN2.x = 1.0-uvN2.x;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN))*1.07;
	half3 N2 = UnpackNormal(tex2D(_Surface1, uvN*3.0)) + (UnpackNormal(tex2D(_Surface2, uvN*3.0))*0.1);
	half3 N3 = UnpackNormal(tex2D(_Surface1, uvN*0.8))*1.2;

	half3 N5 = UnpackNormal(tex2D(_Surface1, uvN2))*0.75;
	
	half heightInt = (_WaveHeight/10.0);
	if (heightInt > 1.0) heightInt = 1.0;
	half3 N6 = UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	half3 N = lerp(N1,(N2*N3),mask2);
	N = lerp(half3(0,0,1),N,lerp(0.0,1.0,_BumpStrength));
	N = lerp(half3(0,0,1),N,lerp(1.0,0.0,mask3));
	N += N6;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	o.Normal = lerp(half3(0,0,1.1),N,_BumpStrength);


	//decode reflection
	IN.ref = (IN.screenPos);
	float4 uv1 = IN.ref; uv1.xy;
	uv1.y -= (N.y*_ReflectStrength);
	half4 refl = tex2Dproj( _ReflectionTex, UNITY_PROJ_COORD(uv1));

	half3 dynRef = lerp(_DynReflColor.rgb,(refl.rgb*2.5*_DynReflColor.rgb*_DynReflColor.a),_DynReflColor.a);
	dynRef = lerp(half3(0,0,0),dynRef,mask);
	dynRef = lerp(dynRef,_DepthColorG.rgb,mask1);
	o.Alpha = 0.0;
	
	//half3 cubeRef = half3(1,0,0);
	half3 cubeRef = texCUBE(_CubeMobile, WorldReflectionVector (IN, o.Normal)).rgb; 
	cubeRef = lerp(half3(0,0,0),cubeRef,mask);
	cubeRef = lerp(cubeRef,_DepthColorG.rgb,mask1);

	o.Albedo = lerp(cubeRef,dynRef,useReflection);
	
	//
	//half3 texwave = tex2D(_WaveMap, IN.uv_FlowMap).rgb;
	//half3 texflow = tex2D(_FlowMap, IN.uv_FlowMap).rgb;
	//o.Albedo *= lerp((1.0-texwave.r),1.0,texflow.r);
	
	
}


ENDCG
















// ------------------------------
//   HOT SPECULAR 
// ------------------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite On
Fog {Mode Linear}

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl

//struct appdata_full {
//	float4 vertex : POSITION;
//	float4 tangent : TANGENT;
//	float3 normal : NORMAL;
//	float2 texcoord : TEXCOORD0;
//};

float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
//float _ShoreAmt;
float _TimeX;
float _TimeY;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;



struct Input {
	float4 pos;
	float4 screenPos;	
	float2 uv_Surface1;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
};


void disp (inout appdata_full v){

	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
}




sampler2D _WaveRamp;
sampler2D _CameraDepthTexture;
sampler2D _DepthRamp;
float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;
float4 _SpecColorH;
float _SpecScatterWidth;
float _ReflDist;
float _ReflBlend;

void surf (Input IN, inout SurfaceOutput o) {


	//calculate distance mask
	float mask = (((IN.screenPos.w + _ReflDist))*(_ReflBlend));
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
	float mask1 = ((IN.screenPos.w - 800.0)*0.0003);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w - 350.0)*0.003);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;
	
	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN))*1.07;
	half3 N2 = UnpackNormal(tex2D(_Surface1, uvN*3.0)) + (UnpackNormal(tex2D(_Surface2, uvN*3.0))*0.1);
	half3 N3 = UnpackNormal(tex2D(_Surface1, uvN*0.8))*1.2;
	half3 N4 = UnpackNormal(tex2D(_Surface1, uvN*0.2))*1.12;
	//half3 N = lerp(N1,(N2*N3),mask2);
	half3 N = lerp((N2*N3),(N2*N3*N4),mask3);
	N = lerp(N,half3(0,0,1.1),mask1);
	//N = lerp(N,N4,mask3);
	
	N = lerp(half3(0,0,1),half3(N.x,N.y,N.z),lerp(0.0,1.0,_BumpStrength));

	o.Normal = lerp(half3(0,0,1.075),N,_BumpStrength);
	//o.Normal = lerp(o.Normal,half3(0,0,1),mask1);
	
	o.Albedo = half3(0.5,0.5,0.5);
	o.Alpha = 0.0;
	
	o.Specular = 0.02+(0.3 * tex2D(_WaveRamp, float2(o.Normal.z,0.5)).r);
	//o.Specular = lerp(0.3,1.0,mask1);
	o.Gloss = _SpecScatterWidth;// * mask2;
	//o.Gloss = lerp(o.Gloss,0.0,mask1);
	_SpecColor = _SpecColorH * _LightColor0;


}


ENDCG













































// ------------------------------
//   AMBIENT REFLECTION 
// ------------------------------
Tags {"Queue"= "Transparent"}
Cull Back
//Blend SrcAlpha OneMinusSrcAlpha
Blend OneMinusDstColor One

ZWrite On
Fog {Mode Off}

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl


//struct appdata_full {
//	float4 vertex : POSITION;
//	float4 tangent : TANGENT;
//	float3 normal : NORMAL;
//	float2 texcoord : TEXCOORD0;
//};

float _Tess;
float _minDist;
float _maxDist;


sampler2D _WaveMaskTex;
sampler2D _WaveLargeTex;
sampler2D _Surface1;
sampler2D _Surface2;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
//float _TimeX;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
//float _ShoreAmt;
float _TimeX;
float _TimeY;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float _ShoreAmt;



struct Input {
    float4 pos;
	float4 screenPos;
	float4 ref;
	float2 uv_Surface1;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
	float3 worldRefl;
    INTERNAL_DATA
};


void disp (inout appdata_full v){

	//calculate waves
	half2 tex = v.texcoord;
	half2 tex2 = v.texcoord;
	
	half2 _offset = half2(_TimeX,_TimeY)*0.9;
	float h = tex2Dlod(_WaveLargeTex, float4(tex.x*_dScaleX*_WaveScale+_offset.x, tex.y*_dScaleY*_WaveScale+_offset.y,0.0,0.0)).r*(_WaveHeight*0.2);
	//
	
	float d = tex2Dlod(_Surface1, float4(tex.x*_dScaleX-_offset.y, tex.y*_dScaleY-_offset.x,0,0)).r * (4.0*_BumpStrength*_Displacement);
	
	//calculate flowmap waves
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	halfCycle = 0.1;
 	float2 flowmap = float2(getflowmap.r,getflowmap.b) * 2.0f - 1.0f;
    float cycleOffset = 1.0;
    float phase0 = cycleOffset * 1.0 + flowMapOffset0;
    float phase1 = cycleOffset * 1.0 + flowMapOffset1;
	flowmap.x = lerp(0.0,flowmap.x,_FlowScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowScale);
	phase0 = lerp(1.0,phase0,_FlowScale);
	phase1 = lerp(phase0,phase1,_FlowScale);
    float f = ( abs( halfCycle - flowMapOffset0 ) / halfCycle );
	float4 waveTex0 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase0),0.0,0.0));
	float4 waveTex1 = tex2Dlod(_WaveTex, float4((tex.xy+_offsetFlow+flowmap*phase1),0.0,0.0));
	half4 waveTex = lerp( waveTex0, waveTex1, f );
	half flowFactor = waveTex.b-waveTex.r;
	
	float z = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0)).g;
	
	half3 waveFac = (v.normal * h *(_WaveHeight*((1.0-z)*(1.0-s))));
	half3 shoreFac = (v.normal * waveFac + (flowFactor*_WaveShoreHeight* (getflowmap.r-getflowmap.g)));
	
	//
	float texwaveFac = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0.0,0.0)).r * (_WaveHeight*4.0);
	v.vertex.xyz += (v.normal * texwaveFac * h) + (shoreFac);
	
}






sampler2D _WaveRamp;
sampler2D _CameraDepthTexture;
sampler2D _DepthRamp;
float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;
//float _BumpStrength;
float4 _SpecColorH;
float _SpecScatterWidth;
float _ReflDist;
float _ReflBlend;
float _ReflectStrength;
sampler2D _ReflectionTex;
samplerCUBE _CubeTex;
float4 _DynReflColor;
float _SpecScatterAmt;
float4 _SpecColorL;


void surf (Input IN, inout SurfaceOutput o) {


	//calculate distance mask
	float mask = (((IN.screenPos.w + _ReflDist))*(_ReflBlend));
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
	float mask1 = ((IN.screenPos.w - 700.0)*0.0002);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;
	float mask2 = ((IN.screenPos.w + 40.0)*0.005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	float mask3 = ((IN.screenPos.w - 10.0)*0.005);
	if (mask3 > 1.0) mask3 = 1.0;
	if (mask3 < 0.0) mask3 = 0.0;

	
	//calculate normal
	float2 uvN = IN.uv_Surface1;
	float2 uvN2 = IN.uv_Surface1;
	uvN2.x = 1.0-uvN2.x;
	half3 N1 = UnpackNormal(tex2D(_Surface1, uvN))*1.07;
	half3 N2 = UnpackNormal(tex2D(_Surface1, uvN*3.0)) + (UnpackNormal(tex2D(_Surface2, uvN*3.0))*0.1);
	half3 N3 = UnpackNormal(tex2D(_Surface1, uvN*0.8))*1.2;

	half3 N5 = UnpackNormal(tex2D(_Surface1, uvN2))*0.75;
	
	half heightInt = (_WaveHeight/10.0);
	if (heightInt > 1.0) heightInt = 1.0;
	half3 N6 = UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	half3 N = lerp(N1,(N2*N3),mask2);
	N = lerp(half3(0,0,1),N,lerp(0.0,1.0,_BumpStrength));
	N = lerp(half3(0,0,1),N,lerp(1.0,0.0,mask3));
	N += N6;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	o.Normal = lerp(half3(0,0,1.1),N,_BumpStrength);


	//decode reflection
	//IN.ref = (IN.screenPos);
	//float4 uv1 = IN.ref; uv1.xy;
	//uv1.y -= (N.y*_ReflectStrength);
	//half4 refl = tex2Dproj( _ReflectionTex, UNITY_PROJ_COORD(uv1));

	half4 reflectionBase = texCUBE(_CubeTex, WorldReflectionVector (IN, o.Normal)); 
	o.Albedo = reflectionBase.rgb;
	

	//o.Albedo *= _DynReflColor.rgb;// lerp(_DynReflColor.rgb,(refl.rgb*2.5*_DynReflColor.rgb),_DynReflColor.a);
	//o.Albedo = lerp(half3(0,0,0),o.Albedo,mask);
	//o.Albedo = lerp(o.Albedo,_DepthColorG.rgb,mask1)*0.5;
	
	//o.Albedo = lerp(half3(0,0,0),o.Albedo,mask3)*o.Normal.z;
	o.Albedo = half3(0,0,0);
	o.Alpha = 0.0;
	
	
	//_SpecScatterAmt
	//_SpecColorL

	_SpecColor = reflectionBase*_SpecColorL;
	o.Specular = _SpecScatterAmt;//_SpecScatterAmt;//0.1;//
	o.Gloss = _SpecColorL.a;//1.0;//
	
	


}
ENDCG













}
FallBack ""
}
