// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Distortion" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Freq("Frequency", Range(1,256)) = 50
		_Scale("Scale", Range(0,1)) = 0.1
		_Speed("Speed", Range(0,10)) = 1
		_Color("Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{
			//"RenderType" = "Opaque"
			"Queue" = "Transparent"
		}
		LOD 200

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			//#pragma surface surf Lambert
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
			float _Freq;
			float _Scale;
			float _Speed;
			float4 _Color;

			float4 frag(v2f i) : SV_Target
			{
				//float x = i.uv.x - 0.5;
				//float y = i.uv.y - 0.5;
				//float dist = x * x + y * y;

				half4 c = tex2D(_MainTex, i.uv + float2(_Scale, 0) * sin((_Time * _Speed + i.uv.y) * _Freq)) * _Color;
				return c;
			}
			ENDCG
		}
	}
}