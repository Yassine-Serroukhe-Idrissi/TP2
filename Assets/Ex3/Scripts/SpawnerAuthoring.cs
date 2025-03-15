using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class SpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public int plantCount = 3000;
        public int preyCount = 3000;
        public int predatorCount = 2500;
        public float gridWidth = 12000;
        public float gridHeight = 12000;
        public float plantStartingLifetimeMin = 5f;
        public float plantStartingLifetimeMax = 15f;
        public float preyStartingLifetimeMin = 5f;
        public float preyStartingLifetimeMax = 15f;
        public float predatorStartingLifetimeMin = 5f;
        public float predatorStartingLifetimeMax = 15f;
        public float preySpeed = 5f;
        public float predatorSpeed = 7f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // Création des entités "Plant"
            for (int i = 0; i < plantCount; i++)
            {
                Entity plant = dstManager.CreateEntity(
                    typeof(Translation),
                    typeof(LifetimeComponent),
                    typeof(VelocityComponent),
                    typeof(PlantTag)
                );
                float startingLifetime = UnityEngine.Random.Range(plantStartingLifetimeMin, plantStartingLifetimeMax);
                LifetimeComponent lifetime = new LifetimeComponent
                {
                    startingLifetime = startingLifetime,
                    lifetime = startingLifetime,
                    decreasingFactor = 1f,
                    alwaysReproduce = false,
                    reproduced = false
                };
                dstManager.SetComponentData(plant, lifetime);
                dstManager.SetComponentData(plant, new Translation
                {
                    Value = new float3(
                        UnityEngine.Random.Range(-gridWidth / 2, gridWidth / 2),
                        UnityEngine.Random.Range(-gridHeight / 2, gridHeight / 2),
                        0f)
                });
                dstManager.SetComponentData(plant, new VelocityComponent { velocity = float3.zero });
            }

            // Création des entités "Prey"
            for (int i = 0; i < preyCount; i++)
            {
                Entity prey = dstManager.CreateEntity(
                    typeof(Translation),
                    typeof(LifetimeComponent),
                    typeof(VelocityComponent),
                    typeof(PreyTag),
                    typeof(PreyComponent)
                );
                float startingLifetime = UnityEngine.Random.Range(preyStartingLifetimeMin, preyStartingLifetimeMax);
                LifetimeComponent lifetime = new LifetimeComponent
                {
                    startingLifetime = startingLifetime,
                    lifetime = startingLifetime,
                    decreasingFactor = 1f,
                    alwaysReproduce = false,
                    reproduced = false
                };
                dstManager.SetComponentData(prey, lifetime);
                dstManager.SetComponentData(prey, new Translation
                {
                    Value = new float3(
                        UnityEngine.Random.Range(-gridWidth / 2, gridWidth / 2),
                        UnityEngine.Random.Range(-gridHeight / 2, gridHeight / 2),
                        0f)
                });
                dstManager.SetComponentData(prey, new VelocityComponent { velocity = float3.zero });
                dstManager.SetComponentData(prey, new PreyComponent { speed = preySpeed });
            }

            // Création des entités "Predator"
            for (int i = 0; i < predatorCount; i++)
            {
                Entity predator = dstManager.CreateEntity(
                    typeof(Translation),
                    typeof(LifetimeComponent),
                    typeof(VelocityComponent),
                    typeof(PredatorTag),
                    typeof(PredatorComponent)
                );
                float startingLifetime = UnityEngine.Random.Range(predatorStartingLifetimeMin, predatorStartingLifetimeMax);
                LifetimeComponent lifetime = new LifetimeComponent
                {
                    startingLifetime = startingLifetime,
                    lifetime = startingLifetime,
                    decreasingFactor = 1f,
                    alwaysReproduce = false,
                    reproduced = false
                };
                dstManager.SetComponentData(predator, lifetime);
                dstManager.SetComponentData(predator, new Translation
                {
                    Value = new float3(
                        UnityEngine.Random.Range(-gridWidth / 2, gridWidth / 2),
                        UnityEngine.Random.Range(-gridHeight / 2, gridHeight / 2),
                        0f)
                });
                dstManager.SetComponentData(predator, new VelocityComponent { velocity = float3.zero });
                dstManager.SetComponentData(predator, new PredatorComponent { speed = predatorSpeed });
            }
        }
    }

