using UnityEngine;
using System.Collections.Generic;
public class SelectorNode : INode
{
    // �ڽ� ��� ��� �ϳ��� �����ϱ� ���� ���
    // Evaluate(��)�� �����ϸ� selector�� �ڽĳ�带 ���ʿ��� ������
    // (�켱���� ���� �ʿ��� ������)�� ������ DFS(���� �켱 Ž��)������� Evaluate(��)�մϴ�.
    List<INode> childs;

    public SelectorNode(List<INode> childs)
    {
        this.childs = childs;
    }

    public NodeState Evaluate()
    {
        if(childs == null)
        {
            return NodeState.Failure;
        }

        foreach(INode child in childs)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Running:
                    return NodeState.Running;
            }
        }

        return NodeState.Failure;
    }
}
