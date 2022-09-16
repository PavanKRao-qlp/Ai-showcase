using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class GOAP_VisualizerEditor : EditorWindow
{
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

      
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/GOAP/Editor/GOAP_VisualizerEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/GOAP/Editor/GOAP_VisualizerEditor.uss");
        root.styleSheets.Add(styleSheet);
    }
}