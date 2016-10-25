Shader "Unlit/OilShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_OriginX("OriginX", float) = 0.5
		_OriginY("OriginY", float) = 0.5
		_Scale("Scale", float) = 3
		_Timer("Timer", float) = 0
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _OriginX;
			float _OriginY;
			float _Scale;
			float _Timer;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float intensity = 0;
				float xx[10] = {0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9};
				float yy[10] = {0.0, -0.1, 0.2, -0.3, 0.4, -0.5, 0.6, -0.7, -0.8, -0.9 };
				float uu[10] = { 0.0, -0.1, 0.2, -0.3, 0.4, -0.5, 0.6, -0.7, -0.8, -0.9 };
				//float vv[10] = {  -0.5,  0.2, -0.3, 0.4, -0.8, 0.6, -0.7, 0.0, -0.1, -0.9 };
				float vv[10] = { 0.4,  -0.2, 0.3, -0.4, 0.25, -0.33, 0.34, -0.21, 0.31, -0.41 };

				// cos and sin of random angles
				float acs[10] = {1, 0.5, -0.866, 0.707, 0.3, 0.1, 0.866, -0.707, -0.95, 0};
				float asn[10] = {0, 0.866, 0.5, -0.707, 0.95, -0.995, 0.5, -0.707, 0.1, -1};
				/*for (int k =0; k < 10; k++)
				{
					float x0 = xx[k] + _Timer *uu[k];
					float y0 = yy[k] + _Timer* vv[k];

					float x = (_Scale * i.uv.x) - x0;
					float y = (_Scale * i.uv.y) - y0;
					float d2 = x*x + y*y;
					float rd2 = 1.0 / (0.2 + d2); // 1 at centre, tails off to zero
					intensity += rd2;
				}*/

				// harmonics
				for (int k = 1; k < 10; k++)
				{
					float x0 = xx[k] + _Timer *uu[k];
					float y0 = yy[k] + _Timer* vv[k];

					float x = 100 + (k * (acs[k]*i.uv.x + asn[k]*i.uv.y)) - x0;
					float y = 100 + (k * (acs[k]*i.uv.y - asn[k]*i.uv.x)) - y0;

					int ix = x;
					x = x - 0.5f  - ix;
					int iy = y;
					y = y - 0.5f - iy;

					float d2 = x*x + y*y;
					float rd2 = 1.0 / (0.2 + d2); // 1 at centre, tails off to zero
					intensity += rd2;
				}
				
				int count = 10 * intensity;
				intensity = count * 0.1;
				count = intensity;
				intensity = intensity - count;
				intensity = 0.1 * (9 + intensity);
				fixed4 col = fixed4(intensity, 0.4*intensity, 0.8*intensity, 1);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);				
				return col;
			}
			ENDCG
		}
	}
}
