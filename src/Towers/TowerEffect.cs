using System;


namespace TowerDefenseMC.Towers
{
    public readonly struct TowerEffect : IEquatable<TowerEffect>
    {
        public string Name { get; }
        public float Damage { get; }
        public float AttackSpeed { get; }

        public TowerEffect(string name, float damage, float attackSpeed) =>
            (Name, Damage, AttackSpeed) = (name, damage, attackSpeed);

        public override int GetHashCode() => (Name, Damage, AttackSpeed).GetHashCode();
        public override bool Equals(object obj) => obj is TowerEffect towerEffect && Equals(towerEffect);

        public bool Equals(TowerEffect other)
        {
            return Name == other.Name && 
                   Math.Abs(Damage - other.Damage) < 0f && 
                   Math.Abs(AttackSpeed - other.AttackSpeed) < 0f;
        } 
    }
}