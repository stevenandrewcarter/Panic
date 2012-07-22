using System.Diagnostics;
using Panic.Model;

namespace Panic.WPF {
  [DebuggerDisplay("{ID}")]
  public class Vertex {
    public string ID { get; private set; }
    public string Name { get; set; }
    public Site Site { get; set; }

    public Vertex(Site aSite) {
      Site = aSite;
      ID = Site.ID.ToString();
      Name = Site.Name;
    }

    public override string ToString() {
      return string.Format("{0}-{1}", ID, Name);
    }
  }
}
