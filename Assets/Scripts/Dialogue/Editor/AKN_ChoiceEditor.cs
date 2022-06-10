using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

[CustomEditor(typeof(AKN_Choice))]
public class AKN_ChoiceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        AKN_Choice currentChoice = (AKN_Choice)target;

        EditorGUILayout.Space();
        using (new GUILayout.HorizontalScope("box"))
        {
            if (GUILayout.Button("Create choice sentence"))
            {
                AKN_Sentence newSentence = CreateInstance<AKN_Sentence>();
                var dir = Directory.GetParent(AssetDatabase.GetAssetPath(target));
                var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(dir + "/New Sentence.asset");

                AssetDatabase.CreateAsset(newSentence, uniqueDir);
                AssetDatabase.SaveAssets();

                currentChoice.choiceSentence = newSentence;

                Selection.activeObject = newSentence;
                EditorUtility.FocusProjectWindow();
            }
        }
    }
}