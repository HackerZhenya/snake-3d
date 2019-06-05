using UnityEngine;
using Utils;

namespace Controllers
{
    public class Terrain : MonoBehaviour
    {
        public readonly Vector2 startSize = new Vector2(44, 44);
        
        void Start()
        {
            gameObject.SetMaterial("Terrain");
            gameObject.transform.Apply(it =>
            {
                it.localScale = new Vector3(startSize.x, 1, startSize.y);
                it.position += Vector3.down * Snake.NodeSize;
            });
        }

        public void Expand(int size) => 
            gameObject.transform.localScale += new Vector3(size, 0, size);

        public void Expand(float factor) => 
            gameObject.transform.localScale *= factor;
    }
}