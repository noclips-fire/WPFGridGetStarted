using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Ribbon;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TestProjekt2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        private void printPreviewItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            myGridControl.View.ShowPrintPreview(this);

            //if (myTabControl.SelectedItem is DXTabItem selectedTab)
            //{
            //    if (selectedTab.Header.ToString() == "CardView")
            //    {
            //        myGridControl.View.ShowPrintPreview(this);
            //    }
            //    else if (selectedTab.Header.ToString() == "TreeListView")
            //    {
            //        secondGridControl.View.ShowPrintPreview(this);
            //    }
            //}

        }

        //private void ResetGridControl(DevExpress.Xpf.Grid.GridControl grid, DevExpress.Xpf.Grid.GridDataViewBase newView, object newSource)
        //{
        //    if (grid == null || newView == null)
        //        return;

        //    grid.View = null;

        //    grid.FilterString = string.Empty;
        //    grid.GroupSummary?.Clear();
        //    grid.TotalSummary?.Clear();

        //    try
        //    {
        //        grid.ClearSorting();
        //        grid.ClearGrouping();
        //    }
        //    catch { }

        //    grid.ItemsSource = null;
        //    grid.View = newView;
        //    grid.ItemsSource = newSource;
        //}

        //private void treeListViewItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    string viewResourceName = e.Item.Tag?.ToString();
        //    if (string.IsNullOrEmpty(viewResourceName)) return;

        //    var newView = Resources[viewResourceName] as DevExpress.Xpf.Grid.GridDataViewBase;
        //    if (newView == null) return;

        //    var viewModel = DataContext as ViewModel;
        //    if (viewModel == null) return;

        //    if (myTabControl.SelectedItem is DXTabItem selectedTab)
        //    {
        //        string tabHeader = selectedTab.Header?.ToString();

        //        if (tabHeader == "CardView")
        //        {
        //            if (newView is DevExpress.Xpf.Grid.TreeListView)
        //            {
        //                MessageBox.Show("TreeListView нельзя использовать с Orders.");
        //                return;
        //            }

        //            ResetGridControl(myGridControl, newView, viewModel.Orders);
        //        }
        //        else if (tabHeader == "TreeListView")
        //        {                  
        //            ResetGridControl(secondGridControl, newView, viewModel.Employees);
        //        }
        //    }
        //}

        //private void treeListViewItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    string viewResourceName = e.Item.Tag?.ToString();
        //    if (string.IsNullOrEmpty(viewResourceName)) return;

        //    var newView = Resources[viewResourceName] as DevExpress.Xpf.Grid.GridDataViewBase;
        //    if (newView == null) return;

        //    if (newView is DevExpress.Xpf.Grid.TableView tableView)
        //    {
        //        tableView.ShowTotalSummary = true;
        //        tableView.AutoWidth = true;
        //        tableView.EditFormShowMode = DevExpress.Xpf.Grid.EditFormShowMode.Inline;
        //        tableView.PrintAllGroups = false;
        //        tableView.AllowConditionalFormattingMenu = true;
        //    }           
        //    else if (newView is DevExpress.Xpf.Grid.CardView cardView)
        //    {
        //        cardView.ShowTotalSummary = true;
        //        cardView.PrintAllGroups = false;
        //    }

        //    myGridControl.View = newView;
        //}


        public class ShippingOrderInfo
        {
            public int OrderID { get; set; }
            public string CustomerID { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? RequiredDate { get; set; }
            public DateTime? ShippedDate { get; set; }
            public string ShipCompany { get; set; }
            public decimal Freight { get; set; }
            public decimal ShippingCost { get; set; }
            public int DeliveryTime { get; set; }
        }

        private void CalculateShippingCostItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(fromDateEditItem.EditValue is DateTime fromDate) || !(toDateEditItem.EditValue is DateTime toDate))
            {
                MessageBox.Show("Please select valid date range.");
                return;
            }

            using (var db = new NorthwindEntities())
            {
                var orders = db.Orders
                    .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate &&
                                o.ShippedDate != null && o.RequiredDate != null)
                    .ToList();

                var calculatedOrders = orders.Select(o => new ShippingOrderInfo
                {
                    OrderID = o.OrderID,
                    CustomerID = o.CustomerID,
                    OrderDate = o.OrderDate,
                    RequiredDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    ShipCompany = o.Shipper?.CompanyName,
                    Freight = o.Freight ?? 0,
                    DeliveryTime = Math.Max((o.RequiredDate.Value - o.ShippedDate.Value).Days, 0),
                    ShippingCost = Math.Max((o.RequiredDate.Value - o.ShippedDate.Value).Days, 0) *
                                   GetRate(o.ShipVia) + (o.Freight ?? 0) * 0.1m
                }).ToList();

                var tempGrid = new DevExpress.Xpf.Grid.GridControl
                {
                    ItemsSource = calculatedOrders
                };

                var tableView = new DevExpress.Xpf.Grid.TableView
                {
                    ShowTotalSummary = true,
                    EditFormShowMode = DevExpress.Xpf.Grid.EditFormShowMode.Inline
                };

                tempGrid.View = tableView;

                tempGrid.Columns.Add(new GridColumn { FieldName = "OrderID" });
                tempGrid.Columns.Add(new GridColumn { FieldName = "CustomerID" });
                tempGrid.Columns.Add(new GridColumn { FieldName = "OrderDate" });
                tempGrid.Columns.Add(new GridColumn { FieldName = "ShipCompany" });

                var deliveryTimeColumn = new GridColumn
                {
                    FieldName = "DeliveryTime"
                };

                tempGrid.Columns.Add(deliveryTimeColumn);

                var shippingCostColumn = new GridColumn
                {
                    FieldName = "ShippingCost"
                };

                shippingCostColumn.EditSettings = new TextEditSettings
                {
                    MaskType = MaskType.Numeric,
                    Mask = "c",
                    DisplayFormat = "c",
                };

                tempGrid.Columns.Add(shippingCostColumn);

                tableView.ShowPrintPreview(tempGrid);
            }

        }

        private decimal GetRate(int? shipVia)
        {
            switch (shipVia)
            {
                case 1:
                    return 5m;  // Speedy Express
                case 2:
                    return 7m;  // United Package
                case 3:
                    return 6m;  // Federal Shipping
                default:
                    return 5m;
            };
        }




    }   
}
