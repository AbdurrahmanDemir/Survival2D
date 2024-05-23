using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header(" Pooling ")]
    private ObjectPool<DamageText> damageTextPool;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTakenDamageCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTakenDamageCallback;
    }

    private void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private void EnemyTakenDamageCallback(Vector2 pos, int value)
    {
        DamageText damageTextInstance = damageTextPool.Get();

        damageTextInstance.transform.position = pos;
        damageTextInstance.Configure(value.ToString());

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }

    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText bonusParticle)
    {
        bonusParticle.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText bonusParticle)
    {
        bonusParticle.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageText bonusParticle)
    {
        Destroy(bonusParticle);
    }

    [ContextMenu("Test Damage")]
    private void Test()
    {
        EnemyTakenDamageCallback(Vector2.zero, UnityEngine.Random.Range(100, 1000));
    }
}
