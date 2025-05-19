using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Component References")]
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _movementInput;
    private bool _isMoving;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rigidbody.gravityScale = 0f;
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
    }

    private void FixedUpdate() {
        if (_movementInput.sqrMagnitude > 0) {
            _rigidbody.linearVelocity = _movementInput.normalized * moveSpeed;
        }
        else {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }
}