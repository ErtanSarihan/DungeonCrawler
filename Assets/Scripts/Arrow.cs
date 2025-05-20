using UnityEngine;

public class Arrow : MonoBehaviour {
  [Header("Arrow Properties")]
  public float speed = 15f;
  public float damage = 1f;
  public float lifeTime = 3f;

  private Vector2 _direction;
  private float _timer;

  public void Initialize(Vector2 direction) {
    _direction = direction.normalized;

    float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    _timer = 0f;
  }

  private void Update() {
    transform.Translate(Vector3.right * (speed * Time.deltaTime));

    _timer += Time.deltaTime;
    if (_timer >= lifeTime) {
      Destroy(gameObject);
    }
  }

  void OnTriggerEnter2D(Collider2D collision) {
    Enemy enemy = collision.GetComponent<Enemy>();

    if (enemy) {
      enemy.TakeDamage(damage);
      Destroy(gameObject);
    }
  }
  
}