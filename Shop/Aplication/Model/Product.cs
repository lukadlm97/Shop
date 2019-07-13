using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Product:INotifyPropertyChanged
    {
        public Product(int productId, string productName, decimal productPrice, int productQuantity, Producer productProducer)
        {
            _productId = productId;
            _productName = productName;
            _productPrice = productPrice;
            _productQuantity = productQuantity;
            _productProducer = productProducer;
        }

        private int _productId;

        public int ProductId
        {
            get { return _productId; }
            set
            {
                _productId = value;
                OnPropertyChanged("ProductId");
            }
        }

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        private decimal _productPrice;

        public decimal ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
                OnPropertyChanged("ProductPrice");
            }
        }
        private int _productQuantity;

        public int ProductQuantity
        {
            get { return _productQuantity; }
            set
            {
                _productQuantity = value;
                OnPropertyChanged("ProductQuantity");
            }
        }
        private Producer _productProducer;

        public event PropertyChangedEventHandler PropertyChanged;

        public Producer ProductProducer
        {
            get { return _productProducer; }
            set
            {
                _productProducer = value;
                OnPropertyChanged("ProductProducer");
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
