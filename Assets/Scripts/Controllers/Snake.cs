using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class Snake : MonoBehaviour
    {
        // Queue<SnakeNode> body;
        LinkedList<SnakeNode> body;
        bool growUp;
        
        public bool UseGroundCheck { get; private set; }
        Vector3 Direction;

        public SnakeNode Head => body.First.Value;

        public float MoveDelay
        {
            get => moveDelay;
            set
            {
                moveDelay = value;
                UpdateMoveDelay();
            }
        }

        float moveDelay = .6f;
        
        public const float NodeOffset = 2.5f;
        public const float NodeSize = 2f;

        void Start()
        {
            tag = "Snake";
            Direction = Vector3.back;
            
            InitSnake();
            UpdateMoveDelay();
            StartCoroutine(StartGroundCheck());
        }

        IEnumerator StartGroundCheck()
        {
            yield return new WaitForSeconds(3);
            UseGroundCheck = true;
        }

        void InitSnake(int length = 3)
        {
            body = new LinkedList<SnakeNode>(new[] {new SnakeNode(Vector3.zero)});

            for (int i = 1; i < length; i++) 
                body.AddFirst(new SnakeNode(body.First.Value, Direction));
        }

        void UpdateMoveDelay()
        {
            CancelInvoke(nameof(Move));
            InvokeRepeating(nameof(Move), 0.0f, MoveDelay);
        }

        public void UpdateDirection(Vector3 direction)
        {
            direction.Normalize();

            if (direction + Direction != Vector3.zero)
                Direction = direction;
        }

        public bool GrowUp() => growUp = true;

        public void Move()
        {
            body.AddFirst(new SnakeNode(body.First.Value, Direction));

            if (growUp)
            {
                growUp = false;
                return;
            }

            body.Last.Value.Hide(true);
            body.RemoveLast();

            if (!body.Select(it => it.Location).IsUnique())
                Game.Instance.GameOver();
        }

        public class SnakeNode
        {
            public readonly GameObject cube;
            readonly Snake snake = Game.Instance.Snake;

            public Vector3 Location => cube.transform.position;

            public SnakeNode(Vector3 point)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = point;
                cube.GetComponent<BoxCollider>().size *= 1.5f;
                // cube.AddComponent<Rigidbody>().useGravity = false;
                cube.SetMaterial("SnakeNode");
                cube.tag = "SnakeNode";

                if (!CheckGround()) 
                    Game.Instance.GameOver();

                Show();
            }

            public SnakeNode(SnakeNode node, Vector3 direction) : this(node.Location + direction * NodeOffset)
            {
            }

            public void Show() => cube.AnimateScale(Vector3.one * NodeSize, snake.moveDelay + .2f);

            public void Hide(bool delete = false)
            {
                var animationTime = snake.MoveDelay + .2f;
                var destroyTime = animationTime + .5f;
                
                cube.AnimateScale(Vector3.zero, animationTime);
                if (delete) Destroy(cube, destroyTime);
            }

            public bool CheckGround() => 
                !Game.Instance.Snake.UseGroundCheck || Physics.Raycast(cube.transform.position, Vector3.down, 10);
        }
    }
}