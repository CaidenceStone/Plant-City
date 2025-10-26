using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public Seed FromSeed;

    public Transform GrowingBranch;
    public float GrowXYSpeed;
    public float GrowZSpeed;
    public float MaxSizeMin;
    public float MaxSizeMax;
    private float maxSizeRoll;
    private bool isGrowing = true;

    public Vector3 BaseScaleSetting = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {

        this.maxSizeRoll = Random.Range(this.MaxSizeMin, this.MaxSizeMax);
        this.transform.localScale = BaseScaleSetting;
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

        Vector3 newSize = new Vector3(transform.localScale.x + GrowXYSpeed * this.BaseScaleSetting.x * Time.deltaTime * this.FromSeed.GrowthSpeedModifier,
            transform.localScale.y + GrowXYSpeed * this.BaseScaleSetting.y * Time.deltaTime * this.FromSeed.GrowthSpeedModifier,
            transform.localScale.z + GrowZSpeed * this.BaseScaleSetting.z * Time.deltaTime * this.FromSeed.GrowthSpeedModifier);
        transform.localScale = newSize;


        if (newSize.z > maxSizeRoll)
        {
            isGrowing = false;
        }
    }
}
