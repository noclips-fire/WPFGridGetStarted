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
using System.Windows.Forms;
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

            using (var db = new NorthwindEntities())
            {
                var customers = db.Customers.ToList();
                comboCustomers.ItemsSource = customers;
                comboCustomers.SelectedIndex = 0; 
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal freight;
                if (!decimal.TryParse(txtFreight.Text, out freight))
                {
                    System.Windows.MessageBox.Show("Invalid Freight value");
                    return;
                }

                if (comboCustomers.SelectedItem is Customer selectedCustomer)
                {
                    using (var db = new NorthwindEntities())
                    {


                        var newOrder = new Order
                        {

                            CustomerID = selectedCustomer.CustomerID,
                            EmployeeID = string.IsNullOrEmpty(txtEmployeeID.Text) ? (int?)null : Convert.ToInt32(txtEmployeeID.Text),
                            OrderDate = DateTime.Now,
                            ShippedDate = DateTime.Now,
                            ShipVia = (int)comboShipVia.SelectedValue,
                            Freight = string.IsNullOrEmpty(txtFreight.Text) ? (decimal?)null : decimal.Parse(txtFreight.Text),
                            RequiredDate = DateTime.Now.AddDays(7),
                            ShipName = "Servus",
                            ShipAddress = selectedCustomer.Address,
                            ShipCity = selectedCustomer.City,
                            ShipRegion = selectedCustomer.Region,
                            ShipCountry = selectedCustomer.Country,
                            ShipPostalCode = selectedCustomer.PostalCode,

                        };

                        db.Orders.Add(newOrder);
                        db.SaveChanges();
                        //db.Orders.Remove(newOrder);
                    }
                }

                System.Windows.MessageBox.Show("Order successfully added!");
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                System.Windows.MessageBox.Show("Details: " + ex.InnerException?.Message);
                //MessageBox.Show("Stack Trace: " + ex.StackTrace);
            }
        }
        private void comboCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCustomers.SelectedItem is Customer selectedCustomer)
            {
                txtShipCity.Text = selectedCustomer.City;
                txtShipCountry.Text = selectedCustomer.Country;
                txtShipAddress.Text = selectedCustomer.Address;
            }
        }
    }
    
}
