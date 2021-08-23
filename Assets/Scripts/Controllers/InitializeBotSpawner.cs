using System.Collections.Generic;
using Test.GameServices;
using Test.Interface;
using Test.Model.Ai;
using UnityEngine;
using Test.Controllers.TimeRemainings;
using Test.Helper;


namespace Test.Controllers
{
    public class InitializeBotSpawner :  IInitialization
	{
		private Dictionary<int, Bot> _botsDictionary;
		private Vector3[] _spawnPoints;
		private ITimeRemaining _timeRemaining;
		private int _countBot = 3;
		private float _reviveDelay = 5.0f;

		public void Initialization()
		{
			if (_countBot <= 0) return;
			_botsDictionary = new Dictionary<int, Bot>(_countBot);
			_spawnPoints = new Vector3[_countBot];

			var bot = Resources.Load<Bot>("Prefabs/Zombie");
			for (var index = 0; index < _countBot; index++)
			{
				//distribute opponents within a radius of the character
				Vector3 botRandomPosition = Patrol.GenerateRandomPoint(Services.Instance.LevelService.PlayerBehaviour.transform);
				_spawnPoints[index] = botRandomPosition;
				var tempBot = Object.Instantiate(bot, botRandomPosition, Quaternion.identity);
				_botsDictionary[tempBot.GetHashCode()] = tempBot;				
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
			_timeRemaining = new TimeRemaining(() => Respawn(bot.GetHashCode()), _reviveDelay);
			_timeRemaining.AddTimeRemaining(_reviveDelay);			
		}

		private void Respawn(int hash)
        {
			var bot = _botsDictionary[hash];
			AddBotToDictionary(bot);
			Vector3 botSpawnPoint = _spawnPoints.RandomObject();
			bot.transform.position = new Vector3(botSpawnPoint.x, botSpawnPoint.y, botSpawnPoint.z);
			bot.Agent.enabled = true;
			bot.IsVisible = true;
			bot.Revive();
		}
    }
}