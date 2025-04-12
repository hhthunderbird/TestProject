using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Treadmill : MonoBehaviour
{
    [SerializeField] private ScenarioBlock _piecePrefab;
    [SerializeField] private int _quantity;

    [SerializeField] private ScenarioBlock[] _pieces;
    [SerializeField] private int _currentFront; //closest to the camera
    [SerializeField] private int _currentBack; //furthest from the camera
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _speedEvo;
    [SerializeField] private float _speedEvoTime = 5f;
    [SerializeField] private float _speedEvoTimeCounter;
    [SerializeField] private float _pieceLength = 2f;
    [SerializeField] private float _xPosition = 0f;
    [SerializeField] private float _destroyThreshold = 0f;

    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private List<GameObject> _obstacles = new();
    [SerializeField] private int _obstacleBasePool = 30;
    [SerializeField] private float _obstaclePlacingTime = 3f;
    [SerializeField] private float _obstaclePlacingTimeEvo;
    [SerializeField] private float _obstacleCounter;
    [SerializeField] private float _obstacleXLimit = 5.32f;

    [SerializeField] private float _obstacleBaseHeight = 1.5f;
    [SerializeField] private float _obstaclePlacementCurvature = 0.02f;

    [SerializeField] private float _score;

    [SerializeField] private TMP_Text _textScore;

    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Button _restartButton;


    private void Awake()
    {
        for ( int i = 0; i < _obstacleBasePool; i++ )
        {
            var ob = Instantiate( _obstaclePrefab );
            ob.transform.position = Vector3.up * 1000f;
            _obstacles.Add( ob );
        }
    }

    void Start()
    {
        PlayerController.OnCollision += OnPlayerCollision;
        _restartButton.onClick.AddListener( OnRestartButton );

        _obstaclePlacingTimeEvo = _obstaclePlacingTime / 200f;

        _speedEvo = _speed / 100f;

        _pieces = new ScenarioBlock[ _quantity ];

        for ( int i = 0; i < _pieces.Length; i++ )
        {
            var piece = Instantiate( _piecePrefab );
            var pos = Vector3.forward * i * _pieceLength;
            pos.x = _xPosition;
            piece.transform.position = pos;
            _pieces[ i ] = piece;
        }

        _pieceLength = Mathf.Abs( _pieceLength );
    }

    private void OnRestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    private void OnPlayerCollision()
    {
        _speedEvo *= 1.2f;
        _obstaclePlacingTimeEvo *= 1.2f;
        _score -= 10;
        if ( _score < 0 )
        {
            Time.timeScale = 0;
            _gameOverScreen.SetActive( true );
        }
    }



    void Update()
    {
        _score += Time.deltaTime;
        _textScore.text = ( ( int ) _score ).ToString();

        if ( _pieces.Length < 1 ) return;

        if ( _speedEvoTimeCounter < _speedEvoTime )
            _speedEvoTimeCounter += Time.deltaTime;

        if ( _speedEvoTimeCounter >= _speedEvoTime )
        {
            _speedEvoTimeCounter = 0;
            _speed += _speedEvo;
        }

        if ( _obstacleCounter < _obstaclePlacingTime )
            _obstacleCounter += Time.deltaTime;

        if ( _obstacleCounter > _obstaclePlacingTime )
        {
            PlaceObstacle();
            _obstacleCounter = 0;
        }


        for ( int i = 0; i < _pieces.Length; i++ )
            _pieces[ i ].Transform.Translate( _speed * Time.deltaTime * -_pieces[ i ].Transform.forward );


        if ( _pieces[ _currentFront ].Transform.position.z < _destroyThreshold )
        {
            _currentBack = ( _currentFront + _pieces.Length - 1 ) % _pieces.Length;

            _pieces[ _currentFront ].UpdateBillboards();

            RemoveObjstacle( _pieces[ _currentFront ].gameObject );

            _pieces[ _currentFront ].Transform.position = _pieces[ _currentBack ].Transform.position + ( Vector3.forward * _pieceLength );

            _currentFront = ( _currentFront + 1 ) % _pieces.Length;
        }
    }

    private void PlaceObstacle()
    {
        var obst = GetObstacle();
        obst.transform.parent = _pieces[ _currentBack ].Transform;

        var distanceToOrigin = obst.transform.position.x;
        var distanceFactor = Mathf.Pow( distanceToOrigin, 2 );
        var x = Random.Range( -_obstacleXLimit, _obstacleXLimit );
        var y = distanceFactor * -_obstaclePlacementCurvature + _obstacleBaseHeight;
        obst.transform.localPosition = new Vector3( x, y, 0 );

    }

    private Transform GetObstacle()
    {
        for ( int i = 0; i < _obstacles.Count; i++ )
        {
            if ( _obstacles[ i ].transform.position.y > 900 )
                return _obstacles[ i ].transform;
        }
        var ob = Instantiate( _obstaclePrefab );
        ob.transform.position = Vector3.up * 1000f;
        _obstacles.Add( ob );
        return ob.transform;
    }

    private void RemoveObjstacle( GameObject piece )
    {
        if ( _pieces[ _currentFront ].Transform.childCount <= 4 ) return;


        _obstaclePlacingTime -= _obstaclePlacingTimeEvo;

        var child = _pieces[ _currentFront ].Transform.GetChild( _pieces[ _currentFront ].Transform.childCount - 1 );
        child.parent = null;
        child.position = Vector3.up * 1000f;
    }
}
