Shader "SSD/SSDRefractionShader"
{
	Properties
	{
		_ReflectivityColor ("Reflectivity Color", Color) = (.5, .5, .5, .5)
		_ReflectivityTexture ("Reflectivity Texture", 2D) = "white" {}

		_IOR("Index of refraction", Range(1.0, 2.5)) = 1.33
		
		_RefractionColor("Refraction Color", Color) = (.5, .5, .5, .5)
		_RefractionTexture("Refraction Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha

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
			float4 _RefractionColor;
			
			sampler2D _RefractionTexture;
			sampler2D _ReflectivityTexture;

			float _IOR;
			
			// NOTE: COMPLETELY FAKE VIZ SHADER, NOT TRYING TO MIMIC REFRACTIVE SURFACES!
			float4 frag (v2f i) : SV_Target
			{	
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                float3 worldRefl = reflect(worldViewDir, normalize(-i.worldNormal));

				// Fake reflection :)
				float f = .2 + .5 * cos(6.28318 * fmod((worldRefl.y * .5 + .5) * 1.5, 1.0) - .5);
				float result = f + pow(1.0 - dot(i.worldNormal, worldViewDir), 4);
				
				// Checker raytracing...
				float d = -1.0 / worldRefl.y;
				float2 p = (float3(0.0, 1.0, 0.0) + worldRefl * d).xz * 2;
				float checker = ((floor(abs(p.x)) + floor(abs(p.y))) % 2);

				float4 reflColor = _ReflectivityColor * tex2D(_ReflectivityTexture, i.uv);
				reflColor.a = 1.0;

				float4 refrColor = _RefractionColor * tex2D(_RefractionTexture, i.uv) * result;
				
				float t = pow(1.0 - dot(i.worldNormal, worldViewDir), .5);
				return lerp(refrColor, reflColor, t);
			}

			ENDCG
		}
	}
}
