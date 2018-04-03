// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Suimono2/water_under_mobile" {


Properties {

	_Tess ("Tessellation", Float) = 4.0
    _minDist ("TessMin", Range(-180.0, 0.0)) = 10.0
    _maxDist ("TessMax", Range(20.0, 500.0)) = 25.0
    _Displacement ("Displacement", Range(0, 8.0)) = 0.3
    _MaskAmt ("Mask Strength", Range(1, 8.0)) = 1.0
    _ShoreAmt ("Shore Strength", Range(0, 2.0)) = 1.0
    _ShoreAmtDk ("Shore Strength Wet", Range(0, 2.0)) = 1.0  
         
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
	_AnimSpeed ("Animation Speed (0.0 - 1.0)", Range(0.0,1.0)) = 1.0
	
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
	_UnderFogDist ("Under Fog Distance", Float) = 1.0
	_UnderColor ("Underwater Color", Color) = (0.25,0.25,0.5,1.0)
	
	_WaveTex ("_WaveTex", 2D) = "gray" {}
	_FlowMap ("_FlowMap", 2D) = "gray" {}
	_FlowScale ("Flowmap Scale", Range(0.1,10.0)) = 0.0

	_WaveMap ("_WaveMap", 2D) = "gray" {}
}



Subshader 
{ 












//-----------------------
//      EDGE FOAM
//-----------------------
Tags {"Queue"= "Transparent"}
Cull Front
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

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;

float _FlowScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;



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
	half fspread = (half3(lerp((1.0-_FoamHeight),1.0,((N6.y)*(_HeightFoamSpread))),0,0).r)*_HeightFoamAmount;
	
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
	o.Albedo += (_EdgeColor.rgb * 2.0 * edgePos);// * (fspread * 0.15));
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
	
	//new
	//o.Albedo = IN.color.rgb;
	//o.Alpha = 1.0;
	
	//o.Alpha = 0.0;
			
}
ENDCG



























//-------------------
//    COLOR HEIGHTS
//-------------------
Tags {"Queue"= "Transparent"}
Cull Front
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
	//ZTest Always

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#include <UnityCG.cginc>
#pragma glsl

struct appdata {
	float4 vertex : POSITION;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
};

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
float _WaveScale;
float _MaskAmt;
float _ShoreAmt;
float _TimeX;
float _TimeY;
float _WaveShoreHeight;

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
	float2 uv_FoamTex;
	float2 uv_FoamOverlay;
	float2 uv_Surface1;
    float2 uv_WaveTex;
	float2 uv_FlowMap;
	float4 screenPos;
	float4 color : Color;
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
	
	
	
	o.Alpha = _LowColor.a;
	o.Alpha += half3(lerp((1.0-_CenterHeight),1.0,(N6.y*(_MaxVariance))),0,0).r * _HighColor.a;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;
	
	o.Albedo = lerp(_LowColor.rgb,_HighColor.rgb,half3(lerp((1.0-_CenterHeight),1.0,(N6.y*(_MaxVariance))),0,0).r);
	if (o.Albedo.r > 1.0) o.Albedo.r = 1.0;
	if (o.Albedo.r < 0.0) o.Albedo.r = 0.0;
	if (o.Albedo.g > 1.0) o.Albedo.g = 1.0;
	if (o.Albedo.g < 0.0) o.Albedo.g = 0.0;
	if (o.Albedo.b > 1.0) o.Albedo.b = 1.0;
	if (o.Albedo.b < 0.0) o.Albedo.b = 0.0;
	
	
	_SpecColor = lerp(_DepthColorG*0.75,half4(0,0,0,0),0.0);
	o.Specular = 0.02;
	o.Gloss = mask2*1.4;
	if (o.Gloss > 1.0) o.Gloss = 1.0;

}
ENDCG


























































// ------------------------------
//   MIRROR REFLECTION 
// ------------------------------
Tags {"Queue"= "Transparent"}
Cull Front
Blend SrcAlpha OneMinusSrcAlpha
//Blend OneMinusDstColor One
ZWrite Off
//ZTest Always
	
	
CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl


