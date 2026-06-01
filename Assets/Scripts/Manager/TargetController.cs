using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(LineRenderer))]
public class TargetController : MonoBehaviour
{
    public static TargetController ActiveTarget { get; private set; }
    public static Vector3 LastKnownPosition { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _collider2D;
    private PosLabelFollower _posLabelFollower;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D.gravityScale = 0f;
        ActiveTarget = this;
        LastKnownPosition = transform.position;

        _posLabelFollower = gameObject.GetComponent<PosLabelFollower>();
        if (_posLabelFollower == null)
        {
            _posLabelFollower = gameObject.AddComponent<PosLabelFollower>();
        }

        _posLabelFollower.Bind(transform, new Vector3(0f, 1.5f, 0f));
    }

    private void OnDestroy()
    {
        if (ActiveTarget == this)
        {
            ActiveTarget = null;
        }
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
