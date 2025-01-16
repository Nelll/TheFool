using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wander", story: "[Self] Navigate To WanderPosition", category: "Action", id: "d92daf8bde23193686df541a5864c0dd")]
public partial class WanderAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private NavMeshAgent agent;
    private Vector3 wanderPosition;
    private float currentWanderTime = 0f;
    private float maxWanderTime = 5f;

    protected override Status OnStart()
    {
        int jitterMin = 0; // 최소 각도
        int jitterMax = 360; // 최대 각도
        float wanderRadius = UnityEngine.Random.Range(2.5f, 6f);
        int wanderJitter = UnityEngine.Random.Range(jitterMin, jitterMax);

        // 목표 위치 = 자신(Self)의 위치 + 각도(wanderJitter)에 해당하는 반지름(wanderRadius) 크기의 원의 둘레 위치
        wanderPosition = Self.Value.transform.position + Utils.GetPositionFromAngle(wanderRadius, wanderJitter);
        agent = Self.Value.GetComponent<NavMeshAgent>();
        agent.SetDestination(wanderPosition);
        currentWanderTime = Time.time;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if((wanderPosition - Self.Value.transform.position).sqrMagnitude < 0.1f
            || Time.time - currentWanderTime > maxWanderTime)
        {
            return Status.Success;
        }

        return Status.Running;
    }
}

