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
            Debug.LogError("LoadScene�d�l�Ƃ����Ώۂ̃V�[�������݂��܂���");
            throw;
        }
    }
}
