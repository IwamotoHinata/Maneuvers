using UnityEngine;
namespace Unit
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreaterData")]
    public class CharacterData : ScriptableObject
    {
        public int characterID;
        public string characterName = "キャラクターネーム";
        public UnitType type;
        public float hp;
        public int recoverySpeed;
        public float moveSpeed;
        public float attackRange;
        public float attackRate;
        public float attackPower;
        public float armor;
        public float staticHitRate;
        public float moveHitRate;
        public float reloadSpeed;
        public float searchRange;
        public float skillTime;
    }
}