struct appdata {
	float4 vertex : POSITION;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
};

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
float _timeX;
float _WaveHeight;
float _WaveScale;
float _MaskAmt;
float _ShoreAmt;
float _TimeX;
float _TimeY;
float _WaveShoreHeight;

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
//sampler2D _ReflectionTex;
samplerCUBE _CubeMobile;
float4 _DynReflColor;
float _UnderReflDist;


void surf (Input IN, inout SurfaceOutput o) {


	//calculate distance mask
	float mask = (((IN.screenPos.w + _UnderReflDist))*0.1);
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
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
	N = lerp(half3(0,0,1),N,lerp(0.0,1.0,lerp(0.0,0.2,_BumpStrength)));
	N = lerp(half3(0,0,1),N,lerp(1.0,0.0,mask3));
	N += (N6);
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	o.Normal = lerp(half3(0,0,1.1),N,lerp(0.0,0.2,_BumpStrength));


	//decode reflection
	//IN.ref = (IN.screenPos);
	//float4 uv1 = IN.ref; uv1.xy;
	//uv1.y -= (N.y*_ReflectStrength);
	//half4 refl = tex2Dproj( _ReflectionTex, UNITY_PROJ_COORD(uv1));

	//o.Albedo = lerp(_DynReflColor.rgb,(refl.rgb*2.5*_DynReflColor.rgb),1.0)*2.0;
	//o.Alpha = lerp(0.1,1.0,mask);

	
	//
	//o.Albedo = lerp(_DepthColorR.rgb,o.Albedo,mask);

	half4 reflectionBase = texCUBE(_CubeMobile, WorldReflectionVector (IN, o.Normal)); 
	//o.Albedo = lerp(_DynReflColor.rgb,(reflectionBase.rgb*2.5*_DynReflColor.rgb*_DynReflColor.a),_DynReflColor.a);
	//o.Albedo = lerp(half3(0,0,0),o.Albedo,mask);
	
	
	o.Albedo = lerp(_DynReflColor.rgb,(reflectionBase.rgb*2.5*_DynReflColor.rgb),1.0)*2.0;
	//o.Alpha = lerp(0.1,1.0,mask);
	o.Albedo = lerp(_DepthColorR.rgb,o.Albedo,mask);
	//o.Albedo = reflectionBase.rgb;//_DepthColorR.rgb;
	o.Alpha = mask;
	
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
	

}




void surf (Input IN, inout SurfaceOutput o) {


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
	

	
	

	
	half4 foamTexture = tex2D(_FoamTex, IN.uv_FoamTex);
	half4 foamTextureb = tex2D(_FoamTex, half2(IN.uv_FoamTex.x*0.1,IN.uv_FoamTex.y*0.1));
	half4 foamTexturec = tex2D(_FoamTex, half2(IN.uv_FoamTex.x*2.5,IN.uv_FoamTex.y*2.5));
	half4 foamOverlay = tex2D(_FoamOverlay, IN.uv_FoamOverlay);
	
	//o.Albedo = _FoamColor * foamOverlay.rgb * 2.0;
	//half alphaCalc = 0.0;


	//height foam
	
	half cspread = tex2D(_FlowMap, IN.uv_FlowMap).g;
	half4 texwave = tex2D(_WaveMap, IN.uv_FlowMap);
	//half fspread = (wspread + cspread);
	//texwave.r *= 0.5;
	half fspread = cspread + (half3(lerp((1.0-_FoamHeight),1.0,((texwave.r*(_WaveHeight/5.0))*(_HeightFoamSpread))),0,0).r)*_HeightFoamAmount;
	
	
	half alphaCalc = lerp(0.0,foamTexture.b,tex2D(_FoamRamp, float2(1.0-fspread, 0.5)).b);
	alphaCalc += lerp(0.0,foamTexture.r,tex2D(_FoamRamp, float2(1.0-fspread, 0.5)).r);
	alphaCalc += lerp(0.0,foamTexture.g,tex2D(_FoamRamp, float2(1.0-fspread, 0.5)).g);
	
	if (alphaCalc > 1.0) alphaCalc = 1.0;
	



	o.Albedo = _FoamColor.rgb * foamOverlay.rgb * 2.0;
	o.Alpha = alphaCalc * _FoamColor.a;

	
	o.Specular = 0.0;
	o.Gloss = 0.0;
	o.Emission = 0.0;
	
	
	//
	//half alph = ((foamTexture.a*1.0) + (foamTexturec.a*0.5) + (foamTextureb.a*0.5));
	//if (alph > 1.0) alph = 1.0;
	//o.Alpha *= alph + foamTextureb.a;
	//if (o.Alpha > 1.0) o.Alpha = 1.0;
	//if (o.Alpha < 0.0) o.Alpha = 0.0;
	//o.Alpha = lerp(o.Alpha,0.0,mask1);
	
	
	

			
}
ENDCG





















