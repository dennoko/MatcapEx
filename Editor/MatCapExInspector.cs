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
        }

        protected override void DrawCustomProperties(Material material)
        {
            isShowCustomProperties = Foldout("Custom MatCap Properties", "Custom MatCap Properties", isShowCustomProperties);
            if(isShowCustomProperties)
            {
                EditorGUILayout.BeginVertical(boxOuter);
                EditorGUILayout.LabelField(GetLoc("Custom MatCap Properties"), customToggleFont);
                
                DrawMatCapSlot("MatCap 1", ref isShowMatCap1, 
                    _CustomMatCap1_Enable, _CustomMatCap1_Color, _CustomMatCap1_MainColorPower, _CustomMatCap1_Tex, 
                    _CustomMatCap1_Blend, _CustomMatCap1_Mask, 
                    _CustomMatCap1_BumpScale, _CustomMatCap1_UseReflection, _CustomMatCap1_DisableBackface,
                    _CustomMatCap1_EnableLighting, _CustomMatCap1_ShadowStrength, 
                    _CustomMatCap1_Blur, _CustomMatCap1_Alpha);

                DrawMatCapSlot("MatCap 2nd", ref isShowMatCap2nd, 
                    _CustomMatCap2nd_Enable, _CustomMatCap2nd_Color, _CustomMatCap2nd_MainColorPower, _CustomMatCap2nd_Tex, 
                    _CustomMatCap2nd_Blend, _CustomMatCap2nd_Mask, 
                    _CustomMatCap2nd_BumpScale, _CustomMatCap2nd_UseReflection, _CustomMatCap2nd_DisableBackface,
                    _CustomMatCap2nd_EnableLighting, _CustomMatCap2nd_ShadowStrength, 
                    _CustomMatCap2nd_Blur, _CustomMatCap2nd_Alpha);

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawMatCapSlot(string title, ref bool isShow, 
            MaterialProperty enable, MaterialProperty color, MaterialProperty mainColorPower, MaterialProperty tex,
            MaterialProperty blend, MaterialProperty mask,
            MaterialProperty bumpScale, MaterialProperty reflection, MaterialProperty disableBackface,
            MaterialProperty lighting, MaterialProperty shadow,
            MaterialProperty blur, MaterialProperty alpha)
        {
            isShow = Foldout(title, title, isShow);
            if(!isShow) return;
            EditorGUILayout.BeginVertical(boxOuter);
            EditorGUILayout.LabelField(title, customToggleFont);
            
            // Check for nulls to prevent errors if properties are missing
            if (enable != null && tex != null && blend != null && mask != null && bumpScale != null && reflection != null && disableBackface != null && lighting != null && shadow != null && blur != null && alpha != null && mainColorPower != null)
            {
                EditorGUILayout.BeginVertical(boxInner);
                
                m_MaterialEditor.ShaderProperty(enable, new GUIContent("Enable"));
                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Texture"), tex, color);
                m_MaterialEditor.ShaderProperty(mainColorPower, new GUIContent("Main Color Strength")); // Added this line
                m_MaterialEditor.ShaderProperty(alpha, new GUIContent("Opacity"));
                
                // Blend Mode
                EditorGUI.BeginChangeCheck();
                string[] blendModes = { "Add", "Screen", "Multiply" };
                int blendMode = (int)blend.floatValue;
                // Clamp index just in case
                if(blendMode < 0 || blendMode >= blendModes.Length) blendMode = 0;
                
                blendMode = EditorGUILayout.Popup(new GUIContent("Blend Mode"), blendMode, blendModes);
                if (EditorGUI.EndChangeCheck()) blend.floatValue = blendMode;
                
                m_MaterialEditor.ShaderProperty(blur, new GUIContent("Blur"));

                // Mask (Tiling shown)
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