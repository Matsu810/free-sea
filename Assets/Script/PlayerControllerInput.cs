using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerInput : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fireInterval = 0.2f;
    public GameObject lockOnBulletPrefab;
    public float lockOnRange = 20f;

    private InputAction lockOnFireAction;
    private InputAction moveAction;
    private InputAction fireAction;

    private Vector2 moveInput = Vector2.zero;
    private bool isFiring = false;
    private float fireTimer = 0f;

    private Player player; // Playerスクリプト参照

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.Enable();

        fireAction = new InputAction("Fire", InputActionType.Button);
        fireAction.AddBinding("<Keyboard>/space");
        fireAction.AddBinding("<Gamepad>/buttonSouth");
        fireAction.Enable();

        fireAction.performed += ctx => isFiring = true;
        fireAction.canceled += ctx => isFiring = false;

        lockOnFireAction = new InputAction("LockOnFire", InputActionType.Button);
        lockOnFireAction.AddBinding("<Keyboard>/l");
        lockOnFireAction.AddBinding("<Gamepad>/buttonWest");
        lockOnFireAction.Enable();

        lockOnFireAction.performed += ctx => FireAtNearestUnderEnemy();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        fireAction.Disable();
        lockOnFireAction.Disable();
    }

    private void FireAtNearestUnderEnemy()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("UnderEnemy");
        if (targets.Length == 0) return;

        GameObject nearest = null;
        float shortestDist = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < shortestDist && dist <= lockOnRange)
            {
                shortestDist = dist;
                nearest = target;
            }
        }

        if (nearest != null)
        {
            Vector3 direction = (nearest.transform.position - player.bulletSpawnPoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            Instantiate(lockOnBulletPrefab, player.bulletSpawnPoint.position, rotation);
        }
    }

    private void Update()
    {
        // 移動処理
        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        // 弾発射
        fireTimer += Time.deltaTime;
        if (isFiring && fireTimer >= fireInterval)
        {
            if (player != null && !player.isDead)
            {
                player.Fire(); // Player.csのFire()を呼び出し
            }
            fireTimer = 0f;
        }
    }
}

//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Controls;

//public class PlayerControllerInput : MonoBehaviour
//{
//    public GameObject bulletPrefab;
//    public Transform bulletSpawnPoint;
//    public float moveSpeed = 5f;
//    public float fireInterval = 0.2f;
//    public GameObject lockOnBulletPrefab;
//    public float lockOnRange = 20f;

//    private InputAction lockOnFireAction;  // 第二の攻撃
//    private InputAction moveAction;
//    private InputAction fireAction;

//    private Vector2 moveInput = Vector2.zero;
//    private bool isFiring = false;
//    private float fireTimer = 0f;

//    private void OnEnable()
//    {
//        // 移動（WASD / ゲームパッド左スティック）
//        moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
//        moveAction.AddCompositeBinding("2DVector")
//            .With("Up", "<Keyboard>/w")
//            .With("Down", "<Keyboard>/s")
//            .With("Left", "<Keyboard>/a")
//            .With("Right", "<Keyboard>/d");
//        moveAction.Enable();

//        // 発射（Space / Gamepad South (Aボタン)）
//        fireAction = new InputAction("Fire", InputActionType.Button);
//        fireAction.AddBinding("<Keyboard>/space");
//        fireAction.AddBinding("<Gamepad>/buttonSouth");
//        fireAction.Enable();

//        // ボタンが押されたら発射フラグ ON/OFF
//        fireAction.performed += ctx => isFiring = true;
//        fireAction.canceled += ctx => isFiring = false;

//        lockOnFireAction = new InputAction("LockOnFire", InputActionType.Button);
//        lockOnFireAction.AddBinding("<Keyboard>/l");
//        lockOnFireAction.AddBinding("<Gamepad>/buttonWest"); // Bボタン
//        lockOnFireAction.Enable();

//        lockOnFireAction.performed += ctx => FireAtNearestUnderEnemy();

//    }

//    private void OnDisable()
//    {
//        moveAction.Disable();
//        fireAction.Disable();
//    }
//    private void FireAtNearestUnderEnemy()
//    {
//        GameObject[] targets = GameObject.FindGameObjectsWithTag("UnderEnemy");
//        if (targets.Length == 0) return;

//        GameObject nearest = null;
//        float shortestDist = Mathf.Infinity;

//        foreach (GameObject target in targets)
//        {
//            float dist = Vector3.Distance(transform.position, target.transform.position);
//            if (dist < shortestDist && dist <= lockOnRange)
//            {
//                shortestDist = dist;
//                nearest = target;
//            }
//        }

//        if (nearest != null)
//        {
//            Vector3 direction = (nearest.transform.position - bulletSpawnPoint.position).normalized;
//            Quaternion rotation = Quaternion.LookRotation(direction);

//            Instantiate(lockOnBulletPrefab, bulletSpawnPoint.position, rotation);
//        }
//    }

//    private void Update()
//    {
//        // 移動処理
//        moveInput = moveAction.ReadValue<Vector2>();
//        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
//        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

//        // 弾発射（0.2秒ごと）
//        fireTimer += Time.deltaTime;
//        if (isFiring && fireTimer >= fireInterval)
//        {
//            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
//            fireTimer = 0f;
//        }
//    }
//}
