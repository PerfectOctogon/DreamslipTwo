using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpectreCoreAI : EnemyAI
{
    
    public float shootingRange;
    public float selfDestructRange;
    public float timeBetweenAttacks;

    public GameObject projectile;
    public GameObject destructionFlames;
    private AudioSource audioSource;

    private bool canAttack = true;
    private Animator animator;
    
    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerScript = player.GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!canAttack || isDead) return;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        
        //Self destruct
        if (distanceToPlayer < selfDestructRange)
        {
            StartCoroutine(SelfDestruct());
        }
        //Shoot at player
        if (distanceToPlayer < shootingRange)
        {
            StartCoroutine(ShootAtPlayer());
        }
    }
    
    private IEnumerator ShootAtPlayer()
    {
        //Shoot player
        canAttack = false;
        animator.SetTrigger("Shoot");
        //yield return new WaitForSeconds(1.6f);
        GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileObject.GetComponent<EnemyProjectile>().SetBulletDamage(CalculateHitDamage());
        projectileObject.GetComponent<EnemyProjectile>().SetDirection((player.position - transform.position).normalized);
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }

    private IEnumerator SelfDestruct()
    {
        canAttack = false;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1);
        audioSource.Play();
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, selfDestructRange, transform.forward)) {
            //print(hit.collider.name);
            if (hit.collider.gameObject.CompareTag("Player")) { hit.collider.gameObject.GetComponent<Player>().Damage(maxDamage * 3); print("Explosion!"); }
        }
        Instantiate(destructionFlames, transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
