Shader "Unlit/GridShader"
{
	Properties
	{
		_MainTex ("Texture", 2D)    = "white" {}
		_Color   ("color"  , Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		ZWrite Off
		Cull   Off
		Blend One OneMinusSrcAlpha // Premultiplied transparency

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex   vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv     : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv     : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float  dis    : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4    _MainTex_ST;
			float4    _Color;

			v2f vert (appdata v)
			{
				v2f o;
		
				    o.dis     = length(v.vertex);
				    o.vertex  = UnityObjectToClipPos( v.vertex);
				    o.uv      = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv );
			    col *= _Color* exp(-i.dis*0.35 + 0.1);
				return col* col.a;
			}
			ENDCG
		}
	}
}
