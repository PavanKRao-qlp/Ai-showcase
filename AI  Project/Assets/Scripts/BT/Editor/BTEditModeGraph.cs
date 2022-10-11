using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BTEditModeGraph : GraphViewUI
{
    public new class UxmlFactory : UxmlFactory<BTEditModeGraph, GraphView.UxmlTraits> { }

    public System.Action<System.Type> BuildNodeAction;


    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        var types = TypeCache.GetTypesDerivedFrom<BTNodeSO>();
        foreach( var type in types)
        {
            if (type == typeof(RootBTNodeSO) || type == typeof(CompositeBTNodeSO)) continue;
            string submenu = "MISC";
            if (type.IsSubclassOf(typeof(CompositeBTNodeSO)))
            {
                submenu = "Compiste";
            }
            var comp = type.IsSubclassOf(typeof(CompositeBTNode));
            evt.menu.AppendAction($"{submenu}/{type.Name.Replace("BTNodeSO","")}", (action) => { BuildNodeAction?.Invoke(type); });
        }
    }

    private NodeViewUI BuildGraphNode(System.Type type)
    {
        return null;
    }

}
