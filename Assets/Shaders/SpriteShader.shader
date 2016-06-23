Shader "Sprites/Overlay Texture" {
	Properties {
		_DecalTex ("Overlay", 2D) = "white" {}
		_MainTex ("Sprite", 2D) = "white" {} 
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
	}
	SubShader {
		Pass {    
			Cull back

			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 

			uniform sampler2D _DecalTex;
			uniform float4 _DecalTex_ST;
			uniform sampler2D _MainTex;    
			uniform float _Cutoff;

			struct vertexInput {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 position_in_world_space : TEXCOORD1;
			};

			vertexOutput vert(vertexInput input) 
			{
				vertexOutput output;

				output.tex = input.texcoord;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.position_in_world_space = 
					mul(_Object2World, input.vertex);
				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				float4 textureColor = tex2D(_MainTex, float2(input.tex.xy));  
				if (textureColor.a < _Cutoff)
				// alpha value less than user-specified threshold?
				{
					discard; // yes: discard this fragment
				} 
				float4 overlayColor = 
					tex2D(_DecalTex, _DecalTex_ST.xy * input.position_in_world_space.xy + _DecalTex_ST.zw);
				return lerp(textureColor, overlayColor, 
					(textureColor.r+textureColor.g+textureColor.b)/3);
			}
			ENDCG
		}
	}

	// The definition of a fallback shader should be commented out 
	// during development:
	// Fallback "Unlit/Transparent Cutout"
	}