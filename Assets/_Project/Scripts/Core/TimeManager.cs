namespace NTVV.Core
{
    using UnityEngine;
    using System;
    using NTVV.Managers;

    /// <summary>
    /// Singleton manager for global game ticks.
    /// broadcasts pulses to all active game objects (Crops, Animals).
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        public static event Action<float> OnTick; // DeltaTime per tick

        [Header("Settings")]
        [SerializeField] private float _tickRate = 1.0f; // Seconds per tick

        private float _tickTimer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            // Only fire ticks when the game is playing
            if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
                return;

            _tickTimer += Time.deltaTime;

            // Robust catch-up: If the fame rate is very low, fire multiple ticks
            // this ensures growth is consistent even during lag spikes.
            while (_tickTimer >= _tickRate)
            {
                _tickTimer -= _tickRate;
                OnTick?.Invoke(_tickRate); // Fire the tick
            }
        }
    }
}
