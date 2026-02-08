using System.Collections.Generic;
using UnityEngine;

namespace OmniumLessons
{
    public class CharacterSpawnController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameData _gameData;
        [SerializeField] private CharacterFactory _characterFactory = new();
        [SerializeField] private Transform _playerTransform;

        [Header("Difficulty Settings")]
        [SerializeField] private int _baseMaxEnemies = 5; 
        [SerializeField] private int _enemiesPerMinute = 2; 

        private float _spawnTimer;
        private float _gameTimer;
        private bool _isSpawningActive;
        
        private List<Character> _activeEnemies = new List<Character>();

        public void Initialize()
        {
            _activeEnemies.Clear();
            _gameTimer = 0;
            _spawnTimer = 0;
            _isSpawningActive = true;
        }

        public void StopSpawning()
        {
            _isSpawningActive = false;
        }

        private void Update()
        {
            if (!_isSpawningActive) return;

            _gameTimer += Time.deltaTime;

            _activeEnemies.RemoveAll(x => x == null || !x.LiveComponent.IsAlive);

            int currentMaxEnemies = CalculateMaxEnemies();

            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0 && _activeEnemies.Count < currentMaxEnemies)
            {
                SpawnEnemy();
                _spawnTimer = _gameData.TimeBetweenEnemySpawn;
            }
        }

        private int CalculateMaxEnemies()
        {
            int minutesPassed = Mathf.FloorToInt(_gameTimer / 60f);
            
            return _baseMaxEnemies + (minutesPassed * _enemiesPerMinute);
        }

        private void SpawnEnemy()
        {
            Vector3 spawnPos = GetSpawnPosition();

            Character newEnemy = _characterFactory.CreateEnemy(spawnPos);
            
            if (newEnemy != null)
            {
                _activeEnemies.Add(newEnemy);
            }
        }

        private Vector3 GetSpawnPosition()
        {
            float randomX = GetRandomOffset();
            float randomZ = GetRandomOffset();

            Vector3 offset = new Vector3(randomX, 0, randomZ);
            
            return _playerTransform.position + offset;
        }

        private float GetRandomOffset()
        {
            bool isPositive = Random.Range(0, 2) > 0;
            float value = Random.Range(_gameData.MinEnemySpawnOffset, _gameData.MaxEnemySpawnOffset);
            return isPositive ? value : -value;
        }
    }
}