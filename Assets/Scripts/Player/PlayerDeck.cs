using System.Collections;
using System.Collections.Generic;
using DeckMake;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    [SerializeField] private MyDeck myDeck;

    public MyDeck MyPlayerCharacterDeck => myDeck;
}
