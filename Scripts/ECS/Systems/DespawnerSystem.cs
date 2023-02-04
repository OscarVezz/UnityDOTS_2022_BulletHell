using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(ProjectileSystem))]
public partial struct DespawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);
        new DestroyJob
        {
            //ElapsedTime = SystemAPI.Time.ElapsedTime,
            Ecb = ecb,
            //globalProjectileComponent = SystemAPI.GetSingleton<GlobalProjectileComponent>()
        }.ScheduleParallel();
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}


//[BurstCompile]
public partial struct DestroyJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;
    //public double ElapsedTime;
    //public GlobalProjectileComponent globalProjectileComponent;

    private void Execute([ChunkIndexInQuery] int chunkIndex, DespawnerComponent despawner, Entity entity)
    {
        
        if (despawner.destroy)
        {
            if (despawner.hitPlayer)
            {
                GameManager.Instance.playerScore += despawner.value * 100;
            }

            Ecb.DestroyEntity(chunkIndex, entity);
        }
    }
}