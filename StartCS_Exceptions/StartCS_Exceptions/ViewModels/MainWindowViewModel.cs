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
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Media3D;

namespace StartCS_Exceptions.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public static ObservableCollection<Client> Clients { get; set; } //= new ObservableCollection<Client>();
        public static List<ClientsInHistory> ClientsInHistories { get; set; }        
        string path = @"..\Debug\Client.xml";

        static MainView MainView;
        static ClientView ClientView;
        static TransactionView TransactionView;
        static HistoryLogView HistoryLogView;
        private LoginView LoginView;
        private UserControl _currentChildView;
        private string _Caption;
        private static Client _Selected;

        public Client Selected
        {
            get => _Selected;
            set => Set(ref _Selected, value);
        }
        
        public UserControl CurrentChildView
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
        private void OnOpenMainViewCommandExecute(object p)
        {
            MainView = new MainView();
            MainView.Show();
            LoginView.Close();
        }

        public ICommand OpenClientViewCommand { get; }
        private void OnOpenClientCiewCommandExecute(object p)
        {
            //CurrentChildView = new ClientViewModel();

            if (ClientView != null) { ClientView.Visibility = Visibility.Collapsed; }
            ClientView = new ClientView();
            CurrentChildView = ClientView;
            Caption = "Customers";
            Icon = IconChar.UserGroup;
        }

        public ICommand OpenTransactionViewCommand { get; }
        private void OnOpenTransactionViewCommandexecute(object p)
        {
            //CurrentChildView = new TransactionViewModel();

            if (TransactionView != null) { TransactionView.Visibility = Visibility.Collapsed; }
            TransactionView = new TransactionView();
            CurrentChildView = TransactionView;   
            Caption = "Transaction";
            Icon = IconChar.MoneyBillTransfer;
        }

        public ICommand SearchClientCommand { get; }
        private void OnSearchClientCommandExecute(object p)
        {
            if (ClientView.SearchClientBox.Text != String.Empty)
            {
                foreach (Client client in Clients)
                {
                    if (ClientView.SearchClientBox.Text == client.ID || ClientView.SearchClientBox.Text == client.Surname || ClientView.SearchClientBox.Text == client.Name || ClientView.SearchClientBox.Text == client.Patronymic)
                    {
                        List<Client> list = new List<Client>();
                        list.Add(client);
                        ClientView.membersDataGrid.ItemsSource = list;
                    }
                }
            }
            else { ClientView.membersDataGrid.ItemsSource = Clients; }
        }

        string historyLogPatch = @"..\Debug\HistoryLog.xml";

        public ICommand OpenHistoryLogViewCommand { get; }
        private void OnOpenHistoryLohViewCommandExecute(object p)
        {
            if (HistoryLogView != null) { HistoryLogView.Visibility = Visibility.Collapsed; }

            HistoryLogView = new HistoryLogView();
            CurrentChildView = HistoryLogView;
            Caption = "История дейстий";
            Icon = IconChar.History;

            if (File.Exists(historyLogPatch))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ClientsInHistory>));
                using (FileStream fs = new FileStream(historyLogPatch, FileMode.OpenOrCreate))
                {
                    List<ClientsInHistory> newClients = xmlSerializer.Deserialize(fs) as List<ClientsInHistory>;
                    if (newClients != null)
                    {
                        foreach (ClientsInHistory client in newClients)
                        {
                            ClientsInHistories.Add(client);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        public ICommand DeleteClientCommand { get; }
        private void OnDeleteClientCommandExecute(object p)
        {
            if (!(p is Client client)) return;
            Clients.Remove(client);
           //WriteToFileHistoryLog($"\nУдалён Менеджером {client.ToString().ToUpper()}");
            XmlSerialize(Clients);
            ClientView.membersDataGrid.Items.Refresh();
        }

        /// <summary>
        /// Сохранение изменений клииента
        /// </summary>
        public ICommand ChangeClientCommand { get; }
        private void OnChangeClientCommandExecute(object p)
        {
            ClientView.membersDataGrid.UnselectAll();
            XmlSerialize(Clients);
            MainView.BellState.Visibility = Visibility.Collapsed;
            var icon = new PackIcon
            {
                Name = "BellStateAlert",
                Kind = PackIconKind.BellAlert,
                Height = 20,
                Width = 20,
                Foreground = new SolidColorBrush(Colors.DarkOrchid)
            };
            
            if (MainView.StackPanel.Children.Count < 5) 
            {
                MainView.StackPanel.Children.Add(icon);
            }

            // Foreground = "#784DFD"
            // Height = "20"
            // Width = "20"
        }

        /// <summary>
        /// Поиск клиента по ID 
        /// </summary>
        public ICommand SearchCommand { get; }
        private void OnSearchCommandExecute(object p)
        {
            SearchCommandMethod();
        }

        void SearchCommandMethod()
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.SearchBox.Text == client.ID)
                {
                    TransactionView.FoundBalanceBlock.Text = client.Bill;
                    TransactionView.DepFoundBalanceBlock.Text = client.DepBill;
                    if (client.DepBill == "Закрытый")
                    {
                        TransactionView.OpenDepositButton.Content = "Открыть";
                        TransactionView.OpenDepositButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        TransactionView.DepButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        TransactionView.DepFoundBalanceBlock.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else
                    {
                        TransactionView.OpenDepositButton.Content = "Закрыть";
                        TransactionView.OpenDepositButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        TransactionView.DepButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        TransactionView.DepFoundBalanceBlock.Background = new SolidColorBrush(Colors.White);
                    }

                    if (client.Bill == "Закрытый")
                    {
                        TransactionView.OpenNonDepositButton.Content = "Открыть";
                        TransactionView.OpenNonDepositButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        TransactionView.NonDepButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        TransactionView.FoundBalanceBlock.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else
                    {
                        TransactionView.OpenNonDepositButton.Content = "Закрыть";
                        TransactionView.OpenNonDepositButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        TransactionView.NonDepButton.Style = TransactionView.OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        TransactionView.FoundBalanceBlock.Background = new SolidColorBrush(Colors.White);
                    }
                }
            }

            foreach (Client client in Clients)
            {
                if (TransactionView.FromAccountTransaction.Text == client.ID && TransactionView.FromAccountTransaction.Text != String.Empty
                    && TransactionView.ToAccountTransaction.Text != string.Empty)
                {
                    TransactionView.FromIDNonDepositBox.Text = client.Bill;
                    TransactionView.FromIDDepositBox.Text = client.DepBill;
                }
            }

            foreach (Client client in Clients)
            {
                if (TransactionView.ToAccountTransaction.Text == client.ID && TransactionView.ToAccountTransaction.Text != String.Empty
                   && TransactionView.FromAccountTransaction.Text != String.Empty)
                {
                    TransactionView.ToIDDepositBox.Text = client.DepBill;
                    TransactionView.ToIDNonDepositBox.Text = client.Bill;
                }
            }
        }

        public ICommand OpenOrCloseNonDepositCommand { get; }
        private void OnOpenNonDepositCommandExecute(object p)
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.SearchBox.Text == client.ID)
                {
                    if (client.Bill == "Закрытый")
                    {
                        client.Bill = "0";
                    }
                    else
                    {
                        if (MessageBox.Show("Внимание весь счёт будет обнулён! Вы уверены что хотите продолжить?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                        {
                            client.Bill = "Закрытый";
                        }
                    }
                }
            }
            SearchCommandMethod();
            XmlSerialize(Clients);
        }

        public ICommand OpenOrCloseDepositCommand { get; }
        private void OnOpenDepositCommandExecute(object p)
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.SearchBox.Text == client.ID)
                {
                    if (client.DepBill == "Закрытый")
                    {
                        client.DepBill = "0";
                    }
                    else
                    {
                        if (MessageBox.Show("Внимание весь депозитный счёт будет обнулён! Вы уверены что хотите продолжить?", "Внимание",
                           MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                        {
                            client.DepBill = "Закрытый";
                        }
                    }
                }
            }
            SearchCommandMethod();
            XmlSerialize(Clients);
        }

        public ICommand NonDepPlusCommand { get; }
        private void OnNonDepPlusCommandExecute(object p)
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.NonDepAccountIDBlock.Text == client.ID && client.Bill != "Закрытый")
                {
                    if (TransactionView.NonDepAmountBlock.Text != string.Empty)
                    {
                        int sums = int.Parse(client.Bill) + int.Parse(TransactionView.NonDepAmountBlock.Text);
                        client.Bill = Convert.ToString(sums);
                    }
                }
            }
            SearchCommandMethod();
            XmlSerialize(Clients);
        }

        public ICommand DepositPlusCommand { get; }
        private void OnDepositPlusCommandExecute(object p)
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.DepAccountIDBlock.Text == client.ID && client.DepBill != "Закрытый")
                {
                    if (TransactionView.DepAmountBlock.Text != string.Empty)
                    {
                        int sums = int.Parse(client.DepBill) + int.Parse(TransactionView.DepAmountBlock.Text);
                        client.DepBill = Convert.ToString(sums);
                    }
                }
            }
            SearchCommandMethod();
            XmlSerialize(Clients);
        }

        public ICommand SearchIDFromToCommand { get; }
        private void OnSearchIDFromToCommandExecute(object p)
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.FromAccountTransaction.Text == client.ID && TransactionView.FromAccountTransaction.Text != String.Empty
                    && TransactionView.ToAccountTransaction.Text != string.Empty)
                {
                    TransactionView.FromIDNonDepositBox.Text = client.Bill;
                    TransactionView.FromIDDepositBox.Text = client.DepBill;

                    if (TransactionView.FromIDNonDepositBox.Text == "Закрытый")
                    {
                        TransactionView.FromIDNonDepositBox.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else { TransactionView.FromIDNonDepositBox.Background = new SolidColorBrush(Colors.White); }

                    if (TransactionView.FromIDDepositBox.Text == "Закрытый")
                    {
                        TransactionView.FromIDDepositBox.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else { TransactionView.FromIDDepositBox.Background = new SolidColorBrush(Colors.White); }

                }
                else continue;
            }

            foreach (Client client in Clients)
            {
                if (TransactionView.ToAccountTransaction.Text == client.ID && TransactionView.ToAccountTransaction.Text != String.Empty
                   && TransactionView.FromAccountTransaction.Text != String.Empty)
                {
                    TransactionView.ToIDDepositBox.Text = client.DepBill;
                    TransactionView.ToIDNonDepositBox.Text = client.Bill;

                    if (TransactionView.ToIDDepositBox.Text == "Закрытый") { TransactionView.ToIDDepositBox.Background = new SolidColorBrush(Colors.DarkRed); }
                    else { TransactionView.ToIDDepositBox.Background = new SolidColorBrush(Colors.White); }

                    if (TransactionView.ToIDNonDepositBox.Text == "Закрытый") { TransactionView.ToIDNonDepositBox.Background = new SolidColorBrush(Colors.DarkRed); }
                    else { TransactionView.ToIDNonDepositBox.Background = new SolidColorBrush(Colors.White); }
                }
                else continue;
            }
        }

        private static int minusClientBalance;
        private static bool ExistToBill = true;
        private static bool ExistToDepBill = true;
        private static Client dataNonDepositClient;
        private static Client dataDepositClient;

        /// <summary>
        /// Перевод счёта от найденного по ID клиента к другому
        /// </summary>
        public ICommand TransferCommand { get; }
        private void OnTransferCommandExecute(object p)
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.FromAccountTransaction.Text == client.ID && TransactionView.FromAccountTransaction.Text != String.Empty
                    && TransactionView.ToAccountTransaction.Text != String.Empty)
                {
                    if (TransactionView.TransactionAmountBlock.Text != String.Empty)
                    {
                        int amount = int.Parse(TransactionView.TransactionAmountBlock.Text);
                        int clientBalance = int.Parse(client.Bill) - amount;
                        client.Bill = Convert.ToString(clientBalance);
                        minusClientBalance = amount;

                        dataNonDepositClient = client;
                    }
                }
            }

            foreach (Client client in Clients)
            {
                if (client.Bill == "Закрытый") { ExistToBill = false; }
                if (TransactionView.ToAccountTransaction.Text == client.ID && TransactionView.ToAccountTransaction.Text != String.Empty
                    && TransactionView.FromAccountTransaction.Text != String.Empty)
                {
                    if (TransactionView.TransactionAmountBlock.Text != String.Empty && client.Bill != "Закрытый")
                    {
                        int sums = int.Parse(client.Bill) + minusClientBalance;
                        client.Bill = Convert.ToString(sums);
                    }
                    else { MessageBox.Show("Счёт клиента закрытый"); }
                }
            }
            SearchCommandMethod();
            XmlSerialize(Clients);
        }

        public ICommand DepTransferCommand { get; }
        private void OnDepTransferCommandExecute(object p)
        {
            foreach (Client client in Clients)
            {
                if (TransactionView.FromAccountTransaction.Text == client.ID && TransactionView.FromAccountTransaction.Text != String.Empty
                    && TransactionView.ToAccountTransaction.Text != String.Empty)
                {
                    if (TransactionView.DepTransactionAmountBlock.Text != String.Empty && client.Bill != "Закрытый")
                    {
                        int amount = int.Parse(TransactionView.DepTransactionAmountBlock.Text);
                        int clientBalance = int.Parse(client.DepBill) - amount;
                        client.DepBill = Convert.ToString(clientBalance);
                        minusClientBalance = amount;

                        dataDepositClient = client;
                    }
                }
            }

            foreach (Client client in Clients)
            {
                if (client.DepBill == "Закрытый") { ExistToDepBill = false; }

                if (TransactionView.ToAccountTransaction.Text == client.ID && TransactionView.ToAccountTransaction.Text != String.Empty
                    && TransactionView.FromAccountTransaction.Text != String.Empty)
                {
                    if (TransactionView.DepTransactionAmountBlock.Text != String.Empty && client.DepBill != "Закрытый")
                    {
                        int sums = int.Parse(client.DepBill) + minusClientBalance;
                        client.DepBill = Convert.ToString(sums);
                    }
                    else { MessageBox.Show("Депозитный счёт клиента закрытый"); }
                }
            }
            SearchCommandMethod();
            XmlSerialize(Clients);
        }

        public ICommand ChooseWorkerCommand { get; }
        private void OnChooseWorkerCommandExecute(object p)
        {
            if (MainView.AngleUpOrDown.Icon == IconChar.AngleUp)
            {
                MainView.AngleUpOrDown.Icon = IconChar.AngleDown;
                MainView.WorkerName.Text = "Консультант";
                MainView.ImageWorker.Fill = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(MainView), "/Images/IconUser.jpg")));
            }
            else
            {
                MainView.AngleUpOrDown.Icon = IconChar.AngleUp;              
                MainView.WorkerName.Text = "Менеджер";
                MainView.ImageWorker.Fill = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(MainView), "/Images/IconManager.png")));
                //ManagerWindow.WorkerImage.Source = new BitmapImage(new Uri("/Images/Manager.png", UriKind.Relative));
            }

        }

        public MainWindowViewModel() 
        {
            OpenMainViewCommand = new LambdaCommand(OnOpenMainViewCommandExecute);
            OpenClientViewCommand = new LambdaCommand(OnOpenClientCiewCommandExecute);
            OpenTransactionViewCommand = new LambdaCommand(OnOpenTransactionViewCommandexecute);
            OpenHistoryLogViewCommand = new LambdaCommand(OnOpenHistoryLohViewCommandExecute);
            DeleteClientCommand = new LambdaCommand(OnDeleteClientCommandExecute);
            ChangeClientCommand = new LambdaCommand(OnChangeClientCommandExecute);
            SearchCommand = new LambdaCommand(OnSearchCommandExecute);
            SearchClientCommand = new LambdaCommand(OnSearchClientCommandExecute);
            OpenOrCloseNonDepositCommand = new LambdaCommand(OnOpenNonDepositCommandExecute);
            OpenOrCloseDepositCommand = new LambdaCommand(OnOpenDepositCommandExecute);
            NonDepPlusCommand = new LambdaCommand(OnNonDepPlusCommandExecute);
            DepositPlusCommand = new LambdaCommand(OnDepositPlusCommandExecute);
            SearchIDFromToCommand = new LambdaCommand(OnSearchIDFromToCommandExecute);
            TransferCommand = new LambdaCommand(OnTransferCommandExecute);
            DepTransferCommand = new LambdaCommand(OnDepTransferCommandExecute);
            ChooseWorkerCommand = new LambdaCommand(OnChooseWorkerCommandExecute);

            Clients = new ObservableCollection<Client>();
            ClientsInHistories = new List<ClientsInHistory>();
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
