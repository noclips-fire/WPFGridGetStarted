using DevExpress.Charts.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TestProjekt2
{
    /// <summary>
    /// Interaction logic for DeleteOrderWindow.xaml
    /// </summary>
    public partial class DeleteOrderWindow : Window
    {
        public Order selOrder;
        public DeleteOrderWindow(Order selectedOrder)
        {
            
            InitializeComponent();

            selOrder = selectedOrder;
            
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new NorthwindEntities()) 
            {
                var orderToDelete = db.Orders.FirstOrDefault(o => o.OrderID == selOrder.OrderID);

                if (orderToDelete != null)
                {
                    db.Orders.Remove(orderToDelete); 
                    db.SaveChanges();
                }

                System.Windows.MessageBox.Show("Order successfully deleted!");
                this.DialogResult = true;
            }
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
