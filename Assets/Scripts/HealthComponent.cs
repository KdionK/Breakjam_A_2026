using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _fullHealth;
    public int FullHealth { get { return _fullHealth; } }
    [SerializeField] private int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } }

    [SerializeField] private int _extraHealth;

    public HealthComponent SetUpHealth(int fullHealth, int extraHealth = 0)
    {
        _fullHealth = fullHealth;
        _currentHealth = fullHealth;
        _extraHealth = extraHealth;
        return this;
    }

}
