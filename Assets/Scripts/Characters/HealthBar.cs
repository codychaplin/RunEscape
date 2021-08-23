using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class HealthBar : MonoBehaviour
{
    public Transform target;
    public GameObject uiPrefab;

    Transform UI;
    Transform cam;
    GameObject canvas;
    Image healthSlider;

    float visibleTime = 5f;
    float lastVisibleTime;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        canvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas");
        UI = Instantiate(uiPrefab, canvas.transform).transform;
        healthSlider = UI.GetChild(0).GetComponent<Image>();
        UI.gameObject.SetActive(false);

        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
    }

    void LateUpdate()
    {
        if (UI != null)
        {
            UI.position = target.position;
            UI.forward = -cam.forward;
        }
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        if (UI != null)
        {
            UI.gameObject.SetActive(true);
            lastVisibleTime = Time.time;

            float healthPercent = currentHealth / (float)maxHealth;
            healthSlider.fillAmount = healthPercent;

            if (Time.time - lastVisibleTime > visibleTime)
                UI.gameObject.SetActive(false);

            if (currentHealth <= 0)
                Destroy(UI.gameObject, 1f);
        }
    }    
}
