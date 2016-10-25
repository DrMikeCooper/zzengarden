Shader "Unlit/UnlitToon"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	    _ColorIndex ("Color Index", Range(0, 15)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			// indicate that our pass is the "base" pass in forward
			// rendering pipeline. It gets ambient and main directional
			// light data set up; light direction in _WorldSpaceLightPos0
			// and color in _LightColor0
			Tags{ "LightMode" = "ForwardBase" }
			//Tags{ "RenderType" = "Opaque" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				half3 normal : NORMAL;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _ColorIndex;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half nl = max(0, dot(i.normal, _WorldSpaceLightPos0.xyz));

				float v0 = (2 * _ColorIndex + 1) / 32.0f;
				fixed4 col = tex2D(_MainTex, float2(nl, v0));

				/*
				_ColorIndex = 0;
				half nl = max(0, dot(i.normal, _WorldSpaceLightPos0.xyz));
				float v0 = (2 * _ColorIndex + 1) / 32.0f;
				float2 uv = i.uv;
				uv.y = v0;
				uv.x = nl;
				fixed4 col = tex2D(_MainTex, uv);
				*/

				// factor in the light color
				//o.diff = nl * _LightColor0;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);	
				return col;
			}
			ENDCG
		}
	}
}
