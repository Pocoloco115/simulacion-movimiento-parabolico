using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator), typeof(TextMeshProUGUI))]
public class CollisionFeedbackAnim : MonoBehaviour
{
    public static CollisionFeedbackAnim Instance { get; private set; }

    private Animator _animator;
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void StartAnim(Vector2 pos)
    {
        _textMeshPro.text = $"Collision detectada en <{pos.x:F2}, {pos.y:F2}>";

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("New State"))
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(AnimCoroutine());
    }

    private IEnumerator AnimCoroutine()
    {
        _animator.SetTrigger("CollisionIn");
        yield return new WaitForSeconds(5f);
        _animator.SetTrigger("CollisionOut");
    }
}
