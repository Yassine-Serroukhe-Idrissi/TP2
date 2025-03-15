using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
    public partial struct PreyLifetimeModifierJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Translation> plantTranslations;
        [ReadOnly] public NativeArray<Translation> predatorTranslations;
        [ReadOnly] public NativeArray<Translation> preyTranslations; // pour la reproduction entre proies
        public float touchingDistance;
        public void Execute(ref LifetimeComponent lifetime, in Translation translation, in PreyTag tag)
        {
            lifetime.decreasingFactor = 1f;
            // Contact avec une plante → divise par 2
            for (int i = 0; i < plantTranslations.Length; i++)
            {
                if (math.distance(plantTranslations[i].Value, translation.Value) < touchingDistance)
                {
                    lifetime.decreasingFactor /= 2f;
                    break;
                }
            }
            // Contact avec un prédateur → multiplie par 2
            for (int i = 0; i < predatorTranslations.Length; i++)
            {
                if (math.distance(predatorTranslations[i].Value, translation.Value) < touchingDistance)
                {
                    lifetime.decreasingFactor *= 2f;
                    break;
                }
            }
            // Contact avec une autre proie → flag reproduction
            for (int i = 0; i < preyTranslations.Length; i++)
            {
                float dist = math.distance(preyTranslations[i].Value, translation.Value);
                if (dist < touchingDistance && dist > 0.001f)
                {
                    lifetime.reproduced = true;
                    break;
                }
            }
        }
    }

    public partial class PreyLifetimeModifierSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float touchingDistance = 1f;
            EntityQuery plantQuery = GetEntityQuery(ComponentType.ReadOnly<PlantTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> plantTranslations = plantQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
            EntityQuery predatorQuery = GetEntityQuery(ComponentType.ReadOnly<PredatorTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> predatorTranslations = predatorQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
            EntityQuery preyQuery = GetEntityQuery(ComponentType.ReadOnly<PreyTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> preyTranslations = preyQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

            new PreyLifetimeModifierJob
            {
                touchingDistance = touchingDistance,
                plantTranslations = plantTranslations,
                predatorTranslations = predatorTranslations,
                preyTranslations = preyTranslations
            }.ScheduleParallel();

            Dependency.Complete();
            plantTranslations.Dispose();
            predatorTranslations.Dispose();
            preyTranslations.Dispose();
        }
    }
