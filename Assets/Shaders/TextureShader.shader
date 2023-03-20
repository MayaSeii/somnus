Shader "Legacy Shaders/Diffuse - Worldspace"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Scale("Texture Scale", Float) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _Color;
        float _Scale;

        struct Input
        {
            float3 worldNormal;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 uv;
            fixed4 c;

            if (abs(IN.worldNormal.x) > 0.5)
            {
                uv = IN.worldPos.yz;
                c = tex2D(_MainTex, uv * _Scale);
            }
            else if (abs(IN.worldNormal.z) > 0.5)
            {
                uv = IN.worldPos.xy;
                c = tex2D(_MainTex, uv * _Scale);
            }
            else
            {
                uv = IN.worldPos.xz;
                c = tex2D(_MainTex, uv * _Scale);
            }

            o.Albedo = c.rgb * _Color;
        }
        
        ENDCG
    }

    Fallback "Legacy Shaders/VertexLit"
}
