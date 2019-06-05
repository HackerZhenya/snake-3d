using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    void Start() => 
        GetComponentInChildren<Button>().onClick
            .AddListener(() => SceneManager.LoadScene("Game"));
}