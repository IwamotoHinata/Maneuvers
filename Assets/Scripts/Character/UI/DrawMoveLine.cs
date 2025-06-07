using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Unit
{
    public class DrawMoveLine : MonoBehaviour
    {
        [Header("�Q�ƌ�")]
        [SerializeField]
        CharacterMove MyCharacterMove;
        [SerializeField]
        CharacterBaseData MyBaseData;
        NavMeshAgent MyAgent;
        LineRenderer MyLineRenderer;
        NavMeshPath Path;
        [Header("�o�H��\�����鋗��")]
        [SerializeField]
        private float viewDistance = 2;
        private Vector3 targetPosition;

        void Start()
        {
            MyLineRenderer = GetComponent<LineRenderer>();

            MyAgent = GetComponentInParent<NavMeshAgent>();

            Path = new NavMeshPath();
            targetPosition = transform.position;
            MyCharacterMove
                .OnMoveTargetPositionChanged
                .Skip(1)
                .Subscribe(TargetPosition =>
                {
                    targetPosition = TargetPosition;              
                }
                );

            MyBaseData
                .OninitialSetting
                .Where(value => value == true)
                .Subscribe(_ =>
                {
                    Debug.Log("DisplayProfile�����l�̐ݒ肪����܂���");
                    Init();
                }).AddTo(this);

            StartCoroutine(RootUpDate());
        }

        private void LateUpdate()
        {
 
        }

        public IEnumerator RootUpDate()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.3f);
                if (MyAgent != null)
                {
                    if (!MyBaseData.isHasInputAuthority())
                    {
                        MyLineRenderer.SetColors(Color.clear, Color.clear);
                    }
                    else
                    {
                        MyLineRenderer.SetColors(Color.yellow, Color.blue);
                        if(MyAgent.enabled) MyAgent.CalculatePath(targetPosition, Path);
                        Vector3[] drawPath = new Vector3[Path.corners.Length + MyCharacterMove.ReturnBookPos().ToArray().Length];
                        Path.corners.CopyTo(drawPath, 0);
                        MyCharacterMove.ReturnBookPos().ToArray().CopyTo(drawPath, Path.corners.Length);
                        MyLineRenderer.positionCount = drawPath.Length;
                        MyLineRenderer.SetPositions(drawPath);
                    }
                }
            }

        }

        void Init()
        {
            /*
            if(!MyBaseData.isHasInputAuthority())
            {
                MyLineRenderer.SetColors(Color.clear, Color.clear);
            }
            */
        }

    }
}