namespace RootCapsule.Core.Types
{
    interface ISerializableObject<T> where T : struct
    {
        T SerializeState();
        void DeserializeState(T data);
    }
}
