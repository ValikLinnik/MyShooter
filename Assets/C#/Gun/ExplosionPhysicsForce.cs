using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using MyNamespace;

namespace UnityStandardAssets.Effects
{
    public class ExplosionPhysicsForce : MonoBehaviour
    {
        public float explosionForce = 4;

        [SerializeField]
        private float _damage = 10;

        [SerializeField]
        private ParticleSystem[] _effects;

        public float Damage
        {
            set
            {
                _damage = value;
            }
        }

        private void OnEnable()
        {
            Initialize(1);
        }

        public void Initialize(float damage)
        {
            Damage = _damage;

            StopAllCoroutines();

            if(_effects != null && _effects.Length > 0)
            {
                foreach (var item in _effects)
                {
                    if(item) 
                    {
                        item.playOnAwake = true;
                        item.gameObject.SetActive(true);
                        item.Play(true);
                    }
                }
            }

            StartCoroutine(OnStart());
        }

        private void OnDisable()
        {
            if(_effects != null && _effects.Length > 0)
            {
                foreach (var item in _effects)
                {
                    if(item) 
                    {
                        item.Clear(true);
                        item.playOnAwake = false;
                        item.Stop(true);
                        item.gameObject.SetActive(false);
                       
                    }
                }
            } 
        }

        private IEnumerator OnStart()
        {
            // wait one frame because some explosions instantiate debris which should then
            // be pushed by physics force
            yield return null;

            float multiplier = GetComponent<ParticleSystemMultiplier>().multiplier;

            float r = 15 * multiplier;
            var cols = Physics.OverlapSphere(transform.position, r);
            var rigidbodies = new List<Rigidbody>();
            var healths = new List<HealthComponent>();

            foreach (var col in cols)
            {
                if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
                {
                    rigidbodies.Add(col.attachedRigidbody);
                }

                if(col) 
                {
                    var health = col.gameObject.GetComponent<HealthComponent>();
                    if(health) 
                    {
                        health.TakeDamege(_damage);
                    }
                }
            }

            foreach (var rb in rigidbodies)
            {
                if(!rb) continue;

                rb.AddExplosionForce(explosionForce*multiplier, transform.position, r, 1*multiplier, ForceMode.Impulse);

            }
        }
    }
}
