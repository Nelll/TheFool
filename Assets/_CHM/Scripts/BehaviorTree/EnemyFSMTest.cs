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
        // => Behavior Graph의 Blackboard에 선언한 변수 값을 설정
        // wayPoints.ToList() 배열 데이터를 리스트로 변경
        behaviorAgent.SetVariableValue("PatrolPoints", wayPoints.ToList());
    }

    // 행동 제어는 Behavior Graph에서 하기 때문에 Update()는 사용 X
}
