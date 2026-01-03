using ClientInformationBackend.Core.Configuration;
using ClientInformationBackend.Core.Constants;
using GRYLibrary.Core.APIServer.CommonDBTypes;
using GRYLibrary.Core.APIServer.ConcreteEnvironments;
using GRYLibrary.Core.APIServer.Services.Init;
using GRYLibrary.Core.APIServer.Services.Interfaces;
using GRYLibrary.Core.APIServer.Settings;
using GRYLibrary.Core.APIServer.Utilities;
using GRYLibrary.Core.APIServer.Utilities.InitializationStates;
using GRYLibrary.Core.Logging.GeneralPurposeLogger;
using GRYLibrary.Core.Misc;
using GRYLibrary.Core.Misc.ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientInformationBackend.Core.Services
{
    public class InitializationService : IInitializationService<CodeUnitSpecificCommandlineParameter>
    {
    
        private InitializationState _InitializationState;
        private readonly object _Lock = new object();
        public InitializationService()
        {
            this.SetInitializationState(new Uninitialized());
        }

        public void Initialize(CodeUnitSpecificCommandlineParameter commandlineParameter)
        {
            try
            {
                this.SetInitializationState(new Initializing());
               
                //nothing to do yet

                this.SetInitializationState(new Initialized());
            }
            catch (System.Exception e)
            {
                this.SetInitializationState(new InitializationFailed());
            }
        }

        public InitializationState GetInitializationState()
        {
            lock (_Lock)
            {
                return this._InitializationState;
            }
        }
        public void SetInitializationState(InitializationState initializationState)
        {
            lock (_Lock)
            {
                this._InitializationState = initializationState;
            }
        }

    }
}
