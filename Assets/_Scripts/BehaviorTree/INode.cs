public enum NodeState
{
    Running,
    Success,
    Failure
}

public interface INode
{
    public NodeState Evaluate();
}
