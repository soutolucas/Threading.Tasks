using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Threading.Tasks.Events;

namespace Threading.Tasks.Core
{
    public class AsyncMethods
    {
        #region Attributes
        private StateEventArgs _stateArgs;
        #endregion

        #region Events
        public event EventHandler<StateEventArgs> StateEvent;

        private void OnStateEvent(StateEventArgs e)
        {
            var handler = StateEvent;
            handler?.Invoke(this, e);
        }
        #endregion

        #region Constructor
        public AsyncMethods()
        {
            _stateArgs = new StateEventArgs();
        }
        #endregion

        #region Public methods
        public async void AsyncVoid()
        {
            SetStart();
            await Task.Delay(5000);
            SetCompleted();
        }

        public async Task TaskAsync()
        {
            SetStart();
            await Task.Delay(2000);
            SetCompleted();
        }

        public async Task StartTaskAsync()
        {
            var task = Task.Run(async () =>
            {
                await Task.Delay(5000);
                Debug.WriteLine($"{nameof(StartTaskAsync)} Done!");
            });

            //Code for use task variable for something...

            await Task.Delay(1000);
        }

        public Task ReturnTaskAsync()
        {
            return Task.Delay(5000);
        }
        #endregion

        #region Private methods
        private void SetStart([CallerMemberName] string memberName = "")
        {
            _stateArgs.State = $"Started - Method {memberName}";
            OnStateEvent(_stateArgs);
        }
        private void SetCompleted([CallerMemberName] string memberName = "")
        {
            _stateArgs.State = $"Completed - Method {memberName}";
            OnStateEvent(_stateArgs);
        }
        #endregion
    }
}