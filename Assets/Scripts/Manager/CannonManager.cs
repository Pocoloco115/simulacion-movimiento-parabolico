using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CannonManager : MonoBehaviour
{
    [SerializeField] private Slider _cannonAngleSliderValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCannonAngle();
    }
    private void UpdateCannonAngle()
    {
        transform.rotation = Quaternion.Euler(0, 0, _cannonAngleSliderValue.value);
    }
}
