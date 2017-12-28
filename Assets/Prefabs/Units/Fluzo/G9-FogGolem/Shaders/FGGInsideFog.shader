// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:1,fgcg:1,fgcb:1,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:33179,y:32725,varname:node_4795,prsc:2|emission-3671-OUT,alpha-798-OUT,voffset-4657-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32438,y:32453,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7989-UVOUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32131,y:32756,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32190,y:32937,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:798,x:32539,y:32944,varname:node_798,prsc:2|A-6074-R,B-2053-A,C-797-A;n:type:ShaderForge.SFN_Panner,id:7989,x:32047,y:32559,varname:node_7989,prsc:2,spu:0.01,spv:0.14|UVIN-7243-OUT;n:type:ShaderForge.SFN_TexCoord,id:7582,x:31703,y:32480,varname:node_7582,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:4657,x:32805,y:33128,varname:node_4657,prsc:2|A-6604-OUT,B-6496-RGB,C-3390-OUT;n:type:ShaderForge.SFN_NormalVector,id:6604,x:32319,y:33251,prsc:2,pt:True;n:type:ShaderForge.SFN_Tex2d,id:6496,x:32459,y:33118,ptovrint:False,ptlb:Displacem,ptin:_Displacem,varname:node_6496,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7989-UVOUT;n:type:ShaderForge.SFN_Multiply,id:7243,x:31893,y:32608,varname:node_7243,prsc:2|A-7582-UVOUT,B-7895-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7895,x:31729,y:32781,ptovrint:False,ptlb:tile,ptin:_tile,varname:node_7895,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:3671,x:32834,y:32823,varname:node_3671,prsc:2|A-6074-RGB,B-2053-RGB,C-4607-RGB,D-8062-OUT;n:type:ShaderForge.SFN_Color,id:4607,x:32603,y:32230,ptovrint:False,ptlb:ColorFog,ptin:_ColorFog,varname:node_4607,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:8062,x:32780,y:32525,ptovrint:False,ptlb:diffuse2,ptin:_diffuse2,varname:node_8062,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.5;n:type:ShaderForge.SFN_Slider,id:3390,x:32432,y:33419,ptovrint:False,ptlb:offset,ptin:_offset,varname:node_3390,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.8;proporder:6074-797-6496-7895-4607-8062-3390;pass:END;sub:END;*/

Shader "manyworlds/FGGInsideFog" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _Displacem ("Displacem", 2D) = "white" {}
        _tile ("tile", Float ) = 3
        _ColorFog ("ColorFog", Color) = (0.5,0.5,0.5,1)
        _diffuse2 ("diffuse2", Float ) = 1.5
        _offset ("offset", Range(0, 0.8)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform sampler2D _Displacem; uniform float4 _Displacem_ST;
            uniform float _tile;
            uniform float4 _ColorFog;
            uniform float _diffuse2;
            uniform float _offset;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_6180 = _Time + _TimeEditor;
                float2 node_7989 = ((o.uv0*_tile)+node_6180.g*float2(0.01,0.14));
                float4 _Displacem_var = tex2Dlod(_Displacem,float4(TRANSFORM_TEX(node_7989, _Displacem),0.0,0));
                v.vertex.xyz += (v.normal*_Displacem_var.rgb*_offset);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_6180 = _Time + _TimeEditor;
                float2 node_7989 = ((i.uv0*_tile)+node_6180.g*float2(0.01,0.14));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_7989, _MainTex));
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_ColorFog.rgb*_diffuse2);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.r*i.vertexColor.a*_TintColor.a));
            }
            ENDCG
        }
    }
}
