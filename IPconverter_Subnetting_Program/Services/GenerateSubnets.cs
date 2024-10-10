using System.Collections.ObjectModel;
using System.Text;
using SubnettingProgram.Models;

namespace SubnettingProgram.Services
{
    public abstract class GenerateSubnets
    {
        /// <summary>
        /// creates an observable collection of subnet objects based on IPaddress and amount of subnets  
        /// </summary>
        /// <param name="IPaddress">IPaddress to generate subnets from</param>
        /// <param name="amountSubnets">amount of subnets to generate</param>
        /// <returns></returns>
        public static ObservableCollection<Subnet> Action(string IPaddress, int amountSubnets)
        {
            ObservableCollection<Subnet> subnets = new ObservableCollection<Subnet>();

            string[] octets = IPaddress.Split('.');

            int netClass = GetNetClass(octets[0]);
            if (netClass == 0)
                return subnets;

            int subnetIndex = netClass;

            for (int i = subnetIndex; i < octets.Length; i++)
            {
                int octet = int.Parse(octets[i]);
                if (octet > 0)
                    subnetIndex = i + 1;
            }
            if (subnetIndex > 3)
                return subnets;

            string subnetmask = BuildSubnetmask(octets, subnetIndex, amountSubnets);
            int availableUnits = GetAvailableUnits(subnetIndex, amountSubnets);

            for (int i = 0; i < amountSubnets; i++)
            {
                string subnetIP = BuildSubnetIP(octets, subnetIndex, amountSubnets, i);
                subnets.Add(new Subnet(subnetIP, subnetmask, availableUnits));
            }
            return subnets;
        }

        /// <summary>
        /// retrieves IP class based on first octet of IPaddress
        /// </summary>
        /// <param name="firstOctet">first octet of IPaddress</param>
        /// <returns></returns>
        private static int GetNetClass(string firstOctet)
        {
            int octet = int.Parse(firstOctet);
            if (octet >= 1 && octet <= 126) return 1;
            if (octet >= 128 && octet <= 191) return 2;
            if (octet >= 192 && octet <= 223) return 3;
            return 0;
        }

        /// <summary>
        /// builds a subnetmask for a subnet
        /// </summary>
        /// <param name="octets">array of original IPaddress octets</param>
        /// <param name="subnetIndex">subnet octet index of IPaddress</param>
        /// <param name="amountSubnets">amount of subnets to retrieve subnetmask</param>
        /// <returns></returns>
        private static string BuildSubnetmask(string[] octets, int subnetIndex, int amountSubnets)
        {
            StringBuilder subnetmask = new StringBuilder();
            for (int i = 0; i < octets.Length; i++)
            {
                if (i < subnetIndex)
                    subnetmask.Append("255");
                else if (i == subnetIndex)
                    subnetmask.Append(SubnetCalculator.GetSubMask(amountSubnets));
                else
                    subnetmask.Append("000");

                if (i < octets.Length - 1)
                    subnetmask.Append(".");
            }
            return subnetmask.ToString();
        }

        /// <summary>
        /// builds an IPaddress for a subnet 
        /// </summary>
        /// <param name="octets">array of original IPaddress octets</param>
        /// <param name="subnetIndex">subnet octet index of IPaddress</param>
        /// <param name="amountSubnets">amount of subnets to retrieve subnetmask base</param>
        /// <param name="index">index of subnet IPaddress to build</param>
        /// <returns></returns>
        private static string BuildSubnetIP(string[] octets, int subnetIndex, int amountSubnets, int index)
        {
            var subnetIP = new StringBuilder();
            int subnet = SubnetCalculator.GetSubBase(amountSubnets) * (index + 1);

            for (int i = 0; i < octets.Length; i++)
            {
                if (i == subnetIndex)
                    subnetIP.Append(subnet);
                else
                    subnetIP.Append(octets[i]);

                if (i < octets.Length - 1)
                    subnetIP.Append(".");
            }

            return subnetIP.ToString();
        }

        /// <summary>
        /// calculates amount of available host on a subnet based on the subnet octet index of an IPaddress and the amount of subnets 
        /// </summary>
        /// <param name="subnetIndex">subnet octet index of IPaddress</param>
        /// <param name="amountSubnet">amount of subnets</param>
        /// <returns></returns>
        private static int GetAvailableUnits(int subnetIndex, int amountSubnet)
        {
            int AvailableUnits = 0;
            int availableBits = SubnetCalculator.GetAvailableBits(amountSubnet);

            if (subnetIndex == 1)
                AvailableUnits = (int)Math.Pow(2, availableBits + 16) - 2;
            else if (subnetIndex == 2)
                AvailableUnits = (int)Math.Pow(2, availableBits + 8) - 2;
            else if (subnetIndex > 2 && amountSubnet == 126)
                return AvailableUnits;
            else
                AvailableUnits = (int)Math.Pow(2, availableBits) - 2;
            return AvailableUnits;
        }
    }
}