using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Bill:INotifyPropertyChanged
    {
        private int _idBill;

        public int IdBill
        {
            get { return _idBill; }
            set {
                _idBill = value;
                OnPropertyChanged("IdBill");
            }
        }


        private decimal _totalBill;

        public decimal TotalBill
        {
            get { return _totalBill; }
            set
            {
                _totalBill = value;
                OnPropertyChanged("TotalBill");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
