using System.Windows.Controls;
using Panic.Repository;
using Panic.WPF.DataModel;
using System.Data;
using System.Windows;
using System.Windows.Data;

namespace Panic.WPF
{
  /// <summary>
  /// Interaction logic for DataConfiguration.xaml
  /// </summary>
  public partial class DataConfiguration : UserControl
  {
    private DataConfigurationViewModel data;
    public DataConfiguration()
    {
      InitializeComponent();
    }

    void data_LoadSiteComboBoxes(System.Collections.Generic.List<Model.Site> sites)
    {
      FromSiteColumn.ItemsSource = sites;
      ToSiteColumn.ItemsSource = sites;
    }

    public void InitialiseRepository(ISiteRepository sites, ILinkRepository links, IHardwareRepository hardware)
    {
      data = new DataConfigurationViewModel();
      data.LoadSiteComboBoxes += new LoadSiteComboBoxesEvent(data_LoadSiteComboBoxes);
      data.Initialise(sites, links, hardware);
      DataContext = data;
    }

    private void btnAddSite_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      data.AddNewSite();      
    }

    private void btnAddLink_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      data.AddNewLink();
    }

    private void btnAddHardware_Click(object sender, RoutedEventArgs e)
    {
      data.AddNewHardware();
    }
  }  
}
