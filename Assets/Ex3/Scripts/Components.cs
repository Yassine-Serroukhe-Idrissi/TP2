using Unity.Entities;
using Unity.Mathematics;

    // Composant gérant la durée de vie
    public struct LifetimeComponent : IComponentData
    {
        public float startingLifetime;
        public float lifetime;
        public float decreasingFactor;
        public bool alwaysReproduce;
        public bool reproduced;
    }

    // Composant pour la vitesse (calculée par les systèmes de mouvement)
    public struct VelocityComponent : IComponentData
    {
        public float3 velocity;
    }

    // Composant spécifique aux proies (par exemple, la vitesse de déplacement)
    public struct PreyComponent : IComponentData
    {
        public float speed;
    }

    // Composant spécifique aux prédateurs (par exemple, la vitesse de déplacement)
    public struct PredatorComponent : IComponentData
    {
        public float speed;
    }

    // Tags pour identifier le type d’entité
    public struct PlantTag : IComponentData { }
    public struct PreyTag : IComponentData { }
    public struct PredatorTag : IComponentData { }
