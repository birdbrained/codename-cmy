﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/UVOffset" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0, 0.1)) = 1
		_Color("Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Tags
		{
			"Queue" = "Transparent"
		}

		Pass
		{
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
			sampler2D _DisplaceTex;
			float _Magnitude;
			float4 _Color;

			float4 frag(v2f i) : SV_Target
			{
				//float2 disp = tex2D(_DisplaceTex, i.uv * _Time.x).xy;
				float2 disp = tex2D(_DisplaceTex, float2(i.uv.x + _Time.x, i.uv.y)).xy;
				disp = ((disp * 2) - 1) * _Magnitude;
				float4 col = tex2D(_MainTex, i.uv + disp);
				return col * _Color;
			}
			ENDCG
		}
	}
}
