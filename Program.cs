using System;
using System.Diagnostics;


namespace DroneBrain
{
    class Program
    {
        public static Process RaspProc = new Process();

        public static void check_procbuff_empty()
        {
            if(!RaspProc.StandardOutput.EndOfStream)
            {
                Console.WriteLine("Process buffer not empty. Content:");
                while(!RaspProc.StandardOutput.EndOfStream) Console.WriteLine(RaspProc.StandardOutput.ReadLine());
                Console.WriteLine("---------------------------");
            }
        }

        private static void Main(string[] args)
        {
            RaspProc.StartInfo.UseShellExecute = false;
            RaspProc.StartInfo.RedirectStandardOutput = true;

            var accelerometerData = new XyzUshortS();
            var magnetometerData = new XyzUshortS();
            var gyroscopeData = new XyzUshortS();


            // Enable bypass mode to get data from magnetometer (AK8963).
            I2C.set_byte(I2C_Regs_Addrs.MPU9255.DevAddr, I2C_Regs_Addrs.MPU9255.IntBypEnAddr, 
                I2C_Regs_Addrs.MPU9255.BypEnBit);

            // Set continuous mode of magnetometer data gathering.
            I2C.set_byte(I2C_Regs_Addrs.AK8963.DevAddr, I2C_Regs_Addrs.AK8963.Cntl1Addr,
                I2C_Regs_Addrs.AK8963.CntnsMode2Set);
           
            Console.WriteLine("Setting up connection with 10DOF IMU...");

            I2C.get_XYZ_data(I2C_Regs_Addrs.MPU9255.DevAddr, I2C_Regs_Addrs.MPU9255.AccAddr, 
                ref accelerometerData);

            I2C.get_XYZ_data(I2C_Regs_Addrs.MPU9255.DevAddr, I2C_Regs_Addrs.MPU9255.GyroAddr, 
                ref gyroscopeData);

            I2C.get_XYZ_data(I2C_Regs_Addrs.AK8963.DevAddr, I2C_Regs_Addrs.AK8963.MagnAddr,
                ref magnetometerData);

            Console.WriteLine(accelerometerData.ToString());
            Console.WriteLine(gyroscopeData.ToString());
            Console.WriteLine(magnetometerData.ToString());

           
        }
    }
}
