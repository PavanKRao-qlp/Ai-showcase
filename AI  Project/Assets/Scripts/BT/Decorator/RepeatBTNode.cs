public class RepeatBTNode : DecoratorBTNode
{
    private int repeatCount = -1;
    private int timesRepeated = 0;

    public RepeatBTNode(int repeatCount = -1) { this.repeatCount = repeatCount; }  

    public override void OnEnter()
    {
        timesRepeated = 0;
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        timesRepeated = 0;
        Reset();
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        var status = ChildNode.Tick();
        if (status != IBTNode.ReturnStatus.RUNNING) {
            timesRepeated++;
            ChildNode.Reset();
        }
        return (repeatCount == -1 || timesRepeated < repeatCount) ? IBTNode.ReturnStatus.RUNNING : status;
    }

    public override void Reset()
    {
        this.status = IBTNode.ReturnStatus.INACTIVE;
        ChildNode.Reset();
    }
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }
}
