using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGeneration))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGeneration map = (MapGeneration)target;

        if (DrawDefaultInspector())
        {
            if (map.GetAutoUpdate())
            {
                map.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            map.GenerateMap();
        }

        base.OnInspectorGUI();
    }

}
