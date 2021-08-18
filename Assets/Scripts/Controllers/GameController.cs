using UnityEngine;


namespace Test.Controllers
{
    public sealed class GameController : MonoBehaviour
    {
        #region Fields
        
        private Controllers _controllers;
        private bool _gameActive = false;
        #endregion


        #region UnityMethods
        private void Awake()
        {
            Initialize();            
        }

        private void Initialize()
        {
            _gameActive = true;
            _controllers = new Controllers();
            Initialization();
            
        }

        private void Update()
        {
            if (!_gameActive)
                return;
            for (var i = 0; i < _controllers.Length; i++)
            {
                _controllers[i].Execute();
            }
        }
        private void OnApplicationQuit()
        {
            Clean();
        }

        #endregion


        #region Methods
        public void Clean()
        {
            _controllers.Clean();
        }

        public void Initialization()
        {
            _controllers.Initialization();
        }
        #endregion
    }
}
