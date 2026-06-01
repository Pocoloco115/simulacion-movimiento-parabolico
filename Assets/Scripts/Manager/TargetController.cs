using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(LineRenderer))]
public class TargetController : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _collider2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D.gravityScale = 0f;
    }
    // Update is called once per frame
    void Update() 
    {

    }
    public void StartTargetBehaviour()
    {
        _rigidbody2D.gravityScale = 1f;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
