using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


    [BurstCompile]
    public partial struct LifetimeSystemJob : IJobEntity
    {
        public float deltaTime;
        public float2 respawnRange; // respawnRange.x pour la portée en X, respawnRange.y pour la portée en Y
        public void Execute(ref LifetimeComponent lifetime, ref Translation translation)
        {
            lifetime.lifetime -= deltaTime * lifetime.decreasingFactor;
            if (lifetime.lifetime <= 0f)
            {
                if (lifetime.reproduced || lifetime.alwaysReproduce)
                {
                    // Réinitialisation de la durée de vie et repositionnement aléatoire
                    lifetime.lifetime = lifetime.startingLifetime;
                    translation.Value = new float3(
                        UnityEngine.Random.Range(-respawnRange.x, respawnRange.x),
                        UnityEngine.Random.Range(-respawnRange.y, respawnRange.y),
                        0f);
                    lifetime.reproduced = false;
                }
                else
                {
                    // On déplace l'entité hors de la zone visible
                    translation.Value = new float3(9999f, 9999f, 9999f);
                }
                // Réinitialisation du facteur de diminution
                lifetime.decreasingFactor = 1f;
            }
        }
    }

    public partial class LifetimeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            // Par exemple, respawnRange fixe (à remplacer par vos valeurs issues de configuration)
            float2 respawnRange = new float2(50f, 50f);
            new LifetimeSystemJob { deltaTime = deltaTime, respawnRange = respawnRange }.ScheduleParallel();
        }
    }

