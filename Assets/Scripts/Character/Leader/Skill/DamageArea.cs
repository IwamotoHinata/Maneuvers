using System.Collections;
using System.Collections.Generic;
using Fusion;
using Online;
using UnityEngine;
using Unit;

public class DamageArea : NetworkBehaviour
{
    private TeamType _myTeamType;
    [SerializeField] private float ultdamage;
    private List<CharacterStatus> _damagedCs = new List<CharacterStatus>();

    [SerializeField] private GameObject effectExplosionPrefab;
    private GameObject _effectExplosion;

    [Networked] private TickTimer Life { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        
        Life = TickTimer.CreateFromSeconds(Runner, 2.0f);
    }

    private void Start()
    {
        _effectExplosion = Instantiate(effectExplosionPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Unit"))
        {
            if (Object.HasStateAuthority)
            {
                HitGuardianLeaderUlt(other.gameObject);
            }
        }
    }

    private void HitGuardianLeaderUlt(GameObject other)
    {
        if (other.TryGetComponent<CharacterBaseData>(out var cbd) && other.TryGetComponent<CharacterStatus>(out var CS))
        {
            if (_damagedCs.Contains(CS)) return;
            _damagedCs.Add(CS);
            // –¡•û
            if (_myTeamType == CS.GetTeamType)
            {
                CS.AddDamage(ultdamage/2);
            }
            // “G
            else
            {
                CS.AddDamage(ultdamage);
            }
        }
    }
    
    public void MyOwnerTypeSet(TeamType type)
    {
        _myTeamType = type;
    }

    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))
        {
            if (_effectExplosion != null)
            {
                Destroy(_effectExplosion);
            }
            Runner.Despawn(Object);
        }
    }
}
