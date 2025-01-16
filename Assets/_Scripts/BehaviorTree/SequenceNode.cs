using UnityEngine;
using System.Collections.Generic;
public class SequenceNode : INode
{
    // �ڽ� ��带 ������� �����ϱ� ���� ���
    // Evaluate(��)�� �����ϸ� sequence�� �ڽ� ��带 ���ʿ��� ������
    // (�켱���� ���� �ʿ��� ���� ��)�� ������ DFS(���� �켱 Ž��)������� Evaluate(��)�մϴ�.
    List<INode> childs;

    public SequenceNode(List<INode> childs)
    {
        this.childs = childs;
    }

    public NodeState Evaluate()
    {
        if(childs == null || childs.Count == 0)
        {
            return NodeState.Failure;
        }

        foreach(INode child in childs)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success: // ��� �ڽ� ��尡 success ��ȯ�� �θ� ��忡 success ��ȯ
                    continue;
                case NodeState.Failure: // �ڽ� ����� Ž���� ����, �θ� ��忡 Failure ��ȯ
                    return NodeState.Failure;
                case NodeState.Running: // �ڽ� ����� Ž���� ����, �θ� ��忡 Running ��ȯ
                    return NodeState.Running;
            }
        }

        return NodeState.Success;
    }
}
