using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseCharacterController
{
    [SerializeField]
    private InputAction _click;
    [SerializeField]
    private InputAction _pos;
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
        _click.Enable();
        _pos.Enable();
        _click.performed += OnClick;
        playerCamera = GetComponentInChildren<Camera>();
    }

    protected override void Start()
    {
        base.Start();
    }
    private void OnDisable()
    {
        _click.performed -= OnClick;
        _click.Disable();
        _pos.Disable();
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