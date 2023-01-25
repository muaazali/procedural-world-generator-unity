using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldGenerator worldGenerator = (WorldGenerator) target;

        if (GUILayout.Button("Generate World")) {
            worldGenerator.GenerateWorld();
        }
    }
}
