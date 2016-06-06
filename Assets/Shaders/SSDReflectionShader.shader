Shader "SSD/SSDReflectionShader"
{
	Properties
	{
		_ReflectivityColor ("Reflectivity Color", Color) = (.5, .5, .5, .5)
		_ReflectivityTexture ("Reflectivity Texture", 2D) = "white" {}
		
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
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 worldPos : TEXCOORD1;
			};

			float4 _ReflectivityTexture_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.worldPos = mul(_Object2World, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv,_ReflectivityTexture);
				return o;
			}

			float4 _ReflectivityColor;
			sampler2D _ReflectivityTexture;
			
			// NOTE: COMPLETELY FAKE VIZ SHADER, NOT TRYING TO MIMIC REFLECTIVE SURFACES!
			float4 frag (v2f i) : SV_Target
			{	
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                float3 worldRefl = reflect(-worldViewDir, normalize(i.worldNormal));

				// Fake reflection :)
				float f = .515 + .5 * cos(6.28318 * fmod((worldRefl.y * .5 + .5) * 1.5, 1.0) - .5);
				float result = f;// * (.25 + .75 * pow(1.0 - dot(i.worldNormal, worldViewDir), 6.0));

				result += pow(1.0 - dot(i.worldNormal, worldViewDir), 1.5) * .25;

				return (_ReflectivityColor * tex2D(_ReflectivityTexture, i.uv)) * result;				
			}

			ENDCG
		}
	}
}
