using UnityEngine;


namespace Test.Interface
{
    public interface IPlayerMovement
    {
        Transform Transform { get;}
        void Walk(float horizontal, float vertical);
    }
}
