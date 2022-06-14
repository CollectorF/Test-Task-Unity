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

    private void Start()
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
        controller.targetCanvas.enabled = false;
        enemyList.Add(controller);
    }

    internal void FirstShotCompleted()
    {
        foreach (var item in enemyList)
        {
            item.SetFirstShotState(true);
        }
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

    public EnemyController FindClosestEnemy(Vector3 target)
    {
        float minDistance = float.PositiveInfinity;
        EnemyController closestEnemyController = null;
        foreach (var enemyController in enemyList)
        {
            Vector3 enemyTarget = enemyController.transform.position;
            float distance = Vector3.Distance(enemyTarget, target);
            if (distance < minDistance)
            {
                closestEnemyController = enemyController;
                minDistance = distance;
            }
        }
        return closestEnemyController;
    }
}