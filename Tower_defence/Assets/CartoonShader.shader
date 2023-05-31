Shader "Custom/CartoonShader" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.01
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 0.5)
        _NumSteps ("Number of Steps", Range(1, 10)) = 3
        _StepHeight ("Step Height", Range(0.001, 1)) = 0.1
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        fixed4 _OutlineColor;
        float _OutlineWidth;
        fixed4 _ShadowColor;
        float _NumSteps;
        float _StepHeight;

        struct Input {
            float2 uv_MainTex;
            float4 vertex : SV_POSITION;
        };

        void vert(inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.uv_MainTex = v.texcoord;
            o.vertex = UnityObjectToClipPos(v.vertex);
        }

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Calculate the outline effect
            float outline = 1 - smoothstep(0, _OutlineWidth, fwidth(IN.uv_MainTex));

            // Calculate the shadow height
            float shadowHeight = floor(IN.uv_MainTex.y * _NumSteps) * _StepHeight;

            // Apply the outline and shadow
            fixed4 outlineColor = _OutlineColor * outline;
            fixed4 shadowColor = _ShadowColor * (1 - outline);
            c.rgb = lerp(c.rgb, shadowColor.rgb, shadowColor.a);
            c.rgb = lerp(c.rgb, outlineColor.rgb, outlineColor.a);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
