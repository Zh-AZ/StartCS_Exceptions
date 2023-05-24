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
    }
}
