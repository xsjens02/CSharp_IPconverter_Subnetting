namespace SubnettingProgram.Models
{
    public class Subnet
    {
        public string IPaddress { get; set; }
        public string SubnetMask { get; set; }
        public int AvailableUnits { get; set; }

        public Subnet(string IPaddress, string subnetMask, int availableUnits)
        {
            this.IPaddress = IPaddress;
            SubnetMask = subnetMask;
            AvailableUnits = availableUnits;
        }
    }
}