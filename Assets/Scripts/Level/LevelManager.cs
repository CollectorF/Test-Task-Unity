using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class LevelManager : MonoBehaviour
{
    private NavMeshSurface[] navMeshSurfaces;

    private void Awake()
    {
        navMeshSurfaces = GetComponents<NavMeshSurface>();
        foreach (var item in navMeshSurfaces)
        {
            item.BuildNavMesh();
        }
    }
}