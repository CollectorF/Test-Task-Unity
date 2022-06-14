using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(0)]
[RequireComponent(typeof(NavMeshSurface))]
public class LevelManager : MonoBehaviour
{
    private NavMeshSurface[] navMeshSurfaces;
    private PlatformEnemiesProcessor[] platforms;
    private int levelsCount;
    private int currentLevelIndex;

    public delegate void OnMoveToNextPlatform();

    public event OnMoveToNextPlatform OnMove;

    private void Awake()
    {
        navMeshSurfaces = GetComponents<NavMeshSurface>();
        platforms = GetComponentsInChildren<PlatformEnemiesProcessor>();
        foreach (var item in navMeshSurfaces)
        {
            item.BuildNavMesh();
        }
        foreach (var item in platforms)
        {
            item.OnAllDead += MovePlayerToNextPlatform;
        }
        levelsCount = SceneManager.sceneCountInBuildSettings;
    }

    private void MovePlayerToNextPlatform()
    {
        OnMove?.Invoke();
    }

    internal void LoadLevel()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex + 1 < levelsCount)
        {
            SceneManager.LoadScene(currentLevelIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}