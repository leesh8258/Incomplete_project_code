using UnityEngine;
using UnityEngine.UI;

public class Enemy_UI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }
}
