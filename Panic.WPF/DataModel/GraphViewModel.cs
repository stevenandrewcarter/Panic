using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;
using Panic.Repository;
using Panic.Model;

namespace Panic.WPF
{
  public class NetworkGraphLayout : GraphLayout<Vertex, Edge, NetworkGraph> { }

  public class GraphPanelViewModel : INotifyPropertyChanged
  {
    #region Private Variables

    private ISiteRepository siteRepository;
    private ILinkRepository linkRepository;
    private string layoutAlgorithmType;
    private NetworkGraph graph;
    private List<String> layoutAlgorithmTypes = new List<string>();
    private Dictionary<int, Vertex> vertices;
    private bool reBuild;

    #endregion

    #region Ctor
    public GraphPanelViewModel(ISiteRepository aSiteRepository, ILinkRepository aLinkRepository)
    {
      Graph = new NetworkGraph(true);
      vertices = new Dictionary<int, Vertex>();
      siteRepository = aSiteRepository;
      siteRepository.SitesChanged += new SitesChangedHandler(siteRepository_SitesChanged);
      linkRepository = aLinkRepository;
      linkRepository.LinksChanged += new LinksChangedHandler(linkRepository_LinksChanged);
      //Add Layout Algorithm Types
      layoutAlgorithmTypes.Add("BoundedFR");
      layoutAlgorithmTypes.Add("Circular");
      layoutAlgorithmTypes.Add("CompoundFDP");
      layoutAlgorithmTypes.Add("EfficientSugiyama");
      layoutAlgorithmTypes.Add("FR");
      layoutAlgorithmTypes.Add("ISOM");
      layoutAlgorithmTypes.Add("KK");
      layoutAlgorithmTypes.Add("LinLog"); // This does work when the Graph Size is 1 
      layoutAlgorithmTypes.Add("Tree");
      RebuildGraph();

      //Pick a default Layout Algorithm Type
      LayoutAlgorithmType = "BoundedFR";      
    }

    public void RebuildGraph()
    {
      if (reBuild)
      {
        reBuild = false;
        vertices.Clear();
        Graph = new NetworkGraph(true);
        List<Site> sites = siteRepository.GetAll();
        foreach (Site s in sites)
        {
          if (s.Enabled)
          {
            vertices.Add(s.ID, new Vertex(s));
          }
        }
        Graph.AddVertexRange(vertices.Values);
        List<Link> links = linkRepository.GetAll();
        foreach (Link l in links)
        {
          if (l.Enabled)
          {
            AddNewGraphEdge(vertices[l.FromSiteID], vertices[l.ToSiteID]);
          }
        }      
      }
    }

    private void linkRepository_LinksChanged(Model.Link aLink)
    {
      reBuild = true;
    }

    private void siteRepository_SitesChanged(Model.Site aSite)
    {
      reBuild = true;
    }
    #endregion    

    #region Private Methods
    private Edge AddNewGraphEdge(Vertex from, Vertex to)
    {
      string edgeString = string.Format("{0}-{1} Connected", from.ID, to.ID);
      Edge newEdge = new Edge(edgeString, from, to);
      Graph.AddEdge(newEdge);
      return newEdge;
    }

    #endregion

    #region Public Properties

    public List<String> LayoutAlgorithmTypes
    {
      get { return layoutAlgorithmTypes; }
    }


    public string LayoutAlgorithmType
    {
      get 
      {
        if (graph.VertexCount < 2 && (layoutAlgorithmType == "LinLog" || layoutAlgorithmType == "EfficientSugiyama"))
        {
          layoutAlgorithmType = "BoundedFR";
        }
        return layoutAlgorithmType; 
      }
      set
      {
        layoutAlgorithmType = value;
        NotifyPropertyChanged("LayoutAlgorithmType");
      }
    }

    public NetworkGraph Graph
    {
      get 
      {        
        return graph; 
      }
      set
      {
        graph = value;
        // NotifyPropertyChanged("Graph");
      }
    }
    #endregion

    #region INotifyPropertyChanged Implementation

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(String info)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(info));
      }
    }

    #endregion
  }
}
