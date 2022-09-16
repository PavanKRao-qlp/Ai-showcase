using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FollowPathTask : BaseTask
{
    public Transform Transform;
    public Vector3[] Path;
    public float Speed;
    public override async Task<bool> Execute(CancellationToken token)
    {
        if (Path == null) return false;
        var ix = 0;
        while (ix < Path.Length) {
            var initPos = this.Transform.position;
            var finalPos = Path[ix];
            finalPos.y = initPos.y;
            var delta = 0f;
            this.Transform.LookAt(finalPos);
            while (this.Transform.position != finalPos)
            {
                token.ThrowIfCancellationRequested();
                delta += Time.deltaTime * Speed;
                if (delta > 1) delta = 1;
                this.Transform.position = Vector3.Lerp(initPos, finalPos, delta);
                await Task.Yield();
            }
            ix++;
        }
        return true;
    }
}
