using NaughtyAttributes.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Elements")]
    Player player;
    EnemyMovement enemyMovement;
    [SerializeField] private ParticleSystem passAwayParticle;

    [Header("SpawnSettings")]
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private SpriteRenderer spawnRenderer;
    [SerializeField] private Collider2D collider;
    [SerializeField] private float playerDetectionRadius;

    bool hasSpawned;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    float attackDelay;
    float attackTimer;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    int health;
    [SerializeField] private TextMeshPro healthText;

    [Header("Action")]
    public static Action< Vector2,int> onDamageTaken;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyRenderer.enabled = false;
        spawnRenderer.enabled = true;

        health = maxHealth;
        healthText.text=health.ToString();

        Vector3 targetScale = spawnRenderer.transform.localScale * 1.2f;
        LeanTween.scale(spawnRenderer.gameObject, targetScale, 0.3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnRendererCompleted);


        attackDelay = 1f / attackFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();
    }
    void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < playerDetectionRadius)
        {
            Attack();
        }
    }
    void SpawnRendererCompleted()
    {
        enemyRenderer.enabled = true;
        spawnRenderer.enabled = false;
        collider.enabled = true;
        hasSpawned = true;
        enemyMovement.StorePlayer(player);

    }
    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }
    private void Attack()
    {
        player.TakeDamage(10);
        attackTimer = 0;
    }
    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;
        healthText.text=health.ToString();
        onDamageTaken?.Invoke(transform.position, damage);
        if (health <= 0)
        {
            PassAway();
        }
    }
    void PassAway()
    {
        passAwayParticle.transform.SetParent(null);
        passAwayParticle.Play();

        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
