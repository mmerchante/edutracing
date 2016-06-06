Shader "SSD/SSDDiffuseShader"
{
	Properties
	{
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ColorTexture ("Color Texture", 2D) = "white" 
		
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			float4 _ColorTexture_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _ColorTexture);
				return o;
			}

			sampler2D _ColorTexture;
			float4 _Color;
			
			float4 frag (v2f i) : SV_Target
			{	
				float3 normal = normalize(i.normal);
				float3 lightDirection = normalize(float3(1, 1, 1));
				float4 color = tex2D(_ColorTexture, i.uv) * _Color;
				float diffuse = saturate(dot(lightDirection, normal));
				return color * diffuse;
			}

			ENDCG
		}
	}
}
