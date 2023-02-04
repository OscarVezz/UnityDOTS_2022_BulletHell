using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using GameJam.Easing;

public readonly partial struct SimpleProjectileMovementAspect : IAspect
{
    private readonly Entity entity;

    private readonly TransformAspect transformAspect;
    private readonly RefRO<ProjectileComponent> projectile;
    private readonly RefRW<DespawnerComponent> despawnerComponent;

    public void Move(float deltaTime, SpawnerComponent globalProjectileValues, double elapsedTime)
    {
        float progression = ((float)elapsedTime - (float)projectile.ValueRO.spawnInitTime) / projectile.ValueRO.lifeTime;
        float velocity = projectile.ValueRO.velocity;
        velocity *= 1 - progression;

        transformAspect.LocalPosition += transformAspect.Forward * deltaTime * velocity;
        

        if (globalProjectileValues.useGlobalProjectileTurning)
        {
            transformAspect.RotateWorld(quaternion.EulerXYZ(globalProjectileValues.globalTurningDirection * deltaTime));
        }

        if(projectile.ValueRO.spawnInitTime + projectile.ValueRO.lifeTime < elapsedTime)
        {
            despawnerComponent.ValueRW.destroy = true;
        }

        if (globalProjectileValues.killAllProjectiles)
        {
            despawnerComponent.ValueRW.destroy = true;
        }

        float2 position = new float2(transformAspect.WorldPosition.x, transformAspect.WorldPosition.z);
        if (math.distance(position, globalProjectileValues.playerPosition) < globalProjectileValues.playerhitbox)
        {
            despawnerComponent.ValueRW.destroy = true;
            despawnerComponent.ValueRW.hitPlayer = true;
            despawnerComponent.ValueRW.value = (projectile.ValueRO.projectileValue * progression) + (projectile.ValueRO.projectileValue * (1 - progression) * 0.06f);
        }

    }

    
    public float GetRandom(RefRW<RandomComponent> randomComponent)
    {
        return randomComponent.ValueRW.random.NextFloat(0f, 15f);
    }
    
}
