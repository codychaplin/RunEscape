using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armour;

    public event System.Action<int, int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            TakeDamage(2);
    }

    public void TakeDamage(int damage)
    {
        damage -= armour.getValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;

        if (OnHealthChanged != null)
            OnHealthChanged(maxHealth, currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " has died");
    }
}
