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
    [SerializeField] private float detectRange = 10f;   // Ž�� ����
    [SerializeField] private float attackRange = 5f;    // ���� ����
    [SerializeField] private float moveSpeed = 3f;      // �̵� �ӵ� 
    [SerializeField] private float rotationSpeed = 10;  // ȸ�� �ӵ�

    [SerializeField] private GameObject[] breatheParticles; // �극�� ��ƼŬ
    [SerializeField] private GameObject[] breatheProp;      // �극�� ��ƼŬ ���� ��Ÿ ���
    [SerializeField] private GameObject[] HitDamageBoxes;   // ������ �޴� �ݶ��̴�

    Vector3 originPosition;
    BehaviorTreeRunner btRunner;
    Transform detectedPlayer;
    Animator animator;
    Health health;

    bool isWake = false;
    int damage;
    int rand;

    public int Damage // �ܺο� ������ �� ����
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

        btRunner = new BehaviorTreeRunner(SettingAttackBT()); // RootNode ����
    }

    private void Update()
    {
        btRunner.Operate(); // Behavior Tree �ݺ�
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
        INode root = new SelectorNode // ���ϴ� �׼��� �����Ű�� ���� ���
        (
            // childs
            new List<INode>
            {
                new SequenceNode // Ž�� ���, Attack
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckIsAttacking),       // ���� �ߴ��� üũ
                        new ActionNode(CheckInAttackRange),       // ���� ���� �������� üũ
                        new ActionNode(DoAttack)                // ����
                    }
                ),
                new SequenceNode // Ž�� ���, Chase
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckDetectEnemy),     // �� Ž��
                        new ActionNode(MoveToDetectedEnemy)     // Ž���� ������ �̵�
                    }
                ),
                new ActionNode(ReturnToOrigin)                  // ���� �ִ� �ڸ��� ���ư��� �׼�
            }
        );

        return root; // �׼� ����
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
        // ���� ������ üũ?
        // ������ ���� ���� �ִϸ��̼��� ���� �߰��� �ʿ䰡 �ִ� ���ϴ�.
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
        // ���� ������ ���Դ��� üũ
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
                // ���� ���� ������
                if (health.currentHealth >= health.maxHealth * 0.6)
                {
                    FirstPhase();
                }
                if (health.currentHealth <= health.maxHealth * 0.6 && health.currentHealth > health.maxHealth * 0.3) // �ִ� ü���� 60�ۼ�Ʈ ������ ��
                {
                    SecondPhase();
                }
                if (health.currentHealth <= health.maxHealth * 0.3) // �ִ� ü���� 30�ۼ�Ʈ ������ ��
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
        // ���Ǿ� �ݶ��̴� ���� �ȿ� ������ Player ���̾�� �� ������Ʈ ������ �����´�.
        // Physic.OverlapSphere(���� ��ġ, ������, ��� ���̾�)
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
        if (rand <= 2) // 20�ۼ�Ʈ Ȯ���� ����
        {
            StartIdle();
        }
        else if (rand <= 4) // 20�ۼ�Ʈ Ȯ���� ���� ����
        {
            if (Vector3.Distance(detectedPlayer.position, transform.position) < 4f)
            {
                StartAttack02();
            }
        }
        else if (rand <= 6) // 20�ۼ�Ʈ Ȯ���� ����
        {
            StartAttack01();
        }
        else if (rand <= 8) // 20�ۼ�Ʈ Ȯ���� ����ġ��
        {
            StartTailAttack();
        }
        else if (rand <= 10) // 20�ۼ�Ʈ Ȯ�� �극��
        {
            StartBreathe();
        }
    }

    void SecondPhase()
    {
        if (rand <= 1) // 10�ۼ�Ʈ Ȯ���� ����
        {
            StartIdle();
        }
        else if (rand <= 2) // 10�ۼ�Ʈ Ȯ���� ���� ����
        {
            if (Vector3.Distance(detectedPlayer.position, transform.position) < 4f)
            {
                StartAttack02();
            }
        }
        else if (rand <= 3) // 10�ۼ�Ʈ Ȯ���� ����
        {
            StartAttack01();
        }
        else if (rand <= 4)   // 10�ۼ�Ʈ Ȯ���� ����ġ��
        {
            StartTailAttack();
        }
        else if (rand <= 6)   // 20�ۼ�Ʈ Ȯ���� ���� �극��
        {
            StartBreathe();
        }
        else if (rand <= 8)   // 20�ۼ�Ʈ Ȯ���� ���� ��Ÿ
        {
            StartFlyAttack();
        }
        else if (rand <= 10)  // 20�ۼ�Ʈ Ȯ���� ���� �극��
        {
            StartFlyBreathe();
        }
    }

    void FinalPhase()
    {
        if (rand <= 2)   // 20�ۼ�Ʈ Ȯ���� ����ġ��
        {
            StartTailAttack();
        }
        else if (rand <= 5)   // 30�ۼ�Ʈ Ȯ���� ���� �극��
        {
            StartBreathe();
        }
        else if (rand <= 8)   // 30�ۼ�Ʈ Ȯ���� ���� ��Ÿ
        {
            StartFlyAttack();
        }
        else if (rand <= 10)  // 20�ۼ�Ʈ Ȯ���� ���� �극��
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

    // HitBox Ȱ��ȭ
    public void StartHitDamageBoxActive()
    {
        // HitBox ��ü Ȱ��ȭ
        for (int i = 0; i < HitDamageBoxes.Length; i++)
        {
            HitDamageBoxes[i].SetActive(true);
        }

    }

    // HitBox ��Ȱ��ȭ
    public void StopHitDamageBoxActive()
    {
        for(int i = 0; i < HitDamageBoxes.Length; i++)
        {
            HitDamageBoxes[i].SetActive(false);
        }
    }
}
