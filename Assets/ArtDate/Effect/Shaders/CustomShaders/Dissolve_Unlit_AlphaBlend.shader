//***coded by 3Fun,2014.09.27
//***
Shader "Custom/Dissolve_Unlit_AlphaBlend" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		
		_Amount ("Amount", Range (0, 1)) = 0.5
		_StartAmount("StartAmount", float) = 0.1
		_TransRange("Transparent Range",Range(0,0.1))=0.01
		_Illuminate ("Illuminate", Range (0, 2)) = 1
		_DissColor ("DissColor", Color) = (1,1,1,1)
		
		_DissolveSrc ("Mask Map", 2D) = "white" {}
		_DissolveSrcBump ("Displacement Map", 2D) = "white" {}
		_Distortion("Distortion Amount",Range(-1,1))=0.5
		_DistSpeed("Displacement Speed",float)=1
		

	}
	SubShader { 
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		
		Pass{
			Tags { "LightMode" = "Forwardbase" } 			
			cull off
			Blend SrcAlpha OneMinusSrcAlpha
			
			Program "vp" {
// Vertex combos: 12
//   opengl - ALU: 6 to 6
//   d3d9 - ALU: 6 to 6
//   d3d11 - ALU: 6 to 6, TEX: 0 to 0, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
Vector 6 [_DissolveSrcBump_ST]
"3.0-!!ARBvp1.0
# 6 ALU
PARAM c[7] = { program.local[0],
		state.matrix.mvp,
		program.local[5..6] };
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[6].xyxy, c[6];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
Vector 5 [_DissolveSrcBump_ST]
"vs_3_0
; 6 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mad o1.zw, v1.xyxy, c5.xyxy, c5
mad o1.xy, v1, c4, c4.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_DissolveSrcBump_ST]
Vector 4 [_MainTex_ST]
Matrix 0 [glstate_matrix_mvp] 4
// Shader Timing Estimate, in Cycles/64 vertex vector:
// ALU: 8.00 (6 instructions), vertex: 32, texture: 0,
//   sequencer: 10,  3 GPRs, 31 threads,
// Performance (if enough threads): ~32 cycles per vector
// * Vertex cycle estimates are assuming 3 vfetch_minis for every vfetch_full,
//     with <= 32 bytes per vfetch_full group.

"vs_360
backbbabaaaaabceaaaaaajaaaaaaaaaaaaaaaceaaaaaaaaaaaaaaoiaaaaaaaa
aaaaaaaaaaaaaamaaaaaaabmaaaaaaldpppoadaaaaaaaaadaaaaaabmaaaaaaaa
aaaaaakmaaaaaafiaaacaaafaaabaaaaaaaaaagmaaaaaaaaaaaaaahmaaacaaae
aaabaaaaaaaaaagmaaaaaaaaaaaaaaiiaaacaaaaaaaeaaaaaaaaaajmaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhafpfdfeaaaaabaaadaaabaaaeaaabaaaa
aaaaaaaafpengbgjgofegfhifpfdfeaaghgmhdhegbhegffpgngbhehcgjhifpgn
hghaaaklaaadaaadaaaeaaaeaaabaaaaaaaaaaaahghdfpddfpdaaadccodacodc
dadddfddcodaaaklaaaaaaaaaaaaaajaaaabaaacaaaaaaaaaaaaaaaaaaaabacb
aaaaaaabaaaaaaacaaaaaaacaaaaacjaaabaaaadaadafaaeaaaapafaaaaaaaaj
aaaabaakdaafcaadaaaabcaamcaaaaaaaaaaeaafaaaabcaameaaaaaaaaaacaaj
aaaaccaaaaaaaaaaafpicaaaaaaaagiiaaaaaaaaafpiaaaaaaaaapmiaaaaaaaa
miapaaabaabliiaakbacadaamiapaaabaamgiiaaklacacabmiapaaabaalbdeje
klacababmiapiadoaagmaadeklacaaabmiadiaaaaalalabkilaaaeaemiamiaaa
aakmkmagilaaafafaaaaaaaaaaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Matrix 256 [glstate_matrix_mvp]
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 467 [_MainTex_ST]
Vector 466 [_DissolveSrcBump_ST]
"sce_vp_rsx // 6 instructions using 1 registers
[Configuration]
8
0000000601010100
[Microcode]
96
401f9c6c011d2800810040d560607f9c401f9c6c011d3808010400d740619f9c
401f9c6c01d0300d8106c0c360403f80401f9c6c01d0200d8106c0c360405f80
401f9c6c01d0100d8106c0c360409f80401f9c6c01d0000d8106c0c360411f81
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 128 // 64 used size, 13 vars
Vector 16 [_MainTex_ST] 4
Vector 48 [_DissolveSrcBump_ST] 4
ConstBuffer "UnityPerDraw" 336 // 64 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
BindCB "$Globals" 0
BindCB "UnityPerDraw" 1
// 7 instructions, 1 temp regs, 0 temp arrays:
// ALU 6 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedaihlieobdjnekdpejfnjjdffcgmhhmmiabaaaaaafiacaaaaadaaaaaa
cmaaaaaakaaaaaaapiaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apadaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefcfiabaaaa
eaaaabaafgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaal
dccabaaaabaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaabaaaaaaogikcaaa
aaaaaaaaabaaaaaadcaaaaalmccabaaaabaaaaaaagbebaaaacaaaaaaagiecaaa
aaaaaaaaadaaaaaakgiocaaaaaaaaaaaadaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 glstate_matrix_mvp;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesVertex;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD;
highp float xlat_mutabledistort;
uniform highp float _StartAmount;
uniform highp float _Illuminate;
uniform highp float _Amount;
uniform highp vec4 _DissColor;
uniform highp vec4 _Color;
uniform highp float distort;
uniform highp float _TransRange;
uniform highp float _DistSpeed;
uniform highp float _Distortion;
uniform sampler2D _DissolveSrcBump;
uniform sampler2D _DissolveSrc;
uniform sampler2D _MainTex;
uniform highp vec4 _Time;
void main ()
{
  xlat_mutabledistort = distort;
  highp vec4 DematBump_1;
  highp float ClipTex_2;
  highp vec4 col_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD.xy);
  col_3 = (tmpvar_4 * _Color);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_DissolveSrc, xlv_TEXCOORD.xy).x;
  ClipTex_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (ClipTex_2 - _Amount);
  lowp vec4 tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD.zw + (_Time.x * _DistSpeed));
  tmpvar_7 = texture2D (_DissolveSrcBump, P_8);
  DematBump_1 = tmpvar_7;
  if ((tmpvar_6 <= 0.0)) {
    xlat_mutabledistort = _Distortion;
  } else {
    if ((tmpvar_6 <= _StartAmount)) {
      xlat_mutabledistort = mix (_Distortion, 0.0, (tmpvar_6 / _StartAmount));
    };
  };
  lowp vec4 tmpvar_9;
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_9 = texture2D (_MainTex, P_10);
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_9 * _Color);
  lowp vec4 tmpvar_12;
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD.xy - ((DematBump_1.xy * xlat_mutabledistort) * 0.1));
  tmpvar_12 = texture2D (_MainTex, P_13);
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * _Color);
  if (((_Amount > 0.0) && (_Amount < 1.0))) {
    if ((tmpvar_6 <= 0.0)) {
      col_3.w = (tmpvar_11.w * mix (1.0, 0.0, (abs(tmpvar_6) / _TransRange)));
      col_3.xyz = (tmpvar_11.xyz + _DissColor.xyz);
    } else {
      if ((tmpvar_6 <= _StartAmount)) {
        col_3.w = tmpvar_14.w;
        col_3.xyz = mix ((tmpvar_14.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (tmpvar_6 / _StartAmount)))), tmpvar_14.xyz, vec3((tmpvar_6 / _StartAmount)));
      };
    };
  } else {
    if ((_Amount >= 1.0)) {
      discard;
    };
  };
  gl_FragData[0] = col_3;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
