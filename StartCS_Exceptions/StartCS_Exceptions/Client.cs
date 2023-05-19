using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StartCS_Exceptions
{
    public class Client : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, args);

                if (args.PropertyName != "ItemSource")
                {
                    WriteToFileHistoryLog(propertyName, ID, Email, Surname, Name, Patronymic, NumberPhone, Address);
                }
            }
        }

        string path = @"..\Debug\HistoryLog.txt";
        async void WriteToFileHistoryLog(string propertyName, string ID, string Email, string Name, string Surname, string Patronymic, string NumberPhone, string Address)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                await sw.WriteLineAsync($"Изменено {propertyName.ToUpper()} У клиента <{ID}>-<{Email}>-<{Name}>-<{Surname}>-<{Patronymic}>-<{NumberPhone}>-<{Address}>");
            }
        }

        private string _ID;
        private string _Email;
        private string _Name;
        private string _Surname;
        private string _Patronymic;
        private string _NumberPhone;
        private string _Address;
        private string _Bill;
        private string _DepBill;

        public string ID
        {
            get => _ID;
            set
            {
                if (Equals(_ID, value)) return;
                _ID = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _Email;
            set
            {
                if (Equals(_Email, value)) return;
                _Email = value;
                OnPropertyChanged("Почта");
            }
        }

        public string Name
        {
            get => _Name;
            set
            {
                if (Equals(_Name, value)) return;
                _Name = value;
                OnPropertyChanged("Имя");
            }
        }

        public string Surname
        {
            get => _Surname;
            set
            {
                if (Equals(_Surname, value)) return;
                _Surname = value;
                OnPropertyChanged("Фамилия");
            }
        }

        public string Patronymic
        {
            get => _Patronymic;
            set
            {
                if (Equals(_Patronymic, value)) return;
                _Patronymic = value;
                OnPropertyChanged("Отчество");
            }
        }

        public string NumberPhone
        {
            get => _NumberPhone;
            set
            {
                if (Equals(_NumberPhone, value)) return;
                _NumberPhone = value;
                OnPropertyChanged("Номер телефона");
            }
        }

        public string Address
        {
            get => _Address;
            set
            {
                if (Equals(_Address, value)) return;
                _Address = value;
                OnPropertyChanged("Адрес");
            }
        }

        public string Bill
        {
            get => _Bill;
            set
            {
                if (Equals(_Bill, value)) return;
                _Bill = value;
                OnPropertyChanged("Счёт");
            }
        }

        public string DepBill
        {
            get => _DepBill;
            set
            {
                if (Equals(_DepBill, value)) return;
                _DepBill = value;
                OnPropertyChanged("Депозитный счёт");
            }
        }

        public Client() { }

        public Client(string id, string email, string name, string surname, string patronymic,
            string numberPhone, string address, string bill, string depBill)
        {
            ID = id;
            Email = email;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            NumberPhone = numberPhone;
            Address = address;
            Bill = bill;
            DepBill = depBill;
        }

        public override string ToString()
        {
            return String.Concat($"{ID} {Email} {Name} {Surname} {Patronymic} {NumberPhone} {Address}");
        }
    }
}
