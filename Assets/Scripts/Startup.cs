using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene( 1 );
    }
}
