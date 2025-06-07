using System.Collections.Generic;
using UnityEngine;

namespace DeckMake
{
    [CreateAssetMenu(fileName = "MyDeck", menuName = "ScriptableObjects/MyDeck")]
    public class MyDeck : ScriptableObject
    {
        [Header("�f�b�L�̏��")]
        [SerializeField] int _selectedIndex;
        [SerializeField] List<DeckData> _deckList = new List<DeckData>();

        [Header("�R�X�g�֘A�̕ϐ�")]
        public int[] leaderPoint;
        public int[] heroCost;

        [Header("���j�b�g�A�C�R���̃X�v���C�g")]
        public Sprite[] leaderIconSprite;
        public Sprite[] heroIconSprites;

        [Header("�X�L���������̃X�v���C�g")]
        public Sprite[] leaderSkillExplanationSprite;
        public Sprite[] heroSkillExplanationSprite;

        [Header("�X�L���A�C�R���̃X�v���C�g")]
        public Sprite[] leaderSkillIconSprite;
        public Sprite[] heroSkillIconSprite;

        [Header("�X�L����")]
        public string[] leaderSkillName;
        public string[] heroSkillName;

        public int SelectedIndex { get => _selectedIndex; set { _selectedIndex = value; } }
        public List<DeckData> DeckList { get => _deckList; set { _deckList = value; } }

        /*  index�Ԗʂ̃f�b�L��Ԃ�  */
        public DeckData getDeckOf(int index)
        {
            return _deckList[index];
        }

        /*  �I�𒆂̃f�b�L��Ԃ�  */
        public DeckData getSelectedDeck()
        {
            return _deckList[_selectedIndex];
        }

        /*   �����Ԗڂ̃f�b�L�̃R�X�g���v�Z���Ԃ�   */
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

        /*   �����Ԗڂ̃f�b�L�̃��[�_�[�̏����|�C���g��Ԃ�   */
        public int getLeaderPoint(int index)
        {
            int heroIndex = getDeckOf(index).Leader;
            return (heroIndex>=0) ? leaderPoint[heroIndex] : 0;
        }

        /*  index�Ԗڂ̃f�b�L�̂��ׂẴX���b�g�Ƀ��j�b�g���I������Ă��邩  */
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

        //�w�肵���f�b�L�ɓ������j�b�g���Z�b�g����Ă���Ȃ�true
        public bool isDuplicationDeckOf(int index)
        {
            var deck = getDeckOf(index);
            var hashSet = new HashSet<int>();  //�d���m�F�p�f�[�^

            for (int unitId = 0; unitId < deck.Unit.Length; unitId++)
            {
                hashSet.Add(convertToHeroFromSkill(deck.Unit[unitId]));
            }

            //�d��������ꍇ�͗v�f��������
            if (deck.Unit.Length > hashSet.Count)
            {
                return true;
            }
            return false;
        }

        //�X�L���ԍ��i0�`8�j���烆�j�b�g�ԍ������Ƃ߂�i0�`5�j
        public int convertToHeroFromSkill(int skillId)
        {
            int heroId = 0;
            switch (skillId)
            {
                case (int)Unit.UnitType.Basic: //��{��
                    heroId = 0;
                    break;
                case (int)Unit.UnitType.Sniper: //�X�i�C�p�[
                    heroId = 1;
                    break;
                case (int)Unit.UnitType.Recon:
                case (int)Unit.UnitType.Funnnel: //���R��
                    heroId = 2;
                    break;
                case (int)Unit.UnitType.Tank:
                case (int)Unit.UnitType.Chainbuff: //�^���N
                    heroId = 3;
                    break;
                case (int)Unit.UnitType.Debuffer: //�f�o�b�t�@�[
                    heroId = 4;
                    break;
                case (int)Unit.UnitType.Assault:
                case (int)Unit.UnitType.Stealth: //�ˌ���
                    heroId = 5;
                    break;
                default:
                    break;
            }
            return heroId;
        }
    }

}