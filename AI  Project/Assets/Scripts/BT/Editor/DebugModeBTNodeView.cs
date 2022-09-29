using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DebugModeBTNodeView : NodeViewUI
{
    public Port ParentPort;
    public DebugModeBTNodeView ParentNode;
    public List<DebugModeBTNodeView> ChildNodes = new List<DebugModeBTNodeView>();
    public IBTNode NodeData;
    public int childIx = 0;
    public int childSpan = 0;
    public int Ypos = 0;
}
