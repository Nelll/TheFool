using UnityEngine;
using System;
public class ActionNode :INode
{
    Func<NodeState> onUpdate = null;

    public ActionNode(Func<NodeState> onUpdate)
    {
        this.onUpdate = onUpdate;
    }

    public NodeState Evaluate()
    {
        // onUpdate의 값이 null이 아닌 경우 실행
        // Invoke()는 onUpdate 내용이 없을 경우 실행하지 않게 해주는 함수
        // ??연산자 : onUpdate?.Invoke()가 null인 경우 NodeState.Failure를 반환
        return onUpdate?.Invoke() ?? NodeState.Failure;
    }
}
