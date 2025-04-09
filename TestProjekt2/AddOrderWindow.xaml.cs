using DevExpress.Mvvm.Native;
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
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();

            comboShipVia.ItemsSource = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1, "Speedy Express"),
                new KeyValuePair<int, string>(2, "United Package"),
                new KeyValuePair<int, string>(3, "Federal Shipping")
            };

            comboShipVia.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new NorthwindEntities())
                {
                    var newOrder = new Order
                    {
                        CustomerID = txtCustomerID.Text,
                        EmployeeID = Convert.ToInt32(txtEmployeeID.Text),
                        OrderDate = DateTime.Now,
                        ShippedDate = DateTime.Now,
                        ShipVia = (int)comboShipVia.SelectedValue,
                        //string companyName = ((KeyValuePair<int, string>)comboShipVia.SelectedItem).Value;
                        Freight = decimal.Parse(txtFreight.Text),
                        RequiredDate = DateTime.Now.AddDays(7)
                    };

                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                }

                MessageBox.Show("Order successfully added!");
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
    
}
