using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Berry : MonoBehaviour
{
    public MeshRenderer MyMeshRenderer;

    public void SetColor(Color toColor)
    {
        this.MyMeshRenderer.material.color = toColor;
    }
}
