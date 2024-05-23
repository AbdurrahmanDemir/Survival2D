using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State
    {
        idle,
        attack
    }

    State state;

    [Header("Elements")]
    [SerializeField] private Transform hitPointTransform;
    [SerializeField] private float hitPointRadius;
    [SerializeField] private int damage;
    [Header("Settings")]
    [SerializeField] private BoxCollider2D hitCollider;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;
    private List<Enemy> damagedEnemies= new List<Enemy>();
    [SerializeField] private float attackDelay;
    private float attackTimer;
    [Header("Animation")]
    Animator animator;
    [SerializeField] private float aimLerp;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        state = State.idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.idle:
                AutoAim();
                break;
            case State.attack:
                Attacking();
                break;
        }

    }
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.right = targetUpVector;
            ManageAttack();

        }
        transform.right = Vector3.Lerp(transform.right, targetUpVector, Time.deltaTime * aimLerp);
        IncrementAttackTimer();
    }

    private void ManageAttack()
    {
        if(attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }
    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.attack;
        damagedEnemies.Clear();
    }
    private void Attacking()
    {
        Attack();
    }
    private void StopAttack()
    {
        state = State.idle;
        damagedEnemies.Clear();
    }
    public void Attack()
    {
        //Collider2D[] enemies = Physics2D.OverlapCircleAll(hitPointTransform.position, hitPointRadius, enemyMask);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitPointTransform.position, hitCollider.bounds.size, hitPointTransform.localEulerAngles.z, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy= enemies[i].GetComponent<Enemy>();

            if (!damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);

            }
        }

    }
    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
            return null;
        

        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();
            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(hitPointTransform.position, hitPointRadius);
    }
}
