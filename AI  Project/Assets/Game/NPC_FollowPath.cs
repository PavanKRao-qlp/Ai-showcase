using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_FollowPath : NPC_ActionBase
{
    Vector3[] path;
    public NPC_FollowPath(NPC_Agent agent) : base(agent) {}

    public override void StartAction()
    {
    }
}
 