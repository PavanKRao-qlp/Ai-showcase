
using UnityEngine;

public class ColorBTNodeSO : BehaviourTreeNodeScriptableObject<ColorBTNode>
{

}


public class BehaviourTreeNodeScriptableObject<T> : ScriptableObject where T : BTNode
{
    public T NodeData;
}