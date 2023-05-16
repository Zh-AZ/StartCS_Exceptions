using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using StartCS_Exceptions.Infrastructure.Commands;
using StartCS_Exceptions.ViewModels.Base;
using StartCS_Exceptions.Views.Windows.MainView;

namespace StartCS_Exceptions.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        static MainView MainView;
        private LoginView LoginView;

        public void OnViewInitialized(LoginView loginView) { LoginView = loginView; }

        public ICommand OpenMainViewCommand { get; }
        private bool CanOpenMainViewCommandExecute(object p) => true;
        private void OnOpenMainViewCommandExecute(object p)
        {
            MainView = new MainView();
            MainView.Show();
            LoginView.Close();
        }

        public MainWindowViewModel() 
        {
            OpenMainViewCommand = new LambdaCommand(OnOpenMainViewCommandExecute, CanOpenMainViewCommandExecute);
        }
    }
}
