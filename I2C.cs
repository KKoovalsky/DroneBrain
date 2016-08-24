using System;
using System.Diagnostics;

namespace DroneBrain
{
    public class XyzUshortS
    {

        public ushort X, Y, Z;
        public byte[] RcvBuf;

        public XyzUshortS()
        {
            RcvBuf = new byte[6];
        }

        public void assign_data()
        {
            X = (ushort)(((ushort)(RcvBuf[0] << 8)) | (ushort)RcvBuf[1]);
            Y = (ushort)(((ushort)(RcvBuf[2] << 8)) | (ushort)RcvBuf[3]);
            Z = (ushort)(((ushort)(RcvBuf[4] << 8)) | (ushort)RcvBuf[5]);
        }

        public override string ToString()   
        {
            return X + " " + Y + " " + Z;
        }
    }

    internal class I2C
    {
        
        public static void get_XYZ_data(byte devAddr, byte regAddr, ref XyzUshortS dest)
        {
            Program.RaspProc.StartInfo.FileName = "i2cget";
            for (byte i = 0; i < 6; i++)
            {
                Program.RaspProc.StartInfo.Arguments = "-y 1 " + devAddr + " " + regAddr;\
                Program.RaspProc.Start();
                var readLine = Program.RaspProc.StandardOutput.ReadLine();
                if (readLine != null)
                    dest.RcvBuf[i] = Convert.ToByte(readLine.Substring(0, 4), 16);
                dest.assign_data();
                regAddr++;
            }

            Program.check_procbuff_empty();
        }

        public static void set_byte(byte devAddr, byte regAddr, byte data)
        {
            Program.RaspProc.StartInfo.FileName = "i2cset";
            Program.RaspProc.StartInfo.Arguments = "-y 1 " + devAddr + " " + regAddr + " " + data;
            Program.RaspProc.Start();

            Program.check_procbuff_empty();
        }
    }

    namespace I2C_Regs_Addrs
    {
        static class MPU9255 {
            public const byte DevAddr = 0x68;

            public const byte AccAddr = 59;
            public const byte GyroAddr = 67;


            public const byte IntBypEnAddr = 0x37;
            public const byte BypEnBit = 0x02;

        }

        static class AK8963
        {
            public const byte DevAddr = 0x0C;
            

            public const byte Cntl1Addr = 0x0A;

            public const byte BitSet = 0x10;
            public const byte CntnsMode2Set = 0x06;


            public const byte MagnAddr = 3;
        }

    }
}
