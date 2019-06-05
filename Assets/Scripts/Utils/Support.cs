using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class Support
    {
        public static T Spawn<T>() where T : MonoBehaviour =>
            new GameObject(typeof(T).ToString()).AddComponent<T>();

        public static T Spawn<T>(PrimitiveType primitiveType) where T : MonoBehaviour =>
            GameObject.CreatePrimitive(primitiveType).Let(obj =>
            {
                obj.name = typeof(T).ToString();
                return obj.AddComponent<T>();
            });

        public static T Find<T>() => GameObject.Find(typeof(T).ToString()).GetComponent<T>();

        public static void SetMaterial(this GameObject obj, string materialName) => 
            obj.GetComponent<Renderer>().material = Resources.Load<Material>($"Materials/{materialName}");

        public static T Apply<T>(this T source, Action<T> action)
        {
            action(source);
            return source;
        }

        public static R Let<T, R>(this T source, Func<T, R> func) => func(source);

        public static void Animate(Func<IEnumerator> action) =>
            Game.Instance.StartCoroutine(action.Invoke());

        public static void AnimateScale(this GameObject obj, Vector3 final, float timeScale = 1f) => 
            Animate(() => ScaleAnimator(obj.transform, obj.transform.localScale, final, timeScale));
        
        public static void AnimateScale(this GameObject obj, Vector3 initial, Vector3 final, float timeScale = 1f) => 
            Animate(() => ScaleAnimator(obj.transform, initial, final, timeScale));

        static IEnumerator ScaleAnimator(Transform transform, Vector3 initial, Vector3 final, float timeScale = 1f)
        {
            for (float progress = 0; progress <= 1; progress += Time.deltaTime * timeScale)
            {
                transform.localScale = Vector3.Lerp(initial, final, progress);
                yield return null;
            }

            transform.localScale = final;
        }

        public static bool IsUnique<T>(this IEnumerable<T> source)
        {
            var array = source as object[] ?? source.Cast<object>().ToArray();
            var count = array.Length;

            return count == array.Distinct().Count();
        }
    }
}