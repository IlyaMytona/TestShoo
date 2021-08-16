using UnityEngine;


namespace Test
{
    public interface IItemDescription
    {
        int ID { get; }
        Sprite Icon { get; }          
    }
}