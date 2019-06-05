using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class Fruit : MonoBehaviour
    {
        Rigidbody rigidbody;
        
        void Start()
        {
            gameObject.SetMaterial("Fruit");
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;

            GenerateFruit();
        }

        void Update()
        {
            transform.Apply(it =>
            {
                it.Rotate(Vector3.up, 1f);
                var pos = it.position;
                it.position = new Vector3(pos.x, Mathf.Sin(Time.time) * .3f, pos.y);
            });
        }

        void GenerateFruit()
        {
            var terrain = Game.Instance.Terrain.transform;
            var position = terrain.position;
            var scale = terrain.localScale / 2;
            var bias = 4f;

            var fruitPosition = new Vector3(
                Random.Range(position.x - scale.x + bias, position.x + scale.x - bias),
                1,
                Random.Range(position.z - scale.z + bias, position.z + scale.z - bias)
            );
            
            print($"Fruit at {fruitPosition}");

            transform.Apply(it =>
            {
                it.position = fruitPosition;
                it.localScale = Vector3.one * .8f;
            });
            
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        void OnCollisionEnter(Collision other)
        {
            print($"Collision {other}");
            if (other.gameObject.CompareTag("SnakeNode"))
            {
                Game.Instance.GotFruit();
                GenerateFruit();
            }
        }
    }
}