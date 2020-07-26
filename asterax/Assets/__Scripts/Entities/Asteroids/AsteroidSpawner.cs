using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __Scripts.Asteroids
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private AsteroidSpawnerScriptableObject asteroidSpawnerScriptableObject; 
        [SerializeField] private APlayerShip playerShip;
        [SerializeField] private float maximumDistanceFromPlayerShip = 2f;

        /// <summary>
        /// <para>Invoked when as asteroid is destroyed. The asteroid passed in the arguments is the destroyed
        /// asteroid</para>
        /// </summary>
        public event Action<Asteroid> OnAsteroidDestroyed;
        /// <summary>
        /// <para>Invoked when all the asteroids are destroyed</para>
        /// </summary>
        public event Action OnAllAsteroidDestroyed;
        
        private List<Asteroid> _asteroids;
        private Action<Asteroid> _onAsteroidDestroyed;
        private Bounds _bounds;
        private Transform _asteroidsAnchor;

        private void Awake()
        {
            _bounds = MapLimit.GetCameraBounds(Camera.main);
            _onAsteroidDestroyed = DestroyAsteroid;
            _asteroids = new List<Asteroid>(10);
        }

        /// <summary>
        /// <para>It instantiates the asteroids for each asteroid prefab</para>
        /// </summary>
        private void Start()
        {
            _asteroidsAnchor = new GameObject("Asteroids anchor").transform;
            for (var i = 0; i < asteroidSpawnerScriptableObject.asteroidPrefabs.Length; i++)
            {
                var asteroidPrefab = asteroidSpawnerScriptableObject.asteroidPrefabs[i];
                InstantiateRandomAsteroidPrefab(asteroidPrefab);
            }
        }

        /// <summary>
        /// <para>Detaches the asteroid children from the asteroid destroyed and invokes the OnAsteroidDestroyed
        /// event</para>
        /// <para>If there are no more asteroids in the scene then the OnAllAsteroidDestroyed event is invoked</para>
        /// <remarks>This method is called whenever an asteroid is destroyed</remarks>
        /// </summary>
        /// <param name="asteroid">The destroyed asteroid</param>
        private void DestroyAsteroid(Asteroid asteroid)
        {
            // asteroid.transform.DetachChildren();
            if (!_asteroids.Contains(asteroid)) return;
            _asteroids.Remove(asteroid);
            foreach (var asteroidChild in asteroid.Children)
            {
                asteroidChild.ActivateRigidbody();
                asteroidChild.transform.parent = null;
            }

            if (OnAsteroidDestroyed != null) OnAsteroidDestroyed(asteroid);
            if (_asteroids.Count == 0 && OnAllAsteroidDestroyed != null) OnAllAsteroidDestroyed();
        }

        /// <summary>
        /// <para>Instantiates the first parent asteroids with a random initial direction</para>
        /// <para>For each asteroid spawn it calls InstantiateChildrenPrefabs</para>
        /// </summary>
        /// <param name="asteroidPrefab"></param>
        private void InstantiateRandomAsteroidPrefab(AsteroidPrefab asteroidPrefab)
        {
            for (var i = 0; i < asteroidPrefab.initialQuantity; i++)
            {
                var asteroid = Instantiate(asteroidPrefab.prefab, GetRandomPosition(), Quaternion.identity, _asteroidsAnchor);
                var asteroidTransform = asteroid.transform;
                asteroid.SetInitialDirection(Quaternion.Euler(0, 0, Random.Range(0, 360)) * asteroidTransform.up);
                asteroid.OnDestroy += () => _onAsteroidDestroyed(asteroid);
                _asteroids.Add(asteroid);
                asteroid.Children = InstantiateChildrenPrefabs(asteroidPrefab, asteroid);
                asteroid.ActivateRigidbody();
            }
        }

        /// <summary>
        /// <para>Searches the correspond prefab for the children of the parent and then calls the method
        /// InstantiateChildrenAsteroidPrefabs with the prefab found</para>
        /// </summary>
        /// <param name="asteroidPrefab">Children prefab</param>
        /// <param name="parent"></param>
        /// <returns>Spawned children</returns>
        private List<Asteroid> InstantiateChildrenPrefabs(AsteroidPrefab asteroidPrefab, Asteroid parent)
        {
            var asteroids = new List<Asteroid>();
            if (asteroidPrefab.childrenQuantity == 0) return asteroids;
            for (var i = 0; i < asteroidSpawnerScriptableObject.asteroidPrefabs.Length; i++)
            {
                var prefab = asteroidSpawnerScriptableObject.asteroidPrefabs[i];

                if (asteroidPrefab != prefab ||
                    i == asteroidSpawnerScriptableObject.asteroidPrefabs.Length - 1) continue;

                var prefabToSpawn = asteroidSpawnerScriptableObject.asteroidPrefabs[i + 1];
                var result = InstantiateChildrenAsteroidPrefabs(prefabToSpawn, parent, asteroidPrefab.childrenQuantity);
                foreach (var asteroidChildren in result)
                {
                    asteroidChildren.Children = InstantiateChildrenPrefabs(prefabToSpawn, asteroidChildren);
                }

                return result;
            }

            return asteroids;
        }

        /// <summary>
        /// <para>Instantiates asteroids with the given prefab and quantity. Sets its parent to the parent passed
        /// as parameter. It gives the asteroids a random position inside the parent and a random rotation</para>
        /// </summary>
        /// <param name="asteroidPrefab"></param>
        /// <param name="parent">The parent for the instantiated asteroids</param>
        /// <param name="quantity">How many asteroids will be instantiated from the prefab</param>
        /// <returns>Spawned children</returns>
        private List<Asteroid> InstantiateChildrenAsteroidPrefabs(AsteroidPrefab asteroidPrefab, Asteroid parent, int quantity)
        {
            var children = new List<Asteroid>();
            var parentTransform = parent.transform;
            var separationBetweenAsteroids = 360 / quantity;
            var initialAngle = Random.Range(0, 360);
            var distance = asteroidSpawnerScriptableObject.spaceBetweenChildren;
            for (var i = 0; i < quantity; i++)
            {
                var rotation = Quaternion.Euler(0 ,0 ,(initialAngle + i *separationBetweenAsteroids) % 360);
                var offset = rotation * parentTransform.up * distance;
                var asteroid = Instantiate(asteroidPrefab.prefab, parentTransform.position + offset, Quaternion.identity, 
                    parentTransform);
                asteroid.Parent = parent;
                asteroid.SetInitialDirection(offset.normalized);
                asteroid.OnDestroy += () => _onAsteroidDestroyed(asteroid);
                _asteroids.Add(asteroid);
                children.Add(asteroid);
            }

            return children;
        }

        /// <summary>
        /// <para>Calculates a random position inside the camera bounds that is at least as far away as the member
        /// variable maximumDistanceFromPLayerShip establish.</para>
        /// </summary>
        /// <returns>The position calculated</returns>
        private Vector2 GetRandomPosition()
        {
            var playerPosition = playerShip.transform.position;
            var position = playerPosition;
            while (Vector3.Distance(playerPosition, position) < maximumDistanceFromPlayerShip)
            {
                var x = Random.Range(_bounds.min.x, _bounds.max.x);
                var y = Random.Range(_bounds.min.y, _bounds.max.y);
                position = new Vector2(x, y);
            }
            
            return position;
        }
    }
}