//----------------------------------------------------------------------------------------------------------------------
// Macro

// Custom variables
#define LIL_CUSTOM_PROPERTIES \
    /* MatCap 1 */ \
    float _CustomMatCap1_Enable; \
    float4 _CustomMatCap1_Color; \
    int _CustomMatCap1_Blend; \
    float _CustomMatCap1_BumpScale; \
    int _CustomMatCap1_UseReflection; \
    int _CustomMatCap1_DisableBackface; \
    float _CustomMatCap1_EnableLighting; \
    float _CustomMatCap1_ShadowStrength; \
    float _CustomMatCap1_Blur; \
    float _CustomMatCap1_Alpha; \
    float _CustomMatCap1_MainColorPower; \
    float _CustomMatCap1_RimPower; \
    float4 _CustomMatCap1_Tex_ST; \
    float4 _CustomMatCap1_Mask_ST; \
    /* MatCap 2nd */ \
    float _CustomMatCap2nd_Enable; \
    float4 _CustomMatCap2nd_Color; \
    int _CustomMatCap2nd_Blend; \
    float _CustomMatCap2nd_BumpScale; \
    int _CustomMatCap2nd_UseReflection; \
    int _CustomMatCap2nd_DisableBackface; \
    float _CustomMatCap2nd_EnableLighting; \
    float _CustomMatCap2nd_ShadowStrength; \
    float _CustomMatCap2nd_Blur; \
    float _CustomMatCap2nd_Alpha; \
    float _CustomMatCap2nd_MainColorPower; \
    float _CustomMatCap2nd_RimPower; \
    float4 _CustomMatCap2nd_Tex_ST; \
    float4 _CustomMatCap2nd_Mask_ST; \
    /* MatCap 3rd */ \
    float _CustomMatCap3rd_Enable; \
    float4 _CustomMatCap3rd_Color; \
    int _CustomMatCap3rd_Blend; \
    float _CustomMatCap3rd_BumpScale; \
    int _CustomMatCap3rd_UseReflection; \
    int _CustomMatCap3rd_DisableBackface; \
    float _CustomMatCap3rd_EnableLighting; \
    float _CustomMatCap3rd_ShadowStrength; \
    float _CustomMatCap3rd_Blur; \
    float _CustomMatCap3rd_Alpha; \
    float _CustomMatCap3rd_MainColorPower; \
    float _CustomMatCap3rd_RimPower; \
    float4 _CustomMatCap3rd_Tex_ST; \
    float4 _CustomMatCap3rd_Mask_ST;

// Custom textures
#define LIL_CUSTOM_TEXTURES \
    TEXTURE2D(_CustomMatCap1_Tex); \
    TEXTURE2D(_CustomMatCap1_Mask); \
    TEXTURE2D(_CustomMatCap2nd_Tex); \
    TEXTURE2D(_CustomMatCap2nd_Mask); \
    TEXTURE2D(_CustomMatCap3rd_Tex); \
    TEXTURE2D(_CustomMatCap3rd_Mask);

// Add vertex shader input
//#define LIL_REQUIRE_APP_POSITION
//#define LIL_REQUIRE_APP_TEXCOORD0
//#define LIL_REQUIRE_APP_TEXCOORD1
//#define LIL_REQUIRE_APP_TEXCOORD2
//#define LIL_REQUIRE_APP_TEXCOORD3
//#define LIL_REQUIRE_APP_TEXCOORD4
//#define LIL_REQUIRE_APP_TEXCOORD5
//#define LIL_REQUIRE_APP_TEXCOORD6
//#define LIL_REQUIRE_APP_TEXCOORD7
//#define LIL_REQUIRE_APP_COLOR
//#define LIL_REQUIRE_APP_NORMAL
//#define LIL_REQUIRE_APP_TANGENT
//#define LIL_REQUIRE_APP_VERTEXID

// Add vertex shader output
//#define LIL_V2F_FORCE_TEXCOORD0
//#define LIL_V2F_FORCE_TEXCOORD1
//#define LIL_V2F_FORCE_POSITION_OS
//#define LIL_V2F_FORCE_POSITION_WS
//#define LIL_V2F_FORCE_POSITION_SS
//#define LIL_V2F_FORCE_NORMAL
//#define LIL_V2F_FORCE_TANGENT
//#define LIL_V2F_FORCE_BITANGENT
//#define LIL_CUSTOM_V2F_MEMBER(id0,id1,id2,id3,id4,id5,id6,id7)

// Add vertex copy
#define LIL_CUSTOM_VERT_COPY

// Inserting a process into the vertex shader
//#define LIL_CUSTOM_VERTEX_OS
//#define LIL_CUSTOM_VERTEX_WS

