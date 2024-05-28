using System.Collections;
using UnityEngine;

public struct DamageInfo
{
	public float Amount;
	public BaseChess Dealer;
	public DamageInfo(BaseChess dealer, float amount)
	{
		Amount = amount;
		Dealer = dealer;
	}
}