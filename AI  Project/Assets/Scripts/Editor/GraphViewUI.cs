using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewUI : GraphView
{
    public new class UxmlFactory : UxmlFactory<GraphViewUI, GraphView.UxmlTraits> { }
    public GraphViewUI() {
        Insert(0, new GridBackground());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        var styleSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/GraphView.uss");
        styleSheets.Add(styleSheet);
    }
}
