using System;
using System.Collections;
using System.Collections.Generic;
using com.IronicEntertainment.Scripts.Data;
using static com.IronicEntertainment.Scripts.Data.Health_Resource;
using UnityEngine;

// Author: Ironee

namespace com.IronicEntertainment.Scripts.Component
{
    /// <summary>
    /// Represents a health component for a game entity, providing functionality for health management, damage, and healing.
    /// </summary>
    public class Health_Component : MonoBehaviour
    {
        [SerializeField] private bool _ProportionalHealthRise;

        private void Awake()
        {
            _Health.Init();

            Damage_Taken += OnDamageTaken;
            Heal_Received += OnHealReceived;
            State_Update += OnStateUpdate;
            Max_Rise += OnMaxRise;
            Max_Lower += OnMaxLower;
            Temp_Lower += OnTempLower;
            Temp_Rise += OnTempRise;
        }


        [SerializeField ]private Health_Resource _Health;

        // Properties exposing health-related information
        public Health_State State { get { return _Health.State; } private set { _Health.State = value; State_Update?.Invoke( value); } }
        public int Points { get { return _Health.Points; } }
        public int Max { get { int temp = _Health.Max + _Health.Temp; if (temp > 1) return temp; else return 1; } }

        // Signals for health-related events 
        public event Action<int, int> Damage_Taken;
        public event Action<int, int> Heal_Received;
        public event Action<Health_State> State_Update;
        public event Action<int, int> Max_Rise;
        public event Action<int, int> Max_Lower;
        public event Action<int, int> Temp_Rise;
        public event Action<int, int> Temp_Lower;



        /// <summary>
        /// Inflicts damage on the entity.
        /// </summary>
        /// <param name="damage">The amount of damage to be inflicted.</param>
        public void TakeDamage(int damage)
        {
            if (damage < 0) return;

            _Health.Points -= damage;

            // Check health thresholds
            bool isZero = Points <= 0,
                isUH = Points < Max / 2,
                isUQ = Points < Max / 4;

            int lmin = 0;

            if (isZero)
            {
                lmin = Points;
                _Health.Points = 0;
            }
            // Emit damage-taken signal
            Damage_Taken?.Invoke(damage + lmin, _Health.Points);

            // Update health state based on thresholds
            UpdateState();
        }

        /// <summary>
        /// Receives healing to restore the entity's health.
        /// </summary>
        /// <param name="heal">The amount of healing to be received.</param>
        public void ReceiveHeal(int heal)
        {
            if (heal < 0) return;

            _Health.Points += heal;

            // Check health thresholds
            bool isMax = Points >= Max,
                isOH = Points > Max / 2,
                isOQ = Points > Max / 4;

            int lmax =0;

            if (isMax)
            {
                lmax = Points - Max;
                _Health.Points = Max;
            }

            // Emit heal-received signal
            Heal_Received?.Invoke( heal - lmax, Points);

            UpdateState();
        }

        /// <summary>
        /// Increases the maximum health value.
        /// </summary>
        /// <param name="health">The amount by which the maximum health is increased.</param>
        public void RiseHealthMax(int health)
        {
            if (health < 0) return;

            float ratio = Points / Max;

            _Health.Max += health;

            if (_ProportionalHealthRise)
            {
                _Health.Points = (int)(Max * ratio);
            }
            else UpdateState();

            // Emit health-max-rise signal
            Max_Rise?.Invoke( health, Max);
        }

        /// <summary>
        /// Decreases the maximum health value.
        /// </summary>
        /// <param name="health">The amount by which the maximum health is decreased.</param>
        public void LowerHealthMax(int health)
        {
            if (health < 0) return;

            float ratio = Points / Max;

            _Health.Max -= health;

            if(_Health.Max < 1) _Health.Max = 1;

            if (_ProportionalHealthRise)
            {
                _Health.Points = (int)(Max * ratio);
            }
            else UpdateState();

            // Emit health-max-lower signal
            Max_Lower?.Invoke(health, Max);

            // Adjust current health if it exceeds the new maximum
            if (Points > Max) TakeDamage(Points - Max);
        }

        /// <summary>
        /// Increases the temporary health value.
        /// </summary>
        /// <param name="health">The amount by which the temporary health is increased.</param>
        public void RiseTempHealth(int health)
        {
            if (health < 0) return;

            float ratio = Points / Max;

            _Health.Temp += health;

            if (_ProportionalHealthRise)
            {
                _Health.Points = (int)(Max * ratio);
            }
            else UpdateState();

            // Emit temp-health-rise signal
            Temp_Rise?.Invoke(health, Max);
        }

        /// <summary>
        /// Decreases the temporary health value.
        /// </summary>
        /// <param name="health">The amount by which the temporary health is decreased.</param>
        public void LowerTempHealth(int health)
        {
            if (health < 0) return;

            float ratio = Points / Max;

            _Health.Temp -= health;

            if (_ProportionalHealthRise)
            {
                _Health.Points = (int)(Max * ratio);
            }
            else UpdateState();

            // Emit temp-health-lower signal
            Temp_Lower?.Invoke(health, Max);

            // Adjust current health if it exceeds the new maximum
            if (Points > Max) TakeDamage(Points - Max);
        }


        public void UpdateState()
        {
            float ratio = ((float)_Health.Points) / Max * 8;

            if (ratio == 0) State = Health_State.Depleted;
            else if (ratio > 0 && ratio <= 1) State = Health_State.UnderHeighth;
            else if (ratio > 1 && ratio <= 4) State = Health_State.UnderHalf;
            else if (ratio > 4 && ratio < 8) State = Health_State.UnderFull;
            else if (ratio == 8) State = Health_State.Full;
        }



        private void OnDamageTaken(int damage, int health)
        {
            Debug.Log($"You have taken {damage} damage. Your health is at {health}");
        }

        private void OnHealReceived(int heal, int health)
        {
            Debug.Log($"You have received {heal} healing. Your health is now {health}");
        }

        private void OnStateUpdate(Health_State state)
        {
            Debug.Log($"{Points}/{Max}");
            Debug.Log($"Health state updated: {state}");
        }

        private void OnMaxRise(int points, int health)
        {
            Debug.Log($"Max health increased by {points}. Current health: {health}");
        }

        private void OnMaxLower(int points, int health)
        {
            Debug.Log($"Max health decreased by {points}. Current health: {health}");
        }

        private void OnTempRise(int points, int health)
        {
            Debug.Log($"Temporary health increased by {points}. Current health: {health}");
        }

        private void OnTempLower(int points, int health)
        {
            Debug.Log($"Temporary health decreased by {points}. Current health: {health}");
        }

        private void OnDestroy()
        {

            Damage_Taken = null;
            Heal_Received = null;
            State_Update = null;
            Max_Rise = null;
            Max_Lower = null;
            Temp_Lower = null;
            Temp_Rise = null;
        }
    }
}
