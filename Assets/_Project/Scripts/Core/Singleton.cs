namespace NTVV.Core
{
    using UnityEngine;

    /// <summary>
    /// Generic Singleton base class for MonoBehaviours.
    /// Supports both standard and Persistent (DontDestroyOnLoad) behavior.
    /// Renamed common logic to avoid repetition in child classes.
    /// </summary>
    /// <typeparam name="T">Type of the Singleton subclass.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        [Header("Singleton Settings")]
        [SerializeField] protected bool _isPersistent = true;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // Look for any instance in the scene
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (_instance == null)
                        {
                            // Create a new GameObject if no instance exists
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).Name + " (Singleton)";
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (_isPersistent)
                {
                    // Ensure the root object persists across scene loads
                    if (transform.parent != null)
                    {
                        transform.SetParent(null);
                    }
                    DontDestroyOnLoad(gameObject);
                }
                
                OnInitialize();
            }
            else if (_instance != this)
            {
                // Destroy duplicate instances
                Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T).Name} detected on {gameObject.name}. Destroying...");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Optional initialization logic for subclasses.
        /// Replaces the Awake method for child classes.
        /// </summary>
        protected virtual void OnInitialize() { }
    }
}
