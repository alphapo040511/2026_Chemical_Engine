using UnityEditor;
using UnityEngine;


// ChemicalObject의 커스텀 인스퍽테 코드입니다.
[CustomEditor(typeof(ChemicalObject))]
public class ChemicalObjectEditor : Editor
{
    private SerializedProperty materialType;

    private SerializedProperty canBurn;
    private SerializedProperty canGetWet;
    private SerializedProperty canFreeze;
    private SerializedProperty canConductElectricity;

    private bool showProperties = true;


    private void OnEnable()
    {
        materialType = serializedObject.FindProperty("materialType");

        canBurn = serializedObject.FindProperty("canBurn");
        canGetWet = serializedObject.FindProperty("canGetWet");
        canFreeze = serializedObject.FindProperty("canFreeze");
        canConductElectricity = serializedObject.FindProperty("canConductElectricity");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Material Type
        EditorGUILayout.PropertyField(
            materialType,
            new GUIContent("Material Preset")
        );

        EditorGUILayout.Space(5);

        // Foldout
        showProperties = EditorGUILayout.Foldout(
            showProperties,
            "속성 설정",
            true
        );

        if (showProperties)
        {
            EditorGUI.indentLevel++;

            bool isCustom =
                materialType.enumValueIndex ==
                (int)ChemicalMaterialType.Custom;

            // Custom이 아니면 읽기 전용
            EditorGUI.BeginDisabledGroup(!isCustom);

            EditorGUILayout.PropertyField(
                canBurn,
                new GUIContent("Can Burn")
            );

            EditorGUILayout.PropertyField(
                canGetWet,
                new GUIContent("Can Get Wet")
            );

            EditorGUILayout.PropertyField(
                canFreeze,
                new GUIContent("Can Freeze")
            );

            EditorGUILayout.PropertyField(
                canConductElectricity,
                new GUIContent("Can Conduct Electricity")
            );

            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}