using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UIElements;
using InfinityPBR;
using TMPro;
public class BossBehaviorAI : MonoBehaviour
{
    [SerializeField] private float detectRange = 10f;   // 탐지 범위
    [SerializeField] private float attackRange = 5f;    // 공격 범위
    [SerializeField] private float moveSpeed = 3f;      // 이동 속도 
    [SerializeField] private float rotationSpeed = 10;  // 회전 속도

    [SerializeField] private GameObject[] breatheParticles; // 브레스 파티클
    [SerializeField] private GameObject[] breatheProp;      // 브레스 파티클 제외 기타 요소
    [SerializeField] private GameObject[] HitDamageBoxes;   // 데미지 받는 콜라이더

    Vector3 originPosition;
    BehaviorTreeRunner btRunner;
    Transform detectedPlayer;
    Animator animator;
    Health health;

    bool isWake = false;
    int damage;
    int rand;

    public int Damage // 외부에 데미지 값 공유
    { 
        get { return damage; }
        set { damage = value; }
    }


    private void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        originPosition = transform.position;
        StartSleep();

        btRunner = new BehaviorTreeRunner(SettingAttackBT()); // RootNode 세팅
    }

    private void Update()
    {
        btRunner.Operate(); // Behavior Tree 반복
        rand = Random.Range(1, 10);

        if (detectedPlayer != null)
        {
            if (Vector3.Magnitude(detectedPlayer.position - transform.position) < attackRange)
            {
                LookAtDetected(detectedPlayer.position, 5);
            }
        }
    }

    // RootNode
    INode SettingAttackBT()
    {
        INode root = new SelectorNode // 원하는 액션을 실행시키기 위한 노드
        (
            // childs
            new List<INode>
            {
                new SequenceNode // 탐색 노드, Attack
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckIsAttacking),       // 공격 했는지 체크
                        new ActionNode(CheckInAttackRange),       // 공격 범위 내부인지 체크
                        new ActionNode(DoAttack)                // 공격
                    }
                ),
                new SequenceNode // 탐색 노드, Chase
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckDetectEnemy),     // 적 탐지
                        new ActionNode(MoveToDetectedEnemy)     // 탐지된 적에게 이동
                    }
                ),
                new ActionNode(ReturnToOrigin)                  // 원래 있던 자리로 돌아가는 액션
            }
        );

        return root; // 액션 실행
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    NodeState CheckIsAttacking()
    {
        // 공격 중인지 체크?
        // 보스가 가진 공격 애니메이션은 전부 추가할 필요가 있는 듯하다.
        //if(state == State.Attack || state == State.FlyAttack)
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            return NodeState.Running;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            return NodeState.Running;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TailWhipL"))
        {
            return NodeState.Running;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BreatheFire"))
        {
            return NodeState.Running;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Breathe Fire Set"))
        {
            return NodeState.Running;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Attack Set"))
        {
            return NodeState.Running;
        }
        return NodeState.Success;
    }

    NodeState CheckInAttackRange()
    {
        // 공격 범위로 들어왔는지 체크
        if (detectedPlayer != null)
        {
            if(Vector3.Magnitude(detectedPlayer.position - transform.position) < attackRange)
            {
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }

    NodeState DoAttack()
    {
        if (detectedPlayer != null)
        {
            if(isWake == true)
            {
                // 랜덤 공격 페이즈
                if (health.currentHealth >= health.maxHealth * 0.6)
                {
                    FirstPhase();
                }
                if (health.currentHealth <= health.maxHealth * 0.6 && health.currentHealth > health.maxHealth * 0.3) // 최대 체력의 60퍼센트 이하일 때
                {
                    SecondPhase();
                }
                if (health.currentHealth <= health.maxHealth * 0.3) // 최대 체력의 30퍼센트 이하일 때
                {
                    FinalPhase();
                }
            }
            return NodeState.Success;
        }
        return NodeState.Failure;
    }

    NodeState CheckDetectEnemy()
    {
        // 스피어 콜라이더 범위 안에 들어오는 Player 레이어로 된 오브젝트 정보를 가져온다.
        // Physic.OverlapSphere(시작 위치, 반지름, 대상 레이어)
        Collider[] overlaps = Physics.OverlapSphere(transform.position, detectRange, LayerMask.GetMask("Player"));

        if (overlaps != null && overlaps.Length > 0)
        {
            detectedPlayer = overlaps[0].transform;

            if (isWake == false)
            {
                StartWakeUp();
            }

            return NodeState.Success;
        }
        detectedPlayer = null;

        return NodeState.Failure;
    }

    NodeState MoveToDetectedEnemy()
    {
        if (detectedPlayer != null)
        {
            if(Vector3.Distance(detectedPlayer.position, transform.position) < attackRange)
            {
                return NodeState.Success;
            }
            if(isWake == true)
            {
                StartMove(detectedPlayer.position);
            }
            return NodeState.Running;
        }
        return NodeState.Failure;
    }

    NodeState ReturnToOrigin()
    {
        if (Vector3.Distance(originPosition, transform.position) < 0.1)
        {
            if(isWake == true)
            {
                StartIdle();
            }
            return NodeState.Success;
        }
        else
        {
            StartMove(originPosition);
            return NodeState.Running;
        }
    }


    void FirstPhase()
    {
        if (rand <= 2) // 20퍼센트 확률로 쉬기
        {
            StartIdle();
        }
        else if (rand <= 4) // 20퍼센트 확률로 발톱 공격
        {
            if (Vector3.Distance(detectedPlayer.position, transform.position) < 4f)
            {
                StartAttack02();
            }
        }
        else if (rand <= 6) // 20퍼센트 확률로 물기
        {
            StartAttack01();
        }
        else if (rand <= 8) // 20퍼센트 확률로 꼬리치기
        {
            StartTailAttack();
        }
        else if (rand <= 10) // 20퍼센트 확률 브레스
        {
            StartBreathe();
        }
    }

    void SecondPhase()
    {
        if (rand <= 1) // 10퍼센트 확률로 쉬기
        {
            StartIdle();
        }
        else if (rand <= 2) // 10퍼센트 확률로 발톱 공격
        {
            if (Vector3.Distance(detectedPlayer.position, transform.position) < 4f)
            {
                StartAttack02();
            }
        }
        else if (rand <= 3) // 10퍼센트 확률로 물기
        {
            StartAttack01();
        }
        else if (rand <= 4)   // 10퍼센트 확률로 꼬리치기
        {
            StartTailAttack();
        }
        else if (rand <= 6)   // 20퍼센트 확률로 지상 브레스
        {
            StartBreathe();
        }
        else if (rand <= 8)   // 20퍼센트 확률로 지면 강타
        {
            StartFlyAttack();
        }
        else if (rand <= 10)  // 20퍼센트 확률로 공중 브레스
        {
            StartFlyBreathe();
        }
    }

    void FinalPhase()
    {
        if (rand <= 2)   // 20퍼센트 확률로 꼬리치기
        {
            StartTailAttack();
        }
        else if (rand <= 5)   // 30퍼센트 확률로 지상 브레스
        {
            StartBreathe();
        }
        else if (rand <= 8)   // 30퍼센트 확률로 지면 강타
        {
            StartFlyAttack();
        }
        else if (rand <= 10)  // 20퍼센트 확률로 공중 브레스
        {
            StartFlyBreathe();
        }
    }

    void StartSleep()
    {
        animator.SetTrigger("Sleep");
    }

    void StartWakeUp()
    {
        StartCoroutine(BossWakeUp());
    }

    IEnumerator BossWakeUp()
    {
        yield return null;
        animator.SetTrigger("Reassemble");

        yield return new WaitForSeconds(4.3f);
        isWake = true;

    }

    void StartIdle()
    {
        animator.SetTrigger("Idle");
    }

    void StartMove(Vector3 position)
    {
        LookAtDetected(position, rotationSpeed);
        animator.SetTrigger("Walk");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            transform.position = Vector3.MoveTowards(transform.position, position, (moveSpeed + 3) * Time.deltaTime);
        }
    }

    void LookAtDetected(Vector3 position, float speed)
    {
        Vector3 direction = position - transform.position;
        if (direction.magnitude > moveSpeed * Time.deltaTime)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    void StartAttack01()
    {
        damage = 5;
        animator.SetTrigger("Attack01");
    }

    void StartAttack02()
    {
        damage = 5;
        animator.SetTrigger("Attack02");
    }

    void StartTailAttack()
    {
        damage = 10;
        animator.SetTrigger("TailWhipL");
    }

    void StartBreathe()
    {
        damage = 15;
        animator.SetTrigger("BreatheFire");
    }

    void StartFlyAttack()
    {
        damage = 15;
        animator.SetTrigger("FlyAttackSet");
    }

    void StartFlyBreathe()
    {
        damage = 20;
        animator.SetTrigger("FlyBreatheFireSet");
    }

    // Dragon Breathe
    public void StartBreatheParticle()
    {
        for(int l =0; l< breatheProp.Length; l++)
        {
            breatheProp[l].SetActive(true);
        }
        for(int p = 0; p < breatheParticles.Length; p++)
        {
            breatheParticles[p].GetComponent<ParticleSystem>().Play();
        }
    }

    public void StopBreatheParticle()
    {
        for (int l = 0; l < breatheProp.Length; l++)
        {
            breatheProp[l].SetActive(false);
        }
        for (int p = 0; p < breatheParticles.Length; p++)
        {
            breatheParticles[p].GetComponent<ParticleSystem>().Stop();
        }
    }

    // HitBox 활성화
    public void StartHitDamageBoxActive()
    {
        // HitBox 전체 활성화
        for (int i = 0; i < HitDamageBoxes.Length; i++)
        {
            HitDamageBoxes[i].SetActive(true);
        }

    }

    // HitBox 비활성화
    public void StopHitDamageBoxActive()
    {
        for(int i = 0; i < HitDamageBoxes.Length; i++)
        {
            HitDamageBoxes[i].SetActive(false);
        }
    }
}
