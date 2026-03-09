using UnityEngine;

public class RTBaker : MonoBehaviour
{
    [SerializeField] RenderTexture targetRT;
    public void Bake()
    {
        if (!targetRT)
            return;
        Texture2D bakeTexture = new Texture2D(targetRT.width,targetRT.height, TextureFormat.ARGB32,false);
        RenderTexture.active = targetRT;
        bakeTexture.ReadPixels(new Rect(0,0,targetRT.width,targetRT.height), 0, 0);
        bakeTexture.Apply();
        string exportPath = $"{Application.dataPath}/PolskiPolakPL/Utilities/RT Baker/RT_Export.png";
        byte[] bytes = bakeTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(exportPath, bytes);
        Debug.Log("BAKE FINISHED!");
    }
}
