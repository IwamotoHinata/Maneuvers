using UnityEngine;

public class GetMouseEvent : MonoBehaviour
{
    private GameObject clickUnit = null;    // �N���b�N����Unit�̎���

    // �R���|�[�l���g
    [SerializeField]
    private UIManager uiManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickUnit = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    clickUnit = hit.collider.gameObject;
                }
            }
        }

        // Unit���N���b�N���Ă��Ȃ��Ȃ�
        if (clickUnit == null)
        {
            uiManager.showStatusPanel(null);
        }
        else
        {
            uiManager.showStatusPanel(clickUnit.GetComponentInParent<FlagOnPlayer>());
        }

    }
}
