using UnityEngine;


namespace Test.Interface
{
    public interface IPoolObject<out T> where T : IModel
    {
        T GetObject(Vector3 position, Quaternion rotation);
        void ReturnToPool(int hash);
    }
}
