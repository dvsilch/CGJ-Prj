Shader "Custom/Outline"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _MainColor("MainColor", Color) = (1, 1, 1, 1)
        _ShadowColor("ShadowColor", Color) = (0, 0, 0, 1)
        _ShadowRange("ShadowRange", Range(0, 1)) = 0.5
        _ShadowSmooth("ShadowSmooth", Range(0, 1)) = 0.5
        [Enum(Off, 0.0, Front, 1.0, Back, 2.0)]_Cull("Cull", Float) = 2

        _RampTex("RampTex", 2D) = "white" {}

        [Toggle(_UseOutline)]_UseOutline("Use Outline", Float) = 1
        _OutlineWidth("Outline Width", Range(0.0, 4.0)) = 1
        _OutlineColor("Outline Color", Color) = (0.5, 0.5, 0.5, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"

            "RenderType" = "Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            Cull [_Cull]
            ZWrite On
            ZTest LEqual
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            sampler2D _MainTex;
            sampler2D _RampTex;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainColor;
                float4 _MainTex_ST;
                float4 _RampTex_ST;
                float4 _ShadowColor;
                float _ShadowRange;
                float _ShadowSmooth;
            CBUFFER_END

            struct a2v
            {
                float3 positionOS : POSITION;
                half3 normalOS : NORMAL;
                half4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                //float3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS);
                //VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normalOS, v.tangentOS);
                o.position = vertexInput.positionCS;
                //o.normalWS = vertexNormalInput.normalWS;
                o.positionWS = vertexInput.positionWS;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                 return tex2D(_MainTex, i.uv) * _MainColor;
                //half4 color = 1;
                //half4 mainTex = tex2D(_MainTex, i.uv);
                //// half3 viewDir = normalize(GetCameraPositionWS() - i.positionWS);
                //Light mainLight = GetMainLight();
                //half3 worldLightDir = mainLight.direction;
                //half halfLambert = dot(worldLightDir, i.normalWS) * 0.5 + 0.5;
                //half ramp = smoothstep(0, _ShadowSmooth, halfLambert - _ShadowRange);
                //// half ramp = tex2D(_RampTex, float2(saturate(halfLambert - _ShadowRange), 0.5)).r;
                //half3 diffuse = lerp(_ShadowColor, _MainColor, ramp);
                //diffuse *= mainTex;
                //color.rgb = diffuse * mainLight.color;

                //// int lightCount = GetAdditionalLightsCount();
                //// for (int index = 0; index < lightCount; index++)
                //// {
                ////     Light light = GetAdditionalLight(index, i.positionWS);
                ////     worldLightDir = light.direction;
                ////     halfLambert = dot(worldLightDir, i.normalWS) * 0.5 + 0.5;
                ////     ramp = smoothstep(0, _ShadowSmooth, halfLambert - _ShadowRange);
                ////     // half ramp = tex2D(_RampTex, float2(saturate(halfLambert - _ShadowRange), 0.5)).r;
                ////     diffuse = lerp(_ShadowColor, _MainColor, ramp);
                ////     diffuse *= mainTex;
                ////     color.rgb += diffuse * light.color;
                //// }

                //return i.position;
            }

            ENDHLSL
        }

        //// This pass is used when drawing to a _CameraNormalsTexture texture
        //Pass
        //{
        //    Name "DepthNormals"
        //    Tags{"LightMode" = "DepthNormals"}

        //    ZWrite On
        //    Cull Off

        //    HLSLPROGRAM
        //    #pragma exclude_renderers gles gles3 glcore
        //    #pragma target 4.5

        //    #pragma vertex DepthNormalsVertex
        //    #pragma fragment DepthNormalsFragment

        //    // -------------------------------------
        //    // Material Keywords
        //    #pragma shader_feature_local _NORMALMAP
        //    #pragma shader_feature_local _PARALLAXMAP
        //    #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
        //    #pragma shader_feature_local_fragment _ALPHATEST_ON
        //    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

        //    // -------------------------------------
        //    // Unity defined keywords
        //    #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
        //    // Universal Pipeline keywords
        //    #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS

        //    //--------------------------------------
        //    // GPU Instancing
        //    #pragma multi_compile_instancing
        //    #pragma multi_compile _ DOTS_INSTANCING_ON

        //    #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
        //    #include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
        //    ENDHLSL
        //}

        //Pass
        //{
        //    Name "Outline"

        //    Cull Front

        //    HLSLPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag

        //    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        //    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        //    CBUFFER_START(UnityPerMaterial)
        //        float _UseOutline;
        //        float4 _OutlineColor;
        //        float _OutlineWidth;
        //        float _CurrentCameraFOV;
        //    CBUFFER_END

        //    struct Attributes
        //    {
        //        float3 positionOS : POSITION;
        //        half3 normalOS : NORMAL;
        //        half4 tangentOS : TANGENT;
        //    };

        //    // If your project has a faster way to get camera fov in shader, you can replace this slow function to your method.
        //    // For example, you write cmd.SetGlobalFloat("_CurrentCameraFOV",cameraFOV) using a new RendererFeature in C#.
        //    // For this tutorial shader, we will keep things simple and use this slower but convenient method to get camera fov
        //    float GetCameraFOV()
        //    {
        //        return _CurrentCameraFOV;
        //        // https://answers.unity.com/questions/770838/how-can-i-extract-the-fov-information-from-the-pro.html
        //        float t = unity_CameraProjection._m11;
        //        float Rad2Deg = 57.29578;
        //        float fov = 2.0 * atan(1.0 / t) * Rad2Deg;
        //        return fov;
        //    }

        //    float ApplyOutlineDistanceFadeOut(float inputMulFix)
        //    {
        //        // make outline "fadeout" if character is too small in camera's view
        //        return saturate(inputMulFix);
        //    }

        //    float GetOutlineCameraFovAndDistanceFixMultiplier(float positionVS_Z)
        //    {
        //        float cameraMulFix;
        //        if (unity_OrthoParams.w == 0)
        //        {
        //            ////////////////////////////////
        //            // Perspective camera case
        //            ////////////////////////////////

        //            // keep outline similar width on screen accoss all camera distance  
        //            cameraMulFix = abs(positionVS_Z);

        //            // can replace to a tonemap function if a smooth stop is needed
        //            cameraMulFix = ApplyOutlineDistanceFadeOut(cameraMulFix);

        //            // keep outline similar width on screen accoss all camera fov
        //            cameraMulFix *= GetCameraFOV();
        //        }
        //        else
        //        {
        //            ////////////////////////////////
        //            // Orthographic camera case
        //            ////////////////////////////////
        //            float orthoSize = abs(unity_OrthoParams.y);
        //            orthoSize = ApplyOutlineDistanceFadeOut(orthoSize);
        //            cameraMulFix = orthoSize * 50; // 50 is a magic number to match perspective camera's outline width
        //        }

        //        return cameraMulFix * 0.00005; // mul a const to make return result = default normal expand amount WS
        //    }

        //    float4 vert(Attributes input) : SV_POSITION
        //    {
        //        VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);

        //        VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

        //        float outlineExpandAmount = _OutlineWidth * GetOutlineCameraFovAndDistanceFixMultiplier(vertexInput.positionVS.z);

        //        float3 positionWS = vertexInput.positionWS + vertexNormalInput.normalWS * outlineExpandAmount;

        //        return TransformWorldToHClip(positionWS);
        //    }

        //    float4 frag() : SV_Target
        //    {
        //        return _OutlineColor;
        //    }

        //    ENDHLSL
        //}
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
