using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
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

            if (myTabControl.SelectedItem is DXTabItem selectedTab)
            {
                if (selectedTab.Header.ToString() == "CardView")
                {
                    myGridControl.View.ShowPrintPreview(this);
                }
                else if (selectedTab.Header.ToString() == "TreeListView")
                {
                    secondGridControl.View.ShowPrintPreview(this);
                }
            }
        }

        private void ResetGridControl(DevExpress.Xpf.Grid.GridControl grid, DevExpress.Xpf.Grid.GridDataViewBase newView, object newSource)
        {
            if (grid == null || newView == null)
                return;

            grid.View = null;

            grid.FilterString = string.Empty;
            grid.GroupSummary?.Clear();
            grid.TotalSummary?.Clear();

            try
            {
                grid.ClearSorting();
                grid.ClearGrouping();
            }
            catch { }

            grid.ItemsSource = null;
            grid.View = newView;
            grid.ItemsSource = newSource;
        }

        private void treeListViewItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string viewResourceName = e.Item.Tag?.ToString();
            if (string.IsNullOrEmpty(viewResourceName)) return;

            var newView = Resources[viewResourceName] as DevExpress.Xpf.Grid.GridDataViewBase;
            if (newView == null) return;

            var viewModel = DataContext as ViewModel;
            if (viewModel == null) return;

            if (myTabControl.SelectedItem is DXTabItem selectedTab)
            {
                string tabHeader = selectedTab.Header?.ToString();

                if (tabHeader == "CardView")
                {
                    if (newView is DevExpress.Xpf.Grid.TreeListView)
                    {
                        MessageBox.Show("TreeListView нельзя использовать с Orders.");
                        return;
                    }

                    ResetGridControl(myGridControl, newView, viewModel.Orders);
                }
                else if (tabHeader == "TreeListView")
                {                  
                    ResetGridControl(secondGridControl, newView, viewModel.Employees);
                }
            }
        }

        //<dxr:RibbonPageGroup Caption = "Order Editing" >
        //                < dxb:BarButtonItem ItemClick = "NewItemAdd_ItemClick" x:Name="Add" Content="Add Order" LargeGlyph="{dx:DXImage SvgImages/Business Objects/BO_Document.svg}"/>
        //            </dxr:RibbonPageGroup>

        //private void NewItemAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        using (var northwindDbContext = new NorthwindEntities())
        //        {
        //            var newOrder = new Order
        //            {
        //                OrderDate = DateTime.Now,
        //                ShipCountry = "USA",
        //                CustomerID = "ALFKI"
        //            };

        //            northwindDbContext.Orders.Add(newOrder);

        //            northwindDbContext.SaveChanges();

        //            MessageBox.Show("New order added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

    }
}
