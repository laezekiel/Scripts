using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.IronicEntertainment.Scripts.Data;
using Godot;
using static com.IronicEntertainment.Scripts.Data.Health_Resource;

// Author: Ironee

namespace com.IronicEntertainment.Scripts.Component
{
    /// <summary>
    /// Represents a health component for a game entity, providing functionality for health management, damage, and healing.
    /// </summary>
    public class Health_Component : Node
    {
        public RandomNumberGenerator _RNG = new RandomNumberGenerator();

        [Export] private int _OriginMax = 5;
        [Export] private string _Health_Name = "Default";

        [Export] private bool _ProportionalHealthRise = true;

        private Health_Resource _Health;
        [Export]private bool _Unique = false;

        // Properties exposing health-related information
        public Health_State State { get { return _Health.State; } private set { _Health.State = value; EmitSignal(nameof(State_Update), value); } }
        public int Points { get { return _Health.Points; } }
        public int Max { get { return Mathf.Max(_Health.Max + _Health.Temp, 1); } }

        // Signals for health-related events
        [Signal] public delegate void Damage_Taken(int damage, int health);
        [Signal] public delegate void Heal_Received(int heal, int health);
        [Signal] public delegate void State_Update(Health_State state);
        [Signal] public delegate void Max_Rise(int points, int health);
        [Signal] public delegate void Max_Lower(int points, int health);
        [Signal] public delegate void Temp_Rise(int points, int health);
        [Signal] public delegate void Temp_Lower(int points, int health);

        /// <summary>
        /// Called when the node is added to the scene.
        /// </summary>
        public override void _Ready()
        {
            _RNG.Randomize();
            _Health = new Health_Resource(_Health_Name, !_Unique);

            _Health.Init(_OriginMax);

            Connect(nameof(Damage_Taken), this, nameof(OnDamageTaken));
            Connect(nameof(Heal_Received), this, nameof(OnHealReceived));
            Connect(nameof(State_Update), this, nameof(OnStateUpdate));
            Connect(nameof(Max_Rise), this, nameof(OnMaxRise));
            Connect(nameof(Max_Lower), this, nameof(OnMaxLower));
            Connect(nameof(Temp_Rise), this, nameof(OnTempRise));
            Connect(nameof(Temp_Lower), this, nameof(OnTempLower));

        }


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
                isUH = Points < Mathf.RoundToInt(Max / 2),
                isUQ = Points < Mathf.RoundToInt(Max / 4);

            int lmin = 0;

            if (isZero)
            {
                lmin = Points;
                _Health.Points = 0;
            }
            // Emit damage-taken signal
            EmitSignal(nameof(Damage_Taken), damage+ lmin, Points);

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
                isOH = Points > Mathf.RoundToInt(Max / 2),
                isOQ = Points > Mathf.RoundToInt(Max / 4);

            int lmax =0;

            if (isMax)
            {
                lmax = Points - Max;
                _Health.Points = Max;
            }

            // Emit heal-received signal
            EmitSignal(nameof(Heal_Received), heal - lmax, Points);

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
                _Health.Points = Mathf.RoundToInt(Max * ratio);
            }
            else UpdateState();

            // Emit health-max-rise signal
            EmitSignal(nameof(Max_Rise), health, Max);
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
                _Health.Points = Mathf.RoundToInt(Max * ratio);
            }
            else UpdateState();

            // Emit health-max-lower signal
            EmitSignal(nameof(Max_Lower), health, Max);

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
                _Health.Points = Mathf.RoundToInt(Max * ratio);
            }
            else UpdateState();

            // Emit temp-health-rise signal
            EmitSignal(nameof(Temp_Rise), health, Max);
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
                _Health.Points = Mathf.RoundToInt(Max * ratio);
            }
            else UpdateState();

            // Emit temp-health-lower signal
            EmitSignal(nameof(Temp_Lower), health, Max);

            // Adjust current health if it exceeds the new maximum
            if (Points > Max) TakeDamage(Points - Max);
        }


        public void UpdateState()
        {
            float ratio = (Points * 1.00f) / Max * 8;

            if (ratio == 0) State = Health_State.Depleted;
            else if (ratio > 0 && ratio <= 1) State = Health_State.UnderHeighth;
            else if (ratio > 1 && ratio <= 4) State = Health_State.UnderHalf;
            else if (ratio > 4 && ratio < 8) State = Health_State.UnderFull;
            else if (ratio == 8) State = Health_State.Full;
        }



        private void OnDamageTaken(int damage, int health)
        {
            GD.Print($"You have taken {damage} damage. Your health is at {health}");
        }

        private void OnHealReceived(int heal, int health)
        {
            GD.Print($"You have received {heal} healing. Your health is now {health}");
        }

        private void OnStateUpdate(Health_State state)
        {
            GD.Print($"{Points}/{Max}");
            GD.Print($"Health state updated: {state}");
        }

        private void OnMaxRise(int points, int health)
        {
            GD.Print($"Max health increased by {points}. Current health: {health}");
        }

        private void OnMaxLower(int points, int health)
        {
            GD.Print($"Max health decreased by {points}. Current health: {health}");
        }

        private void OnTempRise(int points, int health)
        {
            GD.Print($"Temporary health increased by {points}. Current health: {health}");
        }

        private void OnTempLower(int points, int health)
        {
            GD.Print($"Temporary health decreased by {points}. Current health: {health}");
        }
    }
}
