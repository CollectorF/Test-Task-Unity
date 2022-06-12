using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemiesProcessor : MonoBehaviour
{
    private List<EnemyController> enemies;
    private int childCount;

    public delegate void OnAllEnemiesDefeated();

    public event OnAllEnemiesDefeated OnAllDead;

    private void Start()
    {
        Transform transform;
        enemies = new List<EnemyController>();
        childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            transform = gameObject.transform.GetChild(i);
            if (transform.CompareTag("Enemy/Enemy"))
            {
                var enemy = transform.gameObject.GetComponent<EnemyController>();
                enemies.Add(enemy);
            }
        }
    }

    private void Update()
    {
        foreach (var item in enemies)
        {
            if (item.isDead)
            {
                enemies.Remove(item);
                break;
            }
        }
        if (enemies.Count == 0)
        {
            OnAllDead?.Invoke();
        }
    }
}
