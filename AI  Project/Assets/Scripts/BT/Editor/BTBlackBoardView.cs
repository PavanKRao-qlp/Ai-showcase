using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BTBlackBoardView : VisualElement
{
    VisualElement blackBoardHolder;
    private struct EntityUIData
    {
        public VisualElement ParentElement;
        public Dictionary<string, Label> EntityValue;
    }
    private Dictionary<string, EntityUIData> entityDict = new Dictionary<string, EntityUIData>();
    public void Initialize()
    {
        this.Clear();
        entityDict = new Dictionary<string, EntityUIData>();
        blackBoardHolder = new VisualElement();
        this.Add(blackBoardHolder);
    }

    internal void DrawBlackBoard(Blackboard blackboard)
    {
        foreach (var pair in blackboard.Data)
        {
            if (!entityDict.ContainsKey(pair.Key))
            {
                var node = new Foldout();
                node.text = pair.Key;
                blackBoardHolder.Add(node);
                entityDict.Add(pair.Key, new EntityUIData()
                {
                    EntityValue = new Dictionary<string, Label>(),
                    ParentElement = node
                });
            }
            var dict = new Dictionary<string, object>(pair.Value);
            foreach (var data in dict)
            {
                if (!entityDict[pair.Key].EntityValue.ContainsKey(data.Key))
                {
                    var blackBoardValue = new Box();
                    blackBoardValue.style.backgroundColor = new Color(0, 0, 0, 0.5f);
                    blackBoardValue.style.flexDirection = FlexDirection.Row;
                    blackBoardValue.Add(new Label(data.Key));
                    var valHolder = new Box();
                    valHolder.style.backgroundColor = new Color(0, 0, 0, 30);
                    var val = new Label(data.Value.ToString());
                    valHolder.Add(val);
                    blackBoardValue.Add(valHolder);
                    entityDict[pair.Key].EntityValue.Add(data.Key, val);
                    entityDict[pair.Key].ParentElement.Add(blackBoardValue);
                }
                else
                {
                    entityDict[pair.Key].EntityValue[data.Key].text = data.Value.ToString();
                }
            }
        }
    }
}