Shader "SSD/SSDCookTorranceShader"
{
	Properties
	{
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ColorTexture ("Color Texture", 2D) = "white" {}

		_Roughness ("Roughness", Range(.01, 1.0)) = .5
		_RoughnessTexture("Roughness Texture Texture", 2D) = "white" {}

		_SpecularColor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_SpecularTexture("Specular Texture", 2D) = "white" {}

		_NormalTexture("Normal Texture", 2D) = "black"
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
			};

			float4 _ColorTexture_ST;
			float4 _SpecularTexture_ST;
			float4 _ExponentTexture_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _ColorTexture);
				o.uv2 = TRANSFORM_TEX(v.uv, _ExponentTexture);
				o.uv3 = TRANSFORM_TEX(v.uv, _SpecularTexture);
				return o;
			}

			sampler2D _ColorTexture;
			float4 _Color;

			float _Roughness;
			float4 _SpecularColor;

			sampler2D _RoughnessTexture;
			sampler2D _SpecularTexture;
			
			float4 frag (v2f i) : SV_Target
			{	
				float3 normal = normalize(i.normal);

				float3 lightDirection = normalize(float3(1, 1, 1));
				float4 color = tex2D(_ColorTexture, i.uv) * _Color;
				float diffuse = dot(lightDirection, normal);
				
				float3 refl = reflect(lightDirection, normal);

				// NOTE: THIS IS STILL PHONG, JUST A HACK TO VISUALIZE SOMETHING
				float texExponent = length(tex2D(_RoughnessTexture, i.uv2).rgb) / sqrt(3);
				float exponent = (1.0 / _Roughness) * texExponent;

				float specular = pow(saturate(-refl.z), exponent);
				float4 specularColor = _SpecularColor * tex2D(_SpecularTexture, i.uv3);

				specular += saturate(pow(1.0 - abs(normal.z), 4)) * .25;

				return color * diffuse + specularColor * specular;
			}

			ENDCG
		}
	}
}
