using Application.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.ViewModel
{
    public class ShopViewModel:ICommand
    {
        public ObservableCollection<Product> productList = new ObservableCollection<Product>();

        public ObservableCollection<Product> ProductList
        {
            get { return productList; }
            set { productList  = value; }
        }

        private Product _selectedItem;

        public Product SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }

        private ObservableCollection<Product> _billList = new ObservableCollection<Product>();

        public ObservableCollection<Product> BillList
        {
            get { return _billList; }
            set { _billList = value; }
        }

        List<Producer> _producerList = new List<Producer>();

        public event EventHandler CanExecuteChanged;

        public ICommand AddItemCommand
        {
            get;
            private set;
        }

        public ShopViewModel()
        {
            int idProduct=0;
            string nameProduct="";
            decimal priceProduct=0m;
            int quantityProduct=0;
            Producer producerProduct=null;
            LoadProducer();
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Luka\Documents\ShopDB.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select* from dbo.product";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    idProduct = (int)reader["Productid"];
                    nameProduct = reader["ProductName"].ToString();
                    priceProduct = (decimal)reader["productprice"];
                    quantityProduct = (int)reader["productquantity"];
                    int producerId = (int)reader["producerid"];

                    foreach (var producer in _producerList)
                    {
                        if (producerId == producer.ProducerId)
                            producerProduct = producer;
                    }
                    productList.Add(new Product(idProduct, nameProduct, priceProduct, quantityProduct, producerProduct));

                }
            }
            
            
        }

        private void AddBillItem()
        {
            int id = SelectedItem.ProductId;
            string name = SelectedItem.ProductName;
            decimal price = SelectedItem.ProductPrice;
            Producer producer = SelectedItem.ProductProducer;
            Product newBillItem = new Product(id, name, price, 0, producer);
            if (_billList == null)
            {   
                _billList.Add(newBillItem);
                return;
            }
            foreach(var item in _billList)
            {
                if(item.ProductId == SelectedItem.ProductId)
                {
                    item.ProductQuantity++;
                    return;
                }
            }
            _billList.Add(newBillItem);
            return;
        }

        private void LoadProducer()
        {
            int _idProducer;
            string _nameProducer;
            string _addressProducer;
            string _phoneNumberProducer;
            string _cityProducer;
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Luka\Documents\ShopDB.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                string query = $"select* from dbo.Producer";
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _idProducer = (int)reader["ProducerID"];
                    _nameProducer = reader["ProducerName"].ToString();
                    _addressProducer = reader["ProducerAddress"].ToString();
                    _phoneNumberProducer = "000";
                    _cityProducer = reader["ProducerCity"].ToString();
                    Producer newProducer = new Producer(_idProducer,_nameProducer,_addressProducer,_phoneNumberProducer,_cityProducer);
                    _producerList.Add(newProducer);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
