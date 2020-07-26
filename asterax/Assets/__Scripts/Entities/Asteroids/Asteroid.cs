using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __Scripts.Asteroids
{
    [RequireComponent(typeof(Stats), typeof(SpaceObject) )]
    public class Asteroid: MonoBehaviour
    {
        [SerializeField] private float points;
        
        /// <summary>
        /// <para>Invoked on die</para>
        /// </summary>
        public event Action OnDestroy;
        public List<Asteroid> Children { get; set; }
        public float Points
        {
            get { return points; }
        }
        public bool DestroyedByBullet { get; set; }

        public Asteroid Parent { get; set; }

        private Rigidbody _rigidbody;
        private Stats _stats;
        private Transform _transform;
        private bool _activated;
        private SpaceObject _spaceObject;
        private Vector3 _velocity;

        private void Awake()
        {
            _stats = GetComponent<Stats>();
            _stats.OnDie += Destroy;
            _transform = transform;
            _spaceObject = GetComponent<SpaceObject>();
            _spaceObject.enabled = false;
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        
        /// <summary>
        /// <para>Rotates the transform in the z axis</para>
        /// </summary>
        private void FixedUpdate()
        {
            if (!_activated) return;
            
            _transform.Rotate(Vector3.back, _stats.RotationSpeed);
        }

        /// <summary>
        /// <para>Activates the RigidBody setting to false the isKinematic variable and enabling the SpaceObject
        /// component</para>
        /// </summary>
        public void ActivateRigidbody()
        {
            _rigidbody.isKinematic = false;
            _spaceObject.enabled = true;
            _activated = true;
            _rigidbody.velocity = _velocity;
        }

        /// <summary>
        /// <para>Sets the member variable _velocity equals to initialDirection multiplied by a random number between
        /// the movement speed and the maximum movement speed</para>
        /// </summary>
        /// <param name="initialDirection"></param>
        public void SetInitialDirection(Vector3 initialDirection)
        {
            var velocity = Random.Range(_stats.MovementSpeed, _stats.MaxSpeed);
            _velocity = initialDirection * velocity;
        }

        /// <summary>
        /// <para>Destroys the GameObject and invokes the OnDestroy event</para>
        /// </summary>
        private void Destroy()
        {
            if (OnDestroy != null) OnDestroy();
            Destroy(gameObject);
        }

        /// <summary>
        /// <para>Reduces the health of the Stats attached to this GameObject.</para>
        /// <para>If the asteroid is destroyed and the parameter isBullet is true then it sets the member variable
        /// DestroyedByBullet to true</para>
        /// </summary>
        /// <param name="damage">The amount of health that will be depleted</param>
        /// <param name="isBullet">If the GameObject making the damage is a bullet</param>
        public void ReceiveDamage(float damage, bool isBullet)
        {
            if (Parent != null)
            {
                Parent.ReceiveDamage(damage, isBullet);
                return;
            }

            if (isBullet && _stats.Health - damage <= 0.0f)
            {
                DestroyedByBullet = true;
            }

            _stats.Health -= damage;
        }
        
        
    }
}