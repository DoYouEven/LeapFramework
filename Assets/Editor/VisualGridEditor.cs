using UnityEngine;
using UnityEditor;

/// <summary>
/// Do not touch this class
/// </summary>
[CustomEditor(typeof(VisualGrid))]
public class VisualGridEditor : Editor
{

    void OnSceneGUI()
    {

        VisualGrid Target = (VisualGrid)target;
        GUIStyle style = new GUIStyle();
        // Use this for initialization
        style.normal.textColor = Color.black;
        style.alignment = TextAnchor.MiddleCenter;

        Vector3 posAdjust = new Vector3((Target.TileSize2D.x * 0.15f), -Target.TileSize2D.x * 0.15f, 0);
        for (int x = 0; x < Target.grid.slotCount; x++)
        {
            
                Handles.Label(Target[x, 0] - posAdjust, "X: " + 0 + "\nY: " + x, style);
            
            }
        
    }
}

