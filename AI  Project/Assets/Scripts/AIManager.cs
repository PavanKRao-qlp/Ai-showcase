using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public GOAP_World CurrentWorldState { get; private set; }
    public void SetCurrentWorldState(GOAP_World world)
    {
        CurrentWorldState = world;
    }
}
