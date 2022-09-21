
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
        var scriptableObj = target as BehaviorTreeScriptableObject;
        if (scriptableObj && EditorUtility.IsPersistent(scriptableObj))
        {
            if (scriptableObj.RootNode == null)
            {
                try
                {
                    var childNode = ScriptableObject.CreateInstance<RootBTNodeSO>();
                    childNode.Name = target.name;
                    AssetDatabase.AddObjectToAsset(childNode, scriptableObj);
                    scriptableObj.RootNode = childNode;
                    AssetDatabase.SaveAssets();
                    EditorUtility.SetDirty(scriptableObj);
                    EditorUtility.SetDirty(childNode);
                }catch(System.Exception e)
                {

                }
            }
        }
        return myInspector;
    }
}