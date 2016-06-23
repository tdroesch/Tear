Shader "Sprites/Background Diffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
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
		
		sampler2D _MainTex;
		float4 _MainTex_ST;
		
		struct Input
		{
			float3 worldPos;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			v.normal = float3(0,0,-1);
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, _MainTex_ST.xy * IN.worldPos.xy + _MainTex_ST.zw);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	// The definition of a fallback shader should be commented out 
	// during development:
	// Fallback "Unlit/Transparent Cutout"
}
