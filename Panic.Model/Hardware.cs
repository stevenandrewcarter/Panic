
namespace Panic.Model {
  public delegate void PropertyChangedEvent(Hardware aHardware);

  public class Hardware {
    #region Constructor

    public Hardware(int aID) {
      ID = aID;
    }

    #endregion

    #region Events

    public event PropertyChangedEvent Changed;

    #endregion

    #region Properties

    public int ID { get; private set; }
    public string Description { get; set; }
    public string Make { get; set; }
    public string ModelNumber { get; set; }
    public double TXCapacity { get; set; }
    public double RXCapacity { get; set; }
    public bool Enabled { get; set; }
    public string Notes { get; set; }

    #endregion

    #region Public Methods

    public void SetID(int aID) {
      if (ID == 0) {
        ID = aID;
      }
    }

    #endregion
  }
}
