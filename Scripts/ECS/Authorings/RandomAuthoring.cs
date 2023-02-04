using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class RandomAuthoring : MonoBehaviour
{
   
}

public class RandomBaker : Baker<RandomAuthoring>
{
    public override void Bake(RandomAuthoring authoring)
    {
        AddComponent(new RandomComponent
        {
            random = new Unity.Mathematics.Random(1)
        });
    }
}