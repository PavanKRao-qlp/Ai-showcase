using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Newtonsoft.Json;

[CustomEditor(typeof(AIManager))]
public class AIManagerInspector : Editor
{
    Label blackBoardLabel;
    public override VisualElement CreateInspectorGUI()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        // Create a new VisualElement to be the root of our inspector UI
        VisualElement myInspector = new VisualElement();
        InspectorElement.FillDefaultInspector(myInspector, serializedObject, this);
        blackBoardLabel = new Label("Blackboard only in playmode");
        myInspector.Add(blackBoardLabel);
        var button = new Button(() => {
            if (Application.isPlaying)
                blackBoardLabel.text = ((AIManager)target).BlackBoard.ToString();
        });
        button.text = "refresh";
        myInspector.Add(button);
        return myInspector;        
    }


    private void OnDestroy()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
    void OnPlayModeStateChanged(PlayModeStateChange stateChange)
    {
        if(stateChange == PlayModeStateChange.ExitingPlayMode)
        {
           // blackBoardLabel = new Label("Blackboard only in playmode");
        }
    }

}
