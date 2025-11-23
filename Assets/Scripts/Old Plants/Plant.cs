using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Transform GrowingTree;

    public float GrowthSpeedModifier = 1f;

    public float GrowYSpeed;
    public float GrowXZSpeed;

    protected float maxSizeRoll;
    public bool isGrowing { protected set; get; } = true;

    public Branch BranchPF;
    public float OddsOfGrowingBranch = .4f;

    public AnimationCurve OddsOfGrowingPerLifecycle;

    public List<SizeConfiguration> Sizes = new List<SizeConfiguration>();
    private SizeConfiguration chosenSizeTier = null;
    private List<SizeConfiguration> sizeConfigurationIndexes = new List<SizeConfiguration>();

    public LayerMask PlantMask;

    /// <summary>
    /// When determining how far this plant wants another plant to be, it uses its scale multiplied by <see cref="PushRadiusScaleMultiplicative"/> plus this.
    /// </summary>
    public float PushRadiusAdditive = 1f;
    /// <summary>
    /// When determining how far this plant wants another plant to be, it uses its scale multiplied by this plus <see cref="PushRadiusAdditive"/>.
    /// </summary>
    public float PushRadiusScaleMultiplicative = 1.2f;
    public float PushDistancePerSecond = .2f;
    public float PushRadiusTotal
    {
        get
        {
            return this.GrowingTree.localScale.x * this.PushRadiusScaleMultiplicative + this.PushDistancePerSecond;
        }
    }

    float LifecycleStage
    {
        get
        {
            return Mathf.InverseLerp(0, this.maxSizeRoll, GrowingTree.localScale.y);
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
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
    }

    /// <summary>
    /// Returns the smallest scale of all <seealso cref="Sizes"/>.
    /// If there are no elements, returns 1.
    /// </summary>
    public float SmallestScale
    {
        get
        {
            if (this.Sizes?.Count <= 0)
            {
                return 1f;
            }

            float smallestSize = float.MaxValue;

            foreach (SizeConfiguration size in this.Sizes)
            {
                smallestSize = Mathf.Min(size.SizeScaleMin, smallestSize);
            }

            return smallestSize;
        }
    }

    /// <summary>
    /// Returns the largest scale of all <seealso cref="Sizes"/>.
    /// If there are no elements, returns 1.
    /// </summary>
    public float LargestScale
    {
        get
        {
            if (this.Sizes?.Count <= 0)
            {
                return 1f;
            }

            float largestSize = float.MinValue;

            foreach (SizeConfiguration size in this.Sizes)
            {
                largestSize = Mathf.Max(size.SizeScaleMax, largestSize);
            }

            return largestSize;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {

        this.PushyPushie();

        if (!isGrowing)
        {
            return;
        }

        Vector3 newSize = new Vector3(GrowingTree.localScale.x + GrowXZSpeed * Time.deltaTime * GrowthSpeedModifier,
            GrowingTree.localScale.y + GrowYSpeed * Time.deltaTime * GrowthSpeedModifier,
            GrowingTree.localScale.z + GrowXZSpeed * Time.deltaTime * GrowthSpeedModifier);
        GrowingTree.localScale = newSize;
        

        if (newSize.y > maxSizeRoll)
        {
            isGrowing = false;
        }

        this.TryGrowBranch();
    }

    void TryGrowBranch()
    {
        if (Random.Range(0, 1f) >= this.OddsOfGrowingBranch * this.OddsOfGrowingPerLifecycle.Evaluate(this.LifecycleStage))
        {
            return;
        }

        float randomYRotation = Random.Range(0, 360f);
        Branch newBranch = Instantiate(BranchPF, transform.position + Vector3.up * GrowingTree.localScale.y * .2f, Quaternion.Euler(0, randomYRotation, 0), this.transform);
        newBranch.FromSeed = this;
        newBranch.gameObject.SetActive(true);
    }

    void PushyPushie()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, this.PushRadiusTotal, PlantMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in hits)
        {
            Vector3 awayFromMe = (transform.position - collider.transform.position).normalized * this.PushDistancePerSecond * Time.deltaTime;
            collider.transform.position += awayFromMe;
        }
    }
}