// Inserting a process into pixel shader
// MatCap Logic Macro
#define DNKW_MATCAP_LOGIC(idx) \
    if (_CustomMatCap##idx##_Enable > 0.5) { \
        /* Normal Calculation */ \
        float3 N_mc = normalize(fd.N); \
        float bumpScale##idx = _CustomMatCap##idx##_BumpScale; \
        float3 N_orig = normalize(fd.origN); \
        float3 N_main = normalize(fd.N); \
        N_mc = normalize(lerp(N_orig, N_main, bumpScale##idx)); \
        \
        /* Reflection Mode Check */ \
        if (_CustomMatCap##idx##_UseReflection > 0.5) { \
            N_mc = reflect(-fd.V, N_mc); \
        } \
        \
        /* View Space Conversion */ \
        float3 N_vs = mul((float3x3)UNITY_MATRIX_V, N_mc); \
        N_vs.z *= -1.0; \
        float2 uv_mc = N_vs.xy * 0.5 + 0.5; \
        \
        /* Sampling */ \
        float4 mcTex = LIL_SAMPLE_2D_LOD(_CustomMatCap##idx##_Tex, sampler_linear_clamp, uv_mc, _CustomMatCap##idx##_Blur * 8.0); \
        float3 mcColor = mcTex.rgb * _CustomMatCap##idx##_Color.rgb; \
        mcColor *= lerp(1.0, fd.albedo, _CustomMatCap##idx##_MainColorPower); \
        \
        /* Masking */ \
        float2 maskUV##idx = uvMain * _CustomMatCap##idx##_Mask_ST.xy + _CustomMatCap##idx##_Mask_ST.zw; \
        float mask##idx = LIL_SAMPLE_2D(_CustomMatCap##idx##_Mask, sampler_linear_repeat, maskUV##idx).r; \
        \
        /* Backface Disable */ \
        if (_CustomMatCap##idx##_DisableBackface && fd.facing < 0) mask##idx = 0.0; \
        mask##idx *= saturate(_CustomMatCap##idx##_Alpha); \
        \
        /* Rim Mask (Fresnel) */ \
        float rimPower##idx = _CustomMatCap##idx##_RimPower; \
        if (abs(rimPower##idx) > 0.01) { \
            float NdotV##idx = saturate(dot(fd.N, fd.V)); \
            float fresnel##idx; \
            if (rimPower##idx > 0.0) { \
                fresnel##idx = pow(1.0 - NdotV##idx, rimPower##idx); \
            } else { \
                fresnel##idx = 1.0 - pow(1.0 - NdotV##idx, -rimPower##idx); \
            } \
            mask##idx *= fresnel##idx; \
        } \
        \
        /* Lighting Integration */ \
        float shadowFac = lerp(1.0, fd.attenuation * fd.shadowmix, _CustomMatCap##idx##_ShadowStrength); \
        float3 lightFac = lerp(float3(1,1,1), fd.lightColor, _CustomMatCap##idx##_EnableLighting); \
        mcColor *= shadowFac * lightFac; \
        \
        /* Blending */ \
        float3 targetColor = fd.col.rgb; \
        int blend##idx = _CustomMatCap##idx##_Blend; \
        if (blend##idx == 0) targetColor += mcColor; /* Add */ \
        else if (blend##idx == 1) targetColor = 1.0 - (1.0 - targetColor) * (1.0 - mcColor); /* Screen */ \
        else if (blend##idx == 2) targetColor *= mcColor; /* Multiply */ \
        else if (blend##idx == 3) { /* Overlay */ \
            float3 ovl; \
            ovl.r = (targetColor.r < 0.5) ? (2.0 * targetColor.r * mcColor.r) : (1.0 - 2.0 * (1.0 - targetColor.r) * (1.0 - mcColor.r)); \
            ovl.g = (targetColor.g < 0.5) ? (2.0 * targetColor.g * mcColor.g) : (1.0 - 2.0 * (1.0 - targetColor.g) * (1.0 - mcColor.g)); \
            ovl.b = (targetColor.b < 0.5) ? (2.0 * targetColor.b * mcColor.b) : (1.0 - 2.0 * (1.0 - targetColor.b) * (1.0 - mcColor.b)); \
            targetColor = ovl; \
        } \
        else if (blend##idx == 4) { /* Soft Light */ \
            targetColor = (1.0 - 2.0 * mcColor) * targetColor * targetColor + 2.0 * mcColor * targetColor; \
        } \
        else if (blend##idx == 5) targetColor = mcColor; /* Replace */ \
        else if (blend##idx == 6) targetColor -= mcColor; /* Subtract */ \
        else if (blend##idx == 7) targetColor = max(targetColor, mcColor); /* Lighten */ \
        else if (blend##idx == 8) targetColor = min(targetColor, mcColor); /* Darken */ \
        \
        fd.col.rgb = lerp(fd.col.rgb, targetColor, mask##idx); \
    }

#if !defined(UNITY_PASS_SHADOWCASTER)
#define BEFORE_MATCAP \
{ \
    float2 uvMain = fd.uvMain; \
    DNKW_MATCAP_LOGIC(1) \
    DNKW_MATCAP_LOGIC(2nd) \
    DNKW_MATCAP_LOGIC(3rd) \
}
#else
#define BEFORE_MATCAP
#endif

//----------------------------------------------------------------------------------------------------------------------
// Information about variables
//----------------------------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------------------------
// Vertex shader inputs (appdata structure)
//
// Type     Name                    Description
// -------- ----------------------- --------------------------------------------------------------------
// float4   input.positionOS        POSITION
// float2   input.uv0               TEXCOORD0
// float2   input.uv1               TEXCOORD1
// float2   input.uv2               TEXCOORD2
// float2   input.uv3               TEXCOORD3
// float2   input.uv4               TEXCOORD4
// float2   input.uv5               TEXCOORD5
// float2   input.uv6               TEXCOORD6
// float2   input.uv7               TEXCOORD7
// float4   input.color             COLOR
// float3   input.normalOS          NORMAL
// float4   input.tangentOS         TANGENT
// uint     vertexID                SV_VertexID

//----------------------------------------------------------------------------------------------------------------------
// Vertex shader outputs or pixel shader inputs (v2f structure)
//
// The structure depends on the pass.
// Please check lil_pass_xx.hlsl for details.
//
// Type     Name                    Description
// -------- ----------------------- --------------------------------------------------------------------
// float4   output.positionCS       SV_POSITION
// float2   output.uv01             TEXCOORD0 TEXCOORD1
// float2   output.uv23             TEXCOORD2 TEXCOORD3
// float3   output.positionOS       object space position
// float3   output.positionWS       world space position
// float3   output.normalWS         world space normal
// float4   output.tangentWS        world space tangent

//----------------------------------------------------------------------------------------------------------------------
// Variables commonly used in the forward pass
//
// These are members of `lilFragData fd`
//
// Type     Name                    Description
// -------- ----------------------- --------------------------------------------------------------------
// float4   col                     lit color
// float3   albedo                  unlit color
// float3   emissionColor           color of emission
// -------- ----------------------- --------------------------------------------------------------------
// float3   lightColor              color of light
// float3   indLightColor           color of indirectional light
// float3   addLightColor           color of additional light
// float    attenuation             attenuation of light
// float3   invLighting             saturate((1.0 - lightColor) * sqrt(lightColor));
// -------- ----------------------- --------------------------------------------------------------------
// float2   uv0                     TEXCOORD0
// float2   uv1                     TEXCOORD1
// float2   uv2                     TEXCOORD2
// float2   uv3                     TEXCOORD3
// float2   uvMain                  Main UV
// float2   uvMat                   MatCap UV
// float2   uvRim                   Rim Light UV
// float2   uvPanorama              Panorama UV
// float2   uvScn                   Screen UV
// bool     isRightHand             input.tangentWS.w > 0.0;
// -------- ----------------------- --------------------------------------------------------------------
// float3   positionOS              object space position
// float3   positionWS              world space position
// float4   positionCS              clip space position
// float4   positionSS              screen space position
// float    depth                   distance from camera
// -------- ----------------------- --------------------------------------------------------------------
// float3x3 TBN                     tangent / bitangent / normal matrix
// float3   T                       tangent direction
// float3   B                       bitangent direction
// float3   N                       normal direction
// float3   V                       view direction
// float3   L                       light direction
// float3   origN                   normal direction without normal map
// float3   origL                   light direction without sh light
// float3   headV                   middle view direction of 2 cameras
// float3   reflectionN             normal direction for reflection
// float3   matcapN                 normal direction for reflection for MatCap
// float3   matcap2ndN              normal direction for reflection for MatCap 2nd
// float    facing                  VFACE
// -------- ----------------------- --------------------------------------------------------------------
// float    vl                      dot(viewDirection, lightDirection);
// float    hl                      dot(headDirection, lightDirection);
// float    ln                      dot(lightDirection, normalDirection);
// float    nv                      saturate(dot(normalDirection, viewDirection));
// float    nvabs                   abs(dot(normalDirection, viewDirection));
// -------- ----------------------- --------------------------------------------------------------------
// float4   triMask                 TriMask (for lite version)
// float3   parallaxViewDirection   mul(tbnWS, viewDirection);
// float2   parallaxOffset          parallaxViewDirection.xy / (parallaxViewDirection.z+0.5);
// float    anisotropy              strength of anisotropy
// float    smoothness              smoothness
// float    roughness               roughness
// float    perceptualRoughness     perceptual roughness
// float    shadowmix               this variable is 0 in the shadow area
// float    audioLinkValue          volume acquired by AudioLink
// -------- ----------------------- --------------------------------------------------------------------
// uint     renderingLayers         light layer of object (for URP / HDRP)
// uint     featureFlags            feature flags (for HDRP)
// uint2    tileIndex               tile index (for HDRP)