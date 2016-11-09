Shader "Custom/ToonShader" {
	Properties {
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Ramp("Grey Ramp", 2D) = "white" {}
		_Specularity ("Specularity", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags {
			"RenderType" = "Opaque"
		}
		LOD 200

		CGPROGRAM
		#pragma surface surf ToonShader fullforwardshadows
		#pragma target 3.0

		sampler2D _Ramp;

		half4 LightingToonShader(SurfaceOutput s, half3 lightDir, fixed3 viewDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);
			half diff = NdotL;// * 0.5 + 0.5;
			//half3 ramp = tex2D (_Ramp, float2(diff, diff)).rgb;
			//if (NdotL <= 0.0) NdotL = 0;
			//else NdotL = 1;

			fixed3 h = normalize (lightDir + viewDir);
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Gloss * 128) * s.Specular;

			half4 c;
			//c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			if(diff > 1){
				c.rgb = s.Albedo * _LightColor0.rgb * 1 * (atten * 2);
			//}
			//else if(diff > 0.5){
			//	c.rgb = s.Albedo * _LightColor0.rgb * 0.6 * (atten * 2);
			}
			else if(diff > 0.25){
				c.rgb = s.Albedo * _LightColor0.rgb * ((1 - 0.4)*diff+0.4) * (atten * 2);
			}
			else if(diff > 0){
				c.rgb = s.Albedo * _LightColor0.rgb * ((1 - 0.2)*diff+0.2  ) * (atten * 2);
			}
			else
				c.rgb = s.Albedo * _LightColor0.rgb * NdotL * (atten * 2);

			c.rgb = c.rgb + (_LightColor0.rgb * spec * atten * 2);
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		fixed4 _Color;
		half _Specularity;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Specular = _Specularity;
			o.Gloss = _Specularity;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
