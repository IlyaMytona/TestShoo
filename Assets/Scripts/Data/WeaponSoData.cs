using Test.Enum;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon")]
public class WeaponSoData : ScriptableObject
{
    public Sprite WeaponIcon;
    public float Force = 400.0f;
    public float RechergeTime = 0.2f;
    public int CountClip = 99999;
    public float Range = 100f;
    public int PelletsCount = 6;

    public void SetData(AmmunitionType type)
    {
        if (type == AmmunitionType.Bullet) 
        {
            WeaponIcon = Resources.Load<Sprite>("Sprites/Gun");
            Force = 300.0f;
            RechergeTime = 0.2f;
            PelletsCount = 0;
        }

        if (type == AmmunitionType.FractionBullet)
        {
            WeaponIcon = Resources.Load<Sprite>("Sprites/Shotgun");
            Force = 100.0f;
            RechergeTime = 1.0f;
            PelletsCount = 6;
        }
    }
}
