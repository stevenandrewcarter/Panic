using System.Data.SqlServerCe;
using System.Windows;
using Panic.Repository;
using Panic.Repository.SQLRepository;

namespace Panic.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Private Variables
    private ISiteRepository siteRepository;
    private ILinkRepository linkRepository;
    private IHardwareRepository hardwareRepository;
    #endregion
    public MainWindow()
    {
      InitializeComponent();
      SqlCeConnection connection = new SqlCeConnection("Data Source=Panic.sdf");
      siteRepository = new SiteSQL(connection);
      linkRepository = new LinkSQL(connection);
      hardwareRepository = new HardwareSQL(connection);      
      graphVisualiser.InitialiseRepository(siteRepository, linkRepository);
      dataConfiguration.InitialiseRepository(siteRepository, linkRepository, hardwareRepository);
      graphVisualiser.Refresh();
    }

    private void btnGraphVisualiser_Click(object sender, RoutedEventArgs e)
    {
      graphVisualiser.Refresh();
      graphVisualiser.Visibility = System.Windows.Visibility.Visible;
      dataConfiguration.Visibility = System.Windows.Visibility.Hidden;
    }

    private void btnDataConfiguration_Click(object sender, RoutedEventArgs e)
    {
      graphVisualiser.Visibility = System.Windows.Visibility.Hidden;
      dataConfiguration.Visibility = System.Windows.Visibility.Visible;
    }    
  }
}
