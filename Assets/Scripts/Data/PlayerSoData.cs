using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Player")]
public class PlayerSoData : ScriptableObject
{
    public float PlayerHp = 50.0f;
    public float Speed = 10.0f;
    public float JumpForce = 20.0f;
    public float Gravity = -1.5f;
    public float YVelocity = 0.0f;    
}
