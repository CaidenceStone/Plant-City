using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class GreatSpringwillow : GreatPlant
{
    private List<GreatSpringwillowCone> cones { get; set; } = new List<GreatSpringwillowCone>();
    public GreatSpringwillowCone ConePF;

    public float DistanceForTouchSpot = 5f;
    public Vector3? CurveToTouch { get; set; }
    public LayerMask GroundMask;

    public AnimationCurve HeightAtDistancePercent;
    public float DistanceToHeightRatio = .4f;

    public AnimationCurve CurlPercentAtDistancePercent;
    public float MaxRotation = 360f;

    public AnimationCurve OddsOfConeGrowthCount;
    public int MinimumConesToSpawn = 5;
    public int MaximumConesToSpawn = 15;

    protected float maxSizeRoll;
    public List<SizeConfiguration> Sizes = new List<SizeConfiguration>();
    private SizeConfiguration chosenSizeTier = null;
    private List<SizeConfiguration> sizeConfigurationIndexes = new List<SizeConfiguration>();

    private void Start()
    {
        if (this.Sizes?.Count == 0)
        {
            // There are no size tiers configured; use the most simple fallback
            this.chosenSizeTier = new();
        }
        else
        {
            sizeConfigurationIndexes = new List<SizeConfiguration>();
            foreach (SizeConfiguration config in this.Sizes)
            {
                sizeConfigurationIndexes.AddRange(Enumerable.Repeat<SizeConfiguration>(config, config.LikelihoodTickets));
            }

            this.chosenSizeTier = this.sizeConfigurationIndexes[Random.Range(0, this.sizeConfigurationIndexes.Count)];
            this.sizeConfigurationIndexes.Clear();
        }

        this.maxSizeRoll = this.chosenSizeTier.RandomScale();
        transform.localScale = Vector3.one * this.maxSizeRoll;

        this.GrowToNumberOfCones(Mathf.RoundToInt(Mathf.Lerp(MinimumConesToSpawn, MaximumConesToSpawn, OddsOfConeGrowthCount.Evaluate(UnityEngine.Random.Range(0, 1f)))));

        // Pick a random nearby location and try to "look down" at it
        // If the randomly chosen position doesn't work, then for now, just give up! No bending for this Springwillow
        Vector3 randomPosition = transform.position + new Vector3(Random.Range(-DistanceForTouchSpot, DistanceForTouchSpot), 0, Random.Range(-DistanceForTouchSpot, DistanceForTouchSpot));
        RaycastHit hit;
        if (Physics.Raycast(randomPosition + Vector3.up * 10f, Vector3.down, out hit, float.MaxValue, GroundMask))
        {
            this.CurveToTouch = hit.point;
            this.ReachTowardsTouchSpot();
        }
    }

    public void GrowToNumberOfCones(int numberOfCones)
    {
        for (int ii = cones.Count; ii < numberOfCones; ii++)
        {
            Transform parent = this.transform;

            if (ii > 0)
            {
                parent = cones[ii - 1].NextSpringwillowParent;
            }

            GreatSpringwillowCone newCone = Instantiate(ConePF, parent);
            cones.Add(newCone);
        }
    }

    public void ReachTowardsTouchSpot()
    {
        transform.rotation = Quaternion.LookRotation(CurveToTouch.Value - transform.position, Vector3.up);

        // Take the distance between here and the destination, and use that determine a maximum reaching height
        float distanceToDestination = Vector3.Distance(transform.position, CurveToTouch.Value);
        float maxHeight = distanceToDestination * DistanceToHeightRatio;
        Vector3 lastPosition = transform.position;
        float previousRotation = 0;

        for (int ii = 0; ii < cones.Count; ii++)
        {
            float percentileOfPosition = ((float)ii) / (float)(cones.Count);

            /*
            Vector3 midPoint = Vector3.Lerp(transform.position, CurveToTouch.Value, percentileOfPosition);
            midPoint += Vector3.up * HeightAtDistancePercent.Evaluate(percentileOfPosition) * maxHeight;
            cones[ii].transform.position = midPoint;
            */

            float rotation = CurlPercentAtDistancePercent.Evaluate(percentileOfPosition) * MaxRotation;
            cones[ii].transform.localRotation = Quaternion.Euler(0, 0, rotation - previousRotation);
            previousRotation = rotation;

            /*

            CurlPercentAtDistancePercent

            Vector3 normalizedDistanceFromPrevious = (midPoint - lastPosition).normalized;
            cones[ii].transform.rotation = Quaternion.LookRotation(normalizedDistanceFromPrevious, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);

            lastPosition = midPoint;
            */
        }
    }
}
