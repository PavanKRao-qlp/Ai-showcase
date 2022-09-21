using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New_BT", menuName = "Scriptable/AI/BT", order = 1)]
public class BehaviorTreeScriptableObject : ScriptableObject
{
    [HideInInspector] public int BehaviorTree = 10;
    [HideInInspector] public BTNodeSO childNode;
    public List<BTNodeSO> childNodes;
    public BehaviorTreeScriptableObject()
    {
        childNodes = new List<BTNodeSO>();
    }
    public BehaviorTree BuildBehaviorTree()
    {
        return null;
    }  
}
public class BehaviourTreeNodeScriptableObject<T> : ScriptableObject where T : BTNode
{
    public T NodeData;
}

