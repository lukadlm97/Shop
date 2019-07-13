using Application.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    public class ShopViewModel
    {
        public ObservableCollection<Product> productList = new ObservableCollection<Product>();

        public ObservableCollection<Product> ProductList
        {
            get { return productList; }
            set { productList  = value; }
        }
        List<Producer> _producerList = new List<Producer>();
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

                    foreach(var producer in _producerList)
                    {
                        if (producerId == producer.ProducerId)
                            producerProduct = producer;
                    }
                }
                productList.Add(new Product(idProduct,nameProduct,priceProduct,quantityProduct,producerProduct));
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
                SqlCommand command = connection.CreateCommand();
                string query = "select* from dbo.Producer";
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _idProducer = (int)reader["ProducerID"];
                    _nameProducer = reader["ProducerName"].ToString();
                    _addressProducer = reader["ProducerAddress"].ToString();
                    _phoneNumberProducer = reader["ProducerPhoneNumber"].ToString();
                    _cityProducer = reader["ProducerCity"].ToString();
                    Producer newProducer = new Producer(_idProducer,_nameProducer,_addressProducer,_phoneNumberProducer,_cityProducer);
                    _producerList.Add(newProducer);
                }
            }
        }
    }
}
