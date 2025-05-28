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
        // �ړ��iWASD / �Q�[���p�b�h���X�e�B�b�N�j
        moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.Enable();

        // ���ˁiSpace / Gamepad South (A�{�^��)�j
        fireAction = new InputAction("Fire", InputActionType.Button);
        fireAction.AddBinding("<Keyboard>/space");
        fireAction.AddBinding("<Gamepad>/buttonSouth");
        fireAction.Enable();

        // �{�^���������ꂽ�甭�˃t���O ON/OFF
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
        // �ړ�����
        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        // �e���ˁi0.2�b���Ɓj
        fireTimer += Time.deltaTime;
        if (isFiring && fireTimer >= fireInterval)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            fireTimer = 0f;
        }
    }
}
