using UnityEngine;
using UnityEngine.Events;

namespace MyGame
{
    [System.Serializable]
    public class Ref<T> where T : class
    {
        [SerializeField]
        private T instance;
        public T Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                onchange.Invoke();
            }
        }
        public UnityEvent onchange = new UnityEvent();

        public static bool IsEqual(Ref<T> ref1, Ref<T> ref2)
        {
            return (ref1 != null && ref2 != null && ref1.Instance == ref2.Instance);
        }
        /*public static void Copy(Ref<T> refto, Ref<T> reffrom)
        {
            refto.Instance = reffrom.Instance;
        }*/
        public static void Swap(Ref<T> ref1, Ref<T> ref2)
        {
            T swap = ref1.Instance;
            ref1.Instance = ref2.Instance;
            ref2.Instance = swap;
        }
    }
}
