using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{

    private Button _start;
    
    // Use this for initialization
    void Start()
    {
        _start = GetComponent<Button>();
        _start.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
        });
    }

}