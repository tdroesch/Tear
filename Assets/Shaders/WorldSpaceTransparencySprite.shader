Shader "Sprites/WorldSpaceTransparencySprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Clip_X ("Clip X", float) = 0
		_Clip_Y ("Clip Y", float) = 0
		[MaterialToggle] _Inverse_Y ("Inverse Y", float) = 0
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

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog keepalpha
		#pragma multi_compile DUMMY _INVERSE_Y_ON

		sampler2D _MainTex;
		fixed4 _Color;
		float _Clip_X;
		float _Clip_Y;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
			float3 worldPos;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			v.normal = float3(0,0,-1);
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			#if defined(_INVERSE_Y_ON)
			if (IN.worldPos.x < _Clip_X && IN.worldPos.y < _Clip_Y) clip(-1);
			#else
			if (IN.worldPos.x < _Clip_X && IN.worldPos.y > _Clip_Y) clip(-1);
			#endif
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			o.Albedo = c.rgb * c.a;
			o.Alpha = c.a;
		}
		ENDCG
	}

Fallback "Transparent/VertexLit"
}
