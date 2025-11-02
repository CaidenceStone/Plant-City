using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringwillowCone : MonoBehaviour
{
    public Transform NextSpringwillowConeParent;
    public GameObject Model;

    public float SpeedGrowthMultiplier = 1f;

    public float CurrentScale { get; private set; } = 1f;

    public float TargetMaxSize { get; set; } = 5f;
    public bool IsGrowing { get; set; } = true;

    public AnimationCurve XZOverTime;
    public AnimationCurve YOverTime;

    /// <summary>
    /// The percentage position point (index / number of target cones to grow) this is
    /// </summary>
    public float ConeGrownPercentage { get; set; }
    public AnimationCurve OffsetPercentageCurve;

    public void TryGrow(float time)
    {
        if (!this.IsGrowing)
        {
            return;
        }

        float targetScale = Mathf.Min(this.TargetMaxSize, this.CurrentScale + (time * this.SpeedGrowthMultiplier));
        SetSizeScale(targetScale);

        if (targetScale >= this.TargetMaxSize)
        {
            this.IsGrowing = false;
        }
    }

    public void SetSizeScale(float scale)
    {
        this.CurrentScale = scale;
        this.Model.transform.localScale = new Vector3(XZOverTime.Evaluate(scale / this.TargetMaxSize),
            YOverTime.Evaluate(scale / this.TargetMaxSize),
            XZOverTime.Evaluate(scale / this.TargetMaxSize))
            * scale;
        this.NextSpringwillowConeParent.localPosition = (Vector3.up + Vector3.up * this.OffsetPercentageCurve.Evaluate(this.ConeGrownPercentage)) * scale;
    }

    public void SetNextRotation(float amount)
    {
        this.NextSpringwillowConeParent.transform.localRotation = Quaternion.Euler(amount, 0, 0);
    }
}
