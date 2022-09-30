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
        this.outputContainer.BringToFront();
        this.outputContainer.style.borderBottomLeftRadius = this.outputContainer.style.borderBottomRightRadius = 25;
        this.inputContainer.style.borderTopLeftRadius = this.inputContainer.style.borderTopRightRadius = 25;
        this.titleContainer.style.borderTopLeftRadius = this.titleContainer.style.borderTopRightRadius = this.titleContainer.style.borderBottomLeftRadius = this.titleContainer.style.borderBottomRightRadius = 5;
        DefaultColour = titleContainer.style.backgroundColor;
    }

    public void RefreshUIOnTick(IBTNode.ReturnStatus status)
    {
       // Debug.Log($"Pvn {NodeData.GetType().Name} {NodeData.Name}  {status}");
        switch (status)
        {
            case IBTNode.ReturnStatus.INACTIVE:
                {
                    titleContainer.style.backgroundColor = DefaultColour;
                    break;
                }
            case IBTNode.ReturnStatus.RUNNING:
                {
                    titleContainer.style.backgroundColor = new Color(150,150,0,205); break;
                }
            case IBTNode.ReturnStatus.FAILED: { titleContainer.style.backgroundColor = new Color(150, 0, 0); break; }
            case IBTNode.ReturnStatus.SUCCESS: { titleContainer.style.backgroundColor = new Color(0, 50, 0, 205 ); break; }
            default: break;
        }
    }
}
