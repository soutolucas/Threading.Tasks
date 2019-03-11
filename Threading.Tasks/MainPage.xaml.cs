using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Threading.Tasks.Events;
using Threading.Tasks.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Threading.Tasks
{
    public sealed partial class MainPage : Page
    {
        #region Attributes
        private string message;
        private readonly AsyncMethods _methods;
        #endregion
        
        #region Constructor
        public MainPage()
        {
            this.InitializeComponent();
            _methods = new AsyncMethods();
            _methods.StateEvent += UpdateStateHandler;
        }
        #endregion

        #region Deadlock case

        #region The code causes deadlock
        private void BtnGetValueDeadlock_Click(object sender, RoutedEventArgs e)
        {
            var value = GetValueAsync();
            txtDeadlock.Text = value.Result.ToString();

            //The same problem occurs with the code below [Uncomment to test]
            //SetValueAsync().Wait();
            //txtDeadlock.Text = message;

            //Solution for two cases above - [Uncomment to test]
            //var value = GetValueWithConfigureAwaitAsync();
            //txtDeadlock.Text = value.Result.ToString();

            //SetValueAsyncConfigureAwaitAsync().Wait();
            //txtDeadlock.Text = message;
        }
        #endregion

        #region The correct way. Without deadlock
        private async void BtnGetValue_Click(object sender, RoutedEventArgs e)
        {
            var value = await GetValueAsync();
            txtWithoutDeadlock.Text = value.ToString();
        }
        #endregion

        #region Async methods
        private async Task<string> GetValueAsync()
        {
            await Task.Delay(500);
            return "Done!";
        }

        private async Task SetValueAsync()
        {
            await Task.Delay(500);
            message = "Done";
        }

        //[Warning] Using ConfigureAwait(false) to avoid deadlocks is a dangerous practice.
        private async Task<string> GetValueWithConfigureAwaitAsync()
        {
            await Task.Delay(500).ConfigureAwait(false);
            return "Done!";
        }

        private async Task SetValueAsyncConfigureAwaitAsync()
        {
            await Task.Delay(500).ConfigureAwait(false);
            message = "Done";
        }
        #endregion

        #endregion

        #region Others uses for Tasks
        private async void BtnCallAsyncTask_Click(object sender, RoutedEventArgs e)
        {
            _methods.AsyncVoid();
            await _methods.TaskAsync();
        }

        private async void BtnStartTask_Click(object sender, RoutedEventArgs e)
        {
            txtStartTask.Text = "Start...";
            await _methods.StartTaskAsync();
            txtStartTask.Text = "Done!";
        }

        private async void BtnReturnTask_Click(object sender, RoutedEventArgs e)
        {
            txtReturnTask.Text = "Start...";
            await _methods.ReturnTaskAsync();
            txtReturnTask.Text = "Done!";
        }

        private void UpdateStateHandler(object sender, StateEventArgs e) => txtCallAsyncTask.Text = e.State;

        #endregion   
    }
}