#line 336
Output vert( in appdata_base v ) {
    Output o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 340
    o.uv_MainTex.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
    o.uv_MainTex.zw = ((v.texcoord.xy * _DissolveSrcBump_ST.xy) + _DissolveSrcBump_ST.zw);
    return o;
}
out highp vec4 xlv_TEXCOORD;
void main() {
    Output xl_retval;
    appdata_base xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xl_retval = vert( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD = vec4(xl_retval.uv_MainTex);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
void xll_clip_f(float x) {
  if ( x<0.0 ) discard;
}
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 330
struct Output {
    highp vec4 pos;
    highp vec4 uv_MainTex;
};
#line 52
struct appdata_base {
    highp vec4 vertex;
    highp vec3 normal;
    highp vec4 texcoord;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 315
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _MainTex_TexelSize;
uniform sampler2D _DissolveSrc;
#line 319
uniform sampler2D _DissolveSrcBump;
uniform highp vec4 _DissolveSrcBump_ST;
uniform highp float _Distortion;
uniform highp float _DistSpeed;
#line 323
uniform highp float _TransRange;
uniform highp float distort;
uniform highp vec4 _Color;
uniform highp vec4 _DissColor;
#line 327
uniform highp float _Amount;
uniform highp float _Illuminate;
uniform highp float _StartAmount;
#line 336
#line 344
highp float xlat_mutabledistort;
#line 344
highp vec4 frag( in Output IN ) {
    highp vec2 uv = IN.uv_MainTex.xy;
    highp vec4 col = (texture( _MainTex, uv) * _Color);
    #line 348
    highp float ClipTex = texture( _DissolveSrc, IN.uv_MainTex.xy).x;
    highp float ClipAmount = (ClipTex - _Amount);
    highp float Clip = 0.0;
    highp vec4 DematBump = texture( _DissolveSrcBump, (IN.uv_MainTex.zw + (_Time.x * _DistSpeed)));
    #line 352
    if ((ClipAmount <= 0.0)){
        xlat_mutabledistort = _Distortion;
    }
    else{
        if ((ClipAmount <= _StartAmount)){
            #line 358
            xlat_mutabledistort = mix( _Distortion, 0.0, (ClipAmount / _StartAmount));
        }
    }
    highp vec4 col1 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    highp vec4 col2 = (texture( _MainTex, (uv - ((DematBump.xy * xlat_mutabledistort) * 0.1))) * _Color);
    #line 362
    if (((_Amount > 0.0) && (_Amount < 1.0))){
        if ((ClipAmount <= 0.0)){
            #line 366
            col = col1;
            col.w = (col.w * mix( 1.0, 0.0, (abs(ClipAmount) / _TransRange)));
            col.xyz = (col.xyz + _DissColor.xyz);
        }
        else{
            #line 372
            if ((ClipAmount <= _StartAmount)){
                col = col2;
                col.xyz = mix( (col.xyz + ((_DissColor.xyz * _Illuminate) * (1.0 - (ClipAmount / _StartAmount)))), col.xyz, vec3( (ClipAmount / _StartAmount)));
            }
        }
    }
    else{
        if ((_Amount >= 1.0)){
            #line 381
            xll_clip_f(-1.0);
        }
    }
    return col;
}
in highp vec4 xlv_TEXCOORD;
void main() {
    xlat_mutabledistort = distort;
    highp vec4 xl_retval;
    Output xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.uv_MainTex = vec4(xlv_TEXCOORD);
    xl_retval = frag( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

}
Program "fp" {
// Fragment combos: 6
//   opengl - ALU: 51 to 51, TEX: 4 to 4
//   d3d9 - ALU: 49 to 49, TEX: 5 to 5
//   d3d11 - ALU: 23 to 23, TEX: 4 to 4, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 51 ALU, 4 TEX
PARAM c[11] = { program.local[0..9],
		{ 1, 0, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R0.x, fragment.texcoord[0], texture[1], 2D;
ADD R3.x, R0, -c[7];
SGE R3.z, c[10].y, R3.x;
ABS R0.z, R3;
RCP R3.y, c[9].x;
MUL R4.x, R3, R3.y;
MOV R2.w, c[10].x;
MOV R0.x, c[2];
MAD R0.xy, R0.x, c[0].x, fragment.texcoord[0].zwzw;
MOV R0.w, c[1].x;
MOV R3.w, c[7].x;
SGE R4.y, c[9].x, R3.x;
CMP R4.z, -R0, c[10].y, c[10].x;
MUL R0.z, R4, R4.y;
CMP R0.w, -R3.x, c[4].x, R0;
MAD R3.y, -R3.x, R3, c[10].x;
MAD R1.x, R4, -c[1], c[1];
CMP R0.z, -R0, R1.x, R0.w;
TEX R0.xy, R0, texture[2], 2D;
MUL R0.xy, R0, R0.z;
MAD R0.xy, -R0, c[10].z, fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
MUL R1, R0, c[5];
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R2.xyz, R1, c[6];
MUL R0, R0, c[5];
SLT R4.w, c[7].x, R2;
SLT R3.w, c[10].y, R3;
MUL R3.w, R3, R4;
MUL R4.w, R3, R3.z;
CMP R0.xyz, -R4.w, R2, R0;
MOV R2.x, c[8];
MUL R2.xyz, R2.x, c[6];
MUL R2.xyz, R2, R3.y;
MUL R4.z, R3.w, R4;
RCP R3.y, c[3].x;
ABS R3.x, R3;
MAD R5.x, -R3, R3.y, c[10];
MUL R5.x, R1.w, R5;
MUL R4.y, R4.z, R4;
CMP R0.w, -R4, R5.x, R0;
CMP R0, -R4.y, R1, R0;
ADD R3.xyz, R2, R1;
MAD R1.xyz, -R2, R4.x, R3;
CMP result.color.xyz, -R4.y, R1, R0;
ABS R0.y, R3.w;
SGE R0.x, c[7], R2.w;
CMP R0.y, -R0, c[10], c[10].x;
MUL R0.x, R0.y, R0;
MOV result.color.w, R0;
KIL -R0.x;
END
# 51 instructions, 6 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"ps_3_0
; 49 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 1.00000000, 0.00000000, -1.00000000, 0.10000000
dcl_texcoord0 v0
texld r1.x, v0, s1
add r3.x, r1, -c7
cmp r3.z, -r3.x, c10.x, c10.y
add r0.w, -r3.x, c9.x
abs_pp r0.z, r3
cmp r4.y, r0.w, c10.x, c10
cmp_pp r4.z, -r0, c10.x, c10.y
rcp r3.y, c9.x
mul r2.w, r3.x, r3.y
mov r2.x, c7
add r3.w, c10.z, r2.x
mov r0.x, c0
mad r0.xy, c2.x, r0.x, v0.zwzw
mov r0.w, c4.x
mov_pp r2.x, c7
mul_pp r0.z, r4, r4.y
cmp r0.w, -r3.x, c1.x, r0
mad r3.y, -r3.x, r3, c10.x
mad r1.x, r2.w, -c1, c1
cmp r0.z, -r0, r0.w, r1.x
texld r0.xy, r0, s2
mul r1.xy, r0, r0.z
texld r0, v0, s0
mad r1.xy, -r1, c10.w, v0
texld r1, r1, s0
mul r1, r1, c5
mul r0, r0, c5
cmp r2.y, r3.w, c10, c10.x
cmp r2.x, -r2, c10.y, c10
mul_pp r4.x, r2, r2.y
mul_pp r4.z, r4.x, r4
add r2.xyz, r1, c6
mul_pp r4.w, r4.x, r3.z
cmp r0.xyz, -r4.w, r0, r2
mov r2.xyz, c6
mul r2.xyz, c8.x, r2
mul r2.xyz, r2, r3.y
rcp r3.y, c3.x
abs r3.x, r3
mad r5.x, -r3, r3.y, c10
mul r5.x, r5, r1.w
add r3.xyz, r1, r2
mul_pp r4.y, r4.z, r4
cmp r0.w, -r4, r0, r5.x
cmp r0, -r4.y, r0, r1
mad r1.xyz, r2.w, -r2, r3
abs_pp r2.x, r4
cmp r1.w, r3, c10.x, c10.y
cmp_pp r2.x, -r2, c10, c10.y
mul_pp r1.w, r2.x, r1
cmp oC0.xyz, -r4.y, r0, r1
mov_pp r1, -r1.w
mov oC0.w, r0
texkill r1.xyzw
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Float 7 [_Amount]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 2 [_DistSpeed]
Float 1 [_Distortion]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
Vector 0 [_Time]
Float 3 [_TransRange]
Float 4 [distort]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
// Shader Timing Estimate, in Cycles/64 pixel vector:
// ALU: 33.33-37.33 (25-28 instructions), vertex: 0, texture: 16,
//   sequencer: 20, interpolator: 8;    4 GPRs, 48 threads,
// Performance (if enough threads): ~33-37 cycles per vector
// * Performance may change significantly, depending on predicated jump behavior
// * Texture cycle estimates are assuming an 8bit/component texture with no
//     aniso or trilinear filtering.

"ps_360
backbbaaaaaaacgeaaaaaceeaaaaaaaaaaaaaaceaaaaacbiaaaaaceaaaaaaaaa
aaaaaaaaaaaaabpaaaaaaabmaaaaabodppppadaaaaaaaaanaaaaaabmaaaaaaaa
aaaaabnmaaaaabcaaaacaaahaaabaaaaaaaaabciaaaaaaaaaaaaabdiaaacaaaf
aaabaaaaaaaaabeaaaaaaaaaaaaaabfaaaacaaagaaabaaaaaaaaabeaaaaaaaaa
aaaaabflaaadaaabaaabaaaaaaaaabgiaaaaaaaaaaaaabhiaaadaaacaaabaaaa
aaaaabgiaaaaaaaaaaaaabijaaacaaacaaabaaaaaaaaabciaaaaaaaaaaaaabje
aaacaaabaaabaaaaaaaaabciaaaaaaaaaaaaabkaaaacaaaiaaabaaaaaaaaabci
aaaaaaaaaaaaabkmaaadaaaaaaabaaaaaaaaabgiaaaaaaaaaaaaablfaaacaaaj
aaabaaaaaaaaabciaaaaaaaaaaaaabmcaaacaaaaaaabaaaaaaaaabeaaaaaaaaa
aaaaabmiaaacaaadaaabaaaaaaaaabciaaaaaaaaaaaaabneaaacaaaeaaabaaaa
aaaaabciaaaaaaaafpebgngphfgoheaaaaaaaaadaaabaaabaaabaaaaaaaaaaaa
fpedgpgmgphcaaklaaabaaadaaabaaaeaaabaaaaaaaaaaaafpeegjhdhdedgpgm
gphcaafpeegjhdhdgpgmhggffdhcgdaaaaaeaaamaaabaaabaaabaaaaaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhaaafpeegjhdhefdhagfgfgeaafpeegjhd
hegphchegjgpgoaafpejgmgmhfgngjgogbhegfaafpengbgjgofegfhiaafpfdhe
gbhcheebgngphfgoheaafpfegjgngfaafpfehcgbgohdfcgbgoghgfaagegjhdhe
gphcheaahahdfpddfpdaaadccodacodcdadddfddcodaaaklaaaaaaaaaaaaaaab
aaaaaaaaaaaaaaaaaaaaaabeabpmaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaeaaaaaacaebaaaadaaaaaaaaaeaaaaaaaaaaaabacbaaabaaabaaaaaacb
aaaapafaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadpiaaaaaaaaaaaaalpiaaaaa
dnmmmmmnaajegaahgaanbcaabcaaafaaaaaaaaaaeabdmeaabaaaaaaaaaaaeaaj
gabhlaaabaaaaaaaaaaagabndacdbaaafgaaaaaaaaaacaamcacglaaabaaaaaaa
aaaaeaambacilaaabcaaaaaaaaaabacjaaaaccaaaaaaaaaamiadaaabaagmgmbk
claaacaabacabacbbpbpppmiaaaaeaaababaaaabbpbppodpaaaaeaaaemeiaaaa
acmggmgmiaaaahajkmeeabaaaablmgaambaaaappkmibabacaagmblabegajaapp
miaeaaaaacmggmgmilaaababmiaeaaaaaagmgmmgkmacaeaamiaeaaaaaeblgmmg
knaaabaamiadaaabacbkmglaolabaaaabaaicacbbpbppgiiaaaaeaaabaaibaab
bpbppgiiaaaaeaaacacbaaaaaagmgmgmafppahahmiabaaaaaalbgmaaobaaaaaa
miapaaabaadedeaakbabafaahaapaaacaaaaaagmkbacafaamiabaaaaaclbblaa
hhppaaaaemcaaaaabiaaaagmmcaaaaadmiacaaaabibllbaaobiaaaaamiahaaab
bileleaakaacagaamiacaaaabilbmggmilaappppmiaiaaabbibllbaaobacaaaa
hmbaaaaaaaaaaagmocaaaaaaembbaaadbigmblgmegajaaajmiaiaaabbigmblbl
omadabacmiacaaaabiblgmaaobaaaaaaliboaaadbihggmebabagaippmianaaaa
biihgmpaoladaaacmiahaaacbmbemaaaoaaaacaamiahaaaabilbgcgholaaacaa
miahaaabbigmmaloomadabaamiabaaaaaagmgmaacgahppaahaaaaaaaaaaaaagm
ocaaaaaamiaaaaaaaalbmgaadjppppaamiapiaaaaadedeaaocababaaaaaaaaaa
aaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"sce_fp_rsx // 66 instructions using 4 registers
[Configuration]
24
ffffffff000040200001fffe00000000000084c004000000
[Offsets]
10
_Time 1 0
00000130
_Distortion 2 0
00000260000001c0
_DistSpeed 1 0
00000100
_TransRange 1 0
000001f0
distort 1 0
00000220
_Color 2 0
00000310000002a0
_DissColor 2 0
0000036000000330
_Amount 3 0
000000e0000000c000000010
_Illuminate 1 0
00000390
_StartAmount 2 0
000001a000000180
[Microcode]
1056
1800010080021c9cc8000001c800000100000000000000000000000000000000
82061702c8011c9dc8000001c8003fe106800d005c001c9d00020000c8000001
0000000000000000000000000000000018800a00c8001c9d20020000c8000001
0000000000003f8000000000000000009e040100c8011c9dc8000001c8003fe1
18840b00c8001c9d80020000c800000100003f80000000000000000000000000
06840280c9001c9d5d000001c800000110020300000c1c9c00020002c8000001
0000000000000000000000000000000002820c00c80c1c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000037e4180c9041c9dc8000001c8000001
06000400fe001c9d000200005c08000100000000000000000000000000000000
028c0f80c9041c9d00020000c800000100000000000000000000000000000000
117e428001081c9c01040000c800000104820c00fe041c9d00020000c8000001
0000000000000000000000000000000010063a00c8041c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000057e428001181c9cc9040001c8000001
08063a00fe043c9d00020000c800000100000000000000000000000000000000
02820280c9081c9dc9180001c80000011000010000020008c8000001c8000001
00000000000000000000000000000000037e4280c9041c9dab040000c8000001
06001704c8001c9dc8000001c800000110000400c80c0ab50002000200020000
000000000000000000000000000000001804020080001c9cfe000001c8000001
9e001700c8011c9dc8000001c8003fe11e000200c8001c9dc8020001c8000001
0000000000000000000000000000000018020400c8081c9f0002000080080000
cccd3dcc00000000000000000000000006840f80c9081c9daa020000c8000001
000000000000000000000000000000001e0417005c041c9dc8000001c8000001
1e040200c8081c9dc8020001c800000100000000000000000000000000000000
0e000300c8081ff5c8020001c800000100000000000000000000000000000000
10000400c8081ff5540c0003c80800010e060100c8021c9dc8000001c8000001
000000000000000000000000000000001e000100c8080015c8000001c8000001
0e060200c80c1c9d00020000c800000100000000000000000000000000000000
10000100c8001c9dc8000001c80000010e000100c8001c9dc8000001c8000001
0e060400fe0c1c9fc80c0001c80c00010e060300c8081c9dc80c0001c8000001
0e040300c8081c9dc80c0003c80000010e000400c8080015fe0c0001c80c0001
077e4280c9081c9d5d080001c8000001067f5200c8000095c8000001c8000001
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
ConstBuffer "$Globals" 128 // 124 used size, 13 vars
Float 64 [_Distortion]
Float 68 [_DistSpeed]
Float 72 [_TransRange]
Float 76 [distort]
Vector 80 [_Color] 4
Vector 96 [_DissColor] 4
Float 112 [_Amount]
Float 116 [_Illuminate]
Float 120 [_StartAmount]
ConstBuffer "UnityPerCamera" 128 // 16 used size, 8 vars
Vector 0 [_Time] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_DissolveSrc] 2D 1
SetTexture 2 [_DissolveSrcBump] 2D 2
// 34 instructions, 5 temp regs, 0 temp arrays:
// ALU 22 float, 0 int, 1 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedhpbkokeclbiblibfahndjeneogjjodmdabaaaaaaoeafaaaaadaaaaaa
cmaaaaaaieaaaaaaliaaaaaaejfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcceafaaaa
eaaaaaaaejabaaaafjaaaaaeegiocaaaaaaaaaaaaiaaaaaafjaaaaaeegiocaaa
abaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaad
pcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacafaaaaaabnaaaaai
bcaabaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadpanaaaead
akaabaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaaegiccaaaaaaaaaaaagaaaaaa
fgifcaaaaaaaaaaaahaaaaaadcaaaaaldcaabaaaabaaaaaaagiacaaaabaaaaaa
aaaaaaaafgifcaaaaaaaaaaaaeaaaaaaogbkbaaaabaaaaaaefaaaaajpcaabaaa
abaaaaaaegaabaaaabaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaaefaaaaaj
pcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaa
aaaaaaajicaabaaaaaaaaaaaakaabaaaacaaaaaaakiacaiaebaaaaaaaaaaaaaa
ahaaaaaaaoaaaaaiecaabaaaabaaaaaadkaabaaaaaaaaaaackiacaaaaaaaaaaa
ahaaaaaadcaaaaamicaabaaaabaaaaaackaabaaaabaaaaaaakiacaiaebaaaaaa
aaaaaaaaaeaaaaaaakiacaaaaaaaaaaaaeaaaaaabnaaaaaibcaabaaaacaaaaaa
ckiacaaaaaaaaaaaahaaaaaadkaabaaaaaaaaaaadhaaaaakicaabaaaabaaaaaa
akaabaaaacaaaaaadkaabaaaabaaaaaadkiacaaaaaaaaaaaaeaaaaaabnaaaaah
ccaabaaaacaaaaaaabeaaaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaajicaabaaa
aaaaaaaadkaabaiaibaaaaaaaaaaaaaackiacaaaaaaaaaaaaeaaaaaaaaaaaaai
icaabaaaaaaaaaaadkaabaiaebaaaaaaaaaaaaaaabeaaaaaaaaaiadpdhaaaaak
icaabaaaabaaaaaabkaabaaaacaaaaaaakiacaaaaaaaaaaaaeaaaaaadkaabaaa
abaaaaaadiaaaaahdcaabaaaabaaaaaapgapbaaaabaaaaaaegaabaaaabaaaaaa
dcaaaaandcaabaaaabaaaaaaegaabaiaebaaaaaaabaaaaaaaceaaaaamnmmmmdn
mnmmmmdnaaaaaaaaaaaaaaaaegbabaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aeaaaaaaegaobaaaadaaaaaaegiocaaaaaaaaaaaafaaaaaaaaaaaaaibcaabaaa
abaaaaaackaabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdcaaaaajhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaaegacbaaaaeaaaaaadcaaaaal
lcaabaaaabaaaaaaegaibaaaadaaaaaaegiicaaaaaaaaaaaafaaaaaaegaibaia
ebaaaaaaaaaaaaaadcaaaaajhcaabaaaaeaaaaaakgakbaaaabaaaaaaegadbaaa
abaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaabaaaaaaegacbaaaadaaaaaa
egiccaaaaaaaaaaaafaaaaaaegiccaaaaaaaaaaaagaaaaaadiaaaaahicaabaaa
abaaaaaadkaabaaaaaaaaaaadkaabaaaaeaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aaaaaaaaegaobaaaaaaaaaaaegiocaaaaaaaaaaaafaaaaaadhaaaaajpcaabaaa
adaaaaaaagaabaaaacaaaaaaegaobaaaaeaaaaaaegaobaaaaaaaaaaadhaaaaaj
pcaabaaaabaaaaaafgafbaaaacaaaaaaegaobaaaabaaaaaaegaobaaaadaaaaaa
dbaaaaaibcaabaaaacaaaaaaabeaaaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaa
dbaaaaaiccaabaaaacaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadp
abaaaaahbcaabaaaacaaaaaabkaabaaaacaaaaaaakaabaaaacaaaaaadhaaaaaj
pccabaaaaaaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegaobaaaaaaaaaaa
doaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 51 ALU, 4 TEX
PARAM c[11] = { program.local[0..9],
		{ 1, 0, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R0.x, fragment.texcoord[0], texture[1], 2D;
ADD R3.x, R0, -c[7];
SGE R3.z, c[10].y, R3.x;
ABS R0.z, R3;
RCP R3.y, c[9].x;
MUL R4.x, R3, R3.y;
MOV R2.w, c[10].x;
MOV R0.x, c[2];
MAD R0.xy, R0.x, c[0].x, fragment.texcoord[0].zwzw;
MOV R0.w, c[1].x;
MOV R3.w, c[7].x;
SGE R4.y, c[9].x, R3.x;
CMP R4.z, -R0, c[10].y, c[10].x;
MUL R0.z, R4, R4.y;
CMP R0.w, -R3.x, c[4].x, R0;
MAD R3.y, -R3.x, R3, c[10].x;
MAD R1.x, R4, -c[1], c[1];
CMP R0.z, -R0, R1.x, R0.w;
TEX R0.xy, R0, texture[2], 2D;
MUL R0.xy, R0, R0.z;
MAD R0.xy, -R0, c[10].z, fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
MUL R1, R0, c[5];
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R2.xyz, R1, c[6];
MUL R0, R0, c[5];
SLT R4.w, c[7].x, R2;
SLT R3.w, c[10].y, R3;
MUL R3.w, R3, R4;
MUL R4.w, R3, R3.z;
CMP R0.xyz, -R4.w, R2, R0;
MOV R2.x, c[8];
MUL R2.xyz, R2.x, c[6];
MUL R2.xyz, R2, R3.y;
MUL R4.z, R3.w, R4;
RCP R3.y, c[3].x;
ABS R3.x, R3;
MAD R5.x, -R3, R3.y, c[10];
MUL R5.x, R1.w, R5;
MUL R4.y, R4.z, R4;
CMP R0.w, -R4, R5.x, R0;
CMP R0, -R4.y, R1, R0;
ADD R3.xyz, R2, R1;
MAD R1.xyz, -R2, R4.x, R3;
CMP result.color.xyz, -R4.y, R1, R0;
ABS R0.y, R3.w;
SGE R0.x, c[7], R2.w;
CMP R0.y, -R0, c[10], c[10].x;
MUL R0.x, R0.y, R0;
MOV result.color.w, R0;
KIL -R0.x;
END
# 51 instructions, 6 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"ps_3_0
; 49 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 1.00000000, 0.00000000, -1.00000000, 0.10000000
dcl_texcoord0 v0
texld r1.x, v0, s1
add r3.x, r1, -c7
cmp r3.z, -r3.x, c10.x, c10.y
add r0.w, -r3.x, c9.x
abs_pp r0.z, r3
cmp r4.y, r0.w, c10.x, c10
cmp_pp r4.z, -r0, c10.x, c10.y
rcp r3.y, c9.x
mul r2.w, r3.x, r3.y
mov r2.x, c7
add r3.w, c10.z, r2.x
mov r0.x, c0
mad r0.xy, c2.x, r0.x, v0.zwzw
mov r0.w, c4.x
mov_pp r2.x, c7
mul_pp r0.z, r4, r4.y
cmp r0.w, -r3.x, c1.x, r0
mad r3.y, -r3.x, r3, c10.x
mad r1.x, r2.w, -c1, c1
cmp r0.z, -r0, r0.w, r1.x
texld r0.xy, r0, s2
mul r1.xy, r0, r0.z
texld r0, v0, s0
mad r1.xy, -r1, c10.w, v0
texld r1, r1, s0
mul r1, r1, c5
mul r0, r0, c5
cmp r2.y, r3.w, c10, c10.x
cmp r2.x, -r2, c10.y, c10
mul_pp r4.x, r2, r2.y
mul_pp r4.z, r4.x, r4
add r2.xyz, r1, c6
mul_pp r4.w, r4.x, r3.z
cmp r0.xyz, -r4.w, r0, r2
mov r2.xyz, c6
mul r2.xyz, c8.x, r2
mul r2.xyz, r2, r3.y
rcp r3.y, c3.x
abs r3.x, r3
mad r5.x, -r3, r3.y, c10
mul r5.x, r5, r1.w
add r3.xyz, r1, r2
mul_pp r4.y, r4.z, r4
cmp r0.w, -r4, r0, r5.x
cmp r0, -r4.y, r0, r1
mad r1.xyz, r2.w, -r2, r3
abs_pp r2.x, r4
cmp r1.w, r3, c10.x, c10.y
cmp_pp r2.x, -r2, c10, c10.y
mul_pp r1.w, r2.x, r1
cmp oC0.xyz, -r4.y, r0, r1
mov_pp r1, -r1.w
mov oC0.w, r0
texkill r1.xyzw
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Float 7 [_Amount]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 2 [_DistSpeed]
Float 1 [_Distortion]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
Vector 0 [_Time]
Float 3 [_TransRange]
Float 4 [distort]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
// Shader Timing Estimate, in Cycles/64 pixel vector:
// ALU: 33.33-37.33 (25-28 instructions), vertex: 0, texture: 16,
//   sequencer: 20, interpolator: 8;    4 GPRs, 48 threads,
// Performance (if enough threads): ~33-37 cycles per vector
// * Performance may change significantly, depending on predicated jump behavior
// * Texture cycle estimates are assuming an 8bit/component texture with no
//     aniso or trilinear filtering.

"ps_360
backbbaaaaaaacgeaaaaaceeaaaaaaaaaaaaaaceaaaaacbiaaaaaceaaaaaaaaa
aaaaaaaaaaaaabpaaaaaaabmaaaaabodppppadaaaaaaaaanaaaaaabmaaaaaaaa
aaaaabnmaaaaabcaaaacaaahaaabaaaaaaaaabciaaaaaaaaaaaaabdiaaacaaaf
aaabaaaaaaaaabeaaaaaaaaaaaaaabfaaaacaaagaaabaaaaaaaaabeaaaaaaaaa
aaaaabflaaadaaabaaabaaaaaaaaabgiaaaaaaaaaaaaabhiaaadaaacaaabaaaa
aaaaabgiaaaaaaaaaaaaabijaaacaaacaaabaaaaaaaaabciaaaaaaaaaaaaabje
aaacaaabaaabaaaaaaaaabciaaaaaaaaaaaaabkaaaacaaaiaaabaaaaaaaaabci
aaaaaaaaaaaaabkmaaadaaaaaaabaaaaaaaaabgiaaaaaaaaaaaaablfaaacaaaj
aaabaaaaaaaaabciaaaaaaaaaaaaabmcaaacaaaaaaabaaaaaaaaabeaaaaaaaaa
aaaaabmiaaacaaadaaabaaaaaaaaabciaaaaaaaaaaaaabneaaacaaaeaaabaaaa
aaaaabciaaaaaaaafpebgngphfgoheaaaaaaaaadaaabaaabaaabaaaaaaaaaaaa
fpedgpgmgphcaaklaaabaaadaaabaaaeaaabaaaaaaaaaaaafpeegjhdhdedgpgm
gphcaafpeegjhdhdgpgmhggffdhcgdaaaaaeaaamaaabaaabaaabaaaaaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhaaafpeegjhdhefdhagfgfgeaafpeegjhd
hegphchegjgpgoaafpejgmgmhfgngjgogbhegfaafpengbgjgofegfhiaafpfdhe
gbhcheebgngphfgoheaafpfegjgngfaafpfehcgbgohdfcgbgoghgfaagegjhdhe
gphcheaahahdfpddfpdaaadccodacodcdadddfddcodaaaklaaaaaaaaaaaaaaab
aaaaaaaaaaaaaaaaaaaaaabeabpmaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaeaaaaaacaebaaaadaaaaaaaaaeaaaaaaaaaaaabacbaaabaaabaaaaaacb
aaaapafaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadpiaaaaaaaaaaaaalpiaaaaa
dnmmmmmnaajegaahgaanbcaabcaaafaaaaaaaaaaeabdmeaabaaaaaaaaaaaeaaj
gabhlaaabaaaaaaaaaaagabndacdbaaafgaaaaaaaaaacaamcacglaaabaaaaaaa
aaaaeaambacilaaabcaaaaaaaaaabacjaaaaccaaaaaaaaaamiadaaabaagmgmbk
claaacaabacabacbbpbpppmiaaaaeaaababaaaabbpbppodpaaaaeaaaemeiaaaa
acmggmgmiaaaahajkmeeabaaaablmgaambaaaappkmibabacaagmblabegajaapp
miaeaaaaacmggmgmilaaababmiaeaaaaaagmgmmgkmacaeaamiaeaaaaaeblgmmg
knaaabaamiadaaabacbkmglaolabaaaabaaicacbbpbppgiiaaaaeaaabaaibaab
bpbppgiiaaaaeaaacacbaaaaaagmgmgmafppahahmiabaaaaaalbgmaaobaaaaaa
miapaaabaadedeaakbabafaahaapaaacaaaaaagmkbacafaamiabaaaaaclbblaa
hhppaaaaemcaaaaabiaaaagmmcaaaaadmiacaaaabibllbaaobiaaaaamiahaaab
bileleaakaacagaamiacaaaabilbmggmilaappppmiaiaaabbibllbaaobacaaaa
hmbaaaaaaaaaaagmocaaaaaaembbaaadbigmblgmegajaaajmiaiaaabbigmblbl
omadabacmiacaaaabiblgmaaobaaaaaaliboaaadbihggmebabagaippmianaaaa
biihgmpaoladaaacmiahaaacbmbemaaaoaaaacaamiahaaaabilbgcgholaaacaa
miahaaabbigmmaloomadabaamiabaaaaaagmgmaacgahppaahaaaaaaaaaaaaagm
ocaaaaaamiaaaaaaaalbmgaadjppppaamiapiaaaaadedeaaocababaaaaaaaaaa
aaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"sce_fp_rsx // 66 instructions using 4 registers
[Configuration]
24
ffffffff000040200001fffe00000000000084c004000000
[Offsets]
10
_Time 1 0
00000130
_Distortion 2 0
00000260000001c0
_DistSpeed 1 0
00000100
_TransRange 1 0
000001f0
distort 1 0
00000220
_Color 2 0
00000310000002a0
_DissColor 2 0
0000036000000330
_Amount 3 0
000000e0000000c000000010
_Illuminate 1 0
00000390
_StartAmount 2 0
000001a000000180
[Microcode]
1056
1800010080021c9cc8000001c800000100000000000000000000000000000000
82061702c8011c9dc8000001c8003fe106800d005c001c9d00020000c8000001
0000000000000000000000000000000018800a00c8001c9d20020000c8000001
0000000000003f8000000000000000009e040100c8011c9dc8000001c8003fe1
18840b00c8001c9d80020000c800000100003f80000000000000000000000000
06840280c9001c9d5d000001c800000110020300000c1c9c00020002c8000001
0000000000000000000000000000000002820c00c80c1c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000037e4180c9041c9dc8000001c8000001
06000400fe001c9d000200005c08000100000000000000000000000000000000
028c0f80c9041c9d00020000c800000100000000000000000000000000000000
117e428001081c9c01040000c800000104820c00fe041c9d00020000c8000001
0000000000000000000000000000000010063a00c8041c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000057e428001181c9cc9040001c8000001
08063a00fe043c9d00020000c800000100000000000000000000000000000000
02820280c9081c9dc9180001c80000011000010000020008c8000001c8000001
00000000000000000000000000000000037e4280c9041c9dab040000c8000001
06001704c8001c9dc8000001c800000110000400c80c0ab50002000200020000
000000000000000000000000000000001804020080001c9cfe000001c8000001
9e001700c8011c9dc8000001c8003fe11e000200c8001c9dc8020001c8000001
0000000000000000000000000000000018020400c8081c9f0002000080080000
cccd3dcc00000000000000000000000006840f80c9081c9daa020000c8000001
000000000000000000000000000000001e0417005c041c9dc8000001c8000001
1e040200c8081c9dc8020001c800000100000000000000000000000000000000
0e000300c8081ff5c8020001c800000100000000000000000000000000000000
10000400c8081ff5540c0003c80800010e060100c8021c9dc8000001c8000001
000000000000000000000000000000001e000100c8080015c8000001c8000001
0e060200c80c1c9d00020000c800000100000000000000000000000000000000
10000100c8001c9dc8000001c80000010e000100c8001c9dc8000001c8000001
0e060400fe0c1c9fc80c0001c80c00010e060300c8081c9dc80c0001c8000001
0e040300c8081c9dc80c0003c80000010e000400c8080015fe0c0001c80c0001
077e4280c9081c9d5d080001c8000001067f5200c8000095c8000001c8000001
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
ConstBuffer "$Globals" 128 // 124 used size, 13 vars
Float 64 [_Distortion]
Float 68 [_DistSpeed]
Float 72 [_TransRange]
Float 76 [distort]
Vector 80 [_Color] 4
Vector 96 [_DissColor] 4
Float 112 [_Amount]
Float 116 [_Illuminate]
Float 120 [_StartAmount]
ConstBuffer "UnityPerCamera" 128 // 16 used size, 8 vars
Vector 0 [_Time] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_DissolveSrc] 2D 1
SetTexture 2 [_DissolveSrcBump] 2D 2
// 34 instructions, 5 temp regs, 0 temp arrays:
// ALU 22 float, 0 int, 1 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedhpbkokeclbiblibfahndjeneogjjodmdabaaaaaaoeafaaaaadaaaaaa
cmaaaaaaieaaaaaaliaaaaaaejfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcceafaaaa
eaaaaaaaejabaaaafjaaaaaeegiocaaaaaaaaaaaaiaaaaaafjaaaaaeegiocaaa
abaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaad
pcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacafaaaaaabnaaaaai
bcaabaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadpanaaaead
akaabaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaaegiccaaaaaaaaaaaagaaaaaa
fgifcaaaaaaaaaaaahaaaaaadcaaaaaldcaabaaaabaaaaaaagiacaaaabaaaaaa
aaaaaaaafgifcaaaaaaaaaaaaeaaaaaaogbkbaaaabaaaaaaefaaaaajpcaabaaa
abaaaaaaegaabaaaabaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaaefaaaaaj
pcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaa
aaaaaaajicaabaaaaaaaaaaaakaabaaaacaaaaaaakiacaiaebaaaaaaaaaaaaaa
ahaaaaaaaoaaaaaiecaabaaaabaaaaaadkaabaaaaaaaaaaackiacaaaaaaaaaaa
ahaaaaaadcaaaaamicaabaaaabaaaaaackaabaaaabaaaaaaakiacaiaebaaaaaa
aaaaaaaaaeaaaaaaakiacaaaaaaaaaaaaeaaaaaabnaaaaaibcaabaaaacaaaaaa
ckiacaaaaaaaaaaaahaaaaaadkaabaaaaaaaaaaadhaaaaakicaabaaaabaaaaaa
akaabaaaacaaaaaadkaabaaaabaaaaaadkiacaaaaaaaaaaaaeaaaaaabnaaaaah
ccaabaaaacaaaaaaabeaaaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaajicaabaaa
aaaaaaaadkaabaiaibaaaaaaaaaaaaaackiacaaaaaaaaaaaaeaaaaaaaaaaaaai
icaabaaaaaaaaaaadkaabaiaebaaaaaaaaaaaaaaabeaaaaaaaaaiadpdhaaaaak
icaabaaaabaaaaaabkaabaaaacaaaaaaakiacaaaaaaaaaaaaeaaaaaadkaabaaa
abaaaaaadiaaaaahdcaabaaaabaaaaaapgapbaaaabaaaaaaegaabaaaabaaaaaa
dcaaaaandcaabaaaabaaaaaaegaabaiaebaaaaaaabaaaaaaaceaaaaamnmmmmdn
mnmmmmdnaaaaaaaaaaaaaaaaegbabaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aeaaaaaaegaobaaaadaaaaaaegiocaaaaaaaaaaaafaaaaaaaaaaaaaibcaabaaa
abaaaaaackaabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdcaaaaajhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaaegacbaaaaeaaaaaadcaaaaal
lcaabaaaabaaaaaaegaibaaaadaaaaaaegiicaaaaaaaaaaaafaaaaaaegaibaia
ebaaaaaaaaaaaaaadcaaaaajhcaabaaaaeaaaaaakgakbaaaabaaaaaaegadbaaa
abaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaabaaaaaaegacbaaaadaaaaaa
egiccaaaaaaaaaaaafaaaaaaegiccaaaaaaaaaaaagaaaaaadiaaaaahicaabaaa
abaaaaaadkaabaaaaaaaaaaadkaabaaaaeaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aaaaaaaaegaobaaaaaaaaaaaegiocaaaaaaaaaaaafaaaaaadhaaaaajpcaabaaa
adaaaaaaagaabaaaacaaaaaaegaobaaaaeaaaaaaegaobaaaaaaaaaaadhaaaaaj
pcaabaaaabaaaaaafgafbaaaacaaaaaaegaobaaaabaaaaaaegaobaaaadaaaaaa
dbaaaaaibcaabaaaacaaaaaaabeaaaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaa
dbaaaaaiccaabaaaacaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadp
abaaaaahbcaabaaaacaaaaaabkaabaaaacaaaaaaakaabaaaacaaaaaadhaaaaaj
pccabaaaaaaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegaobaaaaaaaaaaa
doaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 51 ALU, 4 TEX
PARAM c[11] = { program.local[0..9],
		{ 1, 0, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R0.x, fragment.texcoord[0], texture[1], 2D;
ADD R3.x, R0, -c[7];
SGE R3.z, c[10].y, R3.x;
ABS R0.z, R3;
RCP R3.y, c[9].x;
MUL R4.x, R3, R3.y;
MOV R2.w, c[10].x;
MOV R0.x, c[2];
MAD R0.xy, R0.x, c[0].x, fragment.texcoord[0].zwzw;
MOV R0.w, c[1].x;
MOV R3.w, c[7].x;
SGE R4.y, c[9].x, R3.x;
CMP R4.z, -R0, c[10].y, c[10].x;
MUL R0.z, R4, R4.y;
CMP R0.w, -R3.x, c[4].x, R0;
MAD R3.y, -R3.x, R3, c[10].x;
MAD R1.x, R4, -c[1], c[1];
CMP R0.z, -R0, R1.x, R0.w;
TEX R0.xy, R0, texture[2], 2D;
MUL R0.xy, R0, R0.z;
MAD R0.xy, -R0, c[10].z, fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
MUL R1, R0, c[5];
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R2.xyz, R1, c[6];
MUL R0, R0, c[5];
SLT R4.w, c[7].x, R2;
SLT R3.w, c[10].y, R3;
MUL R3.w, R3, R4;
MUL R4.w, R3, R3.z;
CMP R0.xyz, -R4.w, R2, R0;
MOV R2.x, c[8];
MUL R2.xyz, R2.x, c[6];
MUL R2.xyz, R2, R3.y;
MUL R4.z, R3.w, R4;
RCP R3.y, c[3].x;
ABS R3.x, R3;
MAD R5.x, -R3, R3.y, c[10];
MUL R5.x, R1.w, R5;
MUL R4.y, R4.z, R4;
CMP R0.w, -R4, R5.x, R0;
CMP R0, -R4.y, R1, R0;
ADD R3.xyz, R2, R1;
MAD R1.xyz, -R2, R4.x, R3;
CMP result.color.xyz, -R4.y, R1, R0;
ABS R0.y, R3.w;
SGE R0.x, c[7], R2.w;
CMP R0.y, -R0, c[10], c[10].x;
MUL R0.x, R0.y, R0;
MOV result.color.w, R0;
KIL -R0.x;
END
# 51 instructions, 6 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"ps_3_0
; 49 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 1.00000000, 0.00000000, -1.00000000, 0.10000000
dcl_texcoord0 v0
texld r1.x, v0, s1
add r3.x, r1, -c7
cmp r3.z, -r3.x, c10.x, c10.y
add r0.w, -r3.x, c9.x
abs_pp r0.z, r3
cmp r4.y, r0.w, c10.x, c10
cmp_pp r4.z, -r0, c10.x, c10.y
rcp r3.y, c9.x
mul r2.w, r3.x, r3.y
mov r2.x, c7
add r3.w, c10.z, r2.x
mov r0.x, c0
mad r0.xy, c2.x, r0.x, v0.zwzw
mov r0.w, c4.x
mov_pp r2.x, c7
mul_pp r0.z, r4, r4.y
cmp r0.w, -r3.x, c1.x, r0
mad r3.y, -r3.x, r3, c10.x
mad r1.x, r2.w, -c1, c1
cmp r0.z, -r0, r0.w, r1.x
texld r0.xy, r0, s2
mul r1.xy, r0, r0.z
texld r0, v0, s0
mad r1.xy, -r1, c10.w, v0
texld r1, r1, s0
mul r1, r1, c5
mul r0, r0, c5
cmp r2.y, r3.w, c10, c10.x
cmp r2.x, -r2, c10.y, c10
mul_pp r4.x, r2, r2.y
mul_pp r4.z, r4.x, r4
add r2.xyz, r1, c6
mul_pp r4.w, r4.x, r3.z
cmp r0.xyz, -r4.w, r0, r2
mov r2.xyz, c6
mul r2.xyz, c8.x, r2
mul r2.xyz, r2, r3.y
rcp r3.y, c3.x
abs r3.x, r3
mad r5.x, -r3, r3.y, c10
mul r5.x, r5, r1.w
add r3.xyz, r1, r2
mul_pp r4.y, r4.z, r4
cmp r0.w, -r4, r0, r5.x
cmp r0, -r4.y, r0, r1
mad r1.xyz, r2.w, -r2, r3
abs_pp r2.x, r4
cmp r1.w, r3, c10.x, c10.y
cmp_pp r2.x, -r2, c10, c10.y
mul_pp r1.w, r2.x, r1
cmp oC0.xyz, -r4.y, r0, r1
mov_pp r1, -r1.w
mov oC0.w, r0
texkill r1.xyzw
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Float 7 [_Amount]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 2 [_DistSpeed]
Float 1 [_Distortion]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
Vector 0 [_Time]
Float 3 [_TransRange]
Float 4 [distort]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
// Shader Timing Estimate, in Cycles/64 pixel vector:
// ALU: 33.33-37.33 (25-28 instructions), vertex: 0, texture: 16,
//   sequencer: 20, interpolator: 8;    4 GPRs, 48 threads,
// Performance (if enough threads): ~33-37 cycles per vector
// * Performance may change significantly, depending on predicated jump behavior
// * Texture cycle estimates are assuming an 8bit/component texture with no
//     aniso or trilinear filtering.

"ps_360
backbbaaaaaaacgeaaaaaceeaaaaaaaaaaaaaaceaaaaacbiaaaaaceaaaaaaaaa
aaaaaaaaaaaaabpaaaaaaabmaaaaabodppppadaaaaaaaaanaaaaaabmaaaaaaaa
aaaaabnmaaaaabcaaaacaaahaaabaaaaaaaaabciaaaaaaaaaaaaabdiaaacaaaf
aaabaaaaaaaaabeaaaaaaaaaaaaaabfaaaacaaagaaabaaaaaaaaabeaaaaaaaaa
aaaaabflaaadaaabaaabaaaaaaaaabgiaaaaaaaaaaaaabhiaaadaaacaaabaaaa
aaaaabgiaaaaaaaaaaaaabijaaacaaacaaabaaaaaaaaabciaaaaaaaaaaaaabje
aaacaaabaaabaaaaaaaaabciaaaaaaaaaaaaabkaaaacaaaiaaabaaaaaaaaabci
aaaaaaaaaaaaabkmaaadaaaaaaabaaaaaaaaabgiaaaaaaaaaaaaablfaaacaaaj
aaabaaaaaaaaabciaaaaaaaaaaaaabmcaaacaaaaaaabaaaaaaaaabeaaaaaaaaa
aaaaabmiaaacaaadaaabaaaaaaaaabciaaaaaaaaaaaaabneaaacaaaeaaabaaaa
aaaaabciaaaaaaaafpebgngphfgoheaaaaaaaaadaaabaaabaaabaaaaaaaaaaaa
fpedgpgmgphcaaklaaabaaadaaabaaaeaaabaaaaaaaaaaaafpeegjhdhdedgpgm
gphcaafpeegjhdhdgpgmhggffdhcgdaaaaaeaaamaaabaaabaaabaaaaaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhaaafpeegjhdhefdhagfgfgeaafpeegjhd
hegphchegjgpgoaafpejgmgmhfgngjgogbhegfaafpengbgjgofegfhiaafpfdhe
gbhcheebgngphfgoheaafpfegjgngfaafpfehcgbgohdfcgbgoghgfaagegjhdhe
gphcheaahahdfpddfpdaaadccodacodcdadddfddcodaaaklaaaaaaaaaaaaaaab
aaaaaaaaaaaaaaaaaaaaaabeabpmaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaeaaaaaacaebaaaadaaaaaaaaaeaaaaaaaaaaaabacbaaabaaabaaaaaacb
aaaapafaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadpiaaaaaaaaaaaaalpiaaaaa
dnmmmmmnaajegaahgaanbcaabcaaafaaaaaaaaaaeabdmeaabaaaaaaaaaaaeaaj
gabhlaaabaaaaaaaaaaagabndacdbaaafgaaaaaaaaaacaamcacglaaabaaaaaaa
aaaaeaambacilaaabcaaaaaaaaaabacjaaaaccaaaaaaaaaamiadaaabaagmgmbk
claaacaabacabacbbpbpppmiaaaaeaaababaaaabbpbppodpaaaaeaaaemeiaaaa
acmggmgmiaaaahajkmeeabaaaablmgaambaaaappkmibabacaagmblabegajaapp
miaeaaaaacmggmgmilaaababmiaeaaaaaagmgmmgkmacaeaamiaeaaaaaeblgmmg
knaaabaamiadaaabacbkmglaolabaaaabaaicacbbpbppgiiaaaaeaaabaaibaab
bpbppgiiaaaaeaaacacbaaaaaagmgmgmafppahahmiabaaaaaalbgmaaobaaaaaa
miapaaabaadedeaakbabafaahaapaaacaaaaaagmkbacafaamiabaaaaaclbblaa
hhppaaaaemcaaaaabiaaaagmmcaaaaadmiacaaaabibllbaaobiaaaaamiahaaab
bileleaakaacagaamiacaaaabilbmggmilaappppmiaiaaabbibllbaaobacaaaa
hmbaaaaaaaaaaagmocaaaaaaembbaaadbigmblgmegajaaajmiaiaaabbigmblbl
omadabacmiacaaaabiblgmaaobaaaaaaliboaaadbihggmebabagaippmianaaaa
biihgmpaoladaaacmiahaaacbmbemaaaoaaaacaamiahaaaabilbgcgholaaacaa
miahaaabbigmmaloomadabaamiabaaaaaagmgmaacgahppaahaaaaaaaaaaaaagm
ocaaaaaamiaaaaaaaalbmgaadjppppaamiapiaaaaadedeaaocababaaaaaaaaaa
aaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"sce_fp_rsx // 66 instructions using 4 registers
[Configuration]
24
ffffffff000040200001fffe00000000000084c004000000
[Offsets]
10
_Time 1 0
00000130
_Distortion 2 0
00000260000001c0
_DistSpeed 1 0
00000100
_TransRange 1 0
000001f0
distort 1 0
00000220
_Color 2 0
00000310000002a0
_DissColor 2 0
0000036000000330
_Amount 3 0
000000e0000000c000000010
_Illuminate 1 0
00000390
_StartAmount 2 0
000001a000000180
[Microcode]
1056
1800010080021c9cc8000001c800000100000000000000000000000000000000
82061702c8011c9dc8000001c8003fe106800d005c001c9d00020000c8000001
0000000000000000000000000000000018800a00c8001c9d20020000c8000001
0000000000003f8000000000000000009e040100c8011c9dc8000001c8003fe1
18840b00c8001c9d80020000c800000100003f80000000000000000000000000
06840280c9001c9d5d000001c800000110020300000c1c9c00020002c8000001
0000000000000000000000000000000002820c00c80c1c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000037e4180c9041c9dc8000001c8000001
06000400fe001c9d000200005c08000100000000000000000000000000000000
028c0f80c9041c9d00020000c800000100000000000000000000000000000000
117e428001081c9c01040000c800000104820c00fe041c9d00020000c8000001
0000000000000000000000000000000010063a00c8041c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000057e428001181c9cc9040001c8000001
08063a00fe043c9d00020000c800000100000000000000000000000000000000
02820280c9081c9dc9180001c80000011000010000020008c8000001c8000001
00000000000000000000000000000000037e4280c9041c9dab040000c8000001
06001704c8001c9dc8000001c800000110000400c80c0ab50002000200020000
000000000000000000000000000000001804020080001c9cfe000001c8000001
9e001700c8011c9dc8000001c8003fe11e000200c8001c9dc8020001c8000001
0000000000000000000000000000000018020400c8081c9f0002000080080000
cccd3dcc00000000000000000000000006840f80c9081c9daa020000c8000001
000000000000000000000000000000001e0417005c041c9dc8000001c8000001
1e040200c8081c9dc8020001c800000100000000000000000000000000000000
0e000300c8081ff5c8020001c800000100000000000000000000000000000000
10000400c8081ff5540c0003c80800010e060100c8021c9dc8000001c8000001
000000000000000000000000000000001e000100c8080015c8000001c8000001
0e060200c80c1c9d00020000c800000100000000000000000000000000000000
10000100c8001c9dc8000001c80000010e000100c8001c9dc8000001c8000001
0e060400fe0c1c9fc80c0001c80c00010e060300c8081c9dc80c0001c8000001
0e040300c8081c9dc80c0003c80000010e000400c8080015fe0c0001c80c0001
077e4280c9081c9d5d080001c8000001067f5200c8000095c8000001c8000001
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
ConstBuffer "$Globals" 128 // 124 used size, 13 vars
Float 64 [_Distortion]
Float 68 [_DistSpeed]
Float 72 [_TransRange]
Float 76 [distort]
Vector 80 [_Color] 4
Vector 96 [_DissColor] 4
Float 112 [_Amount]
Float 116 [_Illuminate]
Float 120 [_StartAmount]
ConstBuffer "UnityPerCamera" 128 // 16 used size, 8 vars
Vector 0 [_Time] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_DissolveSrc] 2D 1
SetTexture 2 [_DissolveSrcBump] 2D 2
// 34 instructions, 5 temp regs, 0 temp arrays:
// ALU 22 float, 0 int, 1 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedhpbkokeclbiblibfahndjeneogjjodmdabaaaaaaoeafaaaaadaaaaaa
cmaaaaaaieaaaaaaliaaaaaaejfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcceafaaaa
eaaaaaaaejabaaaafjaaaaaeegiocaaaaaaaaaaaaiaaaaaafjaaaaaeegiocaaa
abaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaad
pcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacafaaaaaabnaaaaai
bcaabaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadpanaaaead
akaabaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaaegiccaaaaaaaaaaaagaaaaaa
fgifcaaaaaaaaaaaahaaaaaadcaaaaaldcaabaaaabaaaaaaagiacaaaabaaaaaa
aaaaaaaafgifcaaaaaaaaaaaaeaaaaaaogbkbaaaabaaaaaaefaaaaajpcaabaaa
abaaaaaaegaabaaaabaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaaefaaaaaj
pcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaa
aaaaaaajicaabaaaaaaaaaaaakaabaaaacaaaaaaakiacaiaebaaaaaaaaaaaaaa
ahaaaaaaaoaaaaaiecaabaaaabaaaaaadkaabaaaaaaaaaaackiacaaaaaaaaaaa
ahaaaaaadcaaaaamicaabaaaabaaaaaackaabaaaabaaaaaaakiacaiaebaaaaaa
aaaaaaaaaeaaaaaaakiacaaaaaaaaaaaaeaaaaaabnaaaaaibcaabaaaacaaaaaa
ckiacaaaaaaaaaaaahaaaaaadkaabaaaaaaaaaaadhaaaaakicaabaaaabaaaaaa
akaabaaaacaaaaaadkaabaaaabaaaaaadkiacaaaaaaaaaaaaeaaaaaabnaaaaah
ccaabaaaacaaaaaaabeaaaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaajicaabaaa
aaaaaaaadkaabaiaibaaaaaaaaaaaaaackiacaaaaaaaaaaaaeaaaaaaaaaaaaai
icaabaaaaaaaaaaadkaabaiaebaaaaaaaaaaaaaaabeaaaaaaaaaiadpdhaaaaak
icaabaaaabaaaaaabkaabaaaacaaaaaaakiacaaaaaaaaaaaaeaaaaaadkaabaaa
abaaaaaadiaaaaahdcaabaaaabaaaaaapgapbaaaabaaaaaaegaabaaaabaaaaaa
dcaaaaandcaabaaaabaaaaaaegaabaiaebaaaaaaabaaaaaaaceaaaaamnmmmmdn
mnmmmmdnaaaaaaaaaaaaaaaaegbabaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aeaaaaaaegaobaaaadaaaaaaegiocaaaaaaaaaaaafaaaaaaaaaaaaaibcaabaaa
abaaaaaackaabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdcaaaaajhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaaegacbaaaaeaaaaaadcaaaaal
lcaabaaaabaaaaaaegaibaaaadaaaaaaegiicaaaaaaaaaaaafaaaaaaegaibaia
ebaaaaaaaaaaaaaadcaaaaajhcaabaaaaeaaaaaakgakbaaaabaaaaaaegadbaaa
abaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaabaaaaaaegacbaaaadaaaaaa
egiccaaaaaaaaaaaafaaaaaaegiccaaaaaaaaaaaagaaaaaadiaaaaahicaabaaa
abaaaaaadkaabaaaaaaaaaaadkaabaaaaeaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aaaaaaaaegaobaaaaaaaaaaaegiocaaaaaaaaaaaafaaaaaadhaaaaajpcaabaaa
adaaaaaaagaabaaaacaaaaaaegaobaaaaeaaaaaaegaobaaaaaaaaaaadhaaaaaj
pcaabaaaabaaaaaafgafbaaaacaaaaaaegaobaaaabaaaaaaegaobaaaadaaaaaa
dbaaaaaibcaabaaaacaaaaaaabeaaaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaa
dbaaaaaiccaabaaaacaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadp
abaaaaahbcaabaaaacaaaaaabkaabaaaacaaaaaaakaabaaaacaaaaaadhaaaaaj
pccabaaaaaaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegaobaaaaaaaaaaa
doaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_OFF" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 51 ALU, 4 TEX
PARAM c[11] = { program.local[0..9],
		{ 1, 0, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R0.x, fragment.texcoord[0], texture[1], 2D;
ADD R3.x, R0, -c[7];
SGE R3.z, c[10].y, R3.x;
ABS R0.z, R3;
RCP R3.y, c[9].x;
MUL R4.x, R3, R3.y;
MOV R2.w, c[10].x;
MOV R0.x, c[2];
MAD R0.xy, R0.x, c[0].x, fragment.texcoord[0].zwzw;
MOV R0.w, c[1].x;
MOV R3.w, c[7].x;
SGE R4.y, c[9].x, R3.x;
CMP R4.z, -R0, c[10].y, c[10].x;
MUL R0.z, R4, R4.y;
CMP R0.w, -R3.x, c[4].x, R0;
MAD R3.y, -R3.x, R3, c[10].x;
MAD R1.x, R4, -c[1], c[1];
CMP R0.z, -R0, R1.x, R0.w;
TEX R0.xy, R0, texture[2], 2D;
MUL R0.xy, R0, R0.z;
MAD R0.xy, -R0, c[10].z, fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
MUL R1, R0, c[5];
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R2.xyz, R1, c[6];
MUL R0, R0, c[5];
SLT R4.w, c[7].x, R2;
SLT R3.w, c[10].y, R3;
MUL R3.w, R3, R4;
MUL R4.w, R3, R3.z;
CMP R0.xyz, -R4.w, R2, R0;
MOV R2.x, c[8];
MUL R2.xyz, R2.x, c[6];
MUL R2.xyz, R2, R3.y;
MUL R4.z, R3.w, R4;
RCP R3.y, c[3].x;
ABS R3.x, R3;
MAD R5.x, -R3, R3.y, c[10];
MUL R5.x, R1.w, R5;
MUL R4.y, R4.z, R4;
CMP R0.w, -R4, R5.x, R0;
CMP R0, -R4.y, R1, R0;
ADD R3.xyz, R2, R1;
MAD R1.xyz, -R2, R4.x, R3;
CMP result.color.xyz, -R4.y, R1, R0;
ABS R0.y, R3.w;
SGE R0.x, c[7], R2.w;
CMP R0.y, -R0, c[10], c[10].x;
MUL R0.x, R0.y, R0;
MOV result.color.w, R0;
KIL -R0.x;
END
# 51 instructions, 6 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"ps_3_0
; 49 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 1.00000000, 0.00000000, -1.00000000, 0.10000000
dcl_texcoord0 v0
texld r1.x, v0, s1
add r3.x, r1, -c7
cmp r3.z, -r3.x, c10.x, c10.y
add r0.w, -r3.x, c9.x
abs_pp r0.z, r3
cmp r4.y, r0.w, c10.x, c10
cmp_pp r4.z, -r0, c10.x, c10.y
rcp r3.y, c9.x
mul r2.w, r3.x, r3.y
mov r2.x, c7
add r3.w, c10.z, r2.x
mov r0.x, c0
mad r0.xy, c2.x, r0.x, v0.zwzw
mov r0.w, c4.x
mov_pp r2.x, c7
mul_pp r0.z, r4, r4.y
cmp r0.w, -r3.x, c1.x, r0
mad r3.y, -r3.x, r3, c10.x
mad r1.x, r2.w, -c1, c1
cmp r0.z, -r0, r0.w, r1.x
texld r0.xy, r0, s2
mul r1.xy, r0, r0.z
texld r0, v0, s0
mad r1.xy, -r1, c10.w, v0
texld r1, r1, s0
mul r1, r1, c5
mul r0, r0, c5
cmp r2.y, r3.w, c10, c10.x
cmp r2.x, -r2, c10.y, c10
mul_pp r4.x, r2, r2.y
mul_pp r4.z, r4.x, r4
add r2.xyz, r1, c6
mul_pp r4.w, r4.x, r3.z
cmp r0.xyz, -r4.w, r0, r2
mov r2.xyz, c6
mul r2.xyz, c8.x, r2
mul r2.xyz, r2, r3.y
rcp r3.y, c3.x
abs r3.x, r3
mad r5.x, -r3, r3.y, c10
mul r5.x, r5, r1.w
add r3.xyz, r1, r2
mul_pp r4.y, r4.z, r4
cmp r0.w, -r4, r0, r5.x
cmp r0, -r4.y, r0, r1
mad r1.xyz, r2.w, -r2, r3
abs_pp r2.x, r4
cmp r1.w, r3, c10.x, c10.y
cmp_pp r2.x, -r2, c10, c10.y
mul_pp r1.w, r2.x, r1
cmp oC0.xyz, -r4.y, r0, r1
mov_pp r1, -r1.w
mov oC0.w, r0
texkill r1.xyzw
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Float 7 [_Amount]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 2 [_DistSpeed]
Float 1 [_Distortion]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
Vector 0 [_Time]
Float 3 [_TransRange]
Float 4 [distort]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
// Shader Timing Estimate, in Cycles/64 pixel vector:
// ALU: 33.33-37.33 (25-28 instructions), vertex: 0, texture: 16,
//   sequencer: 20, interpolator: 8;    4 GPRs, 48 threads,
// Performance (if enough threads): ~33-37 cycles per vector
// * Performance may change significantly, depending on predicated jump behavior
// * Texture cycle estimates are assuming an 8bit/component texture with no
//     aniso or trilinear filtering.

"ps_360
backbbaaaaaaacgeaaaaaceeaaaaaaaaaaaaaaceaaaaacbiaaaaaceaaaaaaaaa
aaaaaaaaaaaaabpaaaaaaabmaaaaabodppppadaaaaaaaaanaaaaaabmaaaaaaaa
aaaaabnmaaaaabcaaaacaaahaaabaaaaaaaaabciaaaaaaaaaaaaabdiaaacaaaf
aaabaaaaaaaaabeaaaaaaaaaaaaaabfaaaacaaagaaabaaaaaaaaabeaaaaaaaaa
aaaaabflaaadaaabaaabaaaaaaaaabgiaaaaaaaaaaaaabhiaaadaaacaaabaaaa
aaaaabgiaaaaaaaaaaaaabijaaacaaacaaabaaaaaaaaabciaaaaaaaaaaaaabje
aaacaaabaaabaaaaaaaaabciaaaaaaaaaaaaabkaaaacaaaiaaabaaaaaaaaabci
aaaaaaaaaaaaabkmaaadaaaaaaabaaaaaaaaabgiaaaaaaaaaaaaablfaaacaaaj
aaabaaaaaaaaabciaaaaaaaaaaaaabmcaaacaaaaaaabaaaaaaaaabeaaaaaaaaa
aaaaabmiaaacaaadaaabaaaaaaaaabciaaaaaaaaaaaaabneaaacaaaeaaabaaaa
aaaaabciaaaaaaaafpebgngphfgoheaaaaaaaaadaaabaaabaaabaaaaaaaaaaaa
fpedgpgmgphcaaklaaabaaadaaabaaaeaaabaaaaaaaaaaaafpeegjhdhdedgpgm
gphcaafpeegjhdhdgpgmhggffdhcgdaaaaaeaaamaaabaaabaaabaaaaaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhaaafpeegjhdhefdhagfgfgeaafpeegjhd
hegphchegjgpgoaafpejgmgmhfgngjgogbhegfaafpengbgjgofegfhiaafpfdhe
gbhcheebgngphfgoheaafpfegjgngfaafpfehcgbgohdfcgbgoghgfaagegjhdhe
gphcheaahahdfpddfpdaaadccodacodcdadddfddcodaaaklaaaaaaaaaaaaaaab
aaaaaaaaaaaaaaaaaaaaaabeabpmaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaeaaaaaacaebaaaadaaaaaaaaaeaaaaaaaaaaaabacbaaabaaabaaaaaacb
aaaapafaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadpiaaaaaaaaaaaaalpiaaaaa
dnmmmmmnaajegaahgaanbcaabcaaafaaaaaaaaaaeabdmeaabaaaaaaaaaaaeaaj
gabhlaaabaaaaaaaaaaagabndacdbaaafgaaaaaaaaaacaamcacglaaabaaaaaaa
aaaaeaambacilaaabcaaaaaaaaaabacjaaaaccaaaaaaaaaamiadaaabaagmgmbk
claaacaabacabacbbpbpppmiaaaaeaaababaaaabbpbppodpaaaaeaaaemeiaaaa
acmggmgmiaaaahajkmeeabaaaablmgaambaaaappkmibabacaagmblabegajaapp
miaeaaaaacmggmgmilaaababmiaeaaaaaagmgmmgkmacaeaamiaeaaaaaeblgmmg
knaaabaamiadaaabacbkmglaolabaaaabaaicacbbpbppgiiaaaaeaaabaaibaab
bpbppgiiaaaaeaaacacbaaaaaagmgmgmafppahahmiabaaaaaalbgmaaobaaaaaa
miapaaabaadedeaakbabafaahaapaaacaaaaaagmkbacafaamiabaaaaaclbblaa
hhppaaaaemcaaaaabiaaaagmmcaaaaadmiacaaaabibllbaaobiaaaaamiahaaab
bileleaakaacagaamiacaaaabilbmggmilaappppmiaiaaabbibllbaaobacaaaa
hmbaaaaaaaaaaagmocaaaaaaembbaaadbigmblgmegajaaajmiaiaaabbigmblbl
omadabacmiacaaaabiblgmaaobaaaaaaliboaaadbihggmebabagaippmianaaaa
biihgmpaoladaaacmiahaaacbmbemaaaoaaaacaamiahaaaabilbgcgholaaacaa
miahaaabbigmmaloomadabaamiabaaaaaagmgmaacgahppaahaaaaaaaaaaaaagm
ocaaaaaamiaaaaaaaalbmgaadjppppaamiapiaaaaadedeaaocababaaaaaaaaaa
aaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"sce_fp_rsx // 66 instructions using 4 registers
[Configuration]
24
ffffffff000040200001fffe00000000000084c004000000
[Offsets]
10
_Time 1 0
00000130
_Distortion 2 0
00000260000001c0
_DistSpeed 1 0
00000100
_TransRange 1 0
000001f0
distort 1 0
00000220
_Color 2 0
00000310000002a0
_DissColor 2 0
0000036000000330
_Amount 3 0
000000e0000000c000000010
_Illuminate 1 0
00000390
_StartAmount 2 0
000001a000000180
[Microcode]
1056
1800010080021c9cc8000001c800000100000000000000000000000000000000
82061702c8011c9dc8000001c8003fe106800d005c001c9d00020000c8000001
0000000000000000000000000000000018800a00c8001c9d20020000c8000001
0000000000003f8000000000000000009e040100c8011c9dc8000001c8003fe1
18840b00c8001c9d80020000c800000100003f80000000000000000000000000
06840280c9001c9d5d000001c800000110020300000c1c9c00020002c8000001
0000000000000000000000000000000002820c00c80c1c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000037e4180c9041c9dc8000001c8000001
06000400fe001c9d000200005c08000100000000000000000000000000000000
028c0f80c9041c9d00020000c800000100000000000000000000000000000000
117e428001081c9c01040000c800000104820c00fe041c9d00020000c8000001
0000000000000000000000000000000010063a00c8041c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000057e428001181c9cc9040001c8000001
08063a00fe043c9d00020000c800000100000000000000000000000000000000
02820280c9081c9dc9180001c80000011000010000020008c8000001c8000001
00000000000000000000000000000000037e4280c9041c9dab040000c8000001
06001704c8001c9dc8000001c800000110000400c80c0ab50002000200020000
000000000000000000000000000000001804020080001c9cfe000001c8000001
9e001700c8011c9dc8000001c8003fe11e000200c8001c9dc8020001c8000001
0000000000000000000000000000000018020400c8081c9f0002000080080000
cccd3dcc00000000000000000000000006840f80c9081c9daa020000c8000001
000000000000000000000000000000001e0417005c041c9dc8000001c8000001
1e040200c8081c9dc8020001c800000100000000000000000000000000000000
0e000300c8081ff5c8020001c800000100000000000000000000000000000000
10000400c8081ff5540c0003c80800010e060100c8021c9dc8000001c8000001
000000000000000000000000000000001e000100c8080015c8000001c8000001
0e060200c80c1c9d00020000c800000100000000000000000000000000000000
10000100c8001c9dc8000001c80000010e000100c8001c9dc8000001c8000001
0e060400fe0c1c9fc80c0001c80c00010e060300c8081c9dc80c0001c8000001
0e040300c8081c9dc80c0003c80000010e000400c8080015fe0c0001c80c0001
077e4280c9081c9d5d080001c8000001067f5200c8000095c8000001c8000001
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
ConstBuffer "$Globals" 128 // 124 used size, 13 vars
Float 64 [_Distortion]
Float 68 [_DistSpeed]
Float 72 [_TransRange]
Float 76 [distort]
Vector 80 [_Color] 4
Vector 96 [_DissColor] 4
Float 112 [_Amount]
Float 116 [_Illuminate]
Float 120 [_StartAmount]
ConstBuffer "UnityPerCamera" 128 // 16 used size, 8 vars
Vector 0 [_Time] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_DissolveSrc] 2D 1
SetTexture 2 [_DissolveSrcBump] 2D 2
// 34 instructions, 5 temp regs, 0 temp arrays:
// ALU 22 float, 0 int, 1 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedhpbkokeclbiblibfahndjeneogjjodmdabaaaaaaoeafaaaaadaaaaaa
cmaaaaaaieaaaaaaliaaaaaaejfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcceafaaaa
eaaaaaaaejabaaaafjaaaaaeegiocaaaaaaaaaaaaiaaaaaafjaaaaaeegiocaaa
abaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaad
pcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacafaaaaaabnaaaaai
bcaabaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadpanaaaead
akaabaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaaegiccaaaaaaaaaaaagaaaaaa
fgifcaaaaaaaaaaaahaaaaaadcaaaaaldcaabaaaabaaaaaaagiacaaaabaaaaaa
aaaaaaaafgifcaaaaaaaaaaaaeaaaaaaogbkbaaaabaaaaaaefaaaaajpcaabaaa
abaaaaaaegaabaaaabaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaaefaaaaaj
pcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaa
aaaaaaajicaabaaaaaaaaaaaakaabaaaacaaaaaaakiacaiaebaaaaaaaaaaaaaa
ahaaaaaaaoaaaaaiecaabaaaabaaaaaadkaabaaaaaaaaaaackiacaaaaaaaaaaa
ahaaaaaadcaaaaamicaabaaaabaaaaaackaabaaaabaaaaaaakiacaiaebaaaaaa
aaaaaaaaaeaaaaaaakiacaaaaaaaaaaaaeaaaaaabnaaaaaibcaabaaaacaaaaaa
ckiacaaaaaaaaaaaahaaaaaadkaabaaaaaaaaaaadhaaaaakicaabaaaabaaaaaa
akaabaaaacaaaaaadkaabaaaabaaaaaadkiacaaaaaaaaaaaaeaaaaaabnaaaaah
ccaabaaaacaaaaaaabeaaaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaajicaabaaa
aaaaaaaadkaabaiaibaaaaaaaaaaaaaackiacaaaaaaaaaaaaeaaaaaaaaaaaaai
icaabaaaaaaaaaaadkaabaiaebaaaaaaaaaaaaaaabeaaaaaaaaaiadpdhaaaaak
icaabaaaabaaaaaabkaabaaaacaaaaaaakiacaaaaaaaaaaaaeaaaaaadkaabaaa
abaaaaaadiaaaaahdcaabaaaabaaaaaapgapbaaaabaaaaaaegaabaaaabaaaaaa
dcaaaaandcaabaaaabaaaaaaegaabaiaebaaaaaaabaaaaaaaceaaaaamnmmmmdn
mnmmmmdnaaaaaaaaaaaaaaaaegbabaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aeaaaaaaegaobaaaadaaaaaaegiocaaaaaaaaaaaafaaaaaaaaaaaaaibcaabaaa
abaaaaaackaabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdcaaaaajhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaaegacbaaaaeaaaaaadcaaaaal
lcaabaaaabaaaaaaegaibaaaadaaaaaaegiicaaaaaaaaaaaafaaaaaaegaibaia
ebaaaaaaaaaaaaaadcaaaaajhcaabaaaaeaaaaaakgakbaaaabaaaaaaegadbaaa
abaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaabaaaaaaegacbaaaadaaaaaa
egiccaaaaaaaaaaaafaaaaaaegiccaaaaaaaaaaaagaaaaaadiaaaaahicaabaaa
abaaaaaadkaabaaaaaaaaaaadkaabaaaaeaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aaaaaaaaegaobaaaaaaaaaaaegiocaaaaaaaaaaaafaaaaaadhaaaaajpcaabaaa
adaaaaaaagaabaaaacaaaaaaegaobaaaaeaaaaaaegaobaaaaaaaaaaadhaaaaaj
pcaabaaaabaaaaaafgafbaaaacaaaaaaegaobaaaabaaaaaaegaobaaaadaaaaaa
dbaaaaaibcaabaaaacaaaaaaabeaaaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaa
dbaaaaaiccaabaaaacaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadp
abaaaaahbcaabaaaacaaaaaabkaabaaaacaaaaaaakaabaaaacaaaaaadhaaaaaj
pccabaaaaaaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegaobaaaaaaaaaaa
doaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 51 ALU, 4 TEX
PARAM c[11] = { program.local[0..9],
		{ 1, 0, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R0.x, fragment.texcoord[0], texture[1], 2D;
ADD R3.x, R0, -c[7];
SGE R3.z, c[10].y, R3.x;
ABS R0.z, R3;
RCP R3.y, c[9].x;
MUL R4.x, R3, R3.y;
MOV R2.w, c[10].x;
MOV R0.x, c[2];
MAD R0.xy, R0.x, c[0].x, fragment.texcoord[0].zwzw;
MOV R0.w, c[1].x;
MOV R3.w, c[7].x;
SGE R4.y, c[9].x, R3.x;
CMP R4.z, -R0, c[10].y, c[10].x;
MUL R0.z, R4, R4.y;
CMP R0.w, -R3.x, c[4].x, R0;
MAD R3.y, -R3.x, R3, c[10].x;
MAD R1.x, R4, -c[1], c[1];
CMP R0.z, -R0, R1.x, R0.w;
TEX R0.xy, R0, texture[2], 2D;
MUL R0.xy, R0, R0.z;
MAD R0.xy, -R0, c[10].z, fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
MUL R1, R0, c[5];
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R2.xyz, R1, c[6];
MUL R0, R0, c[5];
SLT R4.w, c[7].x, R2;
SLT R3.w, c[10].y, R3;
MUL R3.w, R3, R4;
MUL R4.w, R3, R3.z;
CMP R0.xyz, -R4.w, R2, R0;
MOV R2.x, c[8];
MUL R2.xyz, R2.x, c[6];
MUL R2.xyz, R2, R3.y;
MUL R4.z, R3.w, R4;
RCP R3.y, c[3].x;
ABS R3.x, R3;
MAD R5.x, -R3, R3.y, c[10];
MUL R5.x, R1.w, R5;
MUL R4.y, R4.z, R4;
CMP R0.w, -R4, R5.x, R0;
CMP R0, -R4.y, R1, R0;
ADD R3.xyz, R2, R1;
MAD R1.xyz, -R2, R4.x, R3;
CMP result.color.xyz, -R4.y, R1, R0;
ABS R0.y, R3.w;
SGE R0.x, c[7], R2.w;
CMP R0.y, -R0, c[10], c[10].x;
MUL R0.x, R0.y, R0;
MOV result.color.w, R0;
KIL -R0.x;
END
# 51 instructions, 6 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"ps_3_0
; 49 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 1.00000000, 0.00000000, -1.00000000, 0.10000000
dcl_texcoord0 v0
texld r1.x, v0, s1
add r3.x, r1, -c7
cmp r3.z, -r3.x, c10.x, c10.y
add r0.w, -r3.x, c9.x
abs_pp r0.z, r3
cmp r4.y, r0.w, c10.x, c10
cmp_pp r4.z, -r0, c10.x, c10.y
rcp r3.y, c9.x
mul r2.w, r3.x, r3.y
mov r2.x, c7
add r3.w, c10.z, r2.x
mov r0.x, c0
mad r0.xy, c2.x, r0.x, v0.zwzw
mov r0.w, c4.x
mov_pp r2.x, c7
mul_pp r0.z, r4, r4.y
cmp r0.w, -r3.x, c1.x, r0
mad r3.y, -r3.x, r3, c10.x
mad r1.x, r2.w, -c1, c1
cmp r0.z, -r0, r0.w, r1.x
texld r0.xy, r0, s2
mul r1.xy, r0, r0.z
texld r0, v0, s0
mad r1.xy, -r1, c10.w, v0
texld r1, r1, s0
mul r1, r1, c5
mul r0, r0, c5
cmp r2.y, r3.w, c10, c10.x
cmp r2.x, -r2, c10.y, c10
mul_pp r4.x, r2, r2.y
mul_pp r4.z, r4.x, r4
add r2.xyz, r1, c6
mul_pp r4.w, r4.x, r3.z
cmp r0.xyz, -r4.w, r0, r2
mov r2.xyz, c6
mul r2.xyz, c8.x, r2
mul r2.xyz, r2, r3.y
rcp r3.y, c3.x
abs r3.x, r3
mad r5.x, -r3, r3.y, c10
mul r5.x, r5, r1.w
add r3.xyz, r1, r2
mul_pp r4.y, r4.z, r4
cmp r0.w, -r4, r0, r5.x
cmp r0, -r4.y, r0, r1
mad r1.xyz, r2.w, -r2, r3
abs_pp r2.x, r4
cmp r1.w, r3, c10.x, c10.y
cmp_pp r2.x, -r2, c10, c10.y
mul_pp r1.w, r2.x, r1
cmp oC0.xyz, -r4.y, r0, r1
mov_pp r1, -r1.w
mov oC0.w, r0
texkill r1.xyzw
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Float 7 [_Amount]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 2 [_DistSpeed]
Float 1 [_Distortion]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
Vector 0 [_Time]
Float 3 [_TransRange]
Float 4 [distort]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
// Shader Timing Estimate, in Cycles/64 pixel vector:
// ALU: 33.33-37.33 (25-28 instructions), vertex: 0, texture: 16,
//   sequencer: 20, interpolator: 8;    4 GPRs, 48 threads,
// Performance (if enough threads): ~33-37 cycles per vector
// * Performance may change significantly, depending on predicated jump behavior
// * Texture cycle estimates are assuming an 8bit/component texture with no
//     aniso or trilinear filtering.

"ps_360
backbbaaaaaaacgeaaaaaceeaaaaaaaaaaaaaaceaaaaacbiaaaaaceaaaaaaaaa
aaaaaaaaaaaaabpaaaaaaabmaaaaabodppppadaaaaaaaaanaaaaaabmaaaaaaaa
aaaaabnmaaaaabcaaaacaaahaaabaaaaaaaaabciaaaaaaaaaaaaabdiaaacaaaf
aaabaaaaaaaaabeaaaaaaaaaaaaaabfaaaacaaagaaabaaaaaaaaabeaaaaaaaaa
aaaaabflaaadaaabaaabaaaaaaaaabgiaaaaaaaaaaaaabhiaaadaaacaaabaaaa
aaaaabgiaaaaaaaaaaaaabijaaacaaacaaabaaaaaaaaabciaaaaaaaaaaaaabje
aaacaaabaaabaaaaaaaaabciaaaaaaaaaaaaabkaaaacaaaiaaabaaaaaaaaabci
aaaaaaaaaaaaabkmaaadaaaaaaabaaaaaaaaabgiaaaaaaaaaaaaablfaaacaaaj
aaabaaaaaaaaabciaaaaaaaaaaaaabmcaaacaaaaaaabaaaaaaaaabeaaaaaaaaa
aaaaabmiaaacaaadaaabaaaaaaaaabciaaaaaaaaaaaaabneaaacaaaeaaabaaaa
aaaaabciaaaaaaaafpebgngphfgoheaaaaaaaaadaaabaaabaaabaaaaaaaaaaaa
fpedgpgmgphcaaklaaabaaadaaabaaaeaaabaaaaaaaaaaaafpeegjhdhdedgpgm
gphcaafpeegjhdhdgpgmhggffdhcgdaaaaaeaaamaaabaaabaaabaaaaaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhaaafpeegjhdhefdhagfgfgeaafpeegjhd
hegphchegjgpgoaafpejgmgmhfgngjgogbhegfaafpengbgjgofegfhiaafpfdhe
gbhcheebgngphfgoheaafpfegjgngfaafpfehcgbgohdfcgbgoghgfaagegjhdhe
gphcheaahahdfpddfpdaaadccodacodcdadddfddcodaaaklaaaaaaaaaaaaaaab
aaaaaaaaaaaaaaaaaaaaaabeabpmaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaeaaaaaacaebaaaadaaaaaaaaaeaaaaaaaaaaaabacbaaabaaabaaaaaacb
aaaapafaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadpiaaaaaaaaaaaaalpiaaaaa
dnmmmmmnaajegaahgaanbcaabcaaafaaaaaaaaaaeabdmeaabaaaaaaaaaaaeaaj
gabhlaaabaaaaaaaaaaagabndacdbaaafgaaaaaaaaaacaamcacglaaabaaaaaaa
aaaaeaambacilaaabcaaaaaaaaaabacjaaaaccaaaaaaaaaamiadaaabaagmgmbk
claaacaabacabacbbpbpppmiaaaaeaaababaaaabbpbppodpaaaaeaaaemeiaaaa
acmggmgmiaaaahajkmeeabaaaablmgaambaaaappkmibabacaagmblabegajaapp
miaeaaaaacmggmgmilaaababmiaeaaaaaagmgmmgkmacaeaamiaeaaaaaeblgmmg
knaaabaamiadaaabacbkmglaolabaaaabaaicacbbpbppgiiaaaaeaaabaaibaab
bpbppgiiaaaaeaaacacbaaaaaagmgmgmafppahahmiabaaaaaalbgmaaobaaaaaa
miapaaabaadedeaakbabafaahaapaaacaaaaaagmkbacafaamiabaaaaaclbblaa
hhppaaaaemcaaaaabiaaaagmmcaaaaadmiacaaaabibllbaaobiaaaaamiahaaab
bileleaakaacagaamiacaaaabilbmggmilaappppmiaiaaabbibllbaaobacaaaa
hmbaaaaaaaaaaagmocaaaaaaembbaaadbigmblgmegajaaajmiaiaaabbigmblbl
omadabacmiacaaaabiblgmaaobaaaaaaliboaaadbihggmebabagaippmianaaaa
biihgmpaoladaaacmiahaaacbmbemaaaoaaaacaamiahaaaabilbgcgholaaacaa
miahaaabbigmmaloomadabaamiabaaaaaagmgmaacgahppaahaaaaaaaaaaaaagm
ocaaaaaamiaaaaaaaalbmgaadjppppaamiapiaaaaadedeaaocababaaaaaaaaaa
aaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"sce_fp_rsx // 66 instructions using 4 registers
[Configuration]
24
ffffffff000040200001fffe00000000000084c004000000
[Offsets]
10
_Time 1 0
00000130
_Distortion 2 0
00000260000001c0
_DistSpeed 1 0
00000100
_TransRange 1 0
000001f0
distort 1 0
00000220
_Color 2 0
00000310000002a0
_DissColor 2 0
0000036000000330
_Amount 3 0
000000e0000000c000000010
_Illuminate 1 0
00000390
_StartAmount 2 0
000001a000000180
[Microcode]
1056
1800010080021c9cc8000001c800000100000000000000000000000000000000
82061702c8011c9dc8000001c8003fe106800d005c001c9d00020000c8000001
0000000000000000000000000000000018800a00c8001c9d20020000c8000001
0000000000003f8000000000000000009e040100c8011c9dc8000001c8003fe1
18840b00c8001c9d80020000c800000100003f80000000000000000000000000
06840280c9001c9d5d000001c800000110020300000c1c9c00020002c8000001
0000000000000000000000000000000002820c00c80c1c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000037e4180c9041c9dc8000001c8000001
06000400fe001c9d000200005c08000100000000000000000000000000000000
028c0f80c9041c9d00020000c800000100000000000000000000000000000000
117e428001081c9c01040000c800000104820c00fe041c9d00020000c8000001
0000000000000000000000000000000010063a00c8041c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000057e428001181c9cc9040001c8000001
08063a00fe043c9d00020000c800000100000000000000000000000000000000
02820280c9081c9dc9180001c80000011000010000020008c8000001c8000001
00000000000000000000000000000000037e4280c9041c9dab040000c8000001
06001704c8001c9dc8000001c800000110000400c80c0ab50002000200020000
000000000000000000000000000000001804020080001c9cfe000001c8000001
9e001700c8011c9dc8000001c8003fe11e000200c8001c9dc8020001c8000001
0000000000000000000000000000000018020400c8081c9f0002000080080000
cccd3dcc00000000000000000000000006840f80c9081c9daa020000c8000001
000000000000000000000000000000001e0417005c041c9dc8000001c8000001
1e040200c8081c9dc8020001c800000100000000000000000000000000000000
0e000300c8081ff5c8020001c800000100000000000000000000000000000000
10000400c8081ff5540c0003c80800010e060100c8021c9dc8000001c8000001
000000000000000000000000000000001e000100c8080015c8000001c8000001
0e060200c80c1c9d00020000c800000100000000000000000000000000000000
10000100c8001c9dc8000001c80000010e000100c8001c9dc8000001c8000001
0e060400fe0c1c9fc80c0001c80c00010e060300c8081c9dc80c0001c8000001
0e040300c8081c9dc80c0003c80000010e000400c8080015fe0c0001c80c0001
077e4280c9081c9d5d080001c8000001067f5200c8000095c8000001c8000001
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
ConstBuffer "$Globals" 128 // 124 used size, 13 vars
Float 64 [_Distortion]
Float 68 [_DistSpeed]
Float 72 [_TransRange]
Float 76 [distort]
Vector 80 [_Color] 4
Vector 96 [_DissColor] 4
Float 112 [_Amount]
Float 116 [_Illuminate]
Float 120 [_StartAmount]
ConstBuffer "UnityPerCamera" 128 // 16 used size, 8 vars
Vector 0 [_Time] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_DissolveSrc] 2D 1
SetTexture 2 [_DissolveSrcBump] 2D 2
// 34 instructions, 5 temp regs, 0 temp arrays:
// ALU 22 float, 0 int, 1 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedhpbkokeclbiblibfahndjeneogjjodmdabaaaaaaoeafaaaaadaaaaaa
cmaaaaaaieaaaaaaliaaaaaaejfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcceafaaaa
eaaaaaaaejabaaaafjaaaaaeegiocaaaaaaaaaaaaiaaaaaafjaaaaaeegiocaaa
abaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaad
pcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacafaaaaaabnaaaaai
bcaabaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadpanaaaead
akaabaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaaegiccaaaaaaaaaaaagaaaaaa
fgifcaaaaaaaaaaaahaaaaaadcaaaaaldcaabaaaabaaaaaaagiacaaaabaaaaaa
aaaaaaaafgifcaaaaaaaaaaaaeaaaaaaogbkbaaaabaaaaaaefaaaaajpcaabaaa
abaaaaaaegaabaaaabaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaaefaaaaaj
pcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaa
aaaaaaajicaabaaaaaaaaaaaakaabaaaacaaaaaaakiacaiaebaaaaaaaaaaaaaa
ahaaaaaaaoaaaaaiecaabaaaabaaaaaadkaabaaaaaaaaaaackiacaaaaaaaaaaa
ahaaaaaadcaaaaamicaabaaaabaaaaaackaabaaaabaaaaaaakiacaiaebaaaaaa
aaaaaaaaaeaaaaaaakiacaaaaaaaaaaaaeaaaaaabnaaaaaibcaabaaaacaaaaaa
ckiacaaaaaaaaaaaahaaaaaadkaabaaaaaaaaaaadhaaaaakicaabaaaabaaaaaa
akaabaaaacaaaaaadkaabaaaabaaaaaadkiacaaaaaaaaaaaaeaaaaaabnaaaaah
ccaabaaaacaaaaaaabeaaaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaajicaabaaa
aaaaaaaadkaabaiaibaaaaaaaaaaaaaackiacaaaaaaaaaaaaeaaaaaaaaaaaaai
icaabaaaaaaaaaaadkaabaiaebaaaaaaaaaaaaaaabeaaaaaaaaaiadpdhaaaaak
icaabaaaabaaaaaabkaabaaaacaaaaaaakiacaaaaaaaaaaaaeaaaaaadkaabaaa
abaaaaaadiaaaaahdcaabaaaabaaaaaapgapbaaaabaaaaaaegaabaaaabaaaaaa
dcaaaaandcaabaaaabaaaaaaegaabaiaebaaaaaaabaaaaaaaceaaaaamnmmmmdn
mnmmmmdnaaaaaaaaaaaaaaaaegbabaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aeaaaaaaegaobaaaadaaaaaaegiocaaaaaaaaaaaafaaaaaaaaaaaaaibcaabaaa
abaaaaaackaabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdcaaaaajhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaaegacbaaaaeaaaaaadcaaaaal
lcaabaaaabaaaaaaegaibaaaadaaaaaaegiicaaaaaaaaaaaafaaaaaaegaibaia
ebaaaaaaaaaaaaaadcaaaaajhcaabaaaaeaaaaaakgakbaaaabaaaaaaegadbaaa
abaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaabaaaaaaegacbaaaadaaaaaa
egiccaaaaaaaaaaaafaaaaaaegiccaaaaaaaaaaaagaaaaaadiaaaaahicaabaaa
abaaaaaadkaabaaaaaaaaaaadkaabaaaaeaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aaaaaaaaegaobaaaaaaaaaaaegiocaaaaaaaaaaaafaaaaaadhaaaaajpcaabaaa
adaaaaaaagaabaaaacaaaaaaegaobaaaaeaaaaaaegaobaaaaaaaaaaadhaaaaaj
pcaabaaaabaaaaaafgafbaaaacaaaaaaegaobaaaabaaaaaaegaobaaaadaaaaaa
dbaaaaaibcaabaaaacaaaaaaabeaaaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaa
dbaaaaaiccaabaaaacaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadp
abaaaaahbcaabaaaacaaaaaabkaabaaaacaaaaaaakaabaaaacaaaaaadhaaaaaj
pccabaaaaaaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegaobaaaaaaaaaaa
doaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 51 ALU, 4 TEX
PARAM c[11] = { program.local[0..9],
		{ 1, 0, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R0.x, fragment.texcoord[0], texture[1], 2D;
ADD R3.x, R0, -c[7];
SGE R3.z, c[10].y, R3.x;
ABS R0.z, R3;
RCP R3.y, c[9].x;
MUL R4.x, R3, R3.y;
MOV R2.w, c[10].x;
MOV R0.x, c[2];
MAD R0.xy, R0.x, c[0].x, fragment.texcoord[0].zwzw;
MOV R0.w, c[1].x;
MOV R3.w, c[7].x;
SGE R4.y, c[9].x, R3.x;
CMP R4.z, -R0, c[10].y, c[10].x;
MUL R0.z, R4, R4.y;
CMP R0.w, -R3.x, c[4].x, R0;
MAD R3.y, -R3.x, R3, c[10].x;
MAD R1.x, R4, -c[1], c[1];
CMP R0.z, -R0, R1.x, R0.w;
TEX R0.xy, R0, texture[2], 2D;
MUL R0.xy, R0, R0.z;
MAD R0.xy, -R0, c[10].z, fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
MUL R1, R0, c[5];
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R2.xyz, R1, c[6];
MUL R0, R0, c[5];
SLT R4.w, c[7].x, R2;
SLT R3.w, c[10].y, R3;
MUL R3.w, R3, R4;
MUL R4.w, R3, R3.z;
CMP R0.xyz, -R4.w, R2, R0;
MOV R2.x, c[8];
MUL R2.xyz, R2.x, c[6];
MUL R2.xyz, R2, R3.y;
MUL R4.z, R3.w, R4;
RCP R3.y, c[3].x;
ABS R3.x, R3;
MAD R5.x, -R3, R3.y, c[10];
MUL R5.x, R1.w, R5;
MUL R4.y, R4.z, R4;
CMP R0.w, -R4, R5.x, R0;
CMP R0, -R4.y, R1, R0;
ADD R3.xyz, R2, R1;
MAD R1.xyz, -R2, R4.x, R3;
CMP result.color.xyz, -R4.y, R1, R0;
ABS R0.y, R3.w;
SGE R0.x, c[7], R2.w;
CMP R0.y, -R0, c[10], c[10].x;
MUL R0.x, R0.y, R0;
MOV result.color.w, R0;
KIL -R0.x;
END
# 51 instructions, 6 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"ps_3_0
; 49 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 1.00000000, 0.00000000, -1.00000000, 0.10000000
dcl_texcoord0 v0
texld r1.x, v0, s1
add r3.x, r1, -c7
cmp r3.z, -r3.x, c10.x, c10.y
add r0.w, -r3.x, c9.x
abs_pp r0.z, r3
cmp r4.y, r0.w, c10.x, c10
cmp_pp r4.z, -r0, c10.x, c10.y
rcp r3.y, c9.x
mul r2.w, r3.x, r3.y
mov r2.x, c7
add r3.w, c10.z, r2.x
mov r0.x, c0
mad r0.xy, c2.x, r0.x, v0.zwzw
mov r0.w, c4.x
mov_pp r2.x, c7
mul_pp r0.z, r4, r4.y
cmp r0.w, -r3.x, c1.x, r0
mad r3.y, -r3.x, r3, c10.x
mad r1.x, r2.w, -c1, c1
cmp r0.z, -r0, r0.w, r1.x
texld r0.xy, r0, s2
mul r1.xy, r0, r0.z
texld r0, v0, s0
mad r1.xy, -r1, c10.w, v0
texld r1, r1, s0
mul r1, r1, c5
mul r0, r0, c5
cmp r2.y, r3.w, c10, c10.x
cmp r2.x, -r2, c10.y, c10
mul_pp r4.x, r2, r2.y
mul_pp r4.z, r4.x, r4
add r2.xyz, r1, c6
mul_pp r4.w, r4.x, r3.z
cmp r0.xyz, -r4.w, r0, r2
mov r2.xyz, c6
mul r2.xyz, c8.x, r2
mul r2.xyz, r2, r3.y
rcp r3.y, c3.x
abs r3.x, r3
mad r5.x, -r3, r3.y, c10
mul r5.x, r5, r1.w
add r3.xyz, r1, r2
mul_pp r4.y, r4.z, r4
cmp r0.w, -r4, r0, r5.x
cmp r0, -r4.y, r0, r1
mad r1.xyz, r2.w, -r2, r3
abs_pp r2.x, r4
cmp r1.w, r3, c10.x, c10.y
cmp_pp r2.x, -r2, c10, c10.y
mul_pp r1.w, r2.x, r1
cmp oC0.xyz, -r4.y, r0, r1
mov_pp r1, -r1.w
mov oC0.w, r0
texkill r1.xyzw
"
}

SubProgram "xbox360 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Float 7 [_Amount]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 2 [_DistSpeed]
Float 1 [_Distortion]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
Vector 0 [_Time]
Float 3 [_TransRange]
Float 4 [distort]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
// Shader Timing Estimate, in Cycles/64 pixel vector:
// ALU: 33.33-37.33 (25-28 instructions), vertex: 0, texture: 16,
//   sequencer: 20, interpolator: 8;    4 GPRs, 48 threads,
// Performance (if enough threads): ~33-37 cycles per vector
// * Performance may change significantly, depending on predicated jump behavior
// * Texture cycle estimates are assuming an 8bit/component texture with no
//     aniso or trilinear filtering.

"ps_360
backbbaaaaaaacgeaaaaaceeaaaaaaaaaaaaaaceaaaaacbiaaaaaceaaaaaaaaa
aaaaaaaaaaaaabpaaaaaaabmaaaaabodppppadaaaaaaaaanaaaaaabmaaaaaaaa
aaaaabnmaaaaabcaaaacaaahaaabaaaaaaaaabciaaaaaaaaaaaaabdiaaacaaaf
aaabaaaaaaaaabeaaaaaaaaaaaaaabfaaaacaaagaaabaaaaaaaaabeaaaaaaaaa
aaaaabflaaadaaabaaabaaaaaaaaabgiaaaaaaaaaaaaabhiaaadaaacaaabaaaa
aaaaabgiaaaaaaaaaaaaabijaaacaaacaaabaaaaaaaaabciaaaaaaaaaaaaabje
aaacaaabaaabaaaaaaaaabciaaaaaaaaaaaaabkaaaacaaaiaaabaaaaaaaaabci
aaaaaaaaaaaaabkmaaadaaaaaaabaaaaaaaaabgiaaaaaaaaaaaaablfaaacaaaj
aaabaaaaaaaaabciaaaaaaaaaaaaabmcaaacaaaaaaabaaaaaaaaabeaaaaaaaaa
aaaaabmiaaacaaadaaabaaaaaaaaabciaaaaaaaaaaaaabneaaacaaaeaaabaaaa
aaaaabciaaaaaaaafpebgngphfgoheaaaaaaaaadaaabaaabaaabaaaaaaaaaaaa
fpedgpgmgphcaaklaaabaaadaaabaaaeaaabaaaaaaaaaaaafpeegjhdhdedgpgm
gphcaafpeegjhdhdgpgmhggffdhcgdaaaaaeaaamaaabaaabaaabaaaaaaaaaaaa
fpeegjhdhdgpgmhggffdhcgdechfgnhaaafpeegjhdhefdhagfgfgeaafpeegjhd
hegphchegjgpgoaafpejgmgmhfgngjgogbhegfaafpengbgjgofegfhiaafpfdhe
gbhcheebgngphfgoheaafpfegjgngfaafpfehcgbgohdfcgbgoghgfaagegjhdhe
gphcheaahahdfpddfpdaaadccodacodcdadddfddcodaaaklaaaaaaaaaaaaaaab
aaaaaaaaaaaaaaaaaaaaaabeabpmaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaeaaaaaacaebaaaadaaaaaaaaaeaaaaaaaaaaaabacbaaabaaabaaaaaacb
aaaapafaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadpiaaaaaaaaaaaaalpiaaaaa
dnmmmmmnaajegaahgaanbcaabcaaafaaaaaaaaaaeabdmeaabaaaaaaaaaaaeaaj
gabhlaaabaaaaaaaaaaagabndacdbaaafgaaaaaaaaaacaamcacglaaabaaaaaaa
aaaaeaambacilaaabcaaaaaaaaaabacjaaaaccaaaaaaaaaamiadaaabaagmgmbk
claaacaabacabacbbpbpppmiaaaaeaaababaaaabbpbppodpaaaaeaaaemeiaaaa
acmggmgmiaaaahajkmeeabaaaablmgaambaaaappkmibabacaagmblabegajaapp
miaeaaaaacmggmgmilaaababmiaeaaaaaagmgmmgkmacaeaamiaeaaaaaeblgmmg
knaaabaamiadaaabacbkmglaolabaaaabaaicacbbpbppgiiaaaaeaaabaaibaab
bpbppgiiaaaaeaaacacbaaaaaagmgmgmafppahahmiabaaaaaalbgmaaobaaaaaa
miapaaabaadedeaakbabafaahaapaaacaaaaaagmkbacafaamiabaaaaaclbblaa
hhppaaaaemcaaaaabiaaaagmmcaaaaadmiacaaaabibllbaaobiaaaaamiahaaab
bileleaakaacagaamiacaaaabilbmggmilaappppmiaiaaabbibllbaaobacaaaa
hmbaaaaaaaaaaagmocaaaaaaembbaaadbigmblgmegajaaajmiaiaaabbigmblbl
omadabacmiacaaaabiblgmaaobaaaaaaliboaaadbihggmebabagaippmianaaaa
biihgmpaoladaaacmiahaaacbmbemaaaoaaaacaamiahaaaabilbgcgholaaacaa
miahaaabbigmmaloomadabaamiabaaaaaagmgmaacgahppaahaaaaaaaaaaaaagm
ocaaaaaamiaaaaaaaalbmgaadjppppaamiapiaaaaadedeaaocababaaaaaaaaaa
aaaaaaaaaaaaaaaa"
}

SubProgram "ps3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
Vector 0 [_Time]
Float 1 [_Distortion]
Float 2 [_DistSpeed]
Float 3 [_TransRange]
Float 4 [distort]
Vector 5 [_Color]
Vector 6 [_DissColor]
Float 7 [_Amount]
Float 8 [_Illuminate]
Float 9 [_StartAmount]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DissolveSrc] 2D
SetTexture 2 [_DissolveSrcBump] 2D
"sce_fp_rsx // 66 instructions using 4 registers
[Configuration]
24
ffffffff000040200001fffe00000000000084c004000000
[Offsets]
10
_Time 1 0
00000130
_Distortion 2 0
00000260000001c0
_DistSpeed 1 0
00000100
_TransRange 1 0
000001f0
distort 1 0
00000220
_Color 2 0
00000310000002a0
_DissColor 2 0
0000036000000330
_Amount 3 0
000000e0000000c000000010
_Illuminate 1 0
00000390
_StartAmount 2 0
000001a000000180
[Microcode]
1056
1800010080021c9cc8000001c800000100000000000000000000000000000000
82061702c8011c9dc8000001c8003fe106800d005c001c9d00020000c8000001
0000000000000000000000000000000018800a00c8001c9d20020000c8000001
0000000000003f8000000000000000009e040100c8011c9dc8000001c8003fe1
18840b00c8001c9d80020000c800000100003f80000000000000000000000000
06840280c9001c9d5d000001c800000110020300000c1c9c00020002c8000001
0000000000000000000000000000000002820c00c80c1c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000037e4180c9041c9dc8000001c8000001
06000400fe001c9d000200005c08000100000000000000000000000000000000
028c0f80c9041c9d00020000c800000100000000000000000000000000000000
117e428001081c9c01040000c800000104820c00fe041c9d00020000c8000001
0000000000000000000000000000000010063a00c8041c9d00020000c8000001
000000000000000000000000000000001000010000021c9cc8000001c8000001
00000000000000000000000000000000057e428001181c9cc9040001c8000001
08063a00fe043c9d00020000c800000100000000000000000000000000000000
02820280c9081c9dc9180001c80000011000010000020008c8000001c8000001
00000000000000000000000000000000037e4280c9041c9dab040000c8000001
06001704c8001c9dc8000001c800000110000400c80c0ab50002000200020000
000000000000000000000000000000001804020080001c9cfe000001c8000001
9e001700c8011c9dc8000001c8003fe11e000200c8001c9dc8020001c8000001
0000000000000000000000000000000018020400c8081c9f0002000080080000
cccd3dcc00000000000000000000000006840f80c9081c9daa020000c8000001
000000000000000000000000000000001e0417005c041c9dc8000001c8000001
1e040200c8081c9dc8020001c800000100000000000000000000000000000000
0e000300c8081ff5c8020001c800000100000000000000000000000000000000
10000400c8081ff5540c0003c80800010e060100c8021c9dc8000001c8000001
000000000000000000000000000000001e000100c8080015c8000001c8000001
0e060200c80c1c9d00020000c800000100000000000000000000000000000000
10000100c8001c9dc8000001c80000010e000100c8001c9dc8000001c8000001
0e060400fe0c1c9fc80c0001c80c00010e060300c8081c9dc80c0001c8000001
0e040300c8081c9dc80c0003c80000010e000400c8080015fe0c0001c80c0001
077e4280c9081c9d5d080001c8000001067f5200c8000095c8000001c8000001
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
ConstBuffer "$Globals" 128 // 124 used size, 13 vars
Float 64 [_Distortion]
Float 68 [_DistSpeed]
Float 72 [_TransRange]
Float 76 [distort]
Vector 80 [_Color] 4
Vector 96 [_DissColor] 4
Float 112 [_Amount]
Float 116 [_Illuminate]
Float 120 [_StartAmount]
ConstBuffer "UnityPerCamera" 128 // 16 used size, 8 vars
Vector 0 [_Time] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_DissolveSrc] 2D 1
SetTexture 2 [_DissolveSrcBump] 2D 2
// 34 instructions, 5 temp regs, 0 temp arrays:
// ALU 22 float, 0 int, 1 uint
// TEX 4 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedhpbkokeclbiblibfahndjeneogjjodmdabaaaaaaoeafaaaaadaaaaaa
cmaaaaaaieaaaaaaliaaaaaaejfdeheofaaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaeeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcceafaaaa
eaaaaaaaejabaaaafjaaaaaeegiocaaaaaaaaaaaaiaaaaaafjaaaaaeegiocaaa
abaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaad
pcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacafaaaaaabnaaaaai
bcaabaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadpanaaaead
akaabaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaaegiccaaaaaaaaaaaagaaaaaa
fgifcaaaaaaaaaaaahaaaaaadcaaaaaldcaabaaaabaaaaaaagiacaaaabaaaaaa
aaaaaaaafgifcaaaaaaaaaaaaeaaaaaaogbkbaaaabaaaaaaefaaaaajpcaabaaa
abaaaaaaegaabaaaabaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaaefaaaaaj
pcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaa
aaaaaaajicaabaaaaaaaaaaaakaabaaaacaaaaaaakiacaiaebaaaaaaaaaaaaaa
ahaaaaaaaoaaaaaiecaabaaaabaaaaaadkaabaaaaaaaaaaackiacaaaaaaaaaaa
ahaaaaaadcaaaaamicaabaaaabaaaaaackaabaaaabaaaaaaakiacaiaebaaaaaa
aaaaaaaaaeaaaaaaakiacaaaaaaaaaaaaeaaaaaabnaaaaaibcaabaaaacaaaaaa
ckiacaaaaaaaaaaaahaaaaaadkaabaaaaaaaaaaadhaaaaakicaabaaaabaaaaaa
akaabaaaacaaaaaadkaabaaaabaaaaaadkiacaaaaaaaaaaaaeaaaaaabnaaaaah
ccaabaaaacaaaaaaabeaaaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaajicaabaaa
aaaaaaaadkaabaiaibaaaaaaaaaaaaaackiacaaaaaaaaaaaaeaaaaaaaaaaaaai
icaabaaaaaaaaaaadkaabaiaebaaaaaaaaaaaaaaabeaaaaaaaaaiadpdhaaaaak
icaabaaaabaaaaaabkaabaaaacaaaaaaakiacaaaaaaaaaaaaeaaaaaadkaabaaa
abaaaaaadiaaaaahdcaabaaaabaaaaaapgapbaaaabaaaaaaegaabaaaabaaaaaa
dcaaaaandcaabaaaabaaaaaaegaabaiaebaaaaaaabaaaaaaaceaaaaamnmmmmdn
mnmmmmdnaaaaaaaaaaaaaaaaegbabaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aeaaaaaaegaobaaaadaaaaaaegiocaaaaaaaaaaaafaaaaaaaaaaaaaibcaabaaa
abaaaaaackaabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdcaaaaajhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaaegacbaaaaeaaaaaadcaaaaal
lcaabaaaabaaaaaaegaibaaaadaaaaaaegiicaaaaaaaaaaaafaaaaaaegaibaia
ebaaaaaaaaaaaaaadcaaaaajhcaabaaaaeaaaaaakgakbaaaabaaaaaaegadbaaa
abaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaabaaaaaaegacbaaaadaaaaaa
egiccaaaaaaaaaaaafaaaaaaegiccaaaaaaaaaaaagaaaaaadiaaaaahicaabaaa
abaaaaaadkaabaaaaaaaaaaadkaabaaaaeaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaaipcaabaaa
aaaaaaaaegaobaaaaaaaaaaaegiocaaaaaaaaaaaafaaaaaadhaaaaajpcaabaaa
adaaaaaaagaabaaaacaaaaaaegaobaaaaeaaaaaaegaobaaaaaaaaaaadhaaaaaj
pcaabaaaabaaaaaafgafbaaaacaaaaaaegaobaaaabaaaaaaegaobaaaadaaaaaa
dbaaaaaibcaabaaaacaaaaaaabeaaaaaaaaaaaaaakiacaaaaaaaaaaaahaaaaaa
dbaaaaaiccaabaaaacaaaaaaakiacaaaaaaaaaaaahaaaaaaabeaaaaaaaaaiadp
abaaaaahbcaabaaaacaaaaaabkaabaaaacaaaaaaakaabaaaacaaaaaadhaaaaaj
pccabaaaaaaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegaobaaaaaaaaaaa
doaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "SHADOWS_SCREEN" }
"!!GLES3"
}

}

#LINE 126

		}
	}

	//FallBack "Specular"
}
