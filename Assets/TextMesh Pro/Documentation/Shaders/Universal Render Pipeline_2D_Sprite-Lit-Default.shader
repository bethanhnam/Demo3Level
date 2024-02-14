// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

Shader "Universal Render Pipeline/2D/Sprite-Lit-Default"
{
  Properties
  {
    _MainTex ("Diffuse", 2D) = "white" {}
    _MaskTex ("Mask", 2D) = "white" {}
    _NormalMap ("Normal Map", 2D) = "bump" {}
    [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
    [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
    [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
    [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
      "RenderPipeline" = "UniversalPipeline"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "LIGHTMODE" = "Universal2D"
        "QUEUE" = "Transparent"
        "RenderPipeline" = "UniversalPipeline"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 USE_SHAPE_LIGHT_TYPE_1 USE_SHAPE_LIGHT_TYPE_2 USE_SHAPE_LIGHT_TYPE_3
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      
      
      uniform float4 _ProjectionParams;
      
      uniform float4 unity_MatrixVP[4];
      
      uniform float4 _MainTex_ST;
      
      uniform float2 _ShapeLightBlendFactors0;
      
      uniform float4 _ShapeLightMaskFilter0;
      
      uniform float4 _ShapeLightInvertedFilter0;
      
      uniform float2 _ShapeLightBlendFactors1;
      
      uniform float4 _ShapeLightMaskFilter1;
      
      uniform float4 _ShapeLightInvertedFilter1;
      
      uniform float2 _ShapeLightBlendFactors2;
      
      uniform float4 _ShapeLightMaskFilter2;
      
      uniform float4 _ShapeLightInvertedFilter2;
      
      uniform float2 _ShapeLightBlendFactors3;
      
      uniform float4 _ShapeLightMaskFilter3;
      
      uniform float4 _ShapeLightInvertedFilter3;
      
      uniform float _HDREmulationScale;
      
      uniform float _UseSceneLighting;
      
      uniform float4 _RendererColor;
      
      uniform sampler2D _MainTex;
      
      uniform sampler2D _MaskTex;
      
      uniform sampler2D _ShapeLightTexture0;
      
      uniform sampler2D _ShapeLightTexture1;
      
      uniform sampler2D _ShapeLightTexture2;
      
      uniform sampler2D _ShapeLightTexture3;
      
      
      
      struct appdata_t
      {
          
          float3 vertex : POSITION0;
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
      
      };
      
      
      struct OUT_Data_Vert
      {
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
          
          float2 texcoord1 : TEXCOORD1;
          
          float4 vertex : SV_POSITION;
      
      };
      
      
      struct v2f
      {
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
          
          float2 texcoord1 : TEXCOORD1;
      
      };
      
      
      struct OUT_Data_Frag
      {
          
          float4 color : SV_Target0;
      
      };
      
      
      uniform UnityPerDraw 
          {
          
          #endif
          uniform float4 unity_ObjectToWorld[4];
          
          uniform float4 unity_WorldToObject[4];
          
          uniform float4 unity_LODFade;
          
          uniform float4 unity_WorldTransformParams;
          
          uniform float4 unity_LightData;
          
          uniform float4 unity_LightIndices[2];
          
          uniform float4 unity_ProbesOcclusion;
          
          uniform float4 unity_SpecCube0_HDR;
          
          // uniform float4 unity_LightmapST;
          
          // uniform float4 unity_DynamicLightmapST;
          
          uniform float4 unity_SHAr;
          
          uniform float4 unity_SHAg;
          
          uniform float4 unity_SHAb;
          
          uniform float4 unity_SHBr;
          
          uniform float4 unity_SHBg;
          
          uniform float4 unity_SHBb;
          
          uniform float4 unity_SHC;
          
          #if HLSLCC_ENABLE_UNIFORM_BUFFERS
      };
      
      float4 u_xlat0;
      
      float4 u_xlat1;
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          
          u_xlat0 = in_v.vertex.yyyy * unity_ObjectToWorld[1];
          
          u_xlat0 = unity_ObjectToWorld[0] * in_v.vertex.xxxx + u_xlat0;
          
          u_xlat0 = unity_ObjectToWorld[2] * in_v.vertex.zzzz + u_xlat0;
          
          u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
          
          u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
          
          u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
          
          u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
          
          u_xlat0 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
          
          out_v.vertex = u_xlat0;
          
          u_xlat0.xyz = u_xlat0.xyw / u_xlat0.www;
          
          out_v.color = in_v.color;
          
          out_v.texcoord.xy = in_v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
          
          u_xlat0.y = u_xlat0.y * _ProjectionParams.x;
          
          u_xlat0.xzw = u_xlat0.xzy * float3(0.5, 0.5, 0.5);
          
          u_xlat0.xy = u_xlat0.zz + u_xlat0.xw;
          
          out_v.texcoord1.xy = u_xlat0.xy;
          
          return;
      
      }
      
      
      #define CODE_BLOCK_FRAGMENT
      
      
      
      float4 u_xlat0_d;
      
      float4 u_xlat16_0;
      
      float4 u_xlat16_1;
      
      int u_xlatb1;
      
      float3 u_xlat2;
      
      float4 u_xlat16_3;
      
      float4 u_xlat16_4;
      
      float4 u_xlat16_5;
      
      float4 u_xlat16_6;
      
      float4 u_xlat16_7;
      
      float4 u_xlat16_8;
      
      int u_xlatb29;
      
      float u_xlat16_30;
      
      OUT_Data_Frag frag(v2f in_f)
      {
          
          u_xlat16_0 = texture(_MainTex, in_f.texcoord.xy);
          
          u_xlat0_d = u_xlat16_0 * in_f.color;
          
          #ifdef UNITY_ADRENO_ES3
          u_xlatb1 = (u_xlat0_d.w==0.0);
          
          #else
          u_xlatb1 = u_xlat0_d.w==0.0;
          
          #endif
          if(u_xlatb1)
      {
              discard;
      }
          
          u_xlat16_1 = texture(_MaskTex, in_f.texcoord.xy);
          
          u_xlat16_0 = u_xlat0_d * _RendererColor;
          
          u_xlat2.xyz = texture(_ShapeLightTexture0, in_f.texcoord1.xy).xyz;
          
          u_xlat16_3.x = dot(_ShapeLightMaskFilter0, _ShapeLightMaskFilter0);
          
          #ifdef UNITY_ADRENO_ES3
          u_xlatb29 = (u_xlat16_3.x!=0.0);
          
          #else
          u_xlatb29 = u_xlat16_3.x!=0.0;
          
          #endif
          u_xlat16_3 = (-_ShapeLightInvertedFilter0) + float4(1.0, 1.0, 1.0, 1.0);
          
          u_xlat16_4 = (-u_xlat16_1) + float4(1.0, 1.0, 1.0, 1.0);
          
          u_xlat16_5 = u_xlat16_4 * _ShapeLightInvertedFilter0;
          
          u_xlat16_3 = u_xlat16_3 * u_xlat16_1 + u_xlat16_5;
          
          u_xlat16_3.x = dot(u_xlat16_3, _ShapeLightMaskFilter0);
          
          u_xlat16_3.xyz = u_xlat2.xyz * u_xlat16_3.xxx;
          
          u_xlat16_3.xyz = (int(u_xlatb29)) ? u_xlat16_3.xyz : u_xlat2.xyz;
          
          u_xlat2.xyz = texture(_ShapeLightTexture1, in_f.texcoord1.xy).xyz;
          
          u_xlat16_30 = dot(_ShapeLightMaskFilter1, _ShapeLightMaskFilter1);
          
          #ifdef UNITY_ADRENO_ES3
          u_xlatb29 = (u_xlat16_30!=0.0);
          
          #else
          u_xlatb29 = u_xlat16_30!=0.0;
          
          #endif
          u_xlat16_5 = (-_ShapeLightInvertedFilter1) + float4(1.0, 1.0, 1.0, 1.0);
          
          u_xlat16_6 = u_xlat16_4 * _ShapeLightInvertedFilter1;
          
          u_xlat16_5 = u_xlat16_5 * u_xlat16_1 + u_xlat16_6;
          
          u_xlat16_30 = dot(u_xlat16_5, _ShapeLightMaskFilter1);
          
          u_xlat16_5.xyz = u_xlat2.xyz * float3(u_xlat16_30);
          
          u_xlat16_5.xyz = (int(u_xlatb29)) ? u_xlat16_5.xyz : u_xlat2.xyz;
          
          u_xlat16_6.xyz = u_xlat16_5.xyz * _ShapeLightBlendFactors1.xxx;
          
          u_xlat16_5.xyz = u_xlat16_5.xyz * _ShapeLightBlendFactors1.yyy;
          
          u_xlat2.xyz = texture(_ShapeLightTexture2, in_f.texcoord1.xy).xyz;
          
          u_xlat16_30 = dot(_ShapeLightMaskFilter2, _ShapeLightMaskFilter2);
          
          #ifdef UNITY_ADRENO_ES3
          u_xlatb29 = (u_xlat16_30!=0.0);
          
          #else
          u_xlatb29 = u_xlat16_30!=0.0;
          
          #endif
          u_xlat16_7 = (-_ShapeLightInvertedFilter2) + float4(1.0, 1.0, 1.0, 1.0);
          
          u_xlat16_8 = u_xlat16_4 * _ShapeLightInvertedFilter2;
          
          u_xlat16_7 = u_xlat16_7 * u_xlat16_1 + u_xlat16_8;
          
          u_xlat16_30 = dot(u_xlat16_7, _ShapeLightMaskFilter2);
          
          u_xlat16_7.xyz = u_xlat2.xyz * float3(u_xlat16_30);
          
          u_xlat16_7.xyz = (int(u_xlatb29)) ? u_xlat16_7.xyz : u_xlat2.xyz;
          
          u_xlat2.xyz = texture(_ShapeLightTexture3, in_f.texcoord1.xy).xyz;
          
          u_xlat16_30 = dot(_ShapeLightMaskFilter3, _ShapeLightMaskFilter3);
          
          #ifdef UNITY_ADRENO_ES3
          u_xlatb29 = (u_xlat16_30!=0.0);
          
          #else
          u_xlatb29 = u_xlat16_30!=0.0;
          
          #endif
          u_xlat16_8 = (-_ShapeLightInvertedFilter3) + float4(1.0, 1.0, 1.0, 1.0);
          
          u_xlat16_4 = u_xlat16_4 * _ShapeLightInvertedFilter3;
          
          u_xlat16_1 = u_xlat16_8 * u_xlat16_1 + u_xlat16_4;
          
          u_xlat16_30 = dot(u_xlat16_1, _ShapeLightMaskFilter3);
          
          u_xlat16_4.xyz = u_xlat2.xyz * float3(u_xlat16_30);
          
          u_xlat16_4.xyz = (int(u_xlatb29)) ? u_xlat16_4.xyz : u_xlat2.xyz;
          
          u_xlat16_6.xyz = u_xlat16_3.xyz * _ShapeLightBlendFactors0.xxx + u_xlat16_6.xyz;
          
          u_xlat16_6.xyz = u_xlat16_7.xyz * _ShapeLightBlendFactors2.xxx + u_xlat16_6.xyz;
          
          u_xlat16_6.xyz = u_xlat16_4.xyz * _ShapeLightBlendFactors3.xxx + u_xlat16_6.xyz;
          
          u_xlat16_3.xyz = u_xlat16_3.xyz * _ShapeLightBlendFactors0.yyy + u_xlat16_5.xyz;
          
          u_xlat16_3.xyz = u_xlat16_7.xyz * _ShapeLightBlendFactors2.yyy + u_xlat16_3.xyz;
          
          u_xlat16_3.xyz = u_xlat16_4.xyz * _ShapeLightBlendFactors3.yyy + u_xlat16_3.xyz;
          
          u_xlat16_3.xyz = u_xlat16_0.xyz * u_xlat16_6.xyz + u_xlat16_3.xyz;
          
          u_xlat16_1.xyz = u_xlat16_3.xyz * float3(_HDREmulationScale);
          
          u_xlat16_1.w = u_xlat16_0.w;
          
          u_xlat16_3.x = (-_UseSceneLighting) + 1.0;
          
          u_xlat16_0 = u_xlat16_0 * u_xlat16_3.xxxx;
          
          u_xlat16_0 = u_xlat16_1 * float4(float4(_UseSceneLighting, _UseSceneLighting, _UseSceneLighting, _UseSceneLighting)) + u_xlat16_0;
          
          out_f.color = max(u_xlat16_0, float4(0.0, 0.0, 0.0, 0.0));
          
          return;
      
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "LIGHTMODE" = "NormalsRendering"
        "QUEUE" = "Transparent"
        "RenderPipeline" = "UniversalPipeline"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      
      
      uniform float4 unity_MatrixVP[4];
      
      uniform float4 unity_MatrixV[4];
      
      uniform sampler2D _MainTex;
      
      uniform sampler2D _NormalMap;
      
      
      
      struct appdata_t
      {
          
          float3 vertex : POSITION0;
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
          
          float4 tangent : TANGENT0;
      
      };
      
      
      struct OUT_Data_Vert
      {
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
          
          float3 texcoord1 : TEXCOORD1;
          
          float3 texcoord2 : TEXCOORD2;
          
          float3 texcoord3 : TEXCOORD3;
          
          float4 vertex : SV_POSITION;
      
      };
      
      
      struct v2f
      {
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
          
          float3 texcoord1 : TEXCOORD1;
          
          float3 texcoord2 : TEXCOORD2;
          
          float3 texcoord3 : TEXCOORD3;
      
      };
      
      
      struct OUT_Data_Frag
      {
          
          float4 color : SV_Target0;
      
      };
      
      
      uniform UnityPerDraw 
          {
          
          #endif
          uniform float4 unity_ObjectToWorld[4];
          
          uniform float4 unity_WorldToObject[4];
          
          uniform float4 unity_LODFade;
          
          uniform float4 unity_WorldTransformParams;
          
          uniform float4 unity_LightData;
          
          uniform float4 unity_LightIndices[2];
          
          uniform float4 unity_ProbesOcclusion;
          
          uniform float4 unity_SpecCube0_HDR;
          
          // uniform float4 unity_LightmapST;
          
          // uniform float4 unity_DynamicLightmapST;
          
          uniform float4 unity_SHAr;
          
          uniform float4 unity_SHAg;
          
          uniform float4 unity_SHAb;
          
          uniform float4 unity_SHBr;
          
          uniform float4 unity_SHBg;
          
          uniform float4 unity_SHBb;
          
          uniform float4 unity_SHC;
          
          #if HLSLCC_ENABLE_UNIFORM_BUFFERS
      };
      
      float4 u_xlat0;
      
      float4 u_xlat1;
      
      float3 u_xlat16_2;
      
      float u_xlat9;
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          
          u_xlat0 = in_v.vertex.yyyy * unity_ObjectToWorld[1];
          
          u_xlat0 = unity_ObjectToWorld[0] * in_v.vertex.xxxx + u_xlat0;
          
          u_xlat0 = unity_ObjectToWorld[2] * in_v.vertex.zzzz + u_xlat0;
          
          u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
          
          u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
          
          u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
          
          u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
          
          out_v.vertex = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
          
          out_v.color = in_v.color;
          
          out_v.texcoord.xy = in_v.texcoord.xy;
          
          u_xlat0.x = dot((-unity_ObjectToWorld[2].xyz), (-unity_ObjectToWorld[2].xyz));
          
          u_xlat0.x = max(u_xlat0.x, 1.17549435e-38);
          
          u_xlat0.x = inversesqrt(u_xlat0.x);
          
          u_xlat0.xyz = u_xlat0.xxx * (-unity_ObjectToWorld[2].xyz);
          
          out_v.texcoord1.xyz = u_xlat0.xyz;
          
          u_xlat1.xyz = in_v.tangent.yyy * unity_ObjectToWorld[1].xyz;
          
          u_xlat1.xyz = unity_ObjectToWorld[0].xyz * in_v.tangent.xxx + u_xlat1.xyz;
          
          u_xlat1.xyz = unity_ObjectToWorld[2].xyz * in_v.tangent.zzz + u_xlat1.xyz;
          
          u_xlat9 = dot(u_xlat1.xyz, u_xlat1.xyz);
          
          u_xlat9 = max(u_xlat9, 1.17549435e-38);
          
          u_xlat9 = inversesqrt(u_xlat9);
          
          u_xlat1.xyz = float3(u_xlat9) * u_xlat1.xyz;
          
          out_v.texcoord2.xyz = u_xlat1.xyz;
          
          u_xlat16_2.xyz = u_xlat0.zxy * u_xlat1.yzx;
          
          u_xlat16_2.xyz = u_xlat0.yzx * u_xlat1.zxy + (-u_xlat16_2.xyz);
          
          u_xlat0.xyz = u_xlat16_2.xyz * in_v.tangent.www;
          
          out_v.texcoord3.xyz = u_xlat0.xyz;
          
          return;
      
      }
      
      
      #define CODE_BLOCK_FRAGMENT
      
      
      
      float3 u_xlat0_d;
      
      float3 u_xlat16_0;
      
      float4 u_xlat16_1;
      
      float3 u_xlat16_2_d;
      
      OUT_Data_Frag frag(v2f in_f)
      {
          
          u_xlat16_0.xyz = texture(_NormalMap, in_f.texcoord.xy).xyz;
          
          u_xlat16_1.xyz = u_xlat16_0.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
          
          u_xlat16_2_d.xyz = u_xlat16_1.yyy * in_f.texcoord3.xyz;
          
          u_xlat16_1.xyw = u_xlat16_1.xxx * in_f.texcoord2.xyz + u_xlat16_2_d.xyz;
          
          u_xlat16_1.xyz = u_xlat16_1.zzz * in_f.texcoord1.xyz + u_xlat16_1.xyw;
          
          u_xlat0_d.x = unity_MatrixV[0].x;
          
          u_xlat0_d.y = unity_MatrixV[1].x;
          
          u_xlat0_d.z = unity_MatrixV[2].x;
          
          u_xlat16_2_d.x = dot(u_xlat0_d.xyz, u_xlat16_1.xyz);
          
          u_xlat0_d.x = unity_MatrixV[0].y;
          
          u_xlat0_d.y = unity_MatrixV[1].y;
          
          u_xlat0_d.z = unity_MatrixV[2].y;
          
          u_xlat16_2_d.y = dot(u_xlat0_d.xyz, u_xlat16_1.xyz);
          
          u_xlat0_d.x = unity_MatrixV[0].z;
          
          u_xlat0_d.y = unity_MatrixV[1].z;
          
          u_xlat0_d.z = unity_MatrixV[2].z;
          
          u_xlat16_2_d.z = dot(u_xlat0_d.xyz, u_xlat16_1.xyz);
          
          u_xlat16_1.xyz = u_xlat16_2_d.xyz + float3(1.0, 1.0, 1.0);
          
          out_f.color.xyz = u_xlat16_1.xyz * float3(0.5, 0.5, 0.5);
          
          u_xlat16_0.x = texture(_MainTex, in_f.texcoord.xy).w;
          
          u_xlat0_d.x = u_xlat16_0.x * in_f.color.w;
          
          out_f.color.w = u_xlat0_d.x;
          
          return;
      
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: 
    {
      Tags
      { 
        "LIGHTMODE" = "UniversalForward"
        "QUEUE" = "Transparent"
        "RenderPipeline" = "UniversalPipeline"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      
      
      uniform float4 unity_MatrixVP[4];
      
      uniform sampler2D _MainTex;
      
      
      
      struct appdata_t
      {
          
          float3 vertex : POSITION0;
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
      
      };
      
      
      struct OUT_Data_Vert
      {
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
          
          float4 vertex : SV_POSITION;
      
      };
      
      
      struct v2f
      {
          
          float4 color : COLOR0;
          
          float2 texcoord : TEXCOORD0;
      
      };
      
      
      struct OUT_Data_Frag
      {
          
          float4 color : SV_Target0;
      
      };
      
      
      uniform UnityPerDraw 
          {
          
          #endif
          uniform float4 unity_ObjectToWorld[4];
          
          uniform float4 unity_WorldToObject[4];
          
          uniform float4 unity_LODFade;
          
          uniform float4 unity_WorldTransformParams;
          
          uniform float4 unity_LightData;
          
          uniform float4 unity_LightIndices[2];
          
          uniform float4 unity_ProbesOcclusion;
          
          uniform float4 unity_SpecCube0_HDR;
          
          // uniform float4 unity_LightmapST;
          
          // uniform float4 unity_DynamicLightmapST;
          
          uniform float4 unity_SHAr;
          
          uniform float4 unity_SHAg;
          
          uniform float4 unity_SHAb;
          
          uniform float4 unity_SHBr;
          
          uniform float4 unity_SHBg;
          
          uniform float4 unity_SHBb;
          
          uniform float4 unity_SHC;
          
          #if HLSLCC_ENABLE_UNIFORM_BUFFERS
      };
      
      float4 u_xlat0;
      
      float4 u_xlat1;
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          
          u_xlat0 = in_v.vertex.yyyy * unity_ObjectToWorld[1];
          
          u_xlat0 = unity_ObjectToWorld[0] * in_v.vertex.xxxx + u_xlat0;
          
          u_xlat0 = unity_ObjectToWorld[2] * in_v.vertex.zzzz + u_xlat0;
          
          u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
          
          u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
          
          u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
          
          u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
          
          out_v.vertex = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
          
          out_v.color = in_v.color;
          
          out_v.texcoord.xy = in_v.texcoord.xy;
          
          return;
      
      }
      
      
      #define CODE_BLOCK_FRAGMENT
      
      
      
      float4 u_xlat16_0;
      
      OUT_Data_Frag frag(v2f in_f)
      {
          
          u_xlat16_0 = texture(_MainTex, in_f.texcoord.xy);
          
          out_f.color = u_xlat16_0 * in_f.color;
          
          return;
      
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
