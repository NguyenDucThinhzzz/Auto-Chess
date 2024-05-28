
using System.Collections.Generic;
using UnityEngine;

namespace ResilientCore
{
	public enum EStatType
	{
		ATK,
        DEF,
        HP,
        SPD,
        ATKSPD,
        RNG,
	}
	public class BaseStats: MonoBehaviour
    {
        public Dictionary<EStatType, Stat> StatsMap;
        public Stat Health { get; private set; }
        public Stat Attack { get; private set; }
		public Stat Defence { get; private set; }
		public Stat Speed { get; private set; }
        public Stat AttackSpeed { get; private set; }
		public Stat Range { get; private set; }

		#region Unity Methods
		private void Update()
		{
            Health.UpdateModifierTimer();
            Attack.UpdateModifierTimer();
            Defence.UpdateModifierTimer();
            Speed.UpdateModifierTimer();
            AttackSpeed.UpdateModifierTimer();
            Range.UpdateModifierTimer();
		}
		#endregion

		#region Public Methods
        public void LoadValues(BaseStatsData _data)
        {
            LoadValues(_data.Health, _data.Attack, _data.Defend, _data.Speed, _data.AttackSpeed, _data.Range) ;
		}
        public void LoadValues(float _health, float _attack, float _defend, float _speed, float _atkSpeed, float _range)
        {
            Health = new Stat(_health);
            Attack = new Stat(_attack);
            Defence = new Stat(_defend);
            Speed = new Stat(_speed);
            AttackSpeed = new Stat(_atkSpeed);
			Range = new Stat(_range);
			SetStatsMap();
        }
        public void AddModifier(EStatType type, StatModifier statModifier)
        {
            StatsMap[type].AddModifier(statModifier);
        }
		public void AddModifier(ModifierBaseType modifierBase)
		{
			StatsMap[modifierBase.StatType].AddModifier(modifierBase.Modifier);
		}
		public void RemoveModifier(EStatType type, StatModifier statModifier)
		{
			StatsMap[type].RemoveModifier(statModifier);
		}
		public void RemoveModifier(ModifierBaseType modifierBase)
		{
			StatsMap[modifierBase.StatType].RemoveModifier(modifierBase.Modifier);
		}
		public void ResetAllBuff()
		{
			Health.RemoveAllModifier();
			Attack.RemoveAllModifier();
			Defence.RemoveAllModifier();
			Speed.RemoveAllModifier();
			AttackSpeed.RemoveAllModifier();
			Range.RemoveAllModifier();
		}
		#endregion

		#region Private Methods
		private void SetStatsMap()
        {
            StatsMap = new Dictionary<EStatType, Stat>();
			StatsMap.Add(EStatType.ATK, Attack);
			StatsMap.Add(EStatType.DEF, Defence);
			StatsMap.Add(EStatType.HP, Health);
			StatsMap.Add(EStatType.SPD, Speed);
			StatsMap.Add(EStatType.ATKSPD, AttackSpeed);
            StatsMap.Add(EStatType.RNG, Range);
		}
		#endregion
	}
}
