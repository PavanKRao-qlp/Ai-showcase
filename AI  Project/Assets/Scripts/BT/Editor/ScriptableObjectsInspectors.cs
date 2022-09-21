
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(BehaviorTreeScriptableObject))]
public class BehaviorTreeScriptableObjectEditor : Editor
{
    public VisualTreeAsset m_InspectorXML;

    public override VisualElement CreateInspectorGUI()
    {        
        VisualElement myInspector = new VisualElement();   
        InspectorElement.FillDefaultInspector(myInspector, serializedObject, this);       
        return myInspector;
    }
}