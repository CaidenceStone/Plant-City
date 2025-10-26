using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public Transform GrowingTree;

    public float GrowthSpeedModifier = 1f;

    public float GrowYSpeed;
    public float GrowXZSpeed;
    public float MaxSizeMin;
    public float MaxSizeMax;

    private float maxSizeRoll;
    private bool isGrowing = true;

    public Branch BranchPF;
    public float OddsOfGrowingBranch = .4f;

    public AnimationCurve OddsOfGrowingPerLifecycle;
    float LifecycleStage
    {
        get
        {
            return Mathf.InverseLerp(0, this.maxSizeRoll, GrowingTree.localScale.y);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.maxSizeRoll = Random.Range(this.MaxSizeMin, this.MaxSizeMax);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
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
}
