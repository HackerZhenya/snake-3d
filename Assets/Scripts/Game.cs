using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Terrain = Controllers.Terrain;

public class Game : MonoBehaviour
{
    public static Game Instance;
    
    public Snake Snake { get; private set; }
    public Terrain Terrain { get; private set; }
    

    void Start()
    {
        Instance = this;

        Terrain = Support.Spawn<Terrain>(PrimitiveType.Cube);
        Snake = Support.Spawn<Snake>();
        Support.Spawn<Fruit>(PrimitiveType.Cube);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            Snake.UpdateDirection(Vector3.forward);

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            Snake.UpdateDirection(Vector3.left);

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            Snake.UpdateDirection(Vector3.back);

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            Snake.UpdateDirection(Vector3.right);
    }

    public void GotFruit() => Snake.GrowUp();

    public void GameOver() => SceneManager.LoadScene("Intro");
}