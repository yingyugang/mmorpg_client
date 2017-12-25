  Shader "QFX/SF_VFX/Materialization" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5

		[NoScaleOffset] _MetallicGlossMap("Metallic", 2D) = "white" {}
		[Gamma]  _Metallic ("Metallic", Range(0,1)) = 0
				
		_DissTexture("Dissolve Texture", 2D) = "white" {}
    	[HDR]_DissolveColor("Dissolve Color", Color) = (0,1,0,1)
        _ScrollXSpeed("Scroll X Speed", Float) = 0
    	_ScrollYSpeed("Scroll Y Speed", Float) = 0

        _Center("Dissolve Center (W=Unused)", Vector) = (0,0,0,0)
		_Dissolve("Dissolve", Range(0,5)) = 1
        _Distance("Dissolve Distance", Float) = 1
		_Interpolation("Dissolve Interpolation", Range(0,5)) = 1
		_EmissionInterpolation("Emission Interpolation", Range(0,5)) = 1

		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}

		[NoScaleOffset]_EmissionMap ("Emission", 2D) = "white" { }
		[HDR]_EmissionColor ("Color", Color) = (0,0,0,1)
    }

    SubShader{
        Tags { "RenderType" = "Opaque" }
        LOD 200
    
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Standard addshadow
        #pragma target 3.0

            sampler2D _MainTex;
    
            struct Input {
                float2 uv_MainTex;
                float2 uv_BumpMap;
                float2 uv_DissTexture;
                float3 worldPos;
            };
    
            sampler2D _MetallicGlossMap, _EmissionMap, _BumpMap, _DissTexture;
            float4 _EmissionColor, _DissolveColor, _Center;
            half _Dissolve, _Distance, _Interpolation, _EmissionInterpolation, _Glossiness, _Metallic;
            half _ScrollXSpeed, _ScrollYSpeed;
            fixed4 _Color;
    
            void surf(Input IN, inout SurfaceOutputStandard o) {
                IN.uv_DissTexture.x += _Time.y * _ScrollXSpeed;
                IN.uv_DissTexture.y += _Time.y * _ScrollYSpeed;
                
                fixed4 dissTex = tex2D(_DissTexture, IN.uv_DissTexture);

                float distance = length(_Center.xyz - IN.worldPos.xyz);
                
                float invfade = (dissTex * _Interpolation * _Distance);
                
                clip(_Distance - distance + invfade);
    
                fixed4 color = tex2D(_MetallicGlossMap, IN.uv_MainTex);
                fixed4 emission = tex2D(_EmissionMap, IN.uv_MainTex);
                
                o.Metallic = color.r * _Metallic ;
                o.Smoothness = _Glossiness * color.a;
                
                color = tex2D(_MainTex, IN.uv_MainTex);
                
                o.Albedo = color.rgb * _Color.rgb;
                o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
                
                half em = saturate(1-(_Distance-distance+_EmissionInterpolation));
                
                if (_Distance - distance + invfade < _Dissolve)
                    o.Emission = _DissolveColor;
                else o.Emission = emission * _EmissionColor + em *_DissolveColor.rgb; // * dissTex
                
                o.Alpha = color.a;
            }
            ENDCG
	    }
    Fallback "Diffuse"
}