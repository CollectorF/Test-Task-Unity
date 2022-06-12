using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class LevelManager : MonoBehaviour
{
    private NavMeshSurface[] navMeshSurfaces;
    private PlatformEnemiesProcessor[] platforms;

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
    }

    private void MovePlayerToNextPlatform()
    {
        OnMove?.Invoke();
    }
}