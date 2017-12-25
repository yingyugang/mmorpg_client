// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QFX/SF_VFX/Invisible" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		[HDR]_Color("Color", Color) = (1, 1, 1, 1)
		_Opacity("Opacity", Float) = 1 //BumpMultiply, BumpSpeed, ScrollSpeed
		_Params("Params", Float) = (0.5,4,1,0) //BumpMultiply, BumpSpeed, ScrollSpeed
		_DistanceFade("Distance Fade (X=Near, Y=Far, ZW=Unused)", Float) = (20, 50, 0, 0)
	}
		SubShader{
			GrabPass{ "_GrabTexture" }

			Pass {
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back Lighting Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : SV_POSITION;
				half4 texcoord : TEXCOORD0;
				float4 color : COLOR;
				half4 screenuv : TEXCOORD1;
			};

			sampler2D _MainTex, _GrabTexture;
			fixed4 _Color;
			fixed4 _Params;
			float4 _DistanceFade;
			float _Opacity;

			float4 _MainTex_ST;

			v2f vert(appdata_full v) {
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				float4 tex = tex2Dlod(_MainTex, float4(v.texcoord.xy, 0, 0));
				float3 anim = sin(_Time.y * _Params.y);
				anim = v.normal * tex * _Params.x * lerp(tex, 0, anim) * 0.1;
				v.vertex.xyz += anim;

#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
#endif
				o.color = v.color;
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);

				o.texcoord.y += _Params.z * _Time.y;

				o.texcoord.zw = v.texcoord;

				half4 screenpos = ComputeGrabScreenPos(o.vertex);
				o.screenuv.xy = screenpos.xy / screenpos.w;

				half depth = length(mul(UNITY_MATRIX_MV, v.vertex));
				o.screenuv.z = saturate((_DistanceFade.y - depth) / (_DistanceFade.y - _DistanceFade.x));
				o.screenuv.w = depth;

				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				half2 distort = tex2D(_MainTex, i.texcoord.xy).xy;
				half2 offset = (distort.xy * 2 - 1) * _Params.w * i.screenuv.z * i.color.a;
				half2 uv = i.screenuv.xy + offset;
				half4 dcolor = tex2D(_GrabTexture, uv);
				float4 ff = tex2D(_MainTex, i.texcoord + offset);
				dcolor *= _Color;
				UNITY_OPAQUE_ALPHA(dcolor.a);
				return dcolor;
			}

			ENDCG
		}
		}
			FallBack "Diffuse"
}
