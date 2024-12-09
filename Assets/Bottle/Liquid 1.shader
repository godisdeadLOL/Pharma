Shader "Unlit/Liquid 1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SurfaceOffset ("Surface offset", Vector) = (0, 0, 0)
        _SurfaceScale ("Surface scale", float) = 1

        _WobbleX ("Wobble X", float) = 0
        _WobbleZ ("Wobble Z", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 world : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 _SurfaceOffset;
            float _SurfaceScale;

            float _WobbleX;
            float _WobbleZ;

            v2f vert (appdata v)
            {
                v2f o;
                o.world = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i, fixed vface : VFACE) : SV_Target
            {
                // uv raycast
                float3 A = _WorldSpaceCameraPos;
                float3 B = i.world;

                float y1 = A.y;
                float y2 = B.y;
                float t = (A.y-_SurfaceOffset.y) / (A.y-B.y);

                float3 intersect = A + (B-A)*t;
            
                float2 uv = vface > 0.0 ? i.uv : (intersect.xz-_SurfaceOffset.xz)*_SurfaceScale;

                float light = vface > 0.0 ? 0.5 : 1.0;

                fixed4 col = tex2D(_MainTex, uv) * light;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);


                float wobble = (i.world.x-_SurfaceOffset.x) * _WobbleX + (i.world.z-_SurfaceOffset.z) * _WobbleZ;
                
                clip( -i.world.y + _SurfaceOffset.y + wobble );

                return col;
            }
            ENDCG
        }
    }
}
