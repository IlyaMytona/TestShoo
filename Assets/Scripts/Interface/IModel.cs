using UnityEngine;


namespace Test.Interface
{
    public interface IModel
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
    }
}
