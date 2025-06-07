using UnityEngine;

public class GuardianLeader : BasicLeader
{
    protected override void UseUlt()
    {
        var mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);
            var spawnPos = hit.point; //ƒEƒ‹ƒg‚ÌˆÊ’u‚ðŒˆ’è

            LeaderObserver.RPC_SpawnGuardianArea(spawnPos);
        }
    }
}
