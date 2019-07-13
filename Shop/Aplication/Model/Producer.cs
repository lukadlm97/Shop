using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Producer:INotifyPropertyChanged
    {
        public Producer(int producerId, string producerName, string producerAddress, string producerPhoneNumber, string producerCity)
        {
            _producerId = producerId;
            _producerName = producerName;
            _producerAddress = producerAddress;
            _producerPhoneNumber = producerPhoneNumber;
            _producerCity = producerCity;
        }
        private int _producerId;

        public int ProducerId
        {
            get { return _producerId; }
            set
            {
                _producerId = value;
                OnPropertyChanged("ProducerId");
            }
        }
        private string _producerName;

        public string ProducerName
        {
            get { return _producerName; }
            set
            {
                _producerName = value;
                OnPropertyChanged("ProducerName");
            }
        }
        private string _producerAddress;

        public string ProducerAddress
        {
            get { return _producerAddress; }
            set
            {
                _producerAddress = value;
                OnPropertyChanged("ProducerAddress");
            }
        }
        private string _producerPhoneNumber;

        public string ProducerPhoneNumber
        {
            get { return _producerPhoneNumber; }
            set
            {
                _producerPhoneNumber = value;
                OnPropertyChanged("ProducerPhoneNumber");
            }
        }
        private string _producerCity;
        public string ProducerCity
        {
            get { return _producerCity; }
            set
            {
                _producerCity = value;
                OnPropertyChanged("ProducerCity");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       
    }
}
