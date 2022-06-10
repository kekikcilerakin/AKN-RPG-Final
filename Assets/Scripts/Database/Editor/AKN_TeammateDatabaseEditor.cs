using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

[CustomEditor(typeof(AKN_TeammateDatabaseSO))]
public class AKN_TeammateDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        AKN_TeammateDatabaseSO teammateDatabase = (AKN_TeammateDatabaseSO)target;
        

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Teammate"))
        {
            AKN_TeammateSO newTeammate = CreateInstance<AKN_TeammateSO>();
            var teammateDir = "Assets/ScriptableObjects/Characters/Teammate";
            var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(teammateDir + "/AKN_Teammate_.asset");

            AssetDatabase.CreateAsset(newTeammate, uniqueDir);
            AssetDatabase.SaveAssets();

            teammateDatabase.teammates.Add(newTeammate);

            Selection.activeObject = newTeammate;
            EditorUtility.FocusProjectWindow();
        }
    }
}
