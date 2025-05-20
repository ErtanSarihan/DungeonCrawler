using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {
  [Header("Movement Settings")]
  public float moveSpeed = 5f;

  [Header("Component References")]
  private Rigidbody2D _rigidbody;
  private Animator _animator;
  private SpriteRenderer _spriteRenderer;

  private Vector2 _movementInput;
  private bool _isMoving;

  [Header("Auto-Shooting")]
  public GameObject arrowPrefab;
  public float shootCooldown = 0.5f;
  public float detectionRadius = 7f;
  public Transform shootPoint;
  public LayerMask entityLayer;

  private float _lastShootTime;

  [Header("Health")]
  public int maxHealth = 100;
  private int _currentHealth;

  private void Start() {
    _rigidbody = GetComponent<Rigidbody2D>();
    _animator = GetComponent<Animator>();
    _spriteRenderer = GetComponent<SpriteRenderer>();

    _rigidbody.gravityScale = 0f;
    _currentHealth = maxHealth;
  }

  private void Update() {
    _movementInput.x = Input.GetAxisRaw("Horizontal");
    _movementInput.y = Input.GetAxisRaw("Vertical");

    _isMoving = _movementInput != Vector2.zero;

    _animator.SetBool("isMoving", _isMoving);
    if (_isMoving) {
      _animator.SetFloat("horizontal", _movementInput.x);
      _animator.SetFloat("vertical", _movementInput.y);

      if (_movementInput.x > 0) {
        _spriteRenderer.flipX = true;
      }
      else if (_movementInput.x < 0) {
        _spriteRenderer.flipX = false;
      }
    }

    HandleAutoShooting();
  }

  private void FixedUpdate() {
    if (_movementInput.sqrMagnitude > 0) {
      _rigidbody.linearVelocity = _movementInput.normalized * moveSpeed;
    }
    else {
      _rigidbody.linearVelocity = Vector2.zero;
    }
  }

  void HandleAutoShooting() {
    if (Time.time >= _lastShootTime + shootCooldown) {
      Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, entityLayer);

      if (hitColliders.Length > 0) {
        Transform closestEnemy = GetClosestEnemy(hitColliders);

        if (closestEnemy) {
          ShootAtTarget(closestEnemy);
          _lastShootTime = Time.time;
        }
      }
    }
  }

  Transform GetClosestEnemy(Collider2D[] enemies) {
    Transform closestEnemy = null;
    float closestDistance = Mathf.Infinity;

    foreach (Collider2D enemy in enemies) {
      float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

      if (distanceToEnemy < closestDistance) {
        closestDistance = distanceToEnemy;
        closestEnemy = enemy.transform;
      }
    }

    return closestEnemy;
  }

  void ShootAtTarget(Transform target) {
    // Calculate direction to target
    Vector2 direction = (target.position - transform.position).normalized;

    // Determine spawn position
    Vector3 spawnPosition = shootPoint != null ? shootPoint.position : transform.position;

    // Instantiate the arrow
    GameObject arrowObject = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

    // Initialize the arrow with the direction
    Arrow arrow = arrowObject.GetComponent<Arrow>();
    if (arrow != null) {
      arrow.Initialize(direction);
    }

    // Optional: Face the direction of the target (if your character has directional sprites)
    FaceTarget(direction);
  }

  void FaceTarget(Vector2 direction) {
    // If you're using sprite flipping for direction
    if (direction.x < 0) {
      // Face left
      transform.localScale = new Vector3(-1, 1, 1); // Or however you're handling facing
    }
    else if (direction.x > 0) {
      // Face right
      transform.localScale = new Vector3(1, 1, 1);
    }
  }

  public void TakeDamage(int amount) {
    _currentHealth -= amount;

    // Optional: Play hit effect/animation

    // Check if player is defeated
    if (_currentHealth <= 0) {
      // Player died
      Debug.Log("Player died!");
      // Implement game over logic
    }
  }
}