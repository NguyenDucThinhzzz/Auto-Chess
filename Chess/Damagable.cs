
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Chess
{
	public abstract class Damagable: MonoBehaviour
	{
		[field:SerializeField] public float HP { get; protected set; } = 10f;
		public UnityEvent OnDeath { get; set; } = new UnityEvent();
		public UnityEvent OnDamaged { get; set; } = new UnityEvent();
		public bool IsDead { get; protected set; } = false;
		protected virtual void Awake()
		{
			OnDeath.AddListener(Death);
		}
		protected virtual void OnDestroy()
		{
			OnDeath.RemoveAllListeners();
			OnDamaged.RemoveAllListeners();
		}
		public virtual void TakeDamage(DamageInfo damageInfo)
		{
			if (IsDead) return;
			HP -= damageInfo.Amount;
			OnDamaged.Invoke();
			if(HP <= 0)
			{
				HP = 0;
				IsDead = true;
				OnDeath?.Invoke();
			}
		}
		public virtual void Death()
		{
			gameObject.SetActive(false);
		}
	}
}