using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class MenuHandler : MonoBehaviour
{
    [SerializeField] private Sprite _playButtonSprite;
    [SerializeField] private Sprite _pauseButtonSprite;
    [SerializeField] private Button _quitButton;
    [SerializeField] private GameObject _timeStateImage;
    private InputAction _pauseAction;
    void Awake()
    {
        _pauseAction = new InputAction("Pause", InputActionType.Button, binding: "<Keyboard>/escape");
        _pauseAction.performed += HandlePauseStarted;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _quitButton.onClick.AddListener(QuitGame);
    }
    private void QuitGame()
    {
        Application.Quit();
    }
    private void HandlePauseStarted(InputAction.CallbackContext ctx)
    {
        Time.timeScale = (Time.timeScale > 0f) ? 0f : 1f;
        if (Time.timeScale > 0f)
        {
            _timeStateImage.GetComponent<Image>().sprite = _pauseButtonSprite;
        }
        else
        {
            _timeStateImage.GetComponent<Image>().sprite = _playButtonSprite;
        }
    }
    private void OnEnable()
    {
        _pauseAction?.Enable();
    }
    private void OnDisable()
    {
        _pauseAction?.Disable();
    }
    private void OnDestroy()
    {
        if (_pauseAction != null)
        {
            _pauseAction.performed -= HandlePauseStarted;
            _pauseAction.Dispose();
        }
    }
    
}
