using NaughtyAttributes;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [BoxGroup("HealthBar")]
    [SerializeField] private int _fullHealth;
    public int FullHealth { get { return _fullHealth; } }
    [BoxGroup("HealthBar")]
    [SerializeField] private int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } }

    [BoxGroup("Special Health")]
    [SerializeField] private int _extraHealth;

    public HealthComponent SetUpHealth(int fullHealth, int extraHealth = 0)
    {
        _fullHealth = fullHealth;
        _currentHealth = fullHealth;
        _extraHealth = extraHealth;
        return this;
    }

}
