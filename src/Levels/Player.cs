using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Levels
{
    public class Player : ISubject
    {
        public bool CrystalsIncreased { set; get; }

        private readonly HashSet<IObserver> _observers;
        
        public int MaxHealth { get; }
        
        public int Health { get; private set; }
        
        private int _crystals;
        public int Crystals
        {
            get => _crystals;
            set
            {
                _crystals = value;

                if (_crystals < 0)
                {
                    _crystals = 0;
                }
                
                Notify();
            }
        }
        
        public Player(int health, int crystals)
        {
            _observers = new HashSet<IObserver>();
            MaxHealth = health;
            Health = health;
            _crystals = crystals;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                GD.Print("Game Over");
            }
            Notify();
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in _observers)
            {
                observer.Update();
            }
        }
    }
}