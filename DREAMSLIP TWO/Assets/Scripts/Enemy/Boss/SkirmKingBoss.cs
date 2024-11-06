using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkirmKingBoss : EnemyAI
{
    protected Animator animator;
    [SerializeField] private GameObject projectileObject;
    private float teleportDistance;
    
    public float meleeRange = 5f;
    public float shotgunRange = 10f;
    public float longRange = 20f;
    public float teleportCooldown = 10f;
    public float ultimateHealthThreshold = 0.5f;
    
    private float nextTeleportTime = 0f;
    private bool hasPerformedUltimate = false;

    private bool canAttack = false;
    private bool isTeleporting = false;
    [SerializeField]
    private GameObject ultimateFlames;

    [SerializeField] private GameObject ultimateFlamesDamage;

    private AudioSource audioSource;
    public AudioClip meleeAttackAudio;
    public AudioClip ultimateFlamesAudio;
    public AudioClip ultimateFlamesDamageAudio;
    public AudioClip teleportAudio;
    public AudioClip shotgunAudio;
    
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float attackRange;

    protected GameObject projectile;
    [SerializeField] private Transform projectileSpawnLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        projectile = projectileObject;
        health = maxHealth;
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
            playerScript = player.GetComponent<Player>();
        }
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(WakeUp());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        if (isDead || !canAttack) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float currentHealthPercentage = health / maxHealth;

        // Check if health is below 50% and ultimate attack hasn't been used
        if (!hasPerformedUltimate && currentHealthPercentage <= ultimateHealthThreshold)
        {
            Debug.Log("Performing ultimate");
            StartCoroutine(Ultimate());
            return;
        }

        // If the player is within melee range, perform melee attack
        if (distanceToPlayer <= meleeRange)
        {
            Debug.Log("Performing melee attack");
            StartCoroutine(MeleeAttack());
        }
        // If the player is within shotgun range but outside melee range, perform shotgun attack or basic shoot attack
        else if (distanceToPlayer <= shotgunRange)
        {
            if (Random.value > 0.4f)
            {
                Debug.Log("Shooting at player");
                StartCoroutine(ShootAtPlayer());
            }
            else
            {
                Debug.Log("Shotgun attack");
                StartCoroutine(ShotgunAttack());
            }
        }
        // If the player is outside of shotgun range but within long-range, teleport or shoot
        else if (distanceToPlayer <= longRange)
        {
            if (Time.time >= nextTeleportTime)
            {
                Debug.Log("Teleporting near player");
                Teleport();
                nextTeleportTime = Time.time + teleportCooldown;
            }
            else
            {
                Debug.Log("Shooting at player");
                StartCoroutine(ShootAtPlayer());
            }
        }
        // If the player is outside long-range attacks, teleport near the player
        else
        {
            if (!isTeleporting)
            {
                if (Random.value > 0.7f)
                {
                    Debug.Log("Teleporting to player");
                    StartCoroutine(Teleport());
                    nextTeleportTime = Time.time + teleportCooldown;
                }
                else
                {
                    Debug.Log("Triple shot attack");
                    StartCoroutine(TripleShot());
                    nextTeleportTime = Time.time + teleportCooldown;
                }
            }
        }
    }
    
    //Starting sequence
    private IEnumerator WakeUp()
    {
        animator.SetTrigger("WakeUp");
        yield return new WaitForSeconds(3);
        Debug.Log("Woke up");
        canAttack = true;
    }
    
    //Basic shot aimed at player
    private IEnumerator ShootAtPlayer()
    {
        //Shoot player
        canAttack = false;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(1.6f);
        GameObject projectileObject = Instantiate(projectile, projectileSpawnLocation.position, Quaternion.identity);
        projectileObject.GetComponent<EnemyProjectile>().SetBulletDamage(CalculateHitDamage());
        projectileObject.GetComponent<EnemyProjectile>().SetDirection((player.position - projectileSpawnLocation.position).normalized);
        yield return new WaitForSeconds(timeBetweenAttacks);
        agent.speed = 3.5f;
        canAttack = true;
        animator.SetBool("isWalking", true);
    }
    
    //Teleport near player
    IEnumerator Teleport()
    {
        //Teleport
        isTeleporting = true;
        animator.SetTrigger("Teleport");
        agent.speed = 0;
        Vector3 randomOffset = Random.insideUnitCircle.normalized * teleportDistance;
        randomOffset.z = randomOffset.y;
        randomOffset.y = 0;
        audioSource.clip = teleportAudio;
        Vector3 teleportPosition = player.position + randomOffset;

        // Add logic here to ensure the teleport position is valid (e.g., not inside a wall)
        yield return new WaitForSeconds(0.7f);
        audioSource.Play();
        transform.position = teleportPosition;
        isTeleporting = false;
        // Play teleport animation
    }
    
    //Perform a melee attack aimed at player
    IEnumerator MeleeAttack()
    {
        
        //transform.LookAt(player);
        if (!alreadyAttacked)
        {
            audioSource.clip = meleeAttackAudio;
            agent.speed = 0;
            alreadyAttacked = true;
            animator.SetTrigger("Melee");
            yield return new WaitForSeconds(1f);
            audioSource.Play();
            StartCoroutine(TryDamagePlayer());
            agent.speed = 3.5f;
        }
    }

    //Perform ultimate attack
    private IEnumerator Ultimate()
    {
        audioSource.clip = ultimateFlamesAudio;
        audioSource.Play();
        canAttack = false;
        hasPerformedUltimate = true;
        teleportCooldown = 2;
        shotgunRange = 17;
        longRange = 18;
        minDamage = 14;
        maxDamage = 20;
        // Play ultimate attack animation
        animator.SetTrigger("Ultimate");
        GameObject flames = Instantiate(ultimateFlames, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);

        // Add your ultimate attack logic here, for example, a powerful area-of-effect (AoE) attack
        // Example: create an AoE damage effect around the boss
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 25f);  // Adjust radius
        GameObject flameDestroy = Instantiate(ultimateFlamesDamage, transform.position, Quaternion.identity);
        audioSource.clip = ultimateFlamesDamageAudio;
        audioSource.Play();
        Destroy(flames);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                playerScript.Damage(CalculateHitDamage() * 3); 
            }
        }

        yield return new WaitForSeconds(3);
        Destroy(flameDestroy);
        canAttack = true;
    }
    
    //Teleports and shoots 4  shots at the player
    private IEnumerator TripleShot()
    {
        agent.speed = 0;
        canAttack = false;
        StartCoroutine(Teleport());
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(1.6f);
        int numberOfProjectiles = 4;
        float spreadAngle = 45;  // Adjust to control the spread

        // Calculate the direction towards the player
        Vector3 directionToPlayer = (player.position - projectileSpawnLocation.position).normalized;

        // Loop to create each projectile in the spread
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the angle for each projectile (adjusting to spread out over the Y-axis)
            float currentAngle = spreadAngle * ((i - (numberOfProjectiles / 2f)) / (numberOfProjectiles - 1));
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 spreadDirection = rotation * directionToPlayer;

            // Instantiate the projectile and set its direction and damage
            GameObject projectileObject = Instantiate(projectile, projectileSpawnLocation.position, Quaternion.identity);
            projectileObject.GetComponent<EnemyProjectile>().SetBulletDamage(CalculateHitDamage());
            projectileObject.GetComponent<EnemyProjectile>().SetDirection(spreadDirection);
        }

        agent.speed = 3.5f;
        canAttack = true;
    }

    //Shoots 10 projectiles at the player
    private IEnumerator ShotgunAttack()
    {
        audioSource.clip = shotgunAudio;
        audioSource.Play();
        canAttack = false;
        animator.SetTrigger("Shotgun");

// Wait for the shooting animation to play
        yield return new WaitForSeconds(1.6f);

        // Define the spread parameters
    int numberOfProjectiles = 50;
    float spreadAngle = 360f;  // Adjust to control the spread

    // Calculate the direction towards the player
    Vector3 directionToPlayer = (player.position - projectileSpawnLocation.position).normalized;

    // Loop to create each projectile in the spread
    for (int i = 0; i < numberOfProjectiles; i++)
    {
        // Calculate the angle for each projectile (adjusting to spread out over the Y-axis)
        float currentAngle = spreadAngle * ((i - (numberOfProjectiles / 2f)) / (numberOfProjectiles - 1));
        Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
        Vector3 spreadDirection = rotation * directionToPlayer;

        // Instantiate the projectile and set its direction and damage
        GameObject projectileObject = Instantiate(projectile, projectileSpawnLocation.position, Quaternion.identity);
        projectileObject.GetComponent<EnemyProjectile>().SetBulletDamage(CalculateHitDamage());
        projectileObject.GetComponent<EnemyProjectile>().SetDirection(spreadDirection);
    }


// Wait for the time between attacks
        yield return new WaitForSeconds(timeBetweenAttacks);

// Restore movement and allow attacking again
        agent.speed = 3.5f;
        canAttack = true;
        animator.SetBool("isWalking", true);
    }
    
    IEnumerator TryDamagePlayer() {
        yield return new WaitForSeconds(0.5f);
        if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer)) {
            playerScript.Damage(CalculateHitDamage());
        }
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }
}
