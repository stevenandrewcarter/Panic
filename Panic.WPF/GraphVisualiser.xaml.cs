using System;
using System.Collections.Generic;
using System.Linq;
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
using Panic.Repository;

namespace Panic.WPF
{
  /// <summary>
  /// Interaction logic for GraphVisualiser.xaml
  /// </summary>
  public partial class GraphVisualiser : UserControl
  {
    private GraphPanelViewModel vm;

    public GraphVisualiser()
    {
      InitializeComponent();      
    }

    public void Refresh()
    {
      graphLayout.Graph = null;
      vm.RebuildGraph();
      graphLayout.Graph = vm.Graph;
    }

    public void InitialiseRepository(ISiteRepository sites, ILinkRepository links)
    {
      vm = new GraphPanelViewModel(sites, links);
      DataContext = vm;
    }
  }
}
