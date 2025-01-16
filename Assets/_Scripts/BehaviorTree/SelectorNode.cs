using UnityEngine;
using System.Collections.Generic;
public class SelectorNode : INode
{
    // 자식 노드 가운데 하나를 실행하기 위한 노드
    // Evaluate(평가)를 시작하면 selector의 자식노드를 왼쪽에서 오른쪽
    // (우선도가 높은 쪽에서 낮은쪽)의 순서로 DFS(깊이 우선 탐색)방식으로 Evaluate(평가)합니다.
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
