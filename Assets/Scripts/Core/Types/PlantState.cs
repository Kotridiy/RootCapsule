namespace RootCapsule.Core.Types
{
    [System.Serializable]
    public struct PlantState
    {
        public float LifePoints;
        public LifeStage LifeStage;
    }

    public enum LifeStage
    {
        New,
        Child,
        Teen,
        Adult,
        Refill
    }
}