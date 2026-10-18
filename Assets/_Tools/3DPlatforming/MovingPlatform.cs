using NaughtyAttributes;
using UnityEditor.EditorTools;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [BoxGroup("ObligatoryValues")]
    [SerializeField]
    [Range(0, 20)]
    [Tooltip("the movement to the left and right of the box")]
    private float _moveRange = 10;

    [BoxGroup("ObligatoryValues")]
    [SerializeField]
    [Range(0, 20)]
    [Tooltip("the movement to the left and right of the box")]
    private float _speed = 5;

    [BoxGroup("ObligatoryValues")]
    [SerializeField]
    [Tooltip("Is the platform moving")]
    private bool _onPause = false;

    [BoxGroup("Direction")]
    [SerializeField] private PlatformMovementDirection _direction = PlatformMovementDirection.Horizontal;
    [BoxGroup("Direction")]

    [ShowIf("_direction", PlatformMovementDirection.Horizontal)]
    [SerializeField] private StartPosHorizontal _initialHPosition = StartPosHorizontal.Center;

    [BoxGroup("Direction")]
    [ShowIf("_direction", PlatformMovementDirection.Vertical)]
    [SerializeField] private StartPosVertical _initialVPosition = StartPosVertical.Center;

    private float _posOffset = 0;
    private Rigidbody _platform;

    void Awake()
    {
        _platform = GetComponentInChildren<Rigidbody>();
        if (_platform == null)
        {
            Debug.LogError("No Rigidbody found on the platform. freeing gameobject");
            Destroy(gameObject);
            return;
        }
        Debug.Log("offset: " + _posOffset);
    }

    void FixedUpdate()
    {
        if (_onPause)
            return;

        Vector3 newPosition;
        if (_direction == PlatformMovementDirection.Horizontal)
            newPosition = new Vector3(Mathf.Sin((Time.fixedTime * _speed) + _posOffset) * _moveRange, _platform.transform.position.y, _platform.transform.position.z);
        else
            newPosition = new Vector3(_platform.transform.position.x, Mathf.Sin((Time.fixedTime * _speed) + _posOffset) * _moveRange, _platform.transform.position.z);

        _platform.MovePosition(newPosition);
    }

    void OnValidate()
    {
        if (_platform == null)
            _platform = GetComponentInChildren<Rigidbody>();
        if (_direction == PlatformMovementDirection.Horizontal)
        {
            if (_initialHPosition == StartPosHorizontal.Left)
                _platform.transform.localPosition = new Vector3(-_moveRange, 0, 0);
            else if (_initialHPosition == StartPosHorizontal.Right)
                _platform.transform.localPosition = new Vector3(_moveRange, 0, 0);
            else
                _platform.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            if (_initialVPosition == StartPosVertical.Top)
                _platform.transform.localPosition = new Vector3(0, _moveRange, 0);
            else if (_initialVPosition == StartPosVertical.Bottom)
                _platform.transform.localPosition = new Vector3(0, -_moveRange, 0);
            else
                _platform.transform.localPosition = new Vector3(0, 0, 0);
        }
        if ((_direction == PlatformMovementDirection.Horizontal && _initialHPosition == StartPosHorizontal.Left) || (_direction == PlatformMovementDirection.Vertical && _initialVPosition == StartPosVertical.Bottom))
            _posOffset = -1.2f;
        else if ((_direction == PlatformMovementDirection.Horizontal && _initialHPosition == StartPosHorizontal.Right) || (_direction == PlatformMovementDirection.Vertical && _initialVPosition == StartPosVertical.Top))
            _posOffset = 1.2f;
        else
            _posOffset = 0;
    }

    void OnDrawGizmos()
    {
        Color gizmoColor = Color.green;
        Gizmos.color = gizmoColor;

        if (_direction == PlatformMovementDirection.Horizontal)
            Gizmos.DrawLine(
                new Vector3(transform.position.x - _moveRange, transform.position.y, transform.position.z),
                new Vector3(transform.position.x + _moveRange, transform.position.y, transform.position.z));

        else
            Gizmos.DrawLine(
                new Vector3(transform.position.x, transform.position.y - _moveRange, transform.position.z),
                new Vector3(transform.position.x, transform.position.y + _moveRange, transform.position.z));

    }

}

enum PlatformMovementDirection
{
    Horizontal,
    Vertical
}

enum StartPosHorizontal
{
    none,
    Left,
    Center,
    Right
}
enum StartPosVertical
{
    none,
    Top,
    Center,
    Bottom
}