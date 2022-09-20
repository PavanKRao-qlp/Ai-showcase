using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class GOAP_VisualizerEditor : EditorWindow
{
    private GraphViewUI graph;
    private VisualElement inspector, warningText;

    private AIManager AIManager;

    [MenuItem("GOAP/Debug/Visualize")]
    public static void ShowExample()
    {
        GOAP_VisualizerEditor wnd = GetWindow<GOAP_VisualizerEditor>();
        wnd.titleContent = new GUIContent("GOAP_VisualizerEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        root.Add(splitView);

        var inspectorXml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/GOAP/Editor/Goap_Inspector.uxml");
        VisualElement inspectorUI = inspectorXml.CloneTree();
        splitView.Add(inspectorUI);
        this.inspector = inspectorUI;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/GOAP/Editor/GOAP_VisualizerEditor.uxml");
        VisualElement graphViewUI = visualTree.CloneTree();
        graph = graphViewUI.Q<GraphViewUI>("Graph");
        splitView.Add(graphViewUI);

        //var node = new NodeViewUI()
        //{
        //    name = "node",
        //    title = "hello!"
        //};
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

        if (!Application.isPlaying)
        {
            var label = new Label();
            WarnText("Must Be in Play mode to Debug!!", Color.yellow);
            return;
        }

        PopupulateUI();
    }


    void PopupulateUI()
    {
        AIManager = GameObject.FindObjectOfType<AIManager>();
        if (AIManager == null)
        {
            WarnText("AI Manager Not found!", Color.yellow);
            return;
        }
        if (AIManager.CurrentWorldState == null)
        {
            WarnText("CurrentWorldState Not found!", Color.yellow);
            return;
        }
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (AIManager == null)
            {
                var obj = GameObject.FindObjectOfType<AIManager>();
                if (obj) Repaint();
            }
        }
    }

    private void WarnText(string text , Color color)
    {
        if (warningText != null) graph.Remove(warningText);
        var label = new Label(text);
        warningText = label;
        label.transform.position = new Vector3(0, 0, 0);
        label.style.color = color;
        graph.Add(warningText);
    }
}