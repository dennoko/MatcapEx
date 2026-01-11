MatCapEx 拡張シェーダー実装ガイド
このドキュメントは、MatCap機能に特化した拡張シェーダー「MatCapEx」を作成するための手順書です。 現在の「MatCap 1」の実装（反射モード、裏面無効化、マスク調整機能付き）をベースに、将来的にスロット数を増やせる設計で実装する方法を解説します。

1. プロジェクトの準備
既存の拡張シェーダーフォルダ（例: 
Specular
）を複製し、フォルダ名を MatCapEx に変更します。 中身のファイルも整理し、以下の構成にします。

MatCapEx/
Shaders/
custom.hlsl
lilCustomShaderProperties.lilblock
... (その他のlilToon関連ファイル)
Editor/
CustomInspector.cs
 (クラス名変更が必要)
2. シェーダー名の変更 (CustomInspector.cs)
Editor/CustomInspector.cs
 を開き、シェーダー名を変更します。これがUnity上のシェーダー選択メニューになります。

// クラス名を変更 (ファイル名と一致させる)
public class MatCapExInspector : lilToonInspector
{
    // シェーダー名を定義 (例: Hidden/dennoko_matcapex/...)
    // ※ ここで指定した名前がシェーダーパスのベースになります
    private const string shaderName = "dennoko_matcapex";
    
    // ...
}
3. プロパティの定義 (lilCustomShaderProperties.lilblock)
Shaders/lilCustomShaderProperties.lilblock
 を編集します。 不要なSpecular関連のプロパティを削除し、MatCap用のプロパティを定義します。 スロットを増やす際は、変数名の数字（MatCap1 -> MatCap2）を変更して複製してください。

//----------------------------------------------------------------------------------------------------------------------
// Custom MatCap 1
//----------------------------------------------------------------------------------------------------------------------
[lilToggle]     _CustomMatCap1_Enable         ("Enable MatCap 1", Float) = 1
[lilHDR]        _CustomMatCap1_Color          ("Color", Color) = (1,1,1,1)
                _CustomMatCap1_Tex            ("MatCap Texture", 2D) = "white" {}
[lilEnum]       _CustomMatCap1_Blend          ("Blend Mode|Add|Screen|Multiply", Int) = 2
                _CustomMatCap1_Mask           ("Mask", 2D) = "white" {}
                _CustomMatCap1_BumpScale      ("Normal Strength", Range(0,3)) = 1.0
[lilToggle]     _CustomMatCap1_UseReflection  ("Use Reflection", Int) = 0
[lilToggle]     _CustomMatCap1_DisableBackface("Disable on Backface", Int) = 0
                _CustomMatCap1_EnableLighting ("Enable Lighting", Range(0,1)) = 1.0
                _CustomMatCap1_ShadowStrength ("Shadow Strength", Range(0,1)) = 0.0
                _CustomMatCap1_Blur           ("Blur", Range(0,1)) = 0.0
                _CustomMatCap1_Alpha          ("Opacity", Range(0,1)) = 1.0
// スロット2を追加する場合、上記ブロックをコピーし「MatCap1」を「MatCap2」に置換してください。
4. HLSLロジックの実装 (custom.hlsl)
Shaders/custom.hlsl
 を編集します。

4.1 プロパティ宣言マクロ (LIL_CUSTOM_PROPERTIES)
lilblock
 で定義したプロパティに対応する変数を宣言します。 UVのTiling/Offset用変数（_ST）も忘れずに定義します。

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
    float4 _CustomMatCap1_Tex_ST; \
    float4 _CustomMatCap1_Mask_ST; \
    /* MatCap 2... (以下同様) */
4.2 テクスチャ宣言マクロ (LIL_CUSTOM_TEXTURES)
#define LIL_CUSTOM_TEXTURES \
    TEXTURE2D(_CustomMatCap1_Tex); \
    TEXTURE2D(_CustomMatCap1_Mask); \
    /* MatCap 2... */
4.3 処理の実装 (BEFORE_MATCAP)
MatCap処理をマクロ化しておくと、スロット増設時に便利です。 以下は現在の最新仕様（Reflection, Backface, Mask Tiling対応）をマクロ化したものです。

