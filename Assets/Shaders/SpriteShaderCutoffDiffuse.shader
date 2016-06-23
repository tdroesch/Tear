Shader "Sprites/Overlay Texture Cutoff Diffuse" {
	Properties {
		_DecalTex ("Overlay", 2D) = "white" {}
		_MainTex ("Sprite", 2D) = "white" {} 
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
		_WhiteCutoff ("Overlay Cutoff", Range(0,3)) = 3.0
	}
	SubShader {
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
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		
		sampler2D _DecalTex;
		float4 _DecalTex_ST;
		sampler2D _MainTex;    
		float _Cutoff;
		float _WhiteCutoff;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			v.normal = float3(0,0,-1);
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			clip(c.a - _Cutoff);
			float4 overlayColor = tex2D(_DecalTex, _DecalTex_ST.xy * 
				IN.worldPos.xy + _DecalTex_ST.zw);
			if (c.r + c.g + c.b >= _WhiteCutoff)
				o.Albedo = overlayColor.rgb;
			else o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	// The definition of a fallback shader should be commented out 
	// during development:
	// Fallback "Unlit/Transparent Cutout"
	}