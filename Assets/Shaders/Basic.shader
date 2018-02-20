// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Basic" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1) 
		_TileAmount("Tile Amount", int) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _Color;
			int _TileAmount;

			float4 frag(v2f i) : SV_Target
			{
				//float4 color = float4(i.uv.r, i.uv.g, 1, 1);
				float4 color = tex2D(_MainTex, i.uv * _TileAmount) * _Color;
				//color = mul(UNITY_MATRIX_MVP, _Color);
				return color;
			}
			ENDCG
		}
	}
}
