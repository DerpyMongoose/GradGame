Shader "ToonStyle/RoyalToonShader" {
	Properties {
		[Header(Diffuse Color)]
		_MainTex ("Diffuse Texture", 2D) = "white" {}
		_Color ("Diffuse Color", Color) = (1,1,1,1)

		[Header(Toon Ramp)]
		_Ramp ("Toon Ramp", 2D) = "gray" {}
		_HighlightColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_ShadowColor ("Shadow Color", Color) = (0.2,0.2,0.2,1.0)

		[Header(Specular)]
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Specularity ("Specularity", Range(0,1)) = 0.05
		_SpecularSmooth ("Blur of Specular edge", Range(0,1)) = 0.05
		_Shininess ("Size of Specular Dot", Range(0.01,2)) = 0.1

	
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf ToonColor noforwardadd interpolateview halfasview
		#pragma target 3.0


		sampler2D _MainTex;
		sampler2D _Ramp;

		struct Input {
			float2 uv_MainTex : TEXCOORD0;
		};

		fixed4 _Color;
		fixed4 _HighlightColor;
		fixed4 _ShadowColor;
		fixed _Specularity;
		fixed _SpecularSmooth;
		fixed _Shininess;

		inline half4 LightingToonColor (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten){
			//light strength between 0 and 1
			fixed NdotL = max(0.1, dot(s.Normal, lightDir)*0.5 + 0.5);
			//Color based on ramp texture
			fixed3 ramp = tex2D(_Ramp, fixed2(NdotL,NdotL));

			_ShadowColor = lerp(_HighlightColor, _ShadowColor, _ShadowColor.a);	//Shadows intensity through alpha
			ramp = lerp(_ShadowColor.rgb,_HighlightColor.rgb, ramp);

			//Specular
			half3 h = normalize(lightDir + viewDir);
			float NdotH = max(0, dot(s.Normal, h));
			float spec = pow(NdotH, s.Specular*128.0) * s.Gloss * 2.0;
			spec = smoothstep(0.5-_SpecularSmooth*0.5, 0.5+_SpecularSmooth*0.5, spec);
			spec *= atten;
			ramp *= atten;
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.rgb += _LightColor0.rgb * _SpecColor.rgb * spec;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			o.Gloss = _Specularity;
			o.Specular = _Shininess;
			//o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
