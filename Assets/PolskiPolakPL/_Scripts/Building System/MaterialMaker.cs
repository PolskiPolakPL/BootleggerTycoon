using UnityEngine;

public class MaterialMaker : MonoBehaviour
{

    public static Material MakePreviewMaterial(Material original, float previewOpacity)
    {
        Material mat = new Material(original);

        mat.shader = Shader.Find("Standard");    // Set "Standard" Shader
        mat.name = "prev-" + original.name;        // Material name
        mat.SetFloat("_Mode", 3f);               // Rendering Mode: Transparent
        mat.renderQueue = 3000;                  // Render Queue
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");

        Color color = mat.color;
        color.a = previewOpacity;
        mat.color = color;

        return mat;
    }
}
