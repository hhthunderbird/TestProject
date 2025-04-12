using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _xReference;
    [SerializeField] private float _baseHeight;
    [SerializeField] private float _multiplier;

    [SerializeField] private float _xLimit = 5.32f;
    [SerializeField] private float _speed;
    [SerializeField] private float _sensitivity;


    private Vector2 _startTouchPosition;

    public static event Action OnCollision;

    protected void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    protected void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        var activeTouches = Touch.activeTouches;

        if ( activeTouches.Count > 0 )
        {
            var t0 = activeTouches[ 0 ];

            if ( t0.phase == TouchPhase.Began )
                _startTouchPosition = t0.screenPosition;

            if ( Mouse.current.leftButton.wasPressedThisFrame )
                _startTouchPosition = Mouse.current.position.ReadValue();

            if ( t0.phase == TouchPhase.Moved )
                ApplyMove( ( t0.screenPosition - _startTouchPosition ).x );

            if( Mouse.current.leftButton.isPressed )
                ApplyMove( ( Mouse.current.position.ReadValue() - _startTouchPosition ).x );
        }
    }

    private void ApplyMove( float input )
    {
        var x = input / _sensitivity;

        var pos = transform.position;

        //pos.x += x * Time.deltaTime * _speed;
        pos.x = x / _sensitivity;

        pos.x = Mathf.Clamp( pos.x, -_xLimit, _xLimit );

        var distanceToOrigin = transform.position.x - _xReference;
        var distanceFactor = Mathf.Pow( distanceToOrigin, 2 );
        var y = distanceFactor * -_multiplier + _baseHeight;

        pos.y = y;

        transform.position = pos;
    }


    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag( "obstacle" ) )
        {
            Debug.Log( "Player hit an obstacle!" );
            OnCollision?.Invoke();
        }
    }
}
