using UnityEngine;


namespace Test
{
    public interface IModel
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
    }
}
