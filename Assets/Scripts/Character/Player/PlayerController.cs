using System.Collections;
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
    private GameObject spawnPoint;

    public GameObject WeaponPrefab;
    private Camera playerCamera;
    private GameObject currentWeapon;
    private Vector3 aimPoint;
    private Vector3 tapPoint;

    public PlayerState State => (PlayerState)stats.State;

    protected override void Awake()
    {
        base.Awake();
        clickInputAction.Enable();
        posInputAction.Enable();
        clickInputAction.performed += OnClick;
        playerCamera = GetComponentInChildren<Camera>();
    }

    protected override void Start()
    {
        base.Start();
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
            currentWeapon = Instantiate(WeaponPrefab, spawnPoint.transform.position, Quaternion.identity);
            currentWeapon.transform.LookAt(aimPoint);
        }
    }
}