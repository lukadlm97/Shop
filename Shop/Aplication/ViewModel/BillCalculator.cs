using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.ViewModel
{
    class BillCalculator : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(List<Product> _billList,Product SelectedItem)
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
            foreach (var item in _billList)
            {
                if (item.ProductId == SelectedItem.ProductId)
                {
                    item.ProductQuantity++;
                    return;
                }
            }
            _billList.Add(newBillItem);
            return;
        }
    }
}
