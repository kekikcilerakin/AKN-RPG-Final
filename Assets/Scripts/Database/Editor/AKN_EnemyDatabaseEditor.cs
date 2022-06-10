using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

[CustomEditor(typeof(AKN_EnemyDatabaseSO))]
public class AKN_EnemyDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        AKN_EnemyDatabaseSO enemyDatabase = (AKN_EnemyDatabaseSO)target;
        

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Plain Enemy"))
        {
            AKN_EnemySO newEnemy = CreateInstance<AKN_EnemySO>();
            var enemyDir = "Assets/ScriptableObjects/Characters/Enemy";
            var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(enemyDir + "/AKN_Enemy_Plain_.asset");

            AssetDatabase.CreateAsset(newEnemy, uniqueDir);
            AssetDatabase.SaveAssets();

            enemyDatabase.plainEnemies.Add(newEnemy);

            Selection.activeObject = newEnemy;
            EditorUtility.FocusProjectWindow();
        }

        if (GUILayout.Button("Create Road Enemy"))
        {
            AKN_EnemySO newEnemy = CreateInstance<AKN_EnemySO>();
            var enemyDir = "Assets/ScriptableObjects/Characters/Enemy";
            var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(enemyDir + "/AKN_Enemy_Road_.asset");

            AssetDatabase.CreateAsset(newEnemy, uniqueDir);
            AssetDatabase.SaveAssets();

            enemyDatabase.roadEnemies.Add(newEnemy);

            Selection.activeObject = newEnemy;
            EditorUtility.FocusProjectWindow();
        }

        if (GUILayout.Button("Create Desert Enemy"))
        {
            AKN_EnemySO newEnemy = CreateInstance<AKN_EnemySO>();
            var enemyDir = "Assets/ScriptableObjects/Characters/Enemy";
            var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(enemyDir + "/AKN_Enemy_Desert_.asset");

            AssetDatabase.CreateAsset(newEnemy, uniqueDir);
            AssetDatabase.SaveAssets();

            enemyDatabase.desertEnemies.Add(newEnemy);

            Selection.activeObject = newEnemy;
            EditorUtility.FocusProjectWindow();
        }

        if (GUILayout.Button("Create Forest Enemy"))
        {
            AKN_EnemySO newEnemy = CreateInstance<AKN_EnemySO>();
            var enemyDir = "Assets/ScriptableObjects/Characters/Enemy";
            var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(enemyDir + "/AKN_Enemy_Forest_.asset");

            AssetDatabase.CreateAsset(newEnemy, uniqueDir);
            AssetDatabase.SaveAssets();

            enemyDatabase.forestEnemies.Add(newEnemy);

            Selection.activeObject = newEnemy;
            EditorUtility.FocusProjectWindow();
        }

        if (GUILayout.Button("Create Mountain Enemy"))
        {
            AKN_EnemySO newEnemy = CreateInstance<AKN_EnemySO>();
            var enemyDir = "Assets/ScriptableObjects/Characters/Enemy";
            var uniqueDir = AssetDatabase.GenerateUniqueAssetPath(enemyDir + "/AKN_Enemy_Mountain_.asset");

            AssetDatabase.CreateAsset(newEnemy, uniqueDir);
            AssetDatabase.SaveAssets();

            enemyDatabase.mountainEnemies.Add(newEnemy);

            Selection.activeObject = newEnemy;
            EditorUtility.FocusProjectWindow();
        }
    }
}
