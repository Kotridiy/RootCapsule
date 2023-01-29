using System;

namespace RootCapsule.Model.Fields
{
    public interface IAlive
    {
        event Action Destruction;
    }
}