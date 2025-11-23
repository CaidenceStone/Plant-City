using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlantGrower : MonoBehaviour
{
    public List<Plant> PlantsToGrow = new List<Plant>();
    public int NumberOfPlantsToGrow = 100;
    public float RadiusToGrowIn = 100;
    public LayerMask GroundMask;

    // Start is called before the first frame update
    void Start()
    {
        for (int ii = 0; ii < this.NumberOfPlantsToGrow; ii++)
        {
            Vector3 randomPosition = Random.onUnitSphere;
            randomPosition.y = 0;
            randomPosition *= this.RadiusToGrowIn;

            RaycastHit hit;
            if (!Physics.Raycast(randomPosition + Vector3.up * 100000f, Vector3.down, out hit, float.MaxValue, GroundMask))
            {
                continue;
            }

            Plant toGrow = this.PlantsToGrow[Random.Range(0, this.PlantsToGrow.Count)];

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            rotation *= Quaternion.Euler(Vector3.up * Random.Range(0, 360f));
            Instantiate(toGrow, hit.point, rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
