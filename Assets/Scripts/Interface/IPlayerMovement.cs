using UnityEngine;


namespace Test
{
    public interface IPlayerMovement
    {
        Transform Transform { get;}
        void Walk(float horizontal, float vertical);
    }
}
