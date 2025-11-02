using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Springwillow : Plant
{
    public SpringwillowCone ConePF;

    private float CurNextConeTime = 0;
    public float TimeBetweenCones = 1f;
    public int ConesToGrow = 8;
    /// <summary>
    /// Every child cone has this percentage of their parents maximum size.
    /// </summary>
    public float SizeFalloffLerpValue = .8f;
    /// <summary>
    /// Every child grows at this percent the speed of its parent.
    /// </summary>
    public float GrowthSpeedFalloffLerpValue = .8f;

    private List<SpringwillowCone> cones = new List<SpringwillowCone>();

    private int currentlyGrowingIndex { get; set; } = 0;

    public float BaseSpeedGrowthMultiplier = 5f;

    public float MinRotationAmount = 10f;
    public float MaxRotationAmount = 30f;
    public float ConeRotationAmount { get; set; } = 20f;

    protected override void Start()
    {
        this.ConeRotationAmount = Random.Range(this.MinRotationAmount, this.MaxRotationAmount);
        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (!isGrowing)
        {
            return;
        }

        this.HandleSpawnNewCone();
        this.HandleGrowCones();
    }

    private void HandleSpawnNewCone()
    {
        if (this.cones.Count > this.ConesToGrow)
        {
            return;
        }

        this.CurNextConeTime -= Time.deltaTime * this.BaseSpeedGrowthMultiplier;

        if (this.CurNextConeTime > 0)
        {
            return;
        }

        this.CurNextConeTime = this.TimeBetweenCones;

        SpringwillowCone newCone;

        // If there are any cones existing, take the most recently spawned one. This should place the new cone at its nose.
        // Otherwise, this must be the first cone.
        if (this.cones.Count > 0)
        {
            SpringwillowCone previousCone = this.cones[this.cones.Count - 1];
            newCone = Instantiate<SpringwillowCone>(this.ConePF, previousCone.NextSpringwillowConeParent, false);

            newCone.TargetMaxSize = previousCone.TargetMaxSize * this.SizeFalloffLerpValue;
            newCone.SpeedGrowthMultiplier = previousCone.SpeedGrowthMultiplier * this.GrowthSpeedFalloffLerpValue;
        }
        else
        {
            newCone = Instantiate<SpringwillowCone>(this.ConePF, this.transform, false);
            newCone.TargetMaxSize = maxSizeRoll;
            newCone.SpeedGrowthMultiplier = BaseSpeedGrowthMultiplier;
        }
        newCone.SetNextRotation(this.ConeRotationAmount);
        newCone.gameObject.SetActive(true);
        cones.Add(newCone);
    }

    private void HandleGrowCones()
    {
        foreach (SpringwillowCone cone in this.cones)
        {
            if (!cone.IsGrowing)
            {
                continue;
            }

            cone.TryGrow(Time.deltaTime * this.BaseSpeedGrowthMultiplier);
        }

        /*
        if (this.currentlyGrowingIndex >= this.cones.Count)
        {
            if (this.cones.Count >= this.ConesToGrow)
            {
                this.isGrowing = false;
            }
            return;
        }

        SpringwillowCone curCone = this.cones[this.currentlyGrowingIndex];
        curCone.SetSizeScale(curCone.CurrentScale + Time.deltaTime);

        if (curCone.CurrentScale >= this.maxSizeRoll)
        {
            this.currentlyGrowingIndex++;
        }
        */
    }
}
