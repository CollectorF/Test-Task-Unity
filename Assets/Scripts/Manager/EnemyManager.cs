using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<EnemyController> enemyList;

    private int initialEnemiesCount = 0;
    private int killedEnemiesCount = 0;

    public delegate void DieEvent(float initialCount, float killedCount);

    public event DieEvent OnDie;

    private void Awake()
    {
        var preSpawnedEnemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in preSpawnedEnemies)
        {
            PrepareEnemy(enemy);
        }
        initialEnemiesCount = preSpawnedEnemies.Length;
    }

    private void PrepareEnemy(EnemyController controller)
    {
        controller.OnDie += OnEnemyDie;
        enemyList.Add(controller);
    }

    private void OnEnemyDie(BaseCharacterController controller)
    {
        if (controller is EnemyController enemyController)
        {
            enemyList.Remove(enemyController);
            killedEnemiesCount = initialEnemiesCount - enemyList.Count;
            OnDie?.Invoke(initialEnemiesCount, killedEnemiesCount);
        }
    }
}