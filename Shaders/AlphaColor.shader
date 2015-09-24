Shader "Custom/AlphaColor" {
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_AlphaColor("Alpha Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader {
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200

		// extra pass that renders to depth buffer only
		Pass {
			ZWrite On
			ColorMask 0
		}

		CGPROGRAM
		#pragma surface surf Lambert

		fixed4 _AlphaColor;

		// Note: pointless texture coordinate. I couldn't get Unity (or Cg)
		//       to accept an empty Input structure or omit the inputs.
		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = _AlphaColor.rgb;
			o.Emission = _AlphaColor.rgb; // * _Color.a;
			o.Alpha = _AlphaColor.a;
		}
		ENDCG

		// paste in forward rendering passes from Transparent/Diffuse
		UsePass "Transparent/Diffuse/FORWARD"
	}

	
}