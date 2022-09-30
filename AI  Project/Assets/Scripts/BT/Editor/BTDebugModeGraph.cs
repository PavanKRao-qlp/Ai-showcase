using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BTDebugModeGraph : GraphViewUI
{
    public new class UxmlFactory : UxmlFactory<BTDebugModeGraph, GraphView.UxmlTraits> { }
   
    public void BuildTree(BehaviorTree tree)
    {
        ResetTree(); 
        var nodeView = PropagateNodes(tree.RootNode, 0); 
        CalculateHeight(nodeView);
        PositionNodes(nodeView, Vector2.zero, 0);
    }

    private void PositionNodes(DebugModeBTNodeView nodeView, Vector2 pos, int width = 0)
    {
        int space = 200;
        nodeView.style.top = pos.x;
        int yIx = 0;
        if (nodeView.ParentNode != null) {
            yIx += nodeView.ParentNode.Ypos;
            for (int i = 0; i < nodeView.childIx; i++)
            {
                yIx += nodeView.ParentNode.ChildNodes[i].childSpan;
            }
        }
       // if (width > 3) nodeView.style.visibility = Visibility.Hidden;
        nodeView.style.left  = yIx * space + (space * (nodeView.childSpan/2f));
        nodeView.Ypos = yIx;
        for (int i = 0; i < nodeView.ChildNodes.Count; i++)
        {
            PositionNodes(nodeView.ChildNodes[i], new Vector2(pos.x + space, 0 ), width + 1);
        }
    }

    private int CalculateHeight(DebugModeBTNodeView node)
    {
        int h = 0;
        foreach (var child in node.ChildNodes)
        {
            h += CalculateHeight(child);
        }
        if (node.ChildNodes.Count == 0) h = 1;
        node.childSpan = h;
        return h;
    }

    private void ResetTree()
    {
        foreach (var node in graphElements)
        {
            RemoveElement(node);
        }
    }

    private DebugModeBTNodeView PropagateNodes(IBTNode node, int width = 0)
    {
        DebugModeBTNodeView nodeView = null;
        Type nodeType = node.GetType();
        nodeView = GenerateNodeView(node);
       
        var childList = new List<IBTNode>();
        if (nodeType == typeof(RootBTNode))
        {
            childList.Add((node as RootBTNode).ChildNode);
            nodeView.ParentNode = null;           
        }
        else if ((typeof(CompositeBTNode).IsAssignableFrom(nodeType)))
        {
            nodeView.ParentPort = GeneratePort(nodeView, Direction.Input, nodeView.inputContainer);
            foreach (var child in (node as CompositeBTNode).ChildNodes)
            {
                childList.Add(child);
            }
        }
        else if ((typeof(DecoratorBTNode).IsAssignableFrom(nodeType)))
        {
            nodeView.ParentPort = GeneratePort(nodeView, Direction.Input, nodeView.inputContainer);
            childList.Add((node as DecoratorBTNode).ChildNode);
        }
        else if ((typeof(TaskBTNode).IsAssignableFrom(nodeType)))
        {
            nodeView.ParentPort = GeneratePort(nodeView, Direction.Input, nodeView.inputContainer);
        }

        var ix = 0;
        float h = -50 * childList.Count;
        foreach (var child in childList)
        {
            var childNode = PropagateNodes(child , width+1);
            childNode.childIx = ix;
            var childPort = GeneratePort(nodeView, Direction.Output, nodeView.outputContainer);
            nodeView.ChildNodes.Add(childNode);
            childNode.ParentNode = nodeView;
            var edge = ConnectPorts(childPort, childNode.ParentPort);
            childNode.ParentEdge = edge;
            ix++;
        }
        //var vsElement = new VisualElement();
        //vsElement.style.backgroundColor = new Color(15, 15, 15);
        //vsElement.Add(new Label("G"));
        //nodeView.contentContainer.Add(vsElement);
        nodeView.RefreshPorts();
        nodeView.RefreshExpandedState();
        return nodeView;
    }

    private DebugModeBTNodeView GenerateNodeView(IBTNode node)
    {
        DebugModeBTNodeView nodeView = null;
        Type nodeType = node.GetType();
        nodeView = new DebugModeBTNodeView()
        {
            name = nodeType.Name,
            title = nodeType.Name,
            NodeData = node
        };
        node.OnTick += nodeView.RefreshUIOnTick;
        this.AddElement(nodeView);
        return (nodeView);
    }

    private Port GeneratePort(DebugModeBTNodeView nodeView ,Direction direction, VisualElement container)
    {
        Port port = nodeView.InstantiatePort(Orientation.Vertical,direction, Port.Capacity.Single, typeof(IBTNode));
        port.portName = "";
        container.Add(port);
        return port;
    }
    private Edge ConnectPorts(Port inPort, Port outPort)
    {
        var edge = inPort.ConnectTo(outPort);
       // edge.pickingMode = PickingMode.Ignore;
        AddElement(edge);
        return edge;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var validPorts = new List<Port>();
        ports.ForEach((port) => { if (startPort != port && startPort.direction != port.direction && port != null) validPorts.Add(port); });
        return validPorts;
    }
}
