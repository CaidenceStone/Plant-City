using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chemberry : Plant
{
    public List<Berry> Berries;

    protected override void Start()
    {
        base.Start();

        this.UpdateBerries();
    }

    protected override void Update()
    {
        base.Update();

        this.UpdateBerries();
    }

    void UpdateBerries()
    {
        float smallestScale = this.SmallestScale;
        float largestScale = this.LargestScale;
        float percentageOfSizeRange = Mathf.InverseLerp(smallestScale, largestScale, this.GrowingTree.localScale.y);

        foreach (Berry berry in Berries)
        {
            // Berry, do something!
            Color berryColor = Color.HSVToRGB(percentageOfSizeRange, 1f, 1f);
            berry.SetColor(berryColor);
        }
    }
}
