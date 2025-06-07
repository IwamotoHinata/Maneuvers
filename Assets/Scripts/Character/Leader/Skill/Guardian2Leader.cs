using Online;
using Unit;
using UnityEngine;

public class Guardian2Leader : BasicLeader
{
    private TeamType _myTeamType;
    [SerializeField] private GameObject skillAreaPrefab;
    private GameObject _mySkillArea;
    
    protected override void UseUlt()
    {
        var mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);
            GameLauncher.Runner.Spawn(
                skillAreaPrefab, 
                hit.point, 
                Quaternion.identity,
                RoomPlayer.Local.Object.InputAuthority,
                (_, obj) => obj.GetComponent<Guardian2UltArea>().MyOwnerTypeSet(_myTeamType));
        }
    }
}
