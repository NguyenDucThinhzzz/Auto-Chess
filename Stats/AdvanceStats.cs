
using System.Collections.Generic;
using UnityEngine;

namespace ResilientCore
{

	public enum EAdvanceStatType
	{
        Stamina,
        Energy,
        ERR,
        CritRate,
        CritDamage,
        DMGBonus,
	}

	public class AdvanceStats : MonoBehaviour
    {
        public Dictionary<EAdvanceStatType, Stat> StatsMap = new Dictionary<EAdvanceStatType, Stat>();
		public Stat Stamina { get; private set; }
		public Stat Energy { get; private set; }
		public Stat RechargeRate { get; private set; }
		public Stat CritRate { get; private set; }
		public Stat CritDamage { get; private set; }
		public Stat DamageBonus { get; private set; }

		#region Unity Methods
		private void Update()
		{
			Stamina.UpdateModifierTimer();
			Energy.UpdateModifierTimer();
			RechargeRate.UpdateModifierTimer();
			CritRate.UpdateModifierTimer();
			CritDamage.UpdateModifierTimer();
			DamageBonus.UpdateModifierTimer();
		}
		#endregion

		#region Public Methods
		public void LoadValues(Stat _stamina, Stat _energy, Stat _recharge, Stat _critRate, Stat _critDamage, Stat _dmgBonus)
        {
			Stamina = _stamina;
			Energy = _energy;
			RechargeRate = _recharge;
			CritRate = _critRate;
			CritDamage = _critDamage;
			DamageBonus = _dmgBonus;
            setStatsMap();
        }
        public void LoadValues(AdvanceStatsData _data)
        {
            LoadValues(_data.Stamina, _data.Energy, _data.RechargeRate, _data.CritRate, _data.CritDamage, _data.DamageBonus);
		}
        public void LoadValues(float _stamina, float _energy, float _recharge, float _critRate, float _critDamage, float _dmgBonus)
        {
			Stamina = new Stat(_stamina);
			Energy = new Stat(_energy);
			RechargeRate = new Stat(_recharge);
			CritRate = new Stat(_critRate);
			CritDamage = new Stat(_critDamage);
			DamageBonus = new Stat(_dmgBonus);
			setStatsMap();
        }
        public void AddModifier(EAdvanceStatType type, StatModifier statModifier)
        {
            StatsMap[type].AddModifier(statModifier);
        }
		public void AddModifier(EAdvanceStatType[] types, StatModifier statModifier)
		{
            foreach(EAdvanceStatType type in types)
            {
			    StatsMap[type].AddModifier(statModifier);
            }
		}
		public void RemoveModifier(EAdvanceStatType type, StatModifier statModifier)
		{
			StatsMap[type].RemoveModifier(statModifier);
		}
		public void RemoveModifier(EAdvanceStatType[] types, StatModifier statModifier)
		{
			foreach (EAdvanceStatType type in types)
			{
				StatsMap[type].RemoveModifier(statModifier);
			}
		}
		#endregion

		#region Private Methods
		private void setStatsMap()
        {
			StatsMap.Add(EAdvanceStatType.Stamina, Stamina);
			StatsMap.Add(EAdvanceStatType.Energy, Energy);
			StatsMap.Add(EAdvanceStatType.ERR, RechargeRate);
			StatsMap.Add(EAdvanceStatType.CritRate, CritRate);
			StatsMap.Add(EAdvanceStatType.CritDamage, CritDamage);
			StatsMap.Add(EAdvanceStatType.DMGBonus, DamageBonus);
		}
		#endregion
	}
}
