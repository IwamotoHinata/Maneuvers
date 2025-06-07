using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Online;

public class ReturnMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var button = this.gameObject.GetComponent<Button>();
        button.OnClickAsObservable().
               Subscribe(_ => goMainMenu()).AddTo(this);
    }

    public void goMainMenu()
    { 
        GameLauncher.Runner.Shutdown();
        GridTransition.Instance.nextTransition = "MainMenu";
        GameObject.Find("GridTransition").GetComponent<Animator>().SetTrigger("isTransition");
    }
}
