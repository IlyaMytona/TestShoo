using UnityEngine;
using Test.Interface;
using Test.Behaviour;
using Test.GameServices;
using Test.Helper;
using System.Linq;


namespace Test.Controllers
{
    public class InitializePlayerSpawner : IInitialization
    {
        private Vector3[] _spawnPoints;
        public void Initialization()
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
            int[] spawnPointIds = new int[spawnPoints.Length];
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                string idString = string.Empty;
                foreach (char letter in spawnPoints[i].name)
                {
                    if (IsDigit(letter))
                        idString += letter;
                }
                int id = int.Parse(idString) - 1;
                spawnPointIds[i] = id;
            }
            //Each spawn point takes place in the _spawnPoints array with an index equal to its ID
            _spawnPoints = new Vector3[spawnPointIds.Max() + 1];
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                _spawnPoints[spawnPointIds[i]] = spawnPoints[i].transform.position;
                Services.Instance.LevelService.SpawnPoints[i] = _spawnPoints[spawnPointIds[i]];
            }
            var randomVectorInArrayOfSpawnPoints = _spawnPoints.RandomObject();
            var player = Resources.Load<PlayerBehaviour>("Prefabs/Player");
            var playerObject = UnityEngine.Object.Instantiate(player, randomVectorInArrayOfSpawnPoints, player.transform.rotation);

            Services.Instance.LevelService.PlayerBehaviour = playerObject;
            Services.Instance.UnitsHolderService.OnCreatedUnit(playerObject);
        }

        private bool IsDigit(char character)
        {
            int parseResult;
            return int.TryParse(character.ToString(), out parseResult);
        }
    }
}
