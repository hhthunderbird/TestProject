using UnityEngine;

public class ScenarioBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] _billBoards;

    private Transform _transform;
    public Transform Transform => _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void Start()
    {
        UpdateBillboards();
    }

    public void UpdateBillboards()
    {
        foreach ( var billboard in _billBoards )
            billboard.SetActive( Random.Range( 0f, 1f ) > 0.95f );
        
    }
}