// ------------------------------
//   SPECULAR 
// ------------------------------
Tags {"Queue"= "Transparency"}
Cull Front
//Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
//ZTest Always

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl


struct appdata {
	float4 vertex : POSITION;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
};

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
float _WaveScale;
float _MaskAmt;
float _ShoreAmt;
float _TimeX;
float _TimeY;
float _WaveShoreHeight;

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
float4 _SpecColorH;
float _SpecScatterWidth;
float _ReflDist;
float _ReflBlend;

void surf (Input IN, inout SurfaceOutput o) {


	//calculate distance mask
	float mask = (((IN.screenPos.w + _ReflDist))*(_ReflBlend));
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
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
	half3 N = lerp(N1,(N2*N3),mask2);
	N = lerp(N,N4,mask3);
	
	N = lerp(half3(0,0,1),half3(N.x,N.y,N.z),lerp(0.0,1.0,_BumpStrength));

	o.Normal = lerp(half3(0,0,1.1),N,_BumpStrength);
	
	o.Albedo = half3(0.5,0.5,0.5);
	o.Alpha = 0.0;
	
	o.Specular = 0.2+(0.3 * tex2D(_WaveRamp, float2(o.Normal.z,0.5)).r);
	o.Gloss = _SpecScatterWidth * mask2;
	_SpecColor = _SpecColorH * _LightColor0;

}


ENDCG





















// ---------------------------------
//   SURFACE OPACITY
// ---------------------------------
Tags {"Queue"= "Transparent"}
Cull Front
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
//ZTest Always


CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow vertex:disp nolightmap noambient 
#pragma glsl


struct appdata {
	float4 vertex : POSITION;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
};

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
float _WaveScale;
float _MaskAmt;
float _ShoreAmt;
float _TimeX;
float _TimeY;
float _UnderReflDist;
float _WaveShoreHeight;

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
float _UnderFogDist;

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
float _ReflBlend;
float _ReflDist;
float4 	_UnderColor;

void surf (Input IN, inout SurfaceOutput o) {

	//calculate distance mask
	float mask = (((IN.screenPos.w - (_UnderFogDist*1000.0)))*0.01);
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
	half3 N2 = UnpackNormal(tex2D(_Surface2, uvN));
	half3 N = N1;

	half heightInt = (_WaveHeight/5.0);
	if (heightInt > 1.0) heightInt = 1.0;
	N += UnpackNormal(tex2D(_WaveLargeTex, uvN*_WaveScale))*heightInt;
	if (N.x > 1.0) N.x = 1.0;
	if (N.y > 1.0) N.y = 1.0;
	if (N.z > 1.0) N.z = 1.0;
	
	//Set surface opacity
	o.Albedo = _UnderColor.rgb*2.0;
	o.Alpha = mask;// * _UnderColor.a;
	if (o.Alpha > 1.0) o.Alpha = 1.0;
	if (o.Alpha < 0.0) o.Alpha = 0.0;

}

ENDCG










}
FallBack ""
}
