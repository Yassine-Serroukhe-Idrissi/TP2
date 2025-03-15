using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
    public partial struct MovePreySystemJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Translation> plantTranslations;
        public float deltaTime;
        public float preySpeed;
        public void Execute(ref Translation translation, ref VelocityComponent velocity, in PreyTag tag, in PreyComponent prey)
        {
            float closestDistance = float.MaxValue;
            float3 closestPosition = translation.Value;
            for (int i = 0; i < plantTranslations.Length; i++)
            {
                float dist = math.distance(plantTranslations[i].Value, translation.Value);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPosition = plantTranslations[i].Value;
                }
            }
            float3 direction = math.normalize(closestPosition - translation.Value);
            velocity.velocity = direction * preySpeed;
            translation.Value += velocity.velocity * deltaTime;
        }
    }

    public partial class MovePreySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            // Valeur de vitesse de la proie (à remplacer ou paramétrer via votre configuration)
            float preySpeed = 5f;

            // Récupération des positions de toutes les plantes
            EntityQuery plantQuery = GetEntityQuery(ComponentType.ReadOnly<PlantTag>(), ComponentType.ReadOnly<Translation>());
            NativeArray<Translation> plantTranslations = plantQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

            new MovePreySystemJob
            {
                deltaTime = deltaTime,
                preySpeed = preySpeed,
                plantTranslations = plantTranslations
            }.ScheduleParallel();

            Dependency.Complete();
            plantTranslations.Dispose();
        }
    }

