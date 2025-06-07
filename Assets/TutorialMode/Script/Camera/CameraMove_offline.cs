using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class CameraMove_offline : MonoBehaviour, ILateUpdated
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

        void Start()
        {
            //UpdateManager
            GameObject.Find("UpdateManager").
                GetComponent<UpdateManager_offline>().
                late.Add(this);

            //カーソル固定
            Cursor.lockState = CursorLockMode.Confined;
        }

        public bool UpdateRequest() { return true; }
        public void LateUpdated()
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
        }
    }
}
