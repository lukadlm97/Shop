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
    public class MagacinViewModel
    {
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; }
        }

      
        private ObservableCollection<Producer> _producerList = new ObservableCollection<Producer>();

        public ObservableCollection<Producer> ProducerList
        {
            get { return _producerList; }
            set { _producerList = value; }
        }


        private Product _selectedItem;

        public Product SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }

        private Product _newProduct;

        public Product NewProduct
        {
            get { return _newProduct; }
            set { _newProduct = value; }
        }


        public ICommand AddNewProductInMagacinCommand { get; set; }



        public MagacinViewModel()
        {
            LoadProducer();
            LoadProductList();
            AddNewProductInMagacinCommand = new Command(AddNewProductInMagacinMethod, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parametar)
        {
            return true;
        }

        private void AddNewProductInMagacinMethod(object parametar)
        {
            AddNewProductInMagacin();
        }

        private void AddNewProductInMagacin()
        {
            _products.Add(NewProduct);
        }

        public void LoadProductList()
        {
            int idProduct = 0;
            string nameProduct = "";
            decimal priceProduct = 0m;
            int quantityProduct = 0;
            Producer producerProduct = null;
           
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT* FROM dbo.Product";
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
                    _products.Add(new Product(idProduct, nameProduct, priceProduct, quantityProduct, producerProduct));
                }
            }
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
                    Producer newProducer = new Producer(_idProducer, _nameProducer, _addressProducer, _phoneNumberProducer, _cityProducer);
                    _producerList.Add(newProducer);
                }
            }
        }

        private SqlConnection CreateConnection()
        {
           return new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Luka\Documents\ShopDB.mdf;Integrated Security=True;Connect Timeout=30");
        }
    }
}
