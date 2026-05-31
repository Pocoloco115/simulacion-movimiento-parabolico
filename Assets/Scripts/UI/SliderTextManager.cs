using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Slider))]
public class SliderTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sliderText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sliderText.text = GetComponent<Slider>().value.ToString("F1");
    }

    // Update is called once per frame
    void Update()
    {
        _sliderText.text = GetComponent<Slider>().value.ToString("F0");
    }
}
