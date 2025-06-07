using System.Collections.Generic;
using UnityEngine;

namespace DeckMake
{
    [CreateAssetMenu(fileName = "MyDeck", menuName = "ScriptableObjects/MyDeck")]
    public class MyDeck : ScriptableObject
    {
        [Header("デッキの情報")]
        [SerializeField] int _selectedIndex;
        [SerializeField] List<DeckData> _deckList = new List<DeckData>();

        [Header("コスト関連の変数")]
        public int[] leaderPoint;
        public int[] heroCost;

        [Header("ユニットアイコンのスプライト")]
        public Sprite[] leaderIconSprite;
        public Sprite[] heroIconSprites;

        [Header("スキル説明文のスプライト")]
        public Sprite[] leaderSkillExplanationSprite;
        public Sprite[] heroSkillExplanationSprite;

        [Header("スキルアイコンのスプライト")]
        public Sprite[] leaderSkillIconSprite;
        public Sprite[] heroSkillIconSprite;

        [Header("スキル名")]
        public string[] leaderSkillName;
        public string[] heroSkillName;

        public int SelectedIndex { get => _selectedIndex; set { _selectedIndex = value; } }
        public List<DeckData> DeckList { get => _deckList; set { _deckList = value; } }

        /*  index番面のデッキを返す  */
        public DeckData getDeckOf(int index)
        {
            return _deckList[index];
        }

        /*  選択中のデッキを返す  */
        public DeckData getSelectedDeck()
        {
            return _deckList[_selectedIndex];
        }

        /*   引数番目のデッキのコストを計算し返す   */
        public int getSumCost(int index)
        {
            var list = getDeckOf(index);
            int sumCost = 0;

            for (int i = 0; i < 5; i++)
            {
                int unitIndex = list.Unit[i];
                sumCost += (unitIndex >= 0) ? heroCost[unitIndex] : 0;
            }

            return sumCost;
        }

        /*   引数番目のデッキのリーダーの所持ポイントを返す   */
        public int getLeaderPoint(int index)
        {
            int heroIndex = getDeckOf(index).Leader;
            return (heroIndex>=0) ? leaderPoint[heroIndex] : 0;
        }

        /*  index番目のデッキのすべてのスロットにユニットが選択されているか  */
        public bool isAllSetDeckOf(int index)
        {
            var deck = getDeckOf(index);

            if (deck.Leader < 0)
            {
                return false;
            }

            for(int i=0; i < 5; i++)
            {
                if (deck.Unit[i] < 0)
                {
                    return false;
                }
            }

            return true;
        }

        //指定したデッキに同じユニットがセットされているならtrue
        public bool isDuplicationDeckOf(int index)
        {
            var deck = getDeckOf(index);
            var hashSet = new HashSet<int>();  //重複確認用データ

            for (int unitId = 0; unitId < deck.Unit.Length; unitId++)
            {
                hashSet.Add(convertToHeroFromSkill(deck.Unit[unitId]));
            }

            //重複がある場合は要素数が減る
            if (deck.Unit.Length > hashSet.Count)
            {
                return true;
            }
            return false;
        }

        //スキル番号（0～8）からユニット番号をもとめる（0～5）
        public int convertToHeroFromSkill(int skillId)
        {
            int heroId = 0;
            switch (skillId)
            {
                case (int)Unit.UnitType.Basic: //基本兵
                    heroId = 0;
                    break;
                case (int)Unit.UnitType.Sniper: //スナイパー
                    heroId = 1;
                    break;
                case (int)Unit.UnitType.Recon:
                case (int)Unit.UnitType.Funnnel: //リコン
                    heroId = 2;
                    break;
                case (int)Unit.UnitType.Tank:
                case (int)Unit.UnitType.Chainbuff: //タンク
                    heroId = 3;
                    break;
                case (int)Unit.UnitType.Debuffer: //デバッファー
                    heroId = 4;
                    break;
                case (int)Unit.UnitType.Assault:
                case (int)Unit.UnitType.Stealth: //突撃兵
                    heroId = 5;
                    break;
                default:
                    break;
            }
            return heroId;
        }
    }

}