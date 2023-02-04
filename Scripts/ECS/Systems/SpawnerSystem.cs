using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using GameJam.Events;
using Unity.Entities.UniversalDelegates;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();

        new GetRandomJob
        {
            randomComponent = randomComponent,
            ElapsedTime = SystemAPI.Time.ElapsedTime,
        }.Run();
        

        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);
        JobHandle jobHandle = new ProcessSpawnerJob
        {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            Ecb = ecb
        }.ScheduleParallel(state.Dependency);

        jobHandle.Complete();
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}


//[BurstCompile]
public partial struct ProcessSpawnerJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;
    public double ElapsedTime;

    private void Execute([ChunkIndexInQuery] int chunkIndex, ref SpawnerComponent globalSpawnerEntity)
    {
        if (globalSpawnerEntity.nextSystemTime < ElapsedTime)
        {
            foreach (SpawnersInformation individualSpawner in SpawnersManager.Instance.spawnersInformation)
            {
                if (individualSpawner.react && individualSpawner.nextSpawnTime < ElapsedTime)
                {
                   


                    // Set entity trayectory
                    float3 look = new float3(individualSpawner.position.x, 0, individualSpawner.position.z);


                    int _i = GameManager.Instance._i;
                    int _j = GameManager.Instance._j;
                    float a = 1;

                    for (int j = 0; j < _j; j++) {

                        for (int i = 0; i < _i; i++)
                        {
                            // Spawns a new entity and positions it at the spawner.
                            Entity newEntity = Ecb.Instantiate(chunkIndex, globalSpawnerEntity.Prefab);

                            float random = globalSpawnerEntity.randomValue[i] * a;

                            float3 look2 = math.normalize(look) + (float3)individualSpawner.right *
                                individualSpawner.spawnArc * random;

                            float size = SpawnersManager.Instance.projectileSize;
                            // Set entity position
                            Ecb.SetComponent(chunkIndex, newEntity, new LocalTransform
                            {
                                Position = individualSpawner.position,
                                Rotation = quaternion.LookRotation(math.normalize(look2), new float3(0, 1, 0)),
                                Scale = size
                            });

                            // Set projectile componet
                            Ecb.SetComponent(chunkIndex, newEntity, new ProjectileComponent
                            {
                                velocity = individualSpawner.projectileSpeed,
                                spawnInitTime = ElapsedTime,
                                lifeTime = individualSpawner.projectileLifetime,
                                projectileValue = 5.0f
                            });
                        }

                        a = globalSpawnerEntity.randomValue[j];
                    }


                    

                    // Resets the next spawn time
                    individualSpawner.nextSpawnTime = (float)ElapsedTime + individualSpawner.spawnRate;

                }

            }

            if (globalSpawnerEntity.killAllProjectiles)
            {
                globalSpawnerEntity.killAllProjectiles = false;
            }
            if (SpawnersManager.Instance.killAllProjectiles)
            {
                globalSpawnerEntity.killAllProjectiles = true;
                SpawnersManager.Instance.killAllProjectiles = false;
            }

            globalSpawnerEntity.playerPosition = GameManager.Instance.playerPosition;
            globalSpawnerEntity.playerhitbox = GameManager.Instance.playerHitbox;

            globalSpawnerEntity.nextSystemTime = (float)ElapsedTime + globalSpawnerEntity.systemStepTime;
        }
    }
}



public partial struct GetRandomJob : IJobEntity
{
    [NativeDisableUnsafePtrRestriction] public RefRW<RandomComponent> randomComponent;
    public double ElapsedTime;

    public void Execute(ref SpawnerComponent spawner)
    {
        //if(spawner.systemStepTime < ElapsedTime)
        //{
        for (int i = 0; i < 4; i++)
        {
            spawner.randomValue[i] = randomComponent.ValueRW.random.NextFloat(-1f, 1f);
        }
        //}
    }
}