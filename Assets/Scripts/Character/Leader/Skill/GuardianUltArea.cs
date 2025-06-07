using System.Collections;
using Fusion;
using Unit;
using Online;
using UnityEngine;

public class GuardianUltArea : NetworkBehaviour
{
    private TeamType _myTeamType;
    [SerializeField] private NetworkObject damageArea;
    [SerializeField] private float radiusArea; // �~�̔��a
    // �E���g�Ԋu
    [SerializeField] private float minUltTimeOut;
    [SerializeField] private float maxUltTimeOut;
    [SerializeField] private float ultTimeOut;

    [Networked] private TickTimer Life { get; set; }

    public override void Spawned()
    {
        base.Spawned();

        // �`�[���^�C�v��ݒ�
        _myTeamType = Object.HasInputAuthority ? TeamType.Teammate : TeamType.Enemys;
        Life = TickTimer.CreateFromSeconds(Runner, 15.0f);
        StartCoroutine(nameof(makeUlt));
    }
    
    private IEnumerator makeUlt()
    {
        float time = maxUltTimeOut;
        yield return new WaitForSeconds(ultTimeOut);

        while (!Life.Expired(Runner))
        {
            var aveRand2 = (Random.insideUnitCircle + Random.insideUnitCircle) / 2;
            // �w�肳�ꂽ���a�̉~���̃����_���ʒu���擾
            var circlePos = radiusArea * aveRand2;

            // XZ���ʂŎw�肳�ꂽ���a�A���S�_�̉~���̃����_���ʒu���v�Z
            var spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + gameObject.transform.position;

            GameLauncher.Runner.Spawn(
                damageArea,
                spawnPos,
                Quaternion.identity,
                Object.InputAuthority,
                (_, obj) => obj.GetComponent<DamageArea>().MyOwnerTypeSet(_myTeamType)
            );

            time = time > minUltTimeOut ? time - 0.5f : minUltTimeOut;

            yield return new WaitForSeconds(time);
        }
        Runner.Despawn(Object);
    }
}
