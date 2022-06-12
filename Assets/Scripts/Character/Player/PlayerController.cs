using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : BaseCharacterController
{
    [SerializeField]
    private InputAction clickInputAction;
    [SerializeField]
    private InputAction posInputAction;
    [SerializeField]
    private GameObject weaponSpawnPoint;
    [SerializeField]
    private GameObject WeaponPrefab;
    [SerializeField]
    private List<Vector3> waypoints;

    private Camera playerCamera;
    private GameObject currentWeapon;
    private Vector3 aimPoint;
    private Vector3 tapPoint;
    private int pointNumber;

    public PlayerState State => (PlayerState)stats.State;

    protected override void Awake()
    {
        base.Awake();
        clickInputAction.Enable();
        posInputAction.Enable();
        clickInputAction.performed += OnClick;
        playerCamera = GetComponentInChildren<Camera>();
        pointNumber = 0;
    }

    protected override void Start()
    {
        base.Start();
        agent.SetDestination(waypoints[pointNumber]);
    }
    private void OnDisable()
    {
        clickInputAction.performed -= OnClick;
        clickInputAction.Disable();
        posInputAction.Disable();
    }

    protected override BaseState InitializeState()
    {
        return new PlayerState(starterInfo);
    }

    public void OnClick(InputAction.CallbackContext callback)
    {
        if (true)
        {
#if UNITY_ANDROID || UNITY_IOS
            tapPoint = Touchscreen.current.primaryTouch.position.ReadValue();
#elif UNITY_STANDALONE
            tapPoint = Mouse.current.position.ReadValue();
#endif
            tapPoint += playerCamera.transform.forward * 10f;
            aimPoint = playerCamera.ScreenToWorldPoint(tapPoint);
            currentWeapon = Instantiate(WeaponPrefab, weaponSpawnPoint.transform.position, Quaternion.identity);
            currentWeapon.transform.LookAt(aimPoint);
        }
    }

    public void MoveToNextPlatform()
    {
        pointNumber++;
        agent.SetDestination(waypoints[pointNumber]);
    }
}