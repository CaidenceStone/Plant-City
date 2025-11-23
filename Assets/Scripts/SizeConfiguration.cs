using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SizeConfiguration
{
    /// <summary>
    /// When this <see cref="SizeConfiguration"/> is chosen, the plant that selects it
    /// will grow to be at least this scale at maximum growth.
    /// </summary>
    public float SizeScaleMin = 1;


    /// <summary>
    /// When this <see cref="SizeConfiguration"/> is chosen, the plant that selects it
    /// will grow to be no more than this scale at maximum growth.
    /// </summary>
    public float SizeScaleMax = 1;

    /// <summary>
    /// Provides a curve evaluation to the minimum to maximum
    /// size choice when using this as a size tier.
    /// 0 is SizeScaleMin, 1 is SizeScaleMax. Use a Linear distribution
    /// for most simple use.
    /// </summary>
    public AnimationCurve SizeProbabilityDistributionCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public int LikelihoodTickets = 1;

    public float RandomScale()
    {
        float rand = Random.Range(0, 1f);
        return Mathf.Lerp(this.SizeScaleMin, this.SizeScaleMax, this.SizeProbabilityDistributionCurve.Evaluate(rand));
    }
}
