using System.Windows.Controls;
using Panic.Repository;

namespace Panic.WPF {
  /// <summary>
  /// Interaction logic for GraphVisualiser.xaml
  /// </summary>
  public partial class GraphVisualiser : UserControl {
    private GraphPanelViewModel vm;

    public GraphVisualiser() {
      InitializeComponent();
    }

    public void Refresh() {
      graphLayout.Graph = null;
      vm.RebuildGraph();
      graphLayout.Graph = vm.Graph;
    }

    public void InitialiseRepository(ISiteRepository sites, ILinkRepository links) {
      vm = new GraphPanelViewModel(sites, links);
      DataContext = vm;
    }
  }
}
