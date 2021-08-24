using System;
using Test.Helper;


namespace Test
{
	public interface ISetDamage
	{
		event Action<InfoCollision> OnApplyDamageChange;
		void SetDamage(InfoCollision info);
	}
}