using Test.Interface;

namespace Test.GameServices
{
    public sealed class Services
    {     
        #region ClassLifeCycles

        static Services()
        {
            Instance = new Services();
            Instance.Initialize();            
        }

        #endregion


        #region Properties

        public static Services Instance { get; }        
        public ITimeService TimeService { get; private set; }
        public LevelService LevelService { get; private set; }
        public UnitsHolderService UnitsHolderService { get; private set; }

        #endregion


        #region Methods

        private void Initialize()
        {            
            TimeService = new UnityTimeService();
            LevelService = new LevelService();
            UnitsHolderService = new UnitsHolderService();
        }

        #endregion
    }
}
