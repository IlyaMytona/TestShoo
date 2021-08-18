using UnityEngine;


namespace Test.Data
{
    [System.Serializable]
    public class DamageReceiveData
    {
        #region Fields

        public string attackerPlayerID;
        public int weaponID;
        public float damage;

        #endregion


        #region Methods

        public void InitData(string data)
        {
            DamageReceiveData damageData = JsonUtility.FromJson<DamageReceiveData>(data);

            attackerPlayerID = damageData.attackerPlayerID;
            weaponID = damageData.weaponID;
            damage = damageData.damage;            
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
        
        public override string ToString()
        {
            return string.Format("attackerPlayerID={0} weaponID={1} damage={2} ", attackerPlayerID, weaponID, damage);
        }

        #endregion
    }
}

