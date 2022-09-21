using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class BehaviourTreeGenerator : EditorWindow
{
    private BehaviorTreeScriptableObject activeBehaviorTreeObj;
    private VisualElement inspectorPane;
    private BTEditModeGraph graph;
    private Label header;



    [MenuItem("AI/BT/BehaviourTreeGenerator")]
    public static void ShowWindow()
    {
        BehaviourTreeGenerator wnd = GetWindow<BehaviourTreeGenerator>();
        wnd.titleContent = new GUIContent("BehaviourTreeGenerator");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BT/Editor/BehaviourTreeGenerator.uxml");
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BT/Editor/BehaviourTreeGenerator.uss");
        header = new Label("Select a BT scriptable object!");
        root.Add(header);
        VisualElement uxmlPane = visualTree.Instantiate();       
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        inspectorPane = new VisualElement();
        splitView.Add(inspectorPane);
        graph = uxmlPane.Q<BTEditModeGraph>("graph");
        graph.BuildNodeAction = OnBuildNodeAction;
        splitView.Add(uxmlPane);
        root.Add(splitView);     
        CheckActiveSelection();
    }

    private void CheckActiveSelection()
    {
        if (Selection.activeObject && Selection.activeObject.GetType() == typeof(BehaviorTreeScriptableObject) && EditorUtility.IsPersistent(Selection.activeObject))
        {
            header.text = Selection.activeObject.name;
            activeBehaviorTreeObj = Selection.activeObject as BehaviorTreeScriptableObject;
            DrawGUI();
        }
    }

    private void DrawGUI()
    {
        var inspectElem = new InspectorElement(activeBehaviorTreeObj);
        inspectorPane.Clear();
       // graph.Clear();
        inspectorPane.Add(inspectElem);
        var node = BuildGraphNode(activeBehaviorTreeObj.RootNode);
       // node.style.top = 200;
        if(node != null) graph.AddElement(node);
        var NewNodeButton = new Button(() => {
            var childNode = ScriptableObject.CreateInstance<SequenceBTNodeSO>();
            AssetDatabase.AddObjectToAsset(childNode, activeBehaviorTreeObj);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(activeBehaviorTreeObj);
            EditorUtility.SetDirty(childNode);
            activeBehaviorTreeObj.childNodes.Add(childNode);
        });
        NewNodeButton.text = "NEW CHILD NODE!";
        inspectorPane.Add(NewNodeButton);
        this.Repaint();

    }

    public void OnBuildNodeAction(System.Type type)
    {
        graph.AddElement(BuildGraphNode(null));
    }
    private NodeViewUI BuildGraphNode(BTNodeSO nodeSO)
    {
        var node = new NodeViewUI()
        {
            name = "node",
            title = "hello!"
        };

        node.titleButtonContainer.Add(new Button() { text = "Click Me!" });
        node.titleContainer.Add(new Button() { text = "Click Me!" });
        node.extensionContainer.Add(new Label("Hello extension!"));
        node.inputContainer.Add(new Label("Hello input!"));
        node.mainContainer.Add(new Label("Hello main!"));
        node.outputContainer.Add(new Label("Hello output!"));
        node.topContainer.Add(new Label("Hello Top!"));
        node.contentContainer.Add(new Label("Hello Content!"));
        node.RefreshPorts();
        node.RefreshExpandedState();

        return node;
    }


    private void OnGUI()
    {

    }


    private void OnSelectionChange()
    {
        CheckActiveSelection();
    }    
}
