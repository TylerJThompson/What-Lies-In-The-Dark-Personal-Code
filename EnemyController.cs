using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        Moving, 
        Hit,
        Attacking,
    }
     public enum BurstAction
    {
        None,
        Move,
        Attack,
    }
    
    public State state { get; private set; }
    // Event Handling for SFX and VFX
    public System.Action onSelfHitStarts;
    public System.Action onSelfHitEnds; 
    public System.Action onSelfAttacks;
    private GameObject target;
    NavMeshAgent agent;
    private Animator animator;
    private Coroutine stateTransitionCoroutine = null;
    
    public CamVigenette camVigenette;
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = new Vector3(target.transform.position.x, 0f, target.transform.position.z);
        agent.stoppingDistance = 0.3f;
        
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        state = State.Moving;
    }

    #region Animator Setup 
    void SetAttackTrigger()
    {
        animator.SetTrigger("Attack");
    }

    public void SetHitTrigger()
    {
        animator.SetTrigger("Hit");
        if (onSelfHitStarts != null)
        {
            onSelfHitStarts();
        }
        agent.speed = 0f;
    }

    public void ResetHitTrigger()
    {
        animator.ResetTrigger("Hit");
        agent.speed = 2f;
    }
    #endregion

    #region Enemy Actions 
    public void PerformSingleAttack()
    {
        if(state == State.Moving)
        {
            if(stateTransitionCoroutine != null)
            {
                StopCoroutine(stateTransitionCoroutine);
            }
            stateTransitionCoroutine = StartCoroutine(PerformAttackIE());
            if(onSelfAttacks != null)
            {
                onSelfAttacks();
            }
        }
    }
    
    IEnumerator PerformAttackIE()
    {
        state = State.Attacking;
        SetAttackTrigger();
        camVigenette.RenderEffect();
        yield return new WaitForEndOfFrame();
        state = State.Moving;
        stateTransitionCoroutine = null;
    }
    public BurstAction PerformAction() 
    {
        var action = DetermineAction();
        switch(action)
        {
            case BurstAction.Move:
                break;
            case BurstAction.Attack:
                PerformSingleAttack();
                break;
            default:
                Debug.LogError("unknown burst action");
                break;
        }
        return action;
        
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Decsion Making 
    private float attackRadius = 1.0f;
    private bool IsWithinAttackRadius (Vector3 start, Vector3 destination) {
        float distance = Vector3.Distance (start, destination);
        return distance <= attackRadius;
    }

    public BurstAction DetermineAction()
    {
        var action = BurstAction.None;
        Debug.Log(agent.isStopped);
        // agent.isStopped is always false, so this won't work 
        // action = agent.isStopped ? BurstAction.Attack : BurstAction.Move; 

        if(IsWithinAttackRadius(transform.position, agent.destination))
        {
            action = BurstAction.Attack;
        }
        else
        {
            action = BurstAction.Move;
        }

        return action;
    }
    #endregion
    
    #region Unity Updates
    void FixedUpdate()
    {
        PerformAction();
    }

    private void Update()
    {
        agent.destination = new Vector3(target.transform.position.x, 0f, target.transform.position.z);
        Debug.LogWarning(agent.destination);
    }
    #endregion

}
