using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

[CustomEditor(typeof(AKN_Sentence))]
public class AKN_SentenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        AKN_Sentence currentSentence = (AKN_Sentence)target;

        EditorGUILayout.Space();
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Create next sentence",GUILayout.Width(200)))
            {
                AKN_Sentence newSentence = CreateInstance<AKN_Sentence>();
                var dir = Directory.GetParent(AssetDatabase.GetAssetPath(target));
                var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(dir + "/New Sentence.asset");

                AssetDatabase.CreateAsset(newSentence, uniqueDir);
                AssetDatabase.SaveAssets();

                currentSentence.nextSentence = newSentence;

                Selection.activeObject = newSentence;
                EditorUtility.FocusProjectWindow();
            }

            if (GUILayout.Button("Create a choice",GUILayout.Width(200)))
            {
                AKN_Choice newChoice = CreateInstance<AKN_Choice>();
                var dir = Directory.GetParent(AssetDatabase.GetAssetPath(target));
                var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(dir + "/New Choice.asset");

                AssetDatabase.CreateAsset(newChoice, uniqueDir);
                AssetDatabase.SaveAssets();

                currentSentence.choices.Add(newChoice);

                Selection.activeObject = newChoice;
                EditorUtility.FocusProjectWindow();
            }
        }
    }
}
