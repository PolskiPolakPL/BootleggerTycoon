using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(RTBaker))]
public class CustomBakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RTBaker RTBakerScript = (RTBaker)target;
        DrawDefaultInspector();
        if(GUILayout.Button("Bake Render Texture"))
            RTBakerScript.Bake();
    }
}
