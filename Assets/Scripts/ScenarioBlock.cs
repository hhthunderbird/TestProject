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
        //for ( int i = 0; i < _billBoards.Length; i++ )
        //{
        //    var rend = _billBoards[ i ].GetComponent<Renderer>();

        //    var props = new MaterialPropertyBlock();

        //    var tex = TextureManager.Instance.Textures[ Random.Range( 0, TextureManager.Instance.Textures.Count ) ];

        //    props.SetTexture( "_MainTex", tex );

        //    rend.SetPropertyBlock( props );
        //}

        UpdateBillboards();
    }

    public void UpdateBillboards()
    {
        foreach ( var billboard in _billBoards )
            billboard.SetActive( Random.Range( 0f, 1f ) > 0.95f );
        
    }
}
