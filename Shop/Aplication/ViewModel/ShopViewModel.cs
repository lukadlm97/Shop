using Application.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private Bill _bill;

        public Bill Bill
        {
            get { return _bill; }
            set { _bill = value; }
        }

        private Product _selectedBillItem;

        public Product SelectedBillItem
        {
            get { return _selectedBillItem; }
            set { _selectedBillItem = value; }
        }


        List<Producer> _producerList = new List<Producer>();

        public ICommand AddNewBillItemCommand
        {
            get;
            set;
        }

        public ICommand RemoveBillItemCommand
        {
            get;
            set;
        }

        public ICommand CreateReportCommand
        {
            get;
            set;
        }


        public ShopViewModel()
        {
            int idProduct = 0;
            string nameProduct="";
            decimal priceProduct=0m;
            int quantityProduct = 0;
            Producer producerProduct = null;
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
            Bill = new Bill(0,0);
            AddNewBillItemCommand = new Command(ExecuteAddBillItemMethod,CanExecuteMethod);
            RemoveBillItemCommand = new Command(ExecuteRemoveBillItemMethod,CanExecuteMethod);
            CreateReportCommand = new Command(ExecuteCreateReportMethod,CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parametar)
        {
            return true;
        }
        private void ExecuteAddBillItemMethod(object parametar)
        {
            AddBillItem();
        }

        private void ExecuteRemoveBillItemMethod(object parametar)
        {
            RemoveBillItem();
        }

        private void ExecuteCreateReportMethod(object parametar)
        {
            CreateReport();
        }

        private void RemoveBillItem()
        {
            Func<decimal, decimal, decimal> compute = CalculateMinusSumBill;
            foreach(var billItem in _billList)
            {
                if(SelectedBillItem.ProductId == billItem.ProductId)
                {
                    if(billItem.ProductQuantity == 1)
                    {
                        billItem.ProductQuantity--;
                        Bill.TotalBill = compute(Bill.TotalBill, billItem.ProductPrice);
                        _billList.Remove(SelectedBillItem);
                        return;
                    }
                    billItem.ProductQuantity--;
                    Bill.TotalBill = compute(Bill.TotalBill, billItem.ProductPrice);
                    return;
                }
            }
        }

        private void AddBillItem()
        {
            Product newBillItem = CreatNewProductItem();
            Func<decimal, decimal, decimal> add = CalculateSumBill;
            if (_billList.Count == 0 && SelectedItem.ProductQuantity > 0)
            {
               // Bill.IdBill = 1;
                Bill.TotalBill = 0;
                _billList.Add(newBillItem);
                SelectedItem.ProductQuantity--;
                Bill.TotalBill = add(Bill.TotalBill, newBillItem.ProductPrice);
                return;
            }
            foreach(var item in _billList)
            {
                if(item.ProductId == SelectedItem.ProductId && SelectedItem.ProductQuantity > 0)
                {
                    item.ProductQuantity++;
                    SelectedItem.ProductQuantity--;
                    Bill.TotalBill = add(Bill.TotalBill, newBillItem.ProductPrice);
                    return;
                }
            }
            if (SelectedItem.ProductQuantity <= 0)
            {
                return;
            }
            _billList.Add(newBillItem);
            SelectedItem.ProductQuantity--;
            Bill.TotalBill = add(Bill.TotalBill, newBillItem.ProductPrice);
            return;
        }

        private void CreateReport()
        {
            string path = @"C:\Users\Luka\Desktop\Shop\Shop\Aplication\Bills\"+Bill.IdBill+".txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    string star = "*";
                    string line = "-";
                    sw.WriteLine(star.PadLeft(100,'*'));
                    sw.WriteLine("Fiskalni racun u nasoj prodavnici!");
                    sw.WriteLine(star.PadLeft(100, '*'));
                    sw.WriteLine();
                    sw.WriteLine("Racun ID:" +Bill.IdBill);
                    sw.WriteLine(star.PadLeft(100, '*'));
                    sw.WriteLine("Naziv stavke racuna:"+"\t\t"+"Kolicina"+"\t\t"+"Cena"+"\t\t"+"Ukupan iznos");
                    foreach(var item in _billList)
                    {
                        decimal sumForItem = item.ProductPrice * item.ProductQuantity;
                        sw.WriteLine(item.ProductName+"\t\t\t"+item.ProductQuantity+"\t\t\t"+item.ProductPrice+"\t\t\t"+sumForItem);
                    }
                    sw.WriteLine(line.PadLeft(100, '-'));
                    sw.WriteLine("Ukupno:\t\t\t\t\t\t\t\t\t\t"+Bill.TotalBill);
                    sw.WriteLine(star.PadLeft(100, '*'));
                }
            }
            RefreshDataBase();
            int reportId = Bill.IdBill+ 1;
            createNewBill(reportId);
            RemoveAllBillItems();
        }

        private void RefreshDataBase()
        {
           using(SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Luka\Documents\ShopDB.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                foreach(var product in productList)
                {
                    command.CommandText = $"UPDATE dbo.Product SET productQuantity = {product.ProductQuantity} WHERE productid = {product.ProductId}";
                    command.ExecuteNonQuery();
                    
                }
            }
        }

        public void RemoveAllBillItems()
        {
            var temp = _billList.ToList<Product>();
            foreach(var item in temp)
            {
                _billList.Remove(item);
            }
        }
        public void createNewBill(int id)
        {
            Bill.IdBill = id;
            Bill.TotalBill = 0;
        }

        private Product CreatNewProductItem()
        {
            int id = SelectedItem.ProductId;
            string name = SelectedItem.ProductName;
            decimal price = SelectedItem.ProductPrice;
            Producer producer = SelectedItem.ProductProducer;

            return new Product(id, name, price, 1, producer);
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

        static decimal CalculateSumBill(decimal currentBill,decimal newItemBill)
        {
            return currentBill + newItemBill; 
        }
       
        static decimal CalculateMinusSumBill(decimal currentBill,decimal dropItemBill)
        {
            return currentBill - dropItemBill;
        }
    }
}
