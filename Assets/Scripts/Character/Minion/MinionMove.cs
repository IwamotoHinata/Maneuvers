using UniRx;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Fusion;

namespace Unit
{
    /// <summary>
    /// ミニオンの移動に関する処理を書いたクラス
    /// </summary>
    public class MinionMove : CharacterStateActionBase
    {
        private NavMeshAgent _agent;
        private CharacterStatus _myCharacterStatus;
        private OwnerTypes _myOwnerTypes;

        private ScoreManager _scoreManagerTake2;
        private List<Flag> _flags = new List<Flag>();

        [Networked(OnChanged = nameof(ChangeMovePoint))] private Vector3 ForwardPoint { get; set; }
        [Networked(OnChanged = nameof(ChangeStopMove))] public NetworkBool IsStopped { get; set; }

        public override void Spawned()
        {
            base.Spawned();
            _myCharacterStatus = GetComponent<CharacterStatus>();
            _agent = GetComponent<NavMeshAgent>();
            _flags.AddRange(ScoreManager.Instance.Flag);

            switch (Object.InputAuthority)
            {
                case 1:
                    _myOwnerTypes = OwnerTypes.Host;
                    break;
                case 0:
                    _myOwnerTypes = OwnerTypes.Client;
                    _flags.Reverse();
                    break;
                default:
                    break;
            }

            foreach (var flag in _flags)
            {
                flag.ChangeFlag
                    .Subscribe(_ =>
                    {
                        ForwardPoint = DecideNextForward();
                    })
                    .AddTo(this);
            }
            
            var minionBase = GetComponent<CharacterBaseData>();
            minionBase.OninitialSetting
                .Where(value => value)
                .Subscribe(_ =>
                {
                    ForwardPoint = _flags[0].transform.position;
                    _agent.destination = GetAccessiblePoint(ForwardPoint);
                }
                ).AddTo(this);
        }

        private Vector3 DecideNextForward()
        {
            foreach (var flag in _flags.Where(flag => flag.Owner != _myOwnerTypes))
                return flag.gameObject.transform.position;

            return _flags.Last().transform.position;
        }

        private static void ChangeMovePoint(Changed<MinionMove> changed) =>
            changed.Behaviour.GoNextTarget();

        private void GoNextTarget()
        {
            _agent.destination = GetAccessiblePoint(ForwardPoint);
            IsStopped = false;
            _myCharacterStatus.ChangeCharacterState(CharacterState.Move);
            StopCoroutine(CompletedMove());
            StartCoroutine(CompletedMove());
        }

        private Vector3 GetAccessiblePoint(Vector3 point)
        {
            var goPoint = point;
            if (NavMesh.SamplePosition(point, out var hit, 20.0f, NavMesh.AllAreas))
            {
                goPoint = hit.position;
            }
            return goPoint;
        }

        private static void ChangeStopMove(Changed<MinionMove> changed)
        {
            var behavior = changed.Behaviour;
            behavior._agent.isStopped = behavior.IsStopped;
            behavior._myCharacterStatus.ChangeCharacterState(behavior.IsStopped
                ? CharacterState.Idle
                : CharacterState.Move);
        }

        IEnumerator CompletedMove()
        {
            yield return new WaitUntil(() => CharacterToTargetDistance() < 1);
            IsStopped = true;
        }

        IEnumerator Move()
        {
            yield return null;
        }

        private float CharacterToTargetDistance() //キャラクターと目標地点までの距離
        {
            return Vector3.Distance(transform.position, ForwardPoint);
        }

        public override void StartStateAction()
        {
        }

        public override void StateAction()
        {
            StopCoroutine(Move());
            StartCoroutine(Move());
        }

        public override void EndStateAction()
        {
        }
    }
}