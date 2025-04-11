using UnityEngine;

public class ObstacleHeightAdjuster : MonoBehaviour
{
    [SerializeField] private float _baseHeight;
    [SerializeField] private float _multiplier;

    public void ApplyAdjust()
    {
        var distanceToOrigin = transform.position.x;
        var distanceFactor = Mathf.Pow( distanceToOrigin, 2 );
        var y = distanceFactor * -_multiplier;
        transform.position = new Vector3( transform.position.x, _baseHeight + y, transform.position.z );
    }
}
