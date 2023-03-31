using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMask : TMP_Text
{
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

    public override Material materialForRendering
    {
        get
        {
            var mat = new Material(base.materialForRendering);
            mat.SetInt(StencilComp, (int) CompareFunction.NotEqual);
            return mat;
        }
    }
}
