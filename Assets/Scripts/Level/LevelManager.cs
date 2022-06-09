using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class LevelManager : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }
}