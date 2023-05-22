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
        public TransactionView()
        {
            InitializeComponent();
        }

        static ObservableCollection<Client> Clients = new ObservableCollection<Client>();
        string path = @"..\Debug\Client.xml";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlDeserialize(Clients);
            SearchCommandMethod();
            
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

            void SearchCommandMethod()
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
        }
    }
}
