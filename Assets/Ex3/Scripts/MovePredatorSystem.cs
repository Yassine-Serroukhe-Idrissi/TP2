using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


    [BurstCompile]
    public partial struct MovePredatorSystemJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Translation> preyTranslations;
        public float deltaTime;
        public float predatorSpeed;
        public void Execute(ref Translation translation, ref VelocityComponent velocity, in PredatorTag tag, in PredatorComponent predator)
        {
            float closestDistance = float.MaxValue;
            float3 closestPosition = translation.Value;
            for (int i = 0; i < preyTranslations.Length; i++)
            {
                float dist = math.distance(preyTranslations[i].Value, translation.Value);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPosition = preyTranslations[i].Value;
                }
            }
            float3 direction = math.normalize(closestPosition - translation.Value);
            velocity.velocity = direction * predatorSpeed;
            translation.Value += velocity.velocity * deltaTime;
        }
    }

    public partial class MovePredatorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            float predatorSpeed = 7f;

            // Récupération des positions de toutes les proies
            EntityQuery preyQuery = GetEntityQuery(ComponentType.ReadOnly<PreyTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> preyTranslations = preyQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

            new MovePredatorSystemJob
            {
                deltaTime = deltaTime,
                predatorSpeed = predatorSpeed,
                preyTranslations = preyTranslations
            }.ScheduleParallel();

            Dependency.Complete();
            preyTranslations.Dispose();
        }
    }

