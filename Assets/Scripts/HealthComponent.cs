using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _fullHealth;
    public event Action OnDeath;
    public int FullHealth
    {
        get { return _fullHealth; }
    }

    [SerializeField] private int _currentHealth;

    public int CurrentHealth
    {
        get { return _currentHealth; }
    }

    [SerializeField] private int _extraHealth;
    public event Action onDeath; 

    public HealthComponent SetUpHealth(int fullHealth, int extraHealth = 0)
    {
        _fullHealth = fullHealth;
        _currentHealth = fullHealth;
        _extraHealth = extraHealth;
        return this;
    }

    public int ModifyHealth(int value)
    {
        _currentHealth -= value;
        if (_currentHealth <= 0)
        {
            Death();
        }

        return _currentHealth;
    }

    public int AddHealth(int value,bool extraHealth = false)
    {
        _currentHealth += value;
        _fullHealth += value;
        if (extraHealth)
        {
            _extraHealth += value;
        }
        return _currentHealth;
    }

    public void SetHealth()
    {
        _currentHealth = _fullHealth;
    }
    
    private void Death()
    {
        OnDeath?.Invoke();
    }

}
