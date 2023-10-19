using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [Header("Enemy Setting")]
    public float hitPoints = 100f;
    public float turnSpeed = 15f;
    public Transform target;
    public float ChaseRange;
    private NavMeshAgent agent;
    private float DistancetoTarget;
    private float DistancetoDefault;
    private Animator anim;
    Vector3 DefaultPosition;

    [Header("Enemy SFX")]
    public AudioClip GetHitAudio;
    public AudioClip StepAudio;
    public AudioClip AttackSwingAudio;
    public AudioClip AttackConnectAudio;
    public AudioClip DeathAudio;
    AudioSource EnemyAudio;

    [Header("Enemy VFX")]
    public ParticleSystem SlashEffect;

  
    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponentInChildren<Animator>();
        anim.SetFloat("Hitpoint", hitPoints);
        EnemyAudio = this.GetComponent<AudioSource>();
        DefaultPosition = this.transform.position;
    }
    public void SlashEffectToggleOn(){
        SlashEffect.Play();
    }

    public void step(){
        EnemyAudio.clip = StepAudio;
        EnemyAudio.Play();
    }
    private void Update()
    {
        DistancetoTarget = Vector3.Distance(target.position, transform.position);
        DistancetoDefault = Vector3.Distance(DefaultPosition, transform.position);

        if(DistancetoTarget <= ChaseRange && hitPoints !=0)
        {
            FaceTarget(target.position);
            if (DistancetoTarget > agent.stoppingDistance+2f)
            {
                ChaseTarget();
                SlashEffect.Stop();
            }
            else if (DistancetoTarget <= agent.stoppingDistance)
            {
                Attack();
            }
        }
        else if(DistancetoTarget >= ChaseRange * 2)
        {
            agent.SetDestination(DefaultPosition);
            FaceTarget(DefaultPosition);
            if(DistancetoDefault<= agent.stoppingDistance)
            {
                Debug.Log("Time to stop");
                anim.SetBool("Run", false);
                anim.SetBool("Attack", false);
            }
        }
    }
    private void FaceTarget(Vector3 destination )
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    public void Attack()
    {
        Debug.Log("Attack");
        anim.SetBool("Run", false);
        anim.SetBool("Attack", true);
    }
    public void ChaseTarget ()
    {
        agent.SetDestination(target.position);
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);
    }

    public void HitConnect()
    {
        EnemyAudio.clip = AttackSwingAudio;
        EnemyAudio.Play();
        if(DistancetoTarget <= agent.stoppingDistance)
        {
            EnemyAudio.clip = AttackConnectAudio;
            EnemyAudio.Play();
            target.GetComponent<PlayerLogic>().PlayerGetHit(50f);
        }
    }

    public void TakeDamage(float damage)
    {
        EnemyAudio.clip = GetHitAudio;
        EnemyAudio.Play();
        hitPoints-=damage;
        anim.SetTrigger("GetHit");
        anim.SetFloat("Hitpoint", hitPoints);
        if(hitPoints <= 0)
        {
            EnemyAudio.clip = DeathAudio;
            EnemyAudio.Play();
            Destroy(gameObject, 3f);
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }

    
    
}