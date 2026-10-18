using UnityEngine;
using UnityEngine.InputSystem;

public class GameEssentials : MonoBehaviour
{
    public static GameEssentials Instance { get; private set; }

    [SerializeField] private PauseMenu _pauseMenu;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void OnPause(InputValue value)
    {
        Debug.Log("Pause button pressed");
        if (_pauseMenu.gameObject.activeSelf)
        {
            _pauseMenu.ClosePauseMenu();
        }
        else
        {
            _pauseMenu.OpenPauseMenu();
        }
    }
}
