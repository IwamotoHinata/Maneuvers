using UnityEngine;
using UnityEngine.SceneManagement;

public class GoDeckScene : MonoBehaviour
{
    public void Clicked()
    {
        try
        {
            SceneManager.LoadScene(3);
        }
        catch (System.Exception)
        {
            Debug.LogError("LoadScene仕様とした対象のシーンが存在しません");
            throw;
        }
    }
}
