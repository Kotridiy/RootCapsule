using System;

namespace RootCapsule.Model.Fields
{
    public interface IAlive
    {
        event Action Destruction;
        bool Initialized { get; }

        void Deinitialize();
    }
}