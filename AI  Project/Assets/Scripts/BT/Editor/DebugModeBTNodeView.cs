using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugModeBTNodeView : NodeViewUI
{
    public Port ParentPort;
    public Edge ParentEdge;
    public DebugModeBTNodeView ParentNode;
    public List<DebugModeBTNodeView> ChildNodes = new List<DebugModeBTNodeView>();
    public IBTNode NodeData;
    public int childIx = 0;
    public int childSpan = 0;
    public int Ypos = 0;
    private StyleColor DefaultColour;

    public DebugModeBTNodeView() {

        this.topContainer.style.flexDirection = FlexDirection.Column;
        this.inputContainer.style.flexDirection = FlexDirection.Row;
        this.outputContainer.style.flexDirection = FlexDirection.Row;
        this.topContainer.Add(this.titleContainer);
        this.inputContainer.style.alignSelf = Align.Center;
        this.outputContainer.style.alignSelf = Align.Center;
        this.outputContainer.style.borderBottomLeftRadius = this.outputContainer.style.borderBottomRightRadius = 25;
        this.inputContainer.style.borderTopLeftRadius = this.inputContainer.style.borderTopRightRadius = 25;
        this.titleContainer.style.borderTopLeftRadius = this.titleContainer.style.borderTopRightRadius = this.titleContainer.style.borderBottomLeftRadius = this.titleContainer.style.borderBottomRightRadius = 5;
        this.titleContainer.ElementAt(0).style.fontSize = 22;
        this.titleContainer.ElementAt(0).style.unityFontStyleAndWeight = FontStyle.Bold;
        this.titleContainer.style.borderLeftWidth = this.titleContainer.style.borderRightWidth = this.titleContainer.style.borderTopWidth = 2f;
        this.titleContainer.style.borderBottomWidth = 10f;
        titleContainer.style.flexDirection = FlexDirection.Column;
        DefaultColour = titleContainer.style.backgroundColor;
        this.outputContainer.BringToFront();
    }

    public void RefreshUIOnTick(IBTNode.ReturnStatus status)
    {
        switch (status)
        {
            case IBTNode.ReturnStatus.INACTIVE:
                {
                    if (ParentPort != null)
                        ParentPort.portColor = Color.white;
                    this.titleContainer.style.borderLeftColor = this.titleContainer.style.borderRightColor = this.titleContainer.style.borderTopColor = this.titleContainer.style.borderBottomColor = DefaultColour;
                    break;
                }
            case IBTNode.ReturnStatus.RUNNING:
                {                  
                    if (ParentPort != null)
                        ParentPort.portColor = Color.yellow;
                    this.titleContainer.style.borderLeftColor = this.titleContainer.style.borderRightColor = this.titleContainer.style.borderTopColor = this.titleContainer.style.borderBottomColor = Color.yellow;
                    break;
                }
            case IBTNode.ReturnStatus.FAILURE: { 
                    if (ParentPort != null)
                        ParentPort.portColor = Color.red;
                    this.titleContainer.style.borderLeftColor = this.titleContainer.style.borderRightColor = this.titleContainer.style.borderTopColor = this.titleContainer.style.borderBottomColor = Color.red;
                    break; 
                }
            case IBTNode.ReturnStatus.SUCCESS: {
                    if (ParentPort != null)
                        ParentPort.portColor = Color.green;
                    this.titleContainer.style.borderLeftColor = this.titleContainer.style.borderRightColor = this.titleContainer.style.borderTopColor = this.titleContainer.style.borderBottomColor = Color.green;
                    break; }
            case IBTNode.ReturnStatus.ABORTED: {
                    if (ParentPort != null)
                        ParentPort.portColor = Color.magenta;
                    this.titleContainer.style.borderLeftColor = this.titleContainer.style.borderRightColor = this.titleContainer.style.borderTopColor = this.titleContainer.style.borderBottomColor = Color.magenta;
                    break;
                }
            default: break;
        }
    }

    internal void Update()
    {
        RefreshUIOnTick(NodeData.status);
    }
}
