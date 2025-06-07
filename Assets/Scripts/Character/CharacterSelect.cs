using UnityEngine;

namespace Unit
{
    public class CharacterSelect : MonoBehaviour
    {
        CharacterBaseData _myCharacterBaseData;
        CharacterMove _characterMove;

        void Start()
        {
            _myCharacterBaseData = GetComponent<CharacterBaseData>();
            _characterMove = GetComponent<CharacterMove>();

        }
        public void MultSelected(bool value)
        {
            if (_myCharacterBaseData.isHasInputAuthority())
            {
                _characterMove.iSelect(value);
            }
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0)) {
                _characterMove.iSelect(false);
            }
        }

        public void OnPointerUp()
        {
            _characterMove.iSelect(true);
        }
    }
}