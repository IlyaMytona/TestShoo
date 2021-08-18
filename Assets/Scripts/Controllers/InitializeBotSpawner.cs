using Test.GameServices;
using Test.Interface;
using Test.Model.Ai;
using UnityEngine;


namespace Test.Controllers
{
    public class InitializeBotSpawner :  IInitialization
	{
        private int _countBot = 3;        
		public void Initialization()
		{
			if (_countBot <= 0) return;
			var bot = Resources.Load<Bot>("Prefabs/Zombie");
			for (var index = 0; index < _countBot; index++)
			{
				//distribute opponents within a radius of the character
				var tempBot = Object.Instantiate(bot, Patrol.GenerateRandomPoint(Services.Instance.LevelService.PlayerBehaviour.transform), Quaternion.identity); 
				tempBot.OnDestroyBotEvent += RemoveBotFromDictionary;
				tempBot.Agent.avoidancePriority = index;
				tempBot.Target = Services.Instance.LevelService.PlayerBehaviour.transform; 
				AddBotToDictionary(tempBot);
			}
		}

		private void AddBotToDictionary(Bot bot)
		{
			Services.Instance.UnitsHolderService.OnCreatedUnit(bot);
		}
		private void RemoveBotFromDictionary(Bot bot)
		{
			Services.Instance.UnitsHolderService.OnDestroyUnit(bot);
		}
    }
}