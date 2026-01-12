#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace lilToon
{
    public class MatCapExInspector : lilToonInspector
    {
        // Custom properties
        private static bool isShowMatCap1;
        private MaterialProperty _CustomMatCap1_Enable;
        private MaterialProperty _CustomMatCap1_Color;
        private MaterialProperty _CustomMatCap1_MainColorPower;
        private MaterialProperty _CustomMatCap1_Tex;
        private MaterialProperty _CustomMatCap1_Blend;
        private MaterialProperty _CustomMatCap1_Mask;
        private MaterialProperty _CustomMatCap1_BumpScale;
        private MaterialProperty _CustomMatCap1_UseReflection;
        private MaterialProperty _CustomMatCap1_DisableBackface;
        private MaterialProperty _CustomMatCap1_EnableLighting;
        private MaterialProperty _CustomMatCap1_ShadowStrength;
        private MaterialProperty _CustomMatCap1_Blur;
        private MaterialProperty _CustomMatCap1_Alpha;
        private MaterialProperty _CustomMatCap1_RimPower;

        // Custom properties 2nd
        private static bool isShowMatCap2nd;
        private MaterialProperty _CustomMatCap2nd_Enable;
        private MaterialProperty _CustomMatCap2nd_Color;
        private MaterialProperty _CustomMatCap2nd_MainColorPower;
        private MaterialProperty _CustomMatCap2nd_Tex;
        private MaterialProperty _CustomMatCap2nd_Blend;
        private MaterialProperty _CustomMatCap2nd_Mask;
        private MaterialProperty _CustomMatCap2nd_BumpScale;
        private MaterialProperty _CustomMatCap2nd_UseReflection;
        private MaterialProperty _CustomMatCap2nd_DisableBackface;
        private MaterialProperty _CustomMatCap2nd_EnableLighting;
        private MaterialProperty _CustomMatCap2nd_ShadowStrength;
        private MaterialProperty _CustomMatCap2nd_Blur;
        private MaterialProperty _CustomMatCap2nd_Alpha;
        private MaterialProperty _CustomMatCap2nd_RimPower;

        // Custom properties 3rd
        private static bool isShowMatCap3rd;
        private MaterialProperty _CustomMatCap3rd_Enable;
        private MaterialProperty _CustomMatCap3rd_Color;
        private MaterialProperty _CustomMatCap3rd_MainColorPower;
        private MaterialProperty _CustomMatCap3rd_Tex;
        private MaterialProperty _CustomMatCap3rd_Blend;
        private MaterialProperty _CustomMatCap3rd_Mask;
        private MaterialProperty _CustomMatCap3rd_BumpScale;
        private MaterialProperty _CustomMatCap3rd_UseReflection;
        private MaterialProperty _CustomMatCap3rd_DisableBackface;
        private MaterialProperty _CustomMatCap3rd_EnableLighting;
        private MaterialProperty _CustomMatCap3rd_ShadowStrength;
        private MaterialProperty _CustomMatCap3rd_Blur;
        private MaterialProperty _CustomMatCap3rd_Alpha;
        private MaterialProperty _CustomMatCap3rd_RimPower;

        private static bool isShowCustomProperties;
        private const string shaderName = "dennoko_matcapex";

        protected override void LoadCustomProperties(MaterialProperty[] props, Material material)
        {
            isCustomShader = true;
            ReplaceToCustomShaders();
            isShowRenderMode = !material.shader.name.Contains("Optional");

            _CustomMatCap1_Enable          = FindProperty("_CustomMatCap1_Enable", props);
            _CustomMatCap1_Color           = FindProperty("_CustomMatCap1_Color", props);
            _CustomMatCap1_MainColorPower  = FindProperty("_CustomMatCap1_MainColorPower", props);
            _CustomMatCap1_Tex             = FindProperty("_CustomMatCap1_Tex", props);
            _CustomMatCap1_Blend           = FindProperty("_CustomMatCap1_Blend", props);
            _CustomMatCap1_Mask            = FindProperty("_CustomMatCap1_Mask", props);
            _CustomMatCap1_BumpScale       = FindProperty("_CustomMatCap1_BumpScale", props);
            _CustomMatCap1_UseReflection   = FindProperty("_CustomMatCap1_UseReflection", props);
            _CustomMatCap1_DisableBackface = FindProperty("_CustomMatCap1_DisableBackface", props);
            _CustomMatCap1_EnableLighting  = FindProperty("_CustomMatCap1_EnableLighting", props);
            _CustomMatCap1_ShadowStrength  = FindProperty("_CustomMatCap1_ShadowStrength", props);
            _CustomMatCap1_Blur            = FindProperty("_CustomMatCap1_Blur", props);
            _CustomMatCap1_Alpha           = FindProperty("_CustomMatCap1_Alpha", props);
            _CustomMatCap1_RimPower        = FindProperty("_CustomMatCap1_RimPower", props);

            _CustomMatCap2nd_Enable          = FindProperty("_CustomMatCap2nd_Enable", props);
            _CustomMatCap2nd_Color           = FindProperty("_CustomMatCap2nd_Color", props);
            _CustomMatCap2nd_MainColorPower  = FindProperty("_CustomMatCap2nd_MainColorPower", props);
            _CustomMatCap2nd_Tex             = FindProperty("_CustomMatCap2nd_Tex", props);
            _CustomMatCap2nd_Blend           = FindProperty("_CustomMatCap2nd_Blend", props);
            _CustomMatCap2nd_Mask            = FindProperty("_CustomMatCap2nd_Mask", props);
            _CustomMatCap2nd_BumpScale       = FindProperty("_CustomMatCap2nd_BumpScale", props);
            _CustomMatCap2nd_UseReflection   = FindProperty("_CustomMatCap2nd_UseReflection", props);
            _CustomMatCap2nd_DisableBackface = FindProperty("_CustomMatCap2nd_DisableBackface", props);
            _CustomMatCap2nd_EnableLighting  = FindProperty("_CustomMatCap2nd_EnableLighting", props);
            _CustomMatCap2nd_ShadowStrength  = FindProperty("_CustomMatCap2nd_ShadowStrength", props);
            _CustomMatCap2nd_Blur            = FindProperty("_CustomMatCap2nd_Blur", props);
            _CustomMatCap2nd_Alpha           = FindProperty("_CustomMatCap2nd_Alpha", props);
            _CustomMatCap2nd_RimPower        = FindProperty("_CustomMatCap2nd_RimPower", props);

            _CustomMatCap3rd_Enable          = FindProperty("_CustomMatCap3rd_Enable", props);
            _CustomMatCap3rd_Color           = FindProperty("_CustomMatCap3rd_Color", props);
            _CustomMatCap3rd_MainColorPower  = FindProperty("_CustomMatCap3rd_MainColorPower", props);
            _CustomMatCap3rd_Tex             = FindProperty("_CustomMatCap3rd_Tex", props);
            _CustomMatCap3rd_Blend           = FindProperty("_CustomMatCap3rd_Blend", props);
            _CustomMatCap3rd_Mask            = FindProperty("_CustomMatCap3rd_Mask", props);
            _CustomMatCap3rd_BumpScale       = FindProperty("_CustomMatCap3rd_BumpScale", props);
            _CustomMatCap3rd_UseReflection   = FindProperty("_CustomMatCap3rd_UseReflection", props);
            _CustomMatCap3rd_DisableBackface = FindProperty("_CustomMatCap3rd_DisableBackface", props);
            _CustomMatCap3rd_EnableLighting  = FindProperty("_CustomMatCap3rd_EnableLighting", props);
            _CustomMatCap3rd_ShadowStrength  = FindProperty("_CustomMatCap3rd_ShadowStrength", props);
            _CustomMatCap3rd_Blur            = FindProperty("_CustomMatCap3rd_Blur", props);
            _CustomMatCap3rd_Alpha           = FindProperty("_CustomMatCap3rd_Alpha", props);
            _CustomMatCap3rd_RimPower        = FindProperty("_CustomMatCap3rd_RimPower", props);
        }

        protected override void DrawCustomProperties(Material material)
        {
            EditorGUILayout.BeginVertical(boxOuter);
            
            DrawMatCapSlot("MatCap 1st", ref isShowMatCap1, 
                _CustomMatCap1_Enable, _CustomMatCap1_Color, _CustomMatCap1_MainColorPower, _CustomMatCap1_Tex, 
                _CustomMatCap1_Blend, _CustomMatCap1_Mask, 
                _CustomMatCap1_BumpScale, _CustomMatCap1_UseReflection, _CustomMatCap1_DisableBackface,
                _CustomMatCap1_EnableLighting, _CustomMatCap1_ShadowStrength, 
                _CustomMatCap1_Blur, _CustomMatCap1_Alpha, _CustomMatCap1_RimPower);

            DrawMatCapSlot("MatCap 2nd", ref isShowMatCap2nd, 
                _CustomMatCap2nd_Enable, _CustomMatCap2nd_Color, _CustomMatCap2nd_MainColorPower, _CustomMatCap2nd_Tex, 
                _CustomMatCap2nd_Blend, _CustomMatCap2nd_Mask, 
                _CustomMatCap2nd_BumpScale, _CustomMatCap2nd_UseReflection, _CustomMatCap2nd_DisableBackface,
                _CustomMatCap2nd_EnableLighting, _CustomMatCap2nd_ShadowStrength, 
                _CustomMatCap2nd_Blur, _CustomMatCap2nd_Alpha, _CustomMatCap2nd_RimPower);

            DrawMatCapSlot("MatCap 3rd", ref isShowMatCap3rd, 
                _CustomMatCap3rd_Enable, _CustomMatCap3rd_Color, _CustomMatCap3rd_MainColorPower, _CustomMatCap3rd_Tex, 
                _CustomMatCap3rd_Blend, _CustomMatCap3rd_Mask, 
                _CustomMatCap3rd_BumpScale, _CustomMatCap3rd_UseReflection, _CustomMatCap3rd_DisableBackface,
                _CustomMatCap3rd_EnableLighting, _CustomMatCap3rd_ShadowStrength, 
                _CustomMatCap3rd_Blur, _CustomMatCap3rd_Alpha, _CustomMatCap3rd_RimPower);

            EditorGUILayout.EndVertical();
        }

        private void DrawMatCapSlot(string title, ref bool isShow, 
            MaterialProperty enable, MaterialProperty color, MaterialProperty mainColorPower, MaterialProperty tex,
            MaterialProperty blend, MaterialProperty mask,
            MaterialProperty bumpScale, MaterialProperty reflection, MaterialProperty disableBackface,
            MaterialProperty lighting, MaterialProperty shadow,
            MaterialProperty blur, MaterialProperty alpha, MaterialProperty rimPower)
        {
            isShow = Foldout(title, title, isShow);
            if(!isShow) return;
            EditorGUILayout.BeginVertical(boxOuter);
            EditorGUILayout.LabelField(title, customToggleFont);
            
            // Check for nulls to prevent errors if properties are missing
            if (enable != null && tex != null && blend != null && mask != null && bumpScale != null && reflection != null && disableBackface != null && lighting != null && shadow != null && blur != null && alpha != null && mainColorPower != null && rimPower != null)
            {
                EditorGUILayout.BeginVertical(boxInner);
                
                m_MaterialEditor.ShaderProperty(enable, new GUIContent("Enable", "MatCapを有効にします"));
                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Texture", "MatCapテクスチャを設定します"), tex, color);
                m_MaterialEditor.ShaderProperty(mainColorPower, new GUIContent("Main Color Strength", "メインテクスチャの色をMatCapに乗算する強度。0で無効、1で完全適用"));
                m_MaterialEditor.ShaderProperty(alpha, new GUIContent("Opacity", "MatCapの不透明度"));
                
                // Blend Mode
                EditorGUI.BeginChangeCheck();
                string[] blendModes = { "Add", "Screen", "Multiply", "Overlay", "Soft Light", "Replace", "Subtract", "Lighten", "Darken" };
                int blendMode = (int)blend.floatValue;
                // Clamp index just in case
                if(blendMode < 0 || blendMode >= blendModes.Length) blendMode = 0;
                
                blendMode = EditorGUILayout.Popup(new GUIContent("Blend Mode", "合成モード。Add=加算, Screen=スクリーン, Multiply=乗算, Overlay=オーバーレイ, Soft Light=ソフトライト, Replace=置換, Subtract=減算, Lighten=比較(明), Darken=比較(暗)"), blendMode, blendModes);
                if (EditorGUI.EndChangeCheck()) blend.floatValue = blendMode;
                
                m_MaterialEditor.ShaderProperty(blur, new GUIContent("Blur", "MatCapのぼかし強度。MipMapが必要です"));

                // Mask (Tiling shown)
                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Mask", "MatCapを適用する範囲を制限するマスクテクスチャ。白=適用、黒=非適用"), mask);
                m_MaterialEditor.TextureScaleOffsetProperty(mask);
                
                // Options
                m_MaterialEditor.ShaderProperty(bumpScale, new GUIContent("Normal Strength", "法線マップの影響度。0でフラット、1以上で強調"));
                m_MaterialEditor.ShaderProperty(reflection, new GUIContent("Use Reflection", "視線反射モード。ONで鏡面反射のような挙動になります"));
                m_MaterialEditor.ShaderProperty(disableBackface, new GUIContent("Disable on Backface", "裏面でMatCapを無効にします"));
                m_MaterialEditor.ShaderProperty(lighting, new GUIContent("Enable Lighting", "ライティングの影響を受けるかどうか。0で無効、1で完全適用"));
                m_MaterialEditor.ShaderProperty(shadow, new GUIContent("Shadow Strength", "影部分でのMatCap減衰強度。0で影響なし、1で影部分で完全に消える"));
                m_MaterialEditor.ShaderProperty(rimPower, new GUIContent("Rim Power", "リムマスク（フレネル）。正の値=エッジに適用、負の値=中心に適用、0=無効"));
                
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        protected override void ReplaceToCustomShaders()
        {
            lts         = Shader.Find(shaderName + "/lilToon");
            ltsc        = Shader.Find("Hidden/" + shaderName + "/Cutout");
            ltst        = Shader.Find("Hidden/" + shaderName + "/Transparent");
            ltsot       = Shader.Find("Hidden/" + shaderName + "/OnePassTransparent");
            ltstt       = Shader.Find("Hidden/" + shaderName + "/TwoPassTransparent");

            ltso        = Shader.Find("Hidden/" + shaderName + "/OpaqueOutline");
            ltsco       = Shader.Find("Hidden/" + shaderName + "/CutoutOutline");
            ltsto       = Shader.Find("Hidden/" + shaderName + "/TransparentOutline");
            ltsoto      = Shader.Find("Hidden/" + shaderName + "/OnePassTransparentOutline");
            ltstto      = Shader.Find("Hidden/" + shaderName + "/TwoPassTransparentOutline");

            ltsover     = Shader.Find(shaderName + "/[Optional] Overlay");
            ltsoover    = Shader.Find(shaderName + "/[Optional] OverlayOnePass");
        }

        // You can create a menu like this
        /*
        [MenuItem("Assets/TemplateFull/Convert material to custom shader", false, 1100)]
        private static void ConvertMaterialToCustomShaderMenu()
        {
            if(Selection.objects.Length == 0) return;
            TemplateFullInspector inspector = new TemplateFullInspector();
            for(int i = 0; i < Selection.objects.Length; i++)
            {
                if(Selection.objects[i] is Material)
                {
                    inspector.ConvertMaterialToCustomShader((Material)Selection.objects[i]);
                }
            }
        }
        */
    }
}
#endif