using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerControllerInput : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float moveSpeed = 5f;
    public float fireInterval = 0.2f;

    private InputAction moveAction;
    private InputAction fireAction;

    private Vector2 moveInput = Vector2.zero;
    private bool isFiring = false;
    private float fireTimer = 0f;

    private void OnEnable()
    {
        // 移動（WASD / ゲームパッド左スティック）
        moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.Enable();

        // 発射（Space / Gamepad South (Aボタン)）
        fireAction = new InputAction("Fire", InputActionType.Button);
        fireAction.AddBinding("<Keyboard>/space");
        fireAction.AddBinding("<Gamepad>/buttonSouth");
        fireAction.Enable();

        // ボタンが押されたら発射フラグ ON/OFF
        fireAction.performed += ctx => isFiring = true;
        fireAction.canceled += ctx => isFiring = false;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        fireAction.Disable();
    }

    private void Update()
    {
        // 移動処理
        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        // 弾発射（0.2秒ごと）
        fireTimer += Time.deltaTime;
        if (isFiring && fireTimer >= fireInterval)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            fireTimer = 0f;
        }
    }
}
