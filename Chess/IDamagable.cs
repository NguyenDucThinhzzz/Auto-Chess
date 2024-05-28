
using Unity.VisualScripting;

public interface IDamagable
{
	public float HP { get; set; }
	public void TakeDamage(float damage);
	public void Death();
}