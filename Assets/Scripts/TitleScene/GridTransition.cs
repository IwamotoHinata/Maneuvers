using Fusion;
using Online;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridTransition : MonoBehaviour
{
    public static GridTransition Instance { get; private set; }

    [SerializeField] private GameObject _animationCanvas;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _deckMaker;

    public string nextTransition;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(_animationCanvas);
        }
        else
            Destroy(_animationCanvas);
        //_animationCanvas.SetActive(false);
    }

    public void startTransition()
    {
        _animationCanvas.gameObject.GetComponent<Canvas>().sortingOrder = 1;
    }

    public void transitionChange()
    {
        switch (nextTransition)
        {
            case "MainMenu":
                SceneManager.LoadScene(0);
                break;
            case "Deck":
                _deckMaker.SetActive(true);
                _mainMenu.SetActive(false);
                break;
            case "Tutorial1":
                SceneManager.LoadScene(nextTransition);
                break;
            case "Sec1":
                SceneManager.LoadScene(nextTransition);
                break;
            case "Battle":
                
                break;
            default:
                break;
        }
    }

    public void finishTransition()
    {
        _animationCanvas.gameObject.GetComponent<Canvas>().sortingOrder = -5;
        if (SceneManager.GetActiveScene().name == "MatchingTest")
        {
            _mainMenu = GameObject.Find("MainMenuPanel");
            _deckMaker = GameObject.Find("Canvases").transform.Find("DeckMaker").gameObject;
        }
    }
}
