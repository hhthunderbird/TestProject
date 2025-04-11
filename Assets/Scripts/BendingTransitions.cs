using System.Collections;
using UnityEngine;

public class BendingTransitions : MonoBehaviour
{
    [SerializeField] private float _currentHorizontalValue;
    [SerializeField] private float _currentVerticalValue;
    [SerializeField] private float _valueLimit;

    [SerializeField] private float _minChangingTime;
    [SerializeField] private float _maxChangingTime;
    [SerializeField] private float _nextChangeTime;
    [SerializeField] private float _maxChangingTimeCounter;
    [SerializeField] private float _transitionDurationMultiplier;

    private bool _isBending;

    private Coroutine _bendingCoroutine;

    private void Awake()
    {
        Shader.SetGlobalFloat( "_HorizontalCurvature", 0 );
        Shader.SetGlobalFloat( "_VerticalCurvature", 0 );

        _currentHorizontalValue = Shader.GetGlobalFloat( "_HorizontalCurvature" );
        _currentVerticalValue = Shader.GetGlobalFloat( "_VerticalCurvature" );
    }

    void Start()
    {
        _nextChangeTime = Random.Range( _minChangingTime, _maxChangingTime );
    }

    void Update()
    {
        if ( _maxChangingTimeCounter < _nextChangeTime )
            _maxChangingTimeCounter += Time.deltaTime;

        if ( !_isBending && _maxChangingTimeCounter >= _nextChangeTime )
        {
            if ( _bendingCoroutine != null )
                StopCoroutine( _bendingCoroutine );
            _bendingCoroutine = StartCoroutine( BendingRoutine() );
        }
    }


    private IEnumerator BendingRoutine()
    {
        _isBending = true;

        var nextHorizontalBend = Random.Range( -_valueLimit, _valueLimit );
        var nextVerticalBend = Random.Range( -_valueLimit, _valueLimit );

        var hDuration = Mathf.Abs( nextHorizontalBend ) + Mathf.Abs( _currentHorizontalValue ) / ( _valueLimit * 2 );
        var vDuration = Mathf.Abs( nextVerticalBend ) + Mathf.Abs( _currentVerticalValue ) / ( _valueLimit * 2 );

        var duration = (hDuration + vDuration) * _transitionDurationMultiplier;

        Debug.Log( $"start" );
        Debug.Log( $"duration {duration}" );

        var startHValue = _currentHorizontalValue;
        var startVValue = _currentVerticalValue;

        for ( float t = 0; t < duration; t += Time.deltaTime )
        {
            var lerp = t / duration;

            _currentHorizontalValue = Mathf.Lerp( startHValue, nextHorizontalBend, lerp );
            _currentVerticalValue = Mathf.Lerp( startVValue, nextVerticalBend, lerp );

            Shader.SetGlobalFloat( "_HorizontalCurvature", _currentHorizontalValue );
            Shader.SetGlobalFloat( "_VerticalCurvature", _currentVerticalValue );
            yield return null;
        }

        startHValue = _currentHorizontalValue;
        startVValue = _currentVerticalValue;

        yield return new WaitForSeconds( 2 );

        for ( float t = 0; t <= duration; t += Time.deltaTime )
        {
            var lerp = t / duration;

            _currentHorizontalValue = Mathf.Lerp( startHValue, 0, lerp );
            _currentVerticalValue = Mathf.Lerp( startVValue, 0, lerp );

            Shader.SetGlobalFloat( "_HorizontalCurvature", _currentHorizontalValue );
            Shader.SetGlobalFloat( "_VerticalCurvature", _currentVerticalValue );
            yield return null;
        }

        _nextChangeTime = Random.Range( _minChangingTime, _maxChangingTime );
        _maxChangingTimeCounter = 0;

        _isBending = false;
        Debug.Log( $"end" );
    }
}
