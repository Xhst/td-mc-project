using System.Globalization;

using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Towers;


namespace TowerDefenseMC.UserInterface.Statistics
{
    public class StatisticsInterface : Control
    {
        private VBoxContainer _statisticsContainer;
        private VBoxContainer _effectsContainer;

        private TowerData _towerData;
        private TowerTemplate _tower;

        public override void _Ready()
        {
            _statisticsContainer = GetNode<VBoxContainer>("ReferenceRect/NinePatchRect/Statistics");
            _effectsContainer = GetNode<VBoxContainer>("ReferenceRect/NinePatchRect/AuraEffects");
        }

        public void SetTowerTemplate(TowerTemplate tower)
        {
            _tower = tower;
        }

        public void SetTowerStatisticValues(TowerData towerData)
        {
            _towerData = towerData;

            foreach (Label statistic in _statisticsContainer.GetChildren())
            {
                Label statisticValue = (Label) statistic.GetChild(0);
                statisticValue.Text = StatisticValueToString(statistic.Name);

                if(_tower == null || statistic.GetChildren().Count < 2) continue;

                Label effectValue = (Label) statistic.GetChild(1);

                if(_tower.GetEffects().Count == 0)
                {
                    effectValue.Text = null;
                    continue;
                }

                effectValue.Text = EffectValueToString(statistic.Name);
            }

            foreach (Label auraEffect in _effectsContainer.GetChildren())
            {
                Label percentageValue = (Label) auraEffect.GetChild(0);
                percentageValue.Text = "+" + StatisticValueToString(auraEffect.Name) + "%";
            }
        }

        public void SetDefaultStatisticValues()
        {
            foreach (Label statistic in _statisticsContainer.GetChildren())
            {
                Label statisticValue = (Label) statistic.GetChild(0);
                statisticValue.Text = StatisticValueToString(null);

                if(statistic.GetChildren().Count < 2) continue;

                Label effectValue = (Label) statistic.GetChild(1);
                effectValue.Text = EffectValueToString(null);
            }

            foreach (Label auraEffect in _effectsContainer.GetChildren())
            {
                Label percentageValue = (Label) auraEffect.GetChild(0);
                percentageValue.Text = StatisticValueToString(null) + "%";
            }
        }

        private string StatisticValueToString(string statisticName)
        {
            return statisticName switch
            {
                "Damage" => _towerData.Damage.ToString(CultureInfo.InvariantCulture),
                "AttackSpeed" => _towerData.AttackSpeed.ToString(CultureInfo.InvariantCulture),
                "AttackRange" => _towerData.AttackRange.ToString(),
                "ProjectileSpeed" => _towerData.ProjectileSpeed.ToString(CultureInfo.InvariantCulture),
                "AuraDamage" => _towerData.AuraDamage.ToString(CultureInfo.InvariantCulture),
                "AuraAttackSpeed" => _towerData.AuraAttackSpeed.ToString(CultureInfo.InvariantCulture),
                _ => "0"
            };
        }

        private string EffectValueToString(string statisticName)
        {
            return statisticName switch
            {
                "Damage" => "+" + _tower.GetEffectDamageAdded(),
                "AttackSpeed" => "+" + _tower.GetEffectAttackSpeedAdded(),
                _ => ""
            };
        }
    }
}