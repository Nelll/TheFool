using UnityEngine;
using System.Collections.Generic;
public class SequenceNode : INode
{
    // 자식 노드를 순서대로 실행하기 위한 노드
    // Evaluate(평가)를 시작하면 sequence의 자식 노드를 왼쪽에서 오른쪽
    // (우선도가 높은 쪽에서 낮은 쪽)의 순서로 DFS(깊이 우선 탐색)방식으로 Evaluate(평가)합니다.
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
                case NodeState.Success: // 모든 자식 노드가 success 반환시 부모 노드에 success 반환
                    continue;
                case NodeState.Failure: // 자식 노드의 탐색을 중지, 부모 노드에 Failure 반환
                    return NodeState.Failure;
                case NodeState.Running: // 자식 노드의 탐색을 중지, 부모 노드에 Running 반환
                    return NodeState.Running;
            }
        }

        return NodeState.Success;
    }
}
