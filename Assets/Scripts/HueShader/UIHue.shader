Shader "UI/Hue"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		[Toggle(UNITY_UI_HUE)] _UseUIHUE("Use Hue", Float) = 0
		_Hue("Hue", Float) = 0
		_Sat("Saturation", Float) = 1
		_Val("Value", Float) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			#pragma multi_compile __ UNITY_UI_HUE
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float _Hue;
			float _Sat;
			float _Val;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
				#endif
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;		

			float3 shift_col(float3 RGB, float3 shift)
			{
				float3 RESULT = float3(RGB);
				float s = shift.z*shift.y;
				float sRad = shift.x*3.14159265 / 180;
				float VSU = s*cos(sRad);
				float VSW = s*sin(sRad);

				RESULT.x = (.299*shift.z + .701*VSU + .168*VSW)*RGB.x
					+ (.587*shift.z - .587*VSU + .330*VSW)*RGB.y
					+ (.114*shift.z - .114*VSU - .497*VSW)*RGB.z;

				RESULT.y = (.299*shift.z - .299*VSU - .328*VSW)*RGB.x
					+ (.587*shift.z + .413*VSU + .035*VSW)*RGB.y
					+ (.114*shift.z - .114*VSU + .292*VSW)*RGB.z;

				RESULT.z = (.299*shift.z - .3*VSU + 1.25*VSW)*RGB.x
					+ (.587*shift.z - .588*VSU - 1.05*VSW)*RGB.y
					+ (.114*shift.z + .886*VSU - .203*VSW)*RGB.z;

				return (RESULT);
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				#ifdef UNITY_UI_HUE
				float3 shift = float3(_Hue, _Sat, _Val);
				color = half4(half3(shift_col(color, shift)), color.a);
				#endif
				
				return color;
			}
		ENDCG
		}
	}
}
