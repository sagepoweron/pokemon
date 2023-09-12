using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public static class Helpers
    {
        public static void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static float Degree(this Vector2 vector)
        {
            float value = (float)((Mathf.Atan2(vector.x, vector.y) / Mathf.PI) * 180f);
            if (value < 0) value += 360f;

            return value;
        }

        public static Vector2 Align(this Vector2 inputVector)
        {
            float X = inputVector.x;
            float Y = inputVector.y;

            if (X * X > Y * Y)
            {
                return new Vector2(Mathf.Sign(X), 0);
            }
            else
            {
                return new Vector2(0, Mathf.Sign(Y));
            }
        }

        public static Vector3 SetX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static void Swap<T>(this T[] array, int indexA, int indexB)
        {
            T temp = array[indexA];
            array[indexA] = array[indexB];
            array[indexB] = temp;
        }

        /*[UnityEditor.MenuItem("Cheats/AddGold")]
        public static void AddGold()
        {
            if (Application.isPlaying)
            {
                GM.Instance.inventory.mygold.Amount += 100;
                // unlock code here...
            }
            else
            {
                Debug.LogError("Not in play mode.");
            }
        }*/
    }
}