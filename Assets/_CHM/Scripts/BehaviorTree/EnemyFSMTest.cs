using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;

public class EnemyFSMTest : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent navMeshAgent;
    private BehaviorGraphAgent behaviorAgent;

    public void Setup(Transform target, GameObject[] wayPoints)
    {
        this.target = target;

        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        navMeshAgent.updateUpAxis = false;

        // bool BehaviorGraphAgent.SetVariableValue<T>(string variableName, <T> value);
        // => Behavior Graph�� Blackboard�� ������ ���� ���� ����
        // wayPoints.ToList() �迭 �����͸� ����Ʈ�� ����
        behaviorAgent.SetVariableValue("PatrolPoints", wayPoints.ToList());
    }

    // �ൿ ����� Behavior Graph���� �ϱ� ������ Update()�� ��� X
}
