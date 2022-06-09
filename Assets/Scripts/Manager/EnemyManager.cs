using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<EnemyController> enemyList;

    private void Awake()
    {
        var preSpawnedEnemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in preSpawnedEnemies)
        {
            PrepareEnemy(enemy);
        }
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
        }
    }
}