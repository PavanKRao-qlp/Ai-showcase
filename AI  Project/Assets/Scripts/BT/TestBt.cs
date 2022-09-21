using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBt : MonoBehaviour
{
    BehaviorTree behaviorTree;
    // Start is called before the first frame update
    void Start()
    {
        behaviorTree = new BehaviorTree();
        var rootNode = new RootBTNode();
        behaviorTree.RootNode = rootNode;
        var childNode = new SequenceBTNode();
        rootNode.AddChild(childNode);

        var sequenceNode = new SequenceBTNode();
            sequenceNode.Add(new ColorBTNode(Color.blue));
            sequenceNode.Add(new WaitBTNode(5));
            sequenceNode.Add(new ColorBTNode(Color.yellow)); 
            sequenceNode.Add(new WaitBTNode(1));
        childNode.Add(sequenceNode);

        var selctorNode = new SelectorBTNode();
            var sequenceNode2 = new SequenceBTNode();
            sequenceNode2.Add(new BoolBTNode(false));
            sequenceNode2.Add(new ColorBTNode(Color.green));
        selctorNode.Add(sequenceNode2);
        selctorNode.Add(new ColorBTNode(Color.red));
        childNode.Add(selctorNode);
    }

    // Update is called once per frame
    void Update()
    {
        behaviorTree.Tick();
    }

}

[System.Serializable]
public class ColorBTNode : TaskNode
{
    public Color color;
    bool isDone = false;
    public ColorBTNode(Color color)
    {
        this.color = color;
    }
    public override BTNode.ReturnStatus Tick()
    {
        if (isDone) return BTNode.ReturnStatus.SUCCESS;
        var img = GameObject.Find("player")?.GetComponent<UnityEngine.UI.Image>() ?? null;
        if (img == null) return BTNode.ReturnStatus.FAILED;
        img.color = color;
        isDone = true;
        return BTNode.ReturnStatus.SUCCESS;
    }
    void Reset()
    {
        isDone = false;
    }
}
public class WaitBTNode : TaskNode
{
    bool isDone = false;
    public WaitBTNode(int second) { secs = second; }
    float currentDt = 0;
    int secs;

    public override BTNode.ReturnStatus Tick()
    {
        if (isDone) return BTNode.ReturnStatus.SUCCESS;
        currentDt += Time.deltaTime;
        if (currentDt >= secs)
        {
            return BTNode.ReturnStatus.SUCCESS;
        }
        return BTNode.ReturnStatus.RUNNING;
    }
    void Reset()
    {
        isDone = false;
    }
}
public class BoolBTNode : TaskNode
{
    private bool status;
    public BoolBTNode(bool retStatus) { status = retStatus; }

    public override BTNode.ReturnStatus Tick()
    {
        return status ? BTNode.ReturnStatus.SUCCESS : BTNode.ReturnStatus.FAILED;
    }
}
