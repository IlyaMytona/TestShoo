using UnityEngine;


namespace Test.Interface
{
    public interface IItemDescription
    {
        int ID { get; }
        Sprite Icon { get; }          
    }
}