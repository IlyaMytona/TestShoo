using Test.Enum;
using UnityEngine;


[CreateAssetMenu(fileName = "Bullet", menuName = "Bullets")]
public class AmmunitionSoData : ScriptableObject
{
    public float TimeToDestruct = 5;
    public float BaseDamage = 10;
    public float CurrentDamage;
    public AmmunitionType Type = AmmunitionType.Bullet;
}