// MatCap サンプリング補助マクロ
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
        \
        /* Masking */ \
        /* _ST is applied here assuming DNKW_SAMPLE macro exists or use LIL_SAMPLE_2D with calc */ \
        float2 maskUV##idx = uvMain * _CustomMatCap##idx##_Mask_ST.xy + _CustomMatCap##idx##_Mask_ST.zw; \
        float mask##idx = LIL_SAMPLE_2D(_CustomMatCap##idx##_Mask, sampler_linear_repeat, maskUV##idx).r; \
        \
        /* Backface Disable */ \
        if (_CustomMatCap##idx##_DisableBackface && fd.facing < 0) mask##idx = 0.0; \
        mask##idx *= saturate(_CustomMatCap##idx##_Alpha); \
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
        \
        fd.col.rgb = lerp(fd.col.rgb, targetColor, mask##idx); \
    }
// 本体
#if !defined(UNITY_PASS_SHADOWCASTER)
#define BEFORE_MATCAP \
{ \
    float2 uvMain = fd.uvMain; \
    DNKW_MATCAP_LOGIC(1) \
    /* DNKW_MATCAP_LOGIC(2)  <-- スロットを増やす場合はここに追加 */ \
    /* DNKW_MATCAP_LOGIC(3) */ \
}
#else
#define BEFORE_MATCAP
#endif
5. UIの実装 (CustomInspector.cs)
Editor/CustomInspector.cs
 を編集します。

5.1 変数の宣言と読み込み
スロットごとに変数を宣言し、
LoadCustomProperties
 で読み込みます。

// 変数宣言
private bool isShowMatCap1;
private MaterialProperty _CustomMatCap1_Enable;
// ... 他のプロパティ
protected override void LoadCustomProperties(MaterialProperty[] props, Material material)
{
    // ...
    _CustomMatCap1_Enable = FindProperty("_CustomMatCap1_Enable", props);
    // ... 他のプロパティ
}
5.2 描画処理 (DrawCustomProperties)
パラメータが増えるとコードが肥大化するため、個別の描画関数を作るか、ループ処理のような構造にすると管理しやすくなりますが、単純には以下のように並べます。

protected override void DrawCustomProperties(Material material)
{
    isShowCustomProperties = Foldout("Custom MatCap Properties", "Custom MatCap Properties", isShowCustomProperties);
    if(isShowCustomProperties)
    {
        EditorGUILayout.BeginVertical(boxOuter);
        
        // MatCap 1
        DrawMatCapSlot("MatCap 1", ref isShowMatCap1, 
            _CustomMatCap1_Enable, _CustomMatCap1_Color, _CustomMatCap1_Tex, 
            _CustomMatCap1_Blend, _CustomMatCap1_Mask, 
            _CustomMatCap1_BumpScale, _CustomMatCap1_UseReflection, _CustomMatCap1_DisableBackface,
            _CustomMatCap1_EnableLighting, _CustomMatCap1_ShadowStrength, 
            _CustomMatCap1_Blur, _CustomMatCap1_Alpha);
        // MatCap 2があれば同様に呼び出す
        
        EditorGUILayout.EndVertical();
    }
}
UI描画ヘルパー関数例:

private void DrawMatCapSlot(string title, ref bool isShow, 
    MaterialProperty enable, MaterialProperty color, MaterialProperty tex,
    MaterialProperty blend, MaterialProperty mask,
    MaterialProperty bumpScale, MaterialProperty reflection, MaterialProperty disableBackface,
    MaterialProperty lighting, MaterialProperty shadow,
    MaterialProperty blur, MaterialProperty alpha)
{
    isShow = Foldout(title, title, isShow);
    if(!isShow) return;
    EditorGUILayout.BeginVertical(boxOuter);
    EditorGUILayout.LabelField(title, customToggleFont);
    
    // 必須プロパティのNullチェックを行う
    if (enable != null && tex != null /* ... */)
    {
        EditorGUILayout.BeginVertical(boxInner);
        
        m_MaterialEditor.ShaderProperty(enable, new GUIContent("Enable"));
        m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Texture"), tex, color);
        m_MaterialEditor.ShaderProperty(alpha, new GUIContent("Strength"));
        
        // MatCapテクスチャのTilingは非表示
        
        // Blend Mode
        EditorGUI.BeginChangeCheck();
        string[] blendModes = { "Add", "Screen", "Multiply" };
        int blendMode = (int)blend.floatValue;
        blendMode = EditorGUILayout.Popup(new GUIContent("Blend Mode"), blendMode, blendModes);
        if (EditorGUI.EndChangeCheck()) blend.floatValue = blendMode;
        m_MaterialEditor.ShaderProperty(blur, new GUIContent("Blur"));
        // Mask (Tiling表示あり)
        m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Mask"), mask);
        m_MaterialEditor.TextureScaleOffsetProperty(mask);
        // Options
        m_MaterialEditor.ShaderProperty(bumpScale, new GUIContent("Normal Strength"));
        m_MaterialEditor.ShaderProperty(reflection, new GUIContent("Use Reflection"));
        m_MaterialEditor.ShaderProperty(disableBackface, new GUIContent("Disable on Backface"));
        m_MaterialEditor.ShaderProperty(lighting, new GUIContent("Enable Lighting"));
        m_MaterialEditor.ShaderProperty(shadow, new GUIContent("Shadow Strength"));
        EditorGUILayout.EndVertical();
    }
    EditorGUILayout.EndVertical();
}
これで、MatCapEx として独立したシェーダーを作成し、スロットの増設も容易に行えるようになります。