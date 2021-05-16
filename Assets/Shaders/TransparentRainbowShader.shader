Shader "Unlit/TransparentRainbowShader"
{
    Properties{
		_Alpha ("Alpha", float) = 0
	}

	SubShader{
		Tags{ "RenderType"="Transparent" "Queue"="Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off

		Pass{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			float _Alpha;

			struct appdata{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f{
				float4 position : SV_POSITION;
				float3 normal : TEXCOORD0;
			};

			v2f vert(appdata v){
				v2f output;

				output.position = UnityObjectToClipPos(v.vertex);
                output.normal = UnityObjectToWorldNormal(v.normal);

				return output;
			};

			fixed4 frag(v2f input) : SV_TARGET{
				fixed4 c = 0;
                
                c.rgb = input.normal * 0.5 + 0.5;
				c.a = _Alpha;

                return c;
			};

			ENDCG
		}
	}
}