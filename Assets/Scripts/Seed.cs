using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public Transform GrowingTree;

    public float GrowYSpeed;
    public float GrowXZSpeed;
    public float MaxSizeMin;
    public float MaxSizeMax;

    private float maxSizeRoll;
    private bool isGrowing = true;

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

        Vector3 newSize = new Vector3(transform.localScale.x + GrowXZSpeed * Time.deltaTime,
            transform.localScale.y + GrowYSpeed * Time.deltaTime,
            transform.localScale.z + GrowXZSpeed * Time.deltaTime);
        transform.localScale = newSize;
        

        if (newSize.y > maxSizeRoll)
        {
            isGrowing = false;
        }
    }
}
