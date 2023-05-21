using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private NetworkHealth health;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float distance;
    [SerializeField] private GameObject target;
    
    [SerializeField] private EnemyVision enemyVision;
    [SerializeField] private float range = 1;
    [SerializeField] private float delay = 3;
    [SerializeField] private bool isInRange;
    [SerializeField] private Fortress _fortress;
    private static readonly int _animAttack = Animator.StringToHash("Attack");
    private static readonly int _animSpeed = Animator.StringToHash("Speed");
    private static readonly int _animMotionSpeed = Animator.StringToHash("MotionSpeed");

    private bool attackWaiting = false; // Dont start multiple attacks

    // Start is called before the first frame update
    public void Start()
    {
        TryGetComponent(out health);
        TryGetComponent(out agent);
        TryGetComponent(out animator);
        if (!_fortress) _fortress = GameObject.FindWithTag("Fortress").GetComponent<Fortress>();
        animator.SetFloat(_animMotionSpeed, 1);
    }

    private void ChooseTarget()
    {
        target = enemyVision.GetTarget()
            ? enemyVision.GetTarget()
            : _fortress.GetClosest(transform.position).gameObject;
    }

    private void CheckDistance()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        isInRange = distance <= range;
    }

    private void Attack()
    {
        animator.SetBool(_animAttack, false);

        if (isInRange && !attackWaiting) StartCoroutine(Attacking());
    }

    private void Look()
    {
        if (!isInRange) return;
        // The step size is equal to speed times frame time.
        var step = 50 * Time.deltaTime;
        var targetQ = Quaternion.LookRotation(target.transform.position - transform.position);
        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQ, step);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!health.IsAlive()) return;
        ChooseTarget();
        CheckDistance();

        CalculateSpeed();
        Look();
        Attack();

        agent.SetDestination(isInRange ? transform.position : target.transform.position);
        // transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }

    private void CalculateSpeed()
    {
        float velocity = agent.velocity.magnitude;
        if (velocity > 1.9) velocity = 2;
        else if (velocity < 0.1) velocity = 0;
        animator.SetFloat(_animSpeed, velocity);
    }

    private IEnumerator Attacking()
    {
        attackWaiting = true;
        animator.SetBool(_animAttack, true);
        yield return new WaitForSeconds(delay);
        attackWaiting = false;
    }
}