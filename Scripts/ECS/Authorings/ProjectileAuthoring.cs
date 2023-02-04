using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class ProjectileAuthoring : MonoBehaviour
{
    public float velocity;

    public double spawnInitTime;
    public float lifeTime;

    public float projectileValue;
}

class ProjectileBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        AddComponent(new ProjectileComponent
        {
            velocity = authoring.velocity,

            spawnInitTime = authoring.spawnInitTime,
            lifeTime = authoring.lifeTime,

            projectileValue = authoring.projectileValue
        });
    }
}