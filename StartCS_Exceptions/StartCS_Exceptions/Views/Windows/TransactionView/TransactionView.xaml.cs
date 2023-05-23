using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace StartCS_Exceptions.Views.Windows.TransactionView
{
    /// <summary>
    /// Логика взаимодействия для TransactionView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        static ObservableCollection<Client> Clients; //= new ObservableCollection<Client>();
        string path = @"..\Debug\Client.xml";

        public TransactionView()
        {
            InitializeComponent();
            Clients = new ObservableCollection<Client>();
            XmlDeserialize();
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

        void XmlDeserialize()
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

        void SearchClient()
        {
            foreach (Client client in Clients)
            {
                if (SearchBox.Text == client.ID)
                {
                    FoundBalanceBlock.Text = client.Bill;
                    DepFoundBalanceBlock.Text = client.DepBill;
                    if (client.DepBill == "Закрытый")
                    {
                        OpenDepositButton.Content = "Открыть";
                        OpenDepositButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        DepButton.Style = OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        DepFoundBalanceBlock.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else
                    {
                        OpenDepositButton.Content = "Закрыть";
                        OpenDepositButton.Style = OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        DepButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        DepFoundBalanceBlock.Background = new SolidColorBrush(Colors.White);
                    }

                    if (client.Bill == "Закрытый")
                    {
                        OpenNonDepositButton.Content = "Открыть";
                        OpenNonDepositButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        NonDepButton.Style = OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        FoundBalanceBlock.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else
                    {
                        OpenNonDepositButton.Content = "Закрыть";
                        OpenNonDepositButton.Style = OpenDepositButton.TryFindResource("ButtonColorCloseStyle") as Style;
                        NonDepButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
                        FoundBalanceBlock.Background = new SolidColorBrush(Colors.White);
                    }
                }
            }

            foreach (Client client in Clients)
            {
                if (FromAccountTransaction.Text == client.ID && FromAccountTransaction.Text != String.Empty
                    && ToAccountTransaction.Text != string.Empty)
                {
                    FromIDNonDepositBox.Text = client.Bill;
                    FromIDDepositBox.Text = client.DepBill;
                }
            }

            foreach (Client client in Clients)
            {
                if (ToAccountTransaction.Text == client.ID && ToAccountTransaction.Text != String.Empty
                   && FromAccountTransaction.Text != String.Empty)
                {
                    ToIDDepositBox.Text = client.DepBill;
                    ToIDNonDepositBox.Text = client.Bill;
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchClient();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FoundBalanceBlock.Background = new SolidColorBrush(Colors.White);
            DepFoundBalanceBlock.Background = new SolidColorBrush(Colors.White);
            OpenNonDepositButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
            OpenNonDepositButton.Content = "";
            OpenDepositButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
            OpenDepositButton.Content = "";
            DepButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
            NonDepButton.Style = OpenDepositButton.TryFindResource("ButtonColorOpenStyle") as Style;
            FromIDNonDepositBox.Background = new SolidColorBrush(Colors.White);
            FromIDDepositBox.Background = new SolidColorBrush(Colors.White);
            ToIDNonDepositBox.Background = new SolidColorBrush(Colors.White);
            ToIDDepositBox.Background = new SolidColorBrush(Colors.White);

            SearchBox.Text = "";
            FoundBalanceBlock.Text = "";
            DepFoundBalanceBlock.Text = "";
            NonDepAccountIDBlock.Text = "";
            NonDepAmountBlock.Text = "";
            DepAccountIDBlock.Text = "";
            DepAmountBlock.Text = "";
            FromAccountTransaction.Text = "";
            ToAccountTransaction.Text = "";
            FromIDNonDepositBox.Text = "";
            FromIDDepositBox.Text = "";
            ToIDNonDepositBox.Text = "";
            ToIDDepositBox.Text = "";
            TransactionAmountBlock.Text = "";
            DepTransactionAmountBlock.Text = "";
        }

        private void NonDepButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in Clients)
            {
                if (NonDepAccountIDBlock.Text == client.ID && client.Bill != "Закрытый")
                {
                    if (NonDepAmountBlock.Text != string.Empty)
                    {
                        int sums = int.Parse(client.Bill) + int.Parse(NonDepAmountBlock.Text);
                        client.Bill = Convert.ToString(sums);
                    }
                }
            }
            SearchClient();
            XmlSerialize(Clients);
        }

        private void DepButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in Clients)
            {
                if (DepAccountIDBlock.Text == client.ID && client.DepBill != "Закрытый")
                {
                    if (DepAmountBlock.Text != string.Empty)
                    {
                        int sums = int.Parse(client.DepBill) + int.Parse(DepAmountBlock.Text);
                        client.DepBill = Convert.ToString(sums);
                    }
                }
            }
            SearchClient();
            XmlSerialize(Clients);
        }

        private void OpenNonDepositButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in Clients)
            {
                if (SearchBox.Text == client.ID)
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
            SearchClient();
            XmlSerialize(Clients);
        }

        private void OpenDepositButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in Clients)
            {
                if (SearchBox.Text == client.ID)
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
            SearchClient();
            XmlSerialize(Clients);
        }

        private void SeacrchButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in Clients)
            {
                if (FromAccountTransaction.Text == client.ID && FromAccountTransaction.Text != String.Empty
                    && ToAccountTransaction.Text != string.Empty)
                {
                    FromIDNonDepositBox.Text = client.Bill;
                    FromIDDepositBox.Text = client.DepBill;

                    if (FromIDNonDepositBox.Text == "Закрытый")
                    {
                        FromIDNonDepositBox.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else { FromIDNonDepositBox.Background = new SolidColorBrush(Colors.White); }

                    if (FromIDDepositBox.Text == "Закрытый")
                    {
                        FromIDDepositBox.Background = new SolidColorBrush(Colors.DarkRed);
                    }
                    else { FromIDDepositBox.Background = new SolidColorBrush(Colors.White); }

                }
                else continue;
            }

            foreach (Client client in Clients)
            {
                if (ToAccountTransaction.Text == client.ID && ToAccountTransaction.Text != String.Empty
                   && FromAccountTransaction.Text != String.Empty)
                {
                    ToIDDepositBox.Text = client.DepBill;
                    ToIDNonDepositBox.Text = client.Bill;

                    if (ToIDDepositBox.Text == "Закрытый") { ToIDDepositBox.Background = new SolidColorBrush(Colors.DarkRed); }
                    else { ToIDDepositBox.Background = new SolidColorBrush(Colors.White); }

                    if (ToIDNonDepositBox.Text == "Закрытый") { ToIDNonDepositBox.Background = new SolidColorBrush(Colors.DarkRed); }
                    else { ToIDNonDepositBox.Background = new SolidColorBrush(Colors.White); }
                }
                else continue;
            }
        }

        private static int minusClientBalance;
        private static bool ExistToBill = true;
        private static bool ExistToDepBill = true;
        private static Client dataNonDepositClient;
        private static Client dataDepositClient;

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in Clients)
            {
                if (FromAccountTransaction.Text == client.ID && FromAccountTransaction.Text != String.Empty
                    && ToAccountTransaction.Text != String.Empty)
                {
                    if (TransactionAmountBlock.Text != String.Empty)
                    {
                        int amount = int.Parse(TransactionAmountBlock.Text);
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
                if (ToAccountTransaction.Text == client.ID && ToAccountTransaction.Text != String.Empty
                    && FromAccountTransaction.Text != String.Empty)
                {
                    if (TransactionAmountBlock.Text != String.Empty && client.Bill != "Закрытый")
                    {
                        int sums = int.Parse(client.Bill) + minusClientBalance;
                        client.Bill = Convert.ToString(sums);
                    }
                    else { MessageBox.Show("Счёт клиента закрытый"); }
                }
            }
            SearchClient();
            XmlSerialize(Clients);
        }

        private void DepTransferButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Client client in Clients)
            {
                if (FromAccountTransaction.Text == client.ID && FromAccountTransaction.Text != String.Empty
                    && ToAccountTransaction.Text != String.Empty)
                {
                    if (DepTransactionAmountBlock.Text != String.Empty && client.Bill != "Закрытый")
                    {
                        int amount = int.Parse(DepTransactionAmountBlock.Text);
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

                if (ToAccountTransaction.Text == client.ID && ToAccountTransaction.Text != String.Empty
                    && FromAccountTransaction.Text != String.Empty)
                {
                    if (DepTransactionAmountBlock.Text != String.Empty && client.DepBill != "Закрытый")
                    {
                        int sums = int.Parse(client.DepBill) + minusClientBalance;
                        client.DepBill = Convert.ToString(sums);
                    }
                    else { MessageBox.Show("Депозитный счёт клиента закрытый"); }
                }
            }
            SearchClient();
            XmlSerialize(Clients);
        }
    }
}
