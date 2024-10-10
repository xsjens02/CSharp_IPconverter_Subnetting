namespace SubnettingProgram.Services
{
    public abstract class SubnetCalculator
    {
        /// <summary>
        /// retrieves subnetmask, based on amount of subnets needed
        /// </summary>
        /// <param name="amountSubnets">amount of subnets needed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetSubMask(int amountSubnets) => amountSubnets switch
        {
            2 => 192,
            6 => 224,
            14 => 240,
            30 => 248,
            62 => 252,
            126 => 254,
            _ => throw new ArgumentException("invalid number of subnets")
        };

        /// <summary>
        /// retrieves subnetmask base, based on amount of subnets needed
        /// </summary>
        /// <param name="amountSubnets">amount of subnets needed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetSubBase(int amountSubnets) => amountSubnets switch
        {
            2 => 64,
            6 => 32,
            14 => 16,
            30 => 8,
            62 => 4,
            126 => 2,
            _ => throw new ArgumentException("invalid number of subnets")
        };

        /// <summary>
        /// retrieves available bits in an octet when subnetted, based on amount of subnets needed
        /// </summary>
        /// <param name="amountSubnets">amount of subnets needed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetAvailableBits(int amountSubnets) => amountSubnets switch
        {
            2 => 6,
            6 => 5,
            14 => 4,
            30 => 3,
            62 => 2,
            126 => 1,
            _ => throw new ArgumentException("invalid number of subnets")
        };
    }
}
