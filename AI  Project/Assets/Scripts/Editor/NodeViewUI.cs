using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class NodeViewUI : Node
{
    public new class UxmlFactory : UxmlFactory<NodeViewUI, GraphView.UxmlTraits> { }
    public NodeViewUI()
    {
        this.title = "Action"; 
    }
}
