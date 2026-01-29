using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputControl : MonoBehaviour
{
    // Input Actions
    private InputAction MoveAction;
    private InputAction LookAction;
    private Vector2 moveInput;

    // GameObject and Component references
    private MeshRenderer meshRenderer;
    private SphereCollider col;
    private Rigidbody rb;
    public GameObject Cam;
    public GameObject dieParticlesPrefab;
    private GameObject instantiatedDieParticles;

    // Public variables
    public float speed = 10f;

    // Other variables
    private bool isDead = false;

    // Initialize input actions
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        GameManagerController.Instance.InputActions.FindActionMap("Player").Enable();
    }

    private void Awake()
    {
        MoveAction = GameManagerController.Instance.InputActions.FindActionMap("Player").FindAction("Move");
        MoveAction.Enable();

        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<SphereCollider>();

        GameManagerController.Instance.levels[GameManagerController.Instance.curLevIndex].isComplete = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            Physics.gravity = new Vector3(0, -(Math.Abs(Physics.gravity.y)), 0);
            GameManagerController.Instance.curScene = SceneManager.GetSceneByName("SelectLevelScene");
            GameManagerController.Instance.levels[GameManagerController.Instance.curLevIndex].isComplete = true;
            GameManagerController.Instance.curLevIndex++;
            if (GameManagerController.Instance.curLevIndex < GameManagerController.Instance.levels.Length)
            {
                StartCoroutine(GameManagerController.Instance.SceneTransStart(GameManagerController.Instance.levels[GameManagerController.Instance.curLevIndex].levelName));
            }
        }
    }

    void Update()
    {
        moveInput = MoveAction.ReadValue<Vector2>();
        if (isDead) return;
        if (transform.position.y < GameManagerController.Instance.levels[GameManagerController.Instance.curLevIndex].lowerBound || transform.position.y > GameManagerController.Instance.levels[GameManagerController.Instance.curLevIndex].upperBound)
        {
            if (Physics.gravity.y < 0)
            {
                instantiatedDieParticles = Instantiate(dieParticlesPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            }
            else {
                instantiatedDieParticles = Instantiate(dieParticlesPrefab, transform.position, Quaternion.Euler(0, 0, 180));
            }
            isDead = true;
            StartCoroutine(Death());
        }
    }
    void FixedUpdate()
    {
        if (isDead) return;
        Vector3 move = Cam.transform.forward * moveInput.y + Cam.transform.right * moveInput.x;
        move.y = 0;
        move.Normalize();
        rb.AddForce(move * speed, ForceMode.Force);
    }

    IEnumerator Death()
    {
        meshRenderer.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(instantiatedDieParticles);
        Physics.gravity = new Vector3(0, -(Math.Abs(Physics.gravity.y)), 0);
        StartCoroutine(GameManagerController.Instance.SceneTransStart(GameManagerController.Instance.curScene.name));
    }
}