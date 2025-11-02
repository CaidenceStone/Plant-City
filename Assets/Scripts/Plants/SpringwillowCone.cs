using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringwillowCone : MonoBehaviour
{
    public Transform NextSpringwillowConeParent;
    public GameObject Model;
    /// <summary>
    /// The height of a cone before it starts growing.
    /// </summary>
    public float InitialConeHeight = 2f;

    public float SpeedGrowthMultiplier = 1f;

    public float CurrentScale { get; private set; } = 1f;

    public float TargetMaxSize { get; set; } = 5f;
    public bool IsGrowing { get; set; } = true;

    public void TryGrow(float time)
    {
        float targetScale = Mathf.Min(this.TargetMaxSize, this.CurrentScale + (time * this.SpeedGrowthMultiplier));
        SetSizeScale(targetScale);
    }

    public void SetSizeScale(float scale)
    {
        this.CurrentScale = scale;
        this.Model.transform.localScale = this.Model.transform.localScale.normalized * this.CurrentScale;
        this.NextSpringwillowConeParent.localPosition = Vector3.up * ((scale / 2f) - this.InitialConeHeight);
    }

    public void SetNextRotation(float amount)
    {
        this.NextSpringwillowConeParent.transform.localRotation = Quaternion.Euler(amount, 0, 0);
    }
}
