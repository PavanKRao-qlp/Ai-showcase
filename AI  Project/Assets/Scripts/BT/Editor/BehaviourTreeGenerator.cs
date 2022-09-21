using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class BehaviourTreeGenerator : EditorWindow
{
    private BehaviorTreeScriptableObject activeBehaviorTreeObj;
    private VisualElement inspectorPane;

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
        root.Add(uxmlPane);
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        inspectorPane = new VisualElement();
        splitView.Add(inspectorPane);
        var graphView = new GraphViewUI();
        splitView.Add(graphView);
        root.Add(splitView);     
        CheckActiveSelection();
    }

    private void CheckActiveSelection()
    {
        if (Selection.activeObject && Selection.activeObject.GetType() == typeof(BehaviorTreeScriptableObject))
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
        inspectorPane.Add(inspectElem);
        var NewNodeButton = new Button(() => {
            var childNode = ScriptableObject.CreateInstance<SequenceBTNNodeSO>();
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

    private void OnGUI()
    {

    }


    private void OnSelectionChange()
    {
        CheckActiveSelection();
    }    
}
