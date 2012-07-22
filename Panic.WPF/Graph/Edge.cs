using System.ComponentModel;
using System.Diagnostics;
using QuickGraph;

namespace Panic.WPF {
  /// <summary>
  /// A simple identifiable edge.
  /// </summary>
  [DebuggerDisplay("{Source.ID} -> {Target.ID}")]
  public class Edge : Edge<Vertex>, INotifyPropertyChanged {
    public string ID {
      get;
      private set;
    }

    public Edge(string id, Vertex source, Vertex target)
      : base(source, target) {
      ID = id;
    }

    #region INotifyPropertyChanged Implementation

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string info) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(info));
      }
    }

    #endregion
  }
}
