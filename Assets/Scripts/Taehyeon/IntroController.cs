using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    [SerializeField] private Slider _loadingBar;
    private const float _loadingTime = 2f;
    private float _loadingTimer;

    private void Awake()
    {
        _loadingBar.value = 0;
    }

    private void Update()
    {
        _loadingTimer += Time.deltaTime;
        _loadingBar.value = _loadingTimer / _loadingTime;
        if (_loadingTimer >= _loadingTime)
        {
            SceneManager.LoadScene("NetworkTestScene", LoadSceneMode.Single);
        }
    }
}
