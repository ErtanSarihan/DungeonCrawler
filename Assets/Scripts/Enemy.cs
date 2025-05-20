using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
  [Header("Enemy Properties")]
  public float health = 3;
  public float moveSpeed = 2f;
  public int damageOnContact = 1;

  [Header("Optional")]
  public int xpValue = 1;
  public GameObject xpOrbPrefab;

  private Transform _player;
  private Vector2 _direction;

  private void Start() {
    GameObject playerObject = GameObject.FindWithTag("Player");

    if (playerObject != null) {
      _player = playerObject.transform;
    }
    else {
      Debug.LogWarning("No GameObject with tag 'Player' found! Make sure your player has the 'Player' tag.");
    }
  }

  private void Update() {
    if (_player) {
      _direction = (_player.position - transform.position).normalized;
      transform.position += (Vector3)_direction * (moveSpeed * Time.deltaTime);

      if (_direction.x > 0) {
        transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
      }
      else if (_direction.x < 0) {
        transform.localScale = new Vector3(-0.08f, 0.08f, 0.08f);
      }
    }
  }

  public void TakeDamage(float damage) {
    health -= damage;

    if (health <= 0) {
      Die();
    }
  }

  void Die() {
    // Optional: Spawn death effect
    if (xpOrbPrefab != null) {
      Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
    }

    Destroy(gameObject);
  }

  void OnCollisionEnter2D(Collision2D collision) {
    // Check if collided with player
    if (collision.gameObject.CompareTag("Player")) {
      // Damage player
      PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
      if (playerController != null) {
        // Assuming PlayerController has a TakeDamage method
        playerController.TakeDamage(damageOnContact);
      }
    }
  }
}