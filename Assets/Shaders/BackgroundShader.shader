Shader "Sprites/Background" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Pass {    
			Cull back

			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;

			struct vertexInput {
				float4 vertex : POSITION;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
			};

			vertexOutput vert(vertexInput input) 
			{
				vertexOutput output;

				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.position_in_world_space = 
					mul(_Object2World, input.vertex);
				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				float4 textureColor = tex2D(_MainTex, _MainTex_ST.xy * input.position_in_world_space.xy + _MainTex_ST.zw);
				return textureColor;
			}
			ENDCG
		}
	}
	// The definition of a fallback shader should be commented out 
	// during development:
	// Fallback "Unlit/Transparent Cutout"
}
