Shader "Custom/LiquidSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _SurfaceOffset ("Surface offset", Vector) = (0, 0, 0)
        _SurfaceScale ("Surface scale", float) = 1

        _WobbleX ("Wobble X", float) = 0
        _WobbleZ ("Wobble Z", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float3 worldPos;
            float2 uv_MainTex;
            float vface : VFACE;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        fixed3 _SurfaceOffset;
        fixed _SurfaceScale;

        fixed _WobbleX;
        fixed _WobbleZ;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // uv raycast
            float3 A = _WorldSpaceCameraPos;
            float3 B = IN.worldPos;

            float y1 = A.y;
            float y2 = B.y;
            float t = (A.y-_SurfaceOffset.y) / (A.y-B.y);

            float3 intersect = A + (B-A)*t;
        
            float2 uv = IN.vface > 0.0 ? IN.uv_MainTex : (intersect.xz-_SurfaceOffset.xz)*_SurfaceScale;

            if (IN.vface < 0.0) o.Normal = float3(0, 1, 0);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, uv) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            float wobble = (IN.worldPos.x-_SurfaceOffset.x) * _WobbleX + (IN.worldPos.z-_SurfaceOffset.z) * _WobbleZ;
            clip( -IN.worldPos.y + _SurfaceOffset.y + wobble );
        }
        ENDCG
    }
    FallBack "Diffuse"
}
