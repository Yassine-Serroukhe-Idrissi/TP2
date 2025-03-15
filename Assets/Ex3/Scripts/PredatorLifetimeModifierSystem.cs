using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
    public partial struct PredatorLifetimeModifierJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Translation> predatorTranslations; // pour la reproduction entre prédateurs
        [ReadOnly] public NativeArray<Translation> preyTranslations;
        public float touchingDistance;
        public void Execute(ref LifetimeComponent lifetime, in Translation translation, in PredatorTag tag)
        {
            lifetime.decreasingFactor = 1f;
            // Vérifier la reproduction : contact avec un autre prédateur
            for (int i = 0; i < predatorTranslations.Length; i++)
            {
                float dist = math.distance(predatorTranslations[i].Value, translation.Value);
                if (dist < touchingDistance && dist > 0.001f)
                {
                    lifetime.reproduced = true;
                    break;
                }
            }
            // Contact avec une proie → divise par 2
            for (int i = 0; i < preyTranslations.Length; i++)
            {
                if (math.distance(preyTranslations[i].Value, translation.Value) < touchingDistance)
                {
                    lifetime.decreasingFactor /= 2f;
                    break;
                }
            }
        }
    }

    public partial class PredatorLifetimeModifierSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float touchingDistance = 1f;
            EntityQuery predatorQuery = GetEntityQuery(ComponentType.ReadOnly<PredatorTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> predatorTranslations = predatorQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
            EntityQuery preyQuery = GetEntityQuery(ComponentType.ReadOnly<PreyTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> preyTranslations = preyQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

            new PredatorLifetimeModifierJob
            {
                touchingDistance = touchingDistance,
                predatorTranslations = predatorTranslations,
                preyTranslations = preyTranslations
            }.ScheduleParallel();

            Dependency.Complete();
            predatorTranslations.Dispose();
            preyTranslations.Dispose();
        }
    }

