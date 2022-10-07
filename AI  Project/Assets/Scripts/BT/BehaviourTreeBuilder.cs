using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeBuilder
{
    private BehaviorTree behaviorTree;
    private IBTNode currentNode;

    public BehaviourTreeBuilder()
    {
        behaviorTree = new BehaviorTree();
        currentNode = behaviorTree.RootNode;
    }
    public BehaviorTree BuildTree() {
        if(behaviorTree.Agent == null)
        {
            throw new MissingReferenceException("Agent attached is null, Did you miss calling AttachAgent()?");
        }
        return behaviorTree;
    }
    public BehaviourTreeBuilder AttachBlackBoard(Blackboard blackboard)
    {
        behaviorTree.Blackboard = blackboard;
        return this;
    }
    public BehaviourTreeBuilder AttachAgent(IAgentBT agent)
    {
        behaviorTree.Agent = agent;
        return this;
    }
    public BehaviourTreeBuilder AttachSelector(string tagName = "")
    {
        AttachComposite(new SelectorBTNode() { TagName = tagName });
        return this;
    }
    public BehaviourTreeBuilder AttachSequence(string tagName = "")
    {
        AttachComposite(new SequenceBTNode() { TagName = tagName });
        return this;
    }
    public BehaviourTreeBuilder AttachParallel()
    {
        AttachComposite(new ParallelBTNode());
        return this;
    }
    public BehaviourTreeBuilder AttachComposite<T>(T node) where T : CompositeBTNode
    {
        AttachChild(node);
        currentNode = node;
        return this;
    }
    public BehaviourTreeBuilder AttachDecorater<T>(T node) where T : DecoratorBTNode
    {
        AttachChild(node);
        currentNode = node;
        return this;
    }
    public BehaviourTreeBuilder AttachTask<T>(T node) where T : TaskBTNode
    {
        AttachChild(node);
        return this;
    }
    public BehaviourTreeBuilder AttachConditional<T>(T node) where T : ConditionalBTNode
    {
        AttachChild(node);
        return this;
    }
    public BehaviourTreeBuilder AttachMonitor<T>(T node) where T : MonitorBTNode
    {
        AttachChild(node);
        return this;
    }

    private void AttachChild(IBTNode node)
    {
        System.Type curNodeType = currentNode.GetType();
        if (curNodeType == typeof(RootBTNode))
        {
            (currentNode as RootBTNode).ChildNode = node;
            node.ParentNode = currentNode;
        }
        else if (typeof(CompositeBTNode).IsAssignableFrom(curNodeType))
        {
            (currentNode as CompositeBTNode).ChildNodes.Add(node);
            node.ParentNode = currentNode;
        }
        else if (typeof(DecoratorBTNode).IsAssignableFrom(curNodeType))
        {
            (currentNode as DecoratorBTNode).ChildNode = node;
            node.ParentNode = currentNode;
        }
        else
        {
            throw new System.Exception("Trying to attach to a task leaf node, check for a missing End() ?");
        }
        node.BT = behaviorTree;
    }

    private List<IBTNode> GetChildren(IBTNode node)
    {
        List<IBTNode> nodes = new List<IBTNode>();
        System.Type nodeType = node.GetType();
        if (nodeType == typeof(RootBTNode))
            nodes.Add((node as RootBTNode).ChildNode);
        else if (typeof(CompositeBTNode).IsAssignableFrom(nodeType))
            nodes.AddRange((node as CompositeBTNode).ChildNodes);
        else if (typeof(DecoratorBTNode).IsAssignableFrom(nodeType))
            nodes.Add((node as DecoratorBTNode).ChildNode);        
        return nodes;
    }

    public BehaviourTreeBuilder End()
    {
        if(currentNode.ParentNode == null)
            throw new System.Exception("Trying to end attach of a node with no parent node, check for an extra End() !");
        currentNode = currentNode?.ParentNode;
        return this;
    }
}
