using System.Collections.ObjectModel;
using System.Windows.Input;
using SubnettingProgram.Models;
using SubnettingProgram.Services;
using SubnettingProgram.Utilities;

namespace SubnettingProgram.ViewModels
{
    public class ViewModel : Bindable
    {

        public ICommand CreateSubnetsCMD { get; set; }

        private string _IPaddress;

        public string IPaddress
        {
            get { return _IPaddress; }
            set
            {
                _IPaddress = value;
                OnPropertyChanged();
            }
        }

        public int[] SubnetRanges { get; } = new int[] { 2, 6, 14, 30, 62, 126 };

        private int _selectedSubnetRange;

        public int SelectedSubnetRange
        {
            get { return _selectedSubnetRange; }
            set
            {
                _selectedSubnetRange = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Subnet> _subnetList;

        public ObservableCollection<Subnet> SubnetList
        {
            get { return _subnetList; }
            set
            {
                _subnetList = value;
                OnPropertyChanged();
            }
        }

        public ViewModel()
        {
            _IPaddress = "";
            _selectedSubnetRange = 0;
            _subnetList = new ObservableCollection<Subnet>();
            CreateSubnetsCMD = new RelayCommand(CreateSubnets, CanCreateSubnets);
        }

        private void CreateSubnets()
        {
            SubnetList = GenerateSubnets.Action(IPaddress, SelectedSubnetRange);
        }

        private bool CanCreateSubnets()
        {
            return !_IPaddress.Equals("") &&
                    _selectedSubnetRange != 0;
        }
    }
}
