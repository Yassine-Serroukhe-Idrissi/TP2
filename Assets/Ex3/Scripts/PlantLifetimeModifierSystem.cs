using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

BurstCompile]
    public partial struct PlantLifetimeModifierJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Translation> preyTranslations;
        public float touchingDistance;
        public void Execute(ref LifetimeComponent lifetime, in Translation translation, in PlantTag tag)
        {
            lifetime.decreasingFactor = 1f;
            for (int i = 0; i < preyTranslations.Length; i++)
            {
                if (math.distance(preyTranslations[i].Value, translation.Value) < touchingDistance)
                {
                    lifetime.decreasingFactor *= 2f;
                    break;
                }
            }
        }
    }

    public partial class PlantLifetimeModifierSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float touchingDistance = 1f; // À ajuster selon Ex3Config.TouchingDistance
            EntityQuery preyQuery = GetEntityQuery(ComponentType.ReadOnly<PreyTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> preyTranslations = preyQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

            new PlantLifetimeModifierJob
            {
                touchingDistance = touchingDistance,
                preyTranslations = preyTranslations
            }.ScheduleParallel();

            Dependency.Complete();
            preyTranslations.Dispose();
        }
    }
