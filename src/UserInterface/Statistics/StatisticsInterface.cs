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
            _statisticsContainer = GetNode<VBoxContainer>("ReferenceRect/Statistics");
            _effectsContainer = GetNode<VBoxContainer>("ReferenceRect/AuraEffects");
        }

        public void SetTowerTemplate(TowerTemplate tower)
        {
            _tower = tower;
        }

        public void SetTowerStatisticValues(TowerData towerData, bool isButtonDown)
        {
            _towerData = towerData;

            foreach (Label statistic in _statisticsContainer.GetChildren())
            {
                Label statisticValue = (Label) statistic.GetChild(0);
                statisticValue.Text = StatisticValueToString(statistic.Name);

                if(_tower == null || statistic.GetChildren().Count < 2) continue;

                Label effectValue = (Label) statistic.GetChild(1);

                if(isButtonDown || _tower.GetEffects().Count == 0)
                {
                    effectValue.Text = null;
                    continue;
                }

                effectValue.Text = EffectValueToString(statistic.Name);
            }

            foreach (Label auraEffect in _effectsContainer.GetChildren())
            {
                Label percentuageValue = (Label) auraEffect.GetChild(0);
                percentuageValue.Text = StatisticValueToString(auraEffect.Name) + "%";
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
                Label percentuageValue = (Label) auraEffect.GetChild(0);
                percentuageValue.Text = StatisticValueToString(null) + "%";
            }
        }

        private string StatisticValueToString(string statisticName)
        {
            return statisticName switch
            {
                "Damage" => _towerData.Damage.ToString(),
                "AttackSpeed" => _towerData.AttackSpeed.ToString(),
                "AttackRange" => _towerData.AttackRange.ToString(),
                "ProjectileSpeed" => _towerData.ProjectileSpeed.ToString(),
                "AuraDamage" => _towerData.AuraDamage.ToString(),
                "AuraAttackSpeed" => _towerData.AuraAttackSpeed.ToString(),
                _ => "0"
            };
        }

        private string EffectValueToString(string statisticName)
        {
            return statisticName switch
            {
                "Damage" => "+" + _tower.GetEffectDamageAdded().ToString(),
                "AttackSpeed" => "+" + _tower.GetEffectAttackSpeedAdded().ToString(),
                _ => ""
            };
        }
    }
}
