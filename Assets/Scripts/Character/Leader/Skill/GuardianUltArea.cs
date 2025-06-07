using System.Collections;
using Fusion;
using Unit;
using Online;
using UnityEngine;

public class GuardianUltArea : NetworkBehaviour
{
    private TeamType _myTeamType;
    [SerializeField] private NetworkObject damageArea;
    [SerializeField] private float radiusArea; // ‰~‚Ì”¼Œa
    // ƒEƒ‹ƒgŠÔŠu
    [SerializeField] private float minUltTimeOut;
    [SerializeField] private float maxUltTimeOut;
    [SerializeField] private float ultTimeOut;

    [Networked] private TickTimer Life { get; set; }

    public override void Spawned()
    {
        base.Spawned();

        // ƒ`[ƒ€ƒ^ƒCƒv‚ðÝ’è
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
            // Žw’è‚³‚ê‚½”¼Œa‚Ì‰~“à‚Ìƒ‰ƒ“ƒ_ƒ€ˆÊ’u‚ðŽæ“¾
            var circlePos = radiusArea * aveRand2;

            // XZ•½–Ê‚ÅŽw’è‚³‚ê‚½”¼ŒaA’†S“_‚Ì‰~“à‚Ìƒ‰ƒ“ƒ_ƒ€ˆÊ’u‚ðŒvŽZ
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
