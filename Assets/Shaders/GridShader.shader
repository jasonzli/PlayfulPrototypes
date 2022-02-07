// A procedural grid texture 
Shader "Unlit/GridShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1.,1.,1.,1.) 
        _GridColor ("Grid Line Color", Color) = (1.,1.,1.,1.) 
        _GridThickness ("Thickness", Range(0,1.0)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            };

            float4 _BaseColor;
            float4 _GridColor;
            float _GridThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
               
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //get the worldspace coordinate of the grid
                float4 c = _BaseColor;
                float4 worldPos = mul(unity_ObjectToWorld,float4(i.vertex.xyz,1.0));
                float3 frWPos = frac(worldPos.xyz);
                
                return float4(frWPos,1.);
            }
            ENDCG
        }
    }
}
