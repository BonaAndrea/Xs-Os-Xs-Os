using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManagerModel))]
public class GameManagerModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManagerModel gameManager = (GameManagerModel)target;

        foreach (int[] matrix in gameManager.matrixes)
        {
            EditorGUILayout.LabelField("Matrix:");
            EditorGUI.indentLevel++;

            if (matrix != null)
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    EditorGUILayout.IntField("Element " + i, matrix[i]);
                }
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }
    }
}