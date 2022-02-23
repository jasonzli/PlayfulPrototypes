Shader "Week 4 Ocean/Sonar Effect"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Metallic("Metallic", Range(0,1)) = 0
        _Smoothness("Smoothness", Range(0,1)) = .5
        _SonarSpeed("Sonar Speed", Float) = 1
        _SonarOrigin("Sonar Origin", Vector) = (0,0,0,0)
        _SonarWidth("Sonar Width", Float) = .1
        _SonarTravel("Sonar Travel", Float) = 0
        _SonarRange("Sonar Range", Float) = 0
        [HDR]_SonarColor("Sonar Color", Color) = (0,0,0,1)
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        fixed4 _Color;
        float4 _SonarOrigin;
        float4 _SonarColor;
        float _SonarRange;
        float _SonarTravel;
        float _SonarWidth;
        float _SonarSpeed;
        half _Smoothness;
        half _Metallic;

        struct Input
        {
            float3 worldPos;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            
        }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;

            float dist = 0;
            dist = distance(_SonarOrigin,IN.worldPos);
            
            _SonarTravel = (_SonarSpeed * _Time) % 100;
            
            float diffFromTravel = abs(_SonarTravel - dist);
            
            float d = smoothstep(_SonarWidth,0,diffFromTravel);
            
            o.Emission = _SonarColor * d;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
