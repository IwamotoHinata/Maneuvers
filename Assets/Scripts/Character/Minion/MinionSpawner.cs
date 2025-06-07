using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unit;
using UnityEngine;

public class MinionSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    private CharacterBaseData _profile;
    private CharacterRPCManager _manager;

    [SerializeField] private GameObject spawnEffect;
    
}
