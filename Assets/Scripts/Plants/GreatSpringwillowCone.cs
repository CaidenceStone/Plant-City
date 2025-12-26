using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSpringwillowCone : MonoBehaviour
{
    public Transform NextSpringwillowParent;
    public MeshRenderer Renderer;

    public void SetColor(Color toSet)
    {
        this.Renderer.material.color = toSet;
    }
}
