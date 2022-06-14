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
    private int currentLevelNumber;
    internal LevelState levelState;

    public delegate void OnMoveToNextPlatform();
    public delegate void OnLevelLoad();

    public event OnLevelLoad OnMove;
    public event OnLevelLoad OnLoad;

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
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevelNumber = currentLevelIndex + 1;
    }

    private void MovePlayerToNextPlatform()
    {
        OnMove?.Invoke();
    }

    internal int GetCurrentLevelNumber()
    {
        return currentLevelNumber;
    }

    public void LoadLevel()
    {
        var levelToLoad = currentLevelNumber;
        if (levelState == LevelState.Win)
        {
            levelToLoad = currentLevelNumber + 1;
        }
        if (currentLevelNumber < levelsCount)
        {
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        OnLoad?.Invoke();
    }
}