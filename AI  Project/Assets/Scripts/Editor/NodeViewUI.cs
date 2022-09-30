using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class NodeViewUI : Node
{
    public new class UxmlFactory : UxmlFactory<NodeViewUI, GraphView.UxmlTraits> { }
    public NodeViewUI(string ufile) : base (ufile)
    {
        var styleSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/GraphView.uss");
        this.title = "Action";
    }
    public NodeViewUI()
    {
        var styleSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/GraphView.uss");
        this.title = "Action";
    }
}
