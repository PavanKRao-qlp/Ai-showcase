using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BTDebugger : EditorWindow
{
    private IAgentBT activeBtAgent;
    private VisualElement inspectorPane;
    private BTDebugModeGraph graph;
    private Label header;
    private bool redrawTree = true;

    [MenuItem("AI/BT/BT Debugger")]
    public static void ShowWindow()
    {
        BTDebugger wnd = GetWindow<BTDebugger>();
        wnd.titleContent = new GUIContent("BTDebugger");
    }
    public void CreateGUI()
    {
        redrawTree = true;
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BT/Editor/BehaviourTreeDebugger.uxml");
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BT/Editor/BehaviourTreeGenerator.uss");
        header = new Label("Select a lol");
        root.Add(header);
        VisualElement uxmlPane = visualTree.Instantiate();
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        inspectorPane = new VisualElement();
        splitView.Add(inspectorPane);
        graph = uxmlPane.Q<BTDebugModeGraph>("graph");
        splitView.Add(uxmlPane);
        root.Add(splitView);
        if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<IAgentBT>() != null)
        {
            activeBtAgent = Selection.activeGameObject.GetComponent<IAgentBT>();
        }
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            header.text = "Must be in playmode to visualize!";
            return;

        }
        if (activeBtAgent == null)
        {
            header.text = "No BT agent selected!";
            return;
        }
        if (activeBtAgent.ActiveBehaviorTree == null)
        {
            header.text = "No Behaviour tree found on selected agent!";
            return;
        }
        if (redrawTree == true)
        {
            RebuildTree();
            redrawTree = false;
            header.text = "";
        }
       
    }

    private void OnSelectionChange()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<IAgentBT>() != null)
        {
            var agent = Selection.activeGameObject.GetComponent<IAgentBT>();
            if (agent != null && agent != activeBtAgent)
            {
                activeBtAgent = agent;
                redrawTree = true;
            }
        }
    }

    private void RebuildTree()
    {
        graph.BuildTree(activeBtAgent.ActiveBehaviorTree);
    }

    private void OnDestroy()
    {
      
    }
}
