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
        // onUpdate�� ���� null�� �ƴ� ��� ����
        // Invoke()�� onUpdate ������ ���� ��� �������� �ʰ� ���ִ� �Լ�
        // ??������ : onUpdate?.Invoke()�� null�� ��� NodeState.Failure�� ��ȯ
        return onUpdate?.Invoke() ?? NodeState.Failure;
    }
}
