using QuickGraph;

namespace Panic.WPF
{
  public class NetworkGraph : BidirectionalGraph<Vertex, Edge>
  {
    public NetworkGraph() { }

    public NetworkGraph(bool allowParallelEdges)
      : base(allowParallelEdges) { }

    public NetworkGraph(bool allowParallelEdges, int vertexCapacity)
      : base(allowParallelEdges, vertexCapacity) { }
  }
}
