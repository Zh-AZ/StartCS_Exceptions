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
using StartCS_Exceptions.Views.Windows.HistoryLogView;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace StartCS_Exceptions.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public static ObservableCollection<Client> Clients { get; set; } //= new ObservableCollection<Client>();
        string path = @"..\Debug\Client.xml";

        static MainView MainView;
        private LoginView LoginView;
        private ViewModel _currentChildView;
        private string _Caption;
        private static Client _Selected;

        public Client Selected
        {
            get => _Selected;
            set => Set(ref _Selected, value);
        }

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

        public ICommand OpenHistoryLogViewCommand { get; }
        private bool CanOpenHistoryLogViewCommandExecute(object p) => true;
        private void OnOpenHistoryLohViewCommandExecute(object p)
        {
            CurrentChildView = new HistoryLogViewModel();
            Caption = "History";
            Icon = IconChar.History;
        }

        public MainWindowViewModel() 
        {
            OpenMainViewCommand = new LambdaCommand(OnOpenMainViewCommandExecute, CanOpenMainViewCommandExecute);
            OpenClientViewCommand = new LambdaCommand(OnOpenClientCiewCommandExecute, CanOpenClientViewCommandExecute);
            OpenTransactionViewCommand = new LambdaCommand(OnOpenTransactionViewCommandexecute, CanOpenTransactionViewCommandExecute);
            OpenHistoryLogViewCommand = new LambdaCommand(OnOpenHistoryLohViewCommandExecute, CanOpenHistoryLogViewCommandExecute);

            Clients = new ObservableCollection<Client>();
            if (File.Exists(path)) { XmlDeserialize(Clients); }
            else { GenerationClient(); }
        }

        void GenerationClient()
        {
            Random random = new Random();

            bool billRandomBool;
            bool depBillRandomBool;
            int billRandValue;
            int depBillRandValue;
            int value = random.Next(5, 11);
            string depBill;
            string bill;

            for (int i = 0; i < value; i++)
            {
                billRandomBool = random.Next(2) == 1;
                depBillRandomBool = random.Next(2) == 1;

                billRandValue = random.Next(100, 1001);
                depBillRandValue = random.Next(100, 201);

                if (depBillRandomBool == true) { depBill = depBillRandValue.ToString(); }
                else { depBill = "Закрытый"; }

                if (billRandomBool == true) { bill = billRandValue.ToString(); }
                else { bill = "Закрытый"; }

                Clients.Add(new Client(i.ToString(), Faker.Internet.Email(), Faker.Name.First(), Faker.Name.Middle(),
                    Faker.Name.Last(), Faker.Phone.Number(), Faker.Address.StreetName(), bill, depBill));
            }
            XmlSerialize(Clients);
        }

        public void XmlSerialize(ObservableCollection<Client> clients)
        {
            File.WriteAllText(path, String.Empty);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Client>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, clients);
            }
        }

        void XmlDeserialize(ObservableCollection<Client> clients)
        {
            if (File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Client>));
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    ObservableCollection<Client> newClients = xmlSerializer.Deserialize(fs) as ObservableCollection<Client>;
                    if (newClients != null)
                    {
                        foreach (Client client in newClients)
                        {
                            Clients.Add(client);
                        }
                    }
                }
            }
        }
    }
}
