using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class GOAP_VisualizerEditor : EditorWindow
{
    private GraphViewUI graph;
    private VisualElement element;
    [MenuItem("GOAP/Debug/Visualize")]
    public static void ShowExample()
    {
        GOAP_VisualizerEditor wnd = GetWindow<GOAP_VisualizerEditor>();
        wnd.titleContent = new GUIContent("GOAP_VisualizerEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        root.Add(splitView);

        var inspector = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/GOAP/Editor/Goap_Inspector.uxml");
        VisualElement inspectorUI = inspector.CloneTree();
        splitView.Add(inspectorUI);
        element = inspectorUI;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/GOAP/Editor/GOAP_VisualizerEditor.uxml");
        VisualElement graphViewUI = visualTree.CloneTree();
        graph = graphViewUI.Q<GraphViewUI>("Graph");
        splitView.Add(graphViewUI);



        var node = new NodeViewUI()
        {
            name = "node",
            title = "hello!"
        };

        //node.titleButtonContainer.Add(new Button() { text = "Click Me!" });
        //node.titleContainer.Add(new Button() { text = "Click Me!" });
        //node.extensionContainer.Add(new Label("Hello extension!"));
        //node.inputContainer.Add(new Label("Hello input!"));
        //node.mainContainer.Add(new Label("Hello main!"));
        //node.outputContainer.Add(new Label("Hello output!"));
        //node.topContainer.Add(new Label("Hello Top!"));
        //node.contentContainer.Add(new Label("Hello Content!"));
        //graph.AddElement(node);
        //splitView.Add(graphViewUI);
        //node.RefreshPorts();
        //node.RefreshExpandedState();

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/GOAP/Editor/GOAP_VisualizerEditor.uss");
        root.styleSheets.Add(styleSheet);
    }
}