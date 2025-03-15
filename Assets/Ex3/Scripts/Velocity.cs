using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]

public class VelocityAuthoring : MonoBehaviour
{
    // Add serialized field for setting initial velocity in Inspector
    [SerializeField] private Vector3 initialVelocity;

    class Baker : Baker<VelocityAuthoring>
    {
        public override void Bake(VelocityAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            // Convert Unity Vector3 to float3
            AddComponent(entity, new Velocity { velocity = authoring.initialVelocity });
        }
    }
}

public struct Velocity : IComponentData
{
    public float3 velocity;
}

public partial struct VelocitySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        // Process all entities with both LocalTransform and Velocity components
        foreach (var (transform, velocity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<Velocity>>())
        {
            transform.ValueRW.Position += velocity.ValueRO.velocity * deltaTime;
        }
    }
}