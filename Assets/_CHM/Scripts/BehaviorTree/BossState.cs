using System;
using Unity.Behavior;

[BlackboardEnum]
public enum BossState
{
    Idle,
	Patrol,
	Wander,
	Chase,
	Attack
}
