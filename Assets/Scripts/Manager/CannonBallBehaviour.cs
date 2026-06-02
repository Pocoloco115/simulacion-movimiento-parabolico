using System.Collections;
using UnityEngine;

public class CannonBallBehaviour : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f;
    private PosLabelFollower _posLabelFollower;

    private void Awake()
    {
        _posLabelFollower = gameObject.GetComponent<PosLabelFollower>();
        if (_posLabelFollower == null)
        {
            _posLabelFollower = gameObject.AddComponent<PosLabelFollower>();
        }

        _posLabelFollower.Bind(transform);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Target"))
        {
            if (collision.collider.GetComponentInParent<TargetController>() != null)
            {
                Vector2 impactPoint = (Vector2)transform.position;
                CollisionFeedbackAnim.Instance?.StartAnim(impactPoint);
            }
        }
        Destroy(gameObject);
    }
}
