using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using StartCS_Exceptions.Infrastructure.Commands;
using StartCS_Exceptions.ViewModels.Base;
using StartCS_Exceptions.Views.Windows.ClientView;
using StartCS_Exceptions.Views.Windows.TransactionView;
using StartCS_Exceptions.Views.Windows.MainView;
using FontAwesome.Sharp;
using StartCS_Exceptions.Infrastructure.Commands.Base;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace StartCS_Exceptions.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        static MainView MainView;
        private LoginView LoginView;
        private ViewModel _currentChildView;
        
        private string _Caption;

        public ViewModel CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        private IconChar _Icon;
        public IconChar Icon
        {
            get => _Icon;
            set
            {
                _Icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public void OnViewInitialized(LoginView loginView) { LoginView = loginView; }

        public ICommand OpenMainViewCommand { get; }
        private bool CanOpenMainViewCommandExecute(object p) => true;
        private void OnOpenMainViewCommandExecute(object p)
        {
            MainView = new MainView();
            MainView.Show();
            LoginView.Close();
        }

        public ICommand OpenClientViewCommand { get; }
        private bool CanOpenClientViewCommandExecute(object p) => true;
        private void OnOpenClientCiewCommandExecute(object p)
        {
            //ClientView = new ClientView();
            CurrentChildView = new ClientViewModel();
            Caption = "Customers";
            Icon = IconChar.UserGroup;
        }

        public ICommand OpenTransactionViewCommand { get; }
        private bool CanOpenTransactionViewCommandExecute(object p) => true;
        private void OnOpenTransactionViewCommandexecute(object p)
        {
            CurrentChildView = new TransactionViewModel();
            Caption = "Transaction";
            Icon = IconChar.MoneyBillTransfer;
        }

        public MainWindowViewModel() 
        {
            OpenMainViewCommand = new LambdaCommand(OnOpenMainViewCommandExecute, CanOpenMainViewCommandExecute);
            OpenClientViewCommand = new LambdaCommand(OnOpenClientCiewCommandExecute, CanOpenClientViewCommandExecute);
            OpenTransactionViewCommand = new LambdaCommand(OnOpenTransactionViewCommandexecute, CanOpenTransactionViewCommandExecute);

            
        }
    }
}
