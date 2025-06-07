using UnityEngine;

public class ButtonSe : MonoBehaviour
{
    [SerializeField] private SeType seType;

    public void shotButtonSe()
    {
        SoundManager.Instance.shotSe(seType);
    }
}
