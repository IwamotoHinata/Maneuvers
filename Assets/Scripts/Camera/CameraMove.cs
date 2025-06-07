using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    [Header("速度")]
    [SerializeField] private float _moveSpeedHorizon;
    [SerializeField] private float _moveSpeedVertical;
    [SerializeField] private float _rotateSpeed;

    [Header("カメラズーム")]
    [SerializeField] private float _topHeight;
    [SerializeField] private float _bottomHeight;

    [Header("画面端移動")]
    [SerializeField] private float _upLimit;
    [SerializeField] private float _downLimit;
    [SerializeField] private float _leftLimit;
    [SerializeField] private float _rightLimit;

    [Header("水平方向への移動制限")]
    [SerializeField] private float _xPositionLimit;
    [SerializeField] private float _zPositionLimit;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void LateUpdate()
    {
        //移動方向
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > _upLimit)
            direction += transform.forward;

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < _leftLimit)
        {
            //視点の回転
            if (Input.GetKey(KeyCode.LeftControl))
                transform.Rotate(0, -1 * _rotateSpeed, 0);
            else
                direction += -1 * transform.right;
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < _downLimit)
            direction += -1 * transform.forward;

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > _rightLimit)
        {
            if (Input.GetKey(KeyCode.LeftControl))
                transform.Rotate(0, _rotateSpeed, 0);
            else
                direction += transform.right;
        }

        transform.position += direction.normalized * Time.deltaTime * _moveSpeedHorizon; //横移動
        if (!(transform.position.y > _topHeight && Input.mouseScrollDelta.y < 0)
            && !(transform.position.y < _bottomHeight && Input.mouseScrollDelta.y > 0))
            transform.position -= new Vector3(0, Input.mouseScrollDelta.y * _moveSpeedVertical, 0); //縦移動

        //移動制限
        if (transform.position.y < _bottomHeight)
            transform.position = new Vector3(transform.position.x, _bottomHeight, transform.position.z);
        if (transform.position.y > _topHeight)
            transform.position = new Vector3(transform.position.x, _topHeight, transform.position.z);


        if (transform.position.x > _xPositionLimit)
            transform.position = new Vector3(_xPositionLimit, transform.position.y, transform.position.z);
        if (transform.position.x < -  _xPositionLimit)
            transform.position = new Vector3(- _xPositionLimit, transform.position.y, transform.position.z);


        if (transform.position.z > _zPositionLimit)
            transform.position = new Vector3(transform.position.x, transform.position.y ,_zPositionLimit);
        if (transform.position.z < -_zPositionLimit)
            transform.position = new Vector3(transform.position.x, transform.position.y, - _zPositionLimit);
    }
}