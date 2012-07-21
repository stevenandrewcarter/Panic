using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Panic.Repository;
using System.Data.SqlServerCe;
using Panic.Model;
using System;
using System.Collections.ObjectModel;

namespace Panic.WPF.DataModel
{
  public delegate void LoadSiteComboBoxesEvent(List<Site> sites);

  public class DataConfigurationViewModel : INotifyPropertyChanged
  {
    #region Private Variables
    private ISiteRepository siteRepository;
    private ILinkRepository linkRepository;
    private IHardwareRepository hardwareRepository;
    #endregion

    public ICollectionView Sites { get; private set; }
    public ICollectionView Links { get; private set; }
    public ICollectionView Hardwares { get; private set; }
    // public ObservableCollection<Site> Products { get; private set; }

    public event LoadSiteComboBoxesEvent LoadSiteComboBoxes;

    public DataConfigurationViewModel()
    {
    }
    
    public void Initialise(ISiteRepository aSiteRepository, ILinkRepository aLinkRepository, IHardwareRepository aHardwareRepository)
    {      
      // Create the Site Data Repository
      siteRepository = aSiteRepository;
      siteRepository.Populate();
      siteRepository.SitesChanged += new SitesChangedHandler(siteRepository_SitesChanged);
      BuildSiteInformation();
      // Create the Link Data Repository
      linkRepository = aLinkRepository;
      List<Link> links = linkRepository.Populate();
      Links = CollectionViewSource.GetDefaultView(links);
      // Create the Hardware Data Repository
      hardwareRepository = aHardwareRepository;
      List<Hardware> hardwares = hardwareRepository.Populate();
      Hardwares = CollectionViewSource.GetDefaultView(hardwares);
      NotifyPropertyChanged("Sites");
      NotifyPropertyChanged("Links");
      NotifyPropertyChanged("Hardwares");
    }

    private void siteRepository_SitesChanged(Site aSite)
    {
      BuildSiteInformation();
    }

    private void BuildSiteInformation()
    {
      List<Site> sites = siteRepository.GetAll();
      Sites = CollectionViewSource.GetDefaultView(sites);
      if (LoadSiteComboBoxes != null)
      {
        LoadSiteComboBoxes(sites);
      }
      NotifyPropertyChanged("Sites");
      NotifyPropertyChanged("Links");
    }

    public void AddNewSite()
    {
      Site aSite = new Site(0, "") 
      { 
        Enabled = false,
        Latitude = 0,
        Longitude = 0,
        LocalRX = 0,
        LocalTX = 0
      };
      siteRepository.Add(aSite);
      BuildSiteInformation();
    }

    public void AddNewLink()
    {
      Link aLink = new Link(0)
      {
        Enabled = false,
        FromSiteID = 1,
        ToSiteID = 2,
        RXOverride = 0,
        TXOverride = 0,
        HardwareID = 1
      };
      linkRepository.Add(aLink);
      Links = CollectionViewSource.GetDefaultView(linkRepository.GetAll());
      NotifyPropertyChanged("Links");
    }

    public void AddNewHardware()
    {
      Hardware aHardware = new Hardware(0)
      {
        Enabled = false        
      };
      hardwareRepository.Add(aHardware);
      Hardwares = CollectionViewSource.GetDefaultView(hardwareRepository.GetAll());
      NotifyPropertyChanged("Hardwares");
    }

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
