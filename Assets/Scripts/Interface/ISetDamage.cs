using System;
using Test.Helper;
using Test.Interface;


namespace Test
{
	public interface ISetDamage
	{
		event Action<InfoCollision> OnApplyDamageChange;
		void SetDamage(InfoCollision info);
	}
}