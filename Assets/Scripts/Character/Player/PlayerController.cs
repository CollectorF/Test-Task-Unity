using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseCharacterController
{
    private Camera playerCamera;
    public GameObject WeaponPrefab;

    public PlayerState State => (PlayerState)stats.State;

    protected override void Awake()
    {
        base.Awake();
        playerCamera = GetComponentInChildren<Camera>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override BaseState InitializeState()
    {
        return new PlayerState(starterInfo);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = playerCamera.ScreenPointToRay(context.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit raycastHitInfo))
        {
            GameObject currentWeapon = Instantiate(WeaponPrefab);
            Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
            weaponScript.SetTargetPoint(raycastHitInfo);
            weaponScript.MoveTowards();

        }
    }
}