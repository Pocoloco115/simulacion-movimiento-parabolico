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

        _posLabelFollower.Bind(transform, new Vector3(0f, 0.75f, 0f));
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
        Destroy(gameObject);
    }
}
