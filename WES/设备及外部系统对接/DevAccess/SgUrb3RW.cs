using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReaderB;
using DevInterface;
namespace DevAccess
{
    public class SgUrb3RW:IrfidRW
    {
        #region 私有
        private byte readerID = 0;
        private string ipStr = "192.168.1.200";
        private uint netPort = 6000;
        private bool isOpened = false;
        private static byte comAddr = 0xff;
        private static int portHandle = 0;
        private const int INVEPC_BUFMAX = 256;
        private byte[] pwsdBytes;
        private IDictionary<int, string> errDisp = new Dictionary<int, string>();
        private object rwLock = new object();
        #endregion
        #region 公有
        public bool IsOpened { get { return isOpened; } }
        public byte ReaderID { get; set; }
        public SgUrb3RW(byte readerID, string ip, uint port)
        {
            this.readerID = readerID;
            this.ipStr = ip;
            this.netPort = port;
            pwsdBytes = new byte[8];//System.Text.UTF8Encoding.Default.GetBytes(accessPwsd);
            Array.Clear(pwsdBytes, 0, 8);
        }

       
        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            int re = StaticClassReaderB.OpenNetPort((int)netPort, ipStr, ref comAddr, ref portHandle);
            if (re == 0x35)
            {
                StaticClassReaderB.CloseNetPort(portHandle);
                re = StaticClassReaderB.OpenNetPort((int)netPort, ipStr, ref comAddr, ref portHandle);
            }
            if (re == 0x00)
            {
                isOpened = true;
                return true;
            }
            else
            {
                isOpened = false;
                return false;
            }
            
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            int re= StaticClassReaderB.CloseNetPort(portHandle);
            if (re == 0x00)
            {
                isOpened = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        public int ReadData()//ref byte[] recvByteArray
        {
            byte[] bytes = ReadBytesData();
            if(bytes == null)
            {
                return -1;
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        public byte[] ReadBytesData()
        {
            //先查询标签EPC
            byte[] EPC = new byte[64];
            int Totallen = 0; 
            int CardNum=0;
         

            int fCmdRet = StaticClassReaderB.Inventory_G2(ref comAddr, 0,0, 0, EPC, ref Totallen, ref CardNum,portHandle); 
            if ((fCmdRet == 1)| (fCmdRet == 2)| (fCmdRet == 3)| (fCmdRet == 4)|(fCmdRet == 0xFB) )//代表已查找结束
            {
              
                byte[] CardData = new byte[12];
                byte epcLen = EPC[0];
                byte[] validEPC = new byte[epcLen];
                Array.Copy(EPC,1,validEPC,0,epcLen);
                byte[] epcDataBuf = new byte[8];
                byte mem = 1;//EPC
                byte wordPtr = 2; //块起始地址,从第4个字节开始
                byte wordLen = 6; //块数
                int errCode = 0;
                fCmdRet = StaticClassReaderB.ReadCard_G2(ref comAddr, validEPC, mem, wordPtr, wordLen, pwsdBytes, 0, 0, 0, CardData, epcLen, ref errCode, portHandle);
                if(fCmdRet != 0)
                {
                    return null;
                }
                return CardData;
            }
            else
            {
                return null;
            }
           
        }
        public bool WriteBytesData(byte[] bytesData)
        {
            byte[] bytesDataToWriten = null;
            if (bytesData == null || bytesData.Count() < 0)
            {
                return false;
            }
            bytesDataToWriten = bytesData;
            byte wordLen = (byte)(bytesData.Count() / 2);
            if (wordLen * 2 < bytesData.Count())
            {
                wordLen++;
                bytesDataToWriten = new byte[wordLen * 2];
                bytesDataToWriten[bytesData.Count()] = 0;
                Array.Copy(bytesData, 0, bytesDataToWriten, 0, bytesData.Count());
            }
            byte writeEPClen = (byte)bytesDataToWriten.Count();
            if(writeEPClen>30)
            {
                writeEPClen = 30;
            }
            int errorCode = 0;
            int re = StaticClassReaderB.WriteEPC_G2(ref comAddr, pwsdBytes, bytesDataToWriten, writeEPClen, ref errorCode, portHandle);
            if(re != 0)
            {
                return false;
            }
            return true;
        }
        public Int64 ReadDataInt64()
        {
            byte[] bytes = ReadBytesData();
            if (bytes == null)
            {
                return -1;
            }
            return BitConverter.ToInt64(bytes, 0);
        }
        public bool WriteData(int val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            return WriteBytesData(bytes);
        }
        public bool WriteDataInt64(Int64 val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            return WriteBytesData(bytes);
        }
        public string ReadUID()//读电子标签的label ID
        {
            string tidStr = string.Empty;
            byte[] EPC = Inventory_EPC();
            if(EPC == null || EPC.Count()<2)
            {
                return string.Empty;
            }
            Console.WriteLine("识别EPC：" + ByteArrayToHexString(EPC));
            byte epcLen = (byte)EPC.Count();
            byte mem = 2;//TID
            byte wordPtr = 0; //块起始地址
            byte wordLen =4; //块数
            int errCode = 0;
            byte[] CardData = new byte[12];
            int fCmdRet = StaticClassReaderB.ReadCard_G2(ref comAddr, EPC, mem, wordPtr, wordLen, pwsdBytes, 0, 0, 0, CardData, epcLen, ref errCode, portHandle);
            if (fCmdRet != 0)
            {
                Console.WriteLine("读TID数据区失败，返回:" + fCmdRet);
                return string.Empty;
            }
            string strUID = ByteArrayToHexString(CardData);
            return strUID;
           /*
            string tidStr = string.Empty;
            int cards = 0;
            byte[] InvData = new byte[64];
            string sTID;
            byte AdrTID = 2;
            byte LenTID = 4;
            byte TIDFlag = 1;
            int Totallen = 0;
            int re = StaticClassReaderB.Inventory_G2(ref comAddr, AdrTID, LenTID, TIDFlag, InvData, ref Totallen, ref cards, portHandle);
            if (re == 0x30 || re == 0x37)
            {
                //网络断开
                IsOpened = false;
                if(!Connect())
                {
                    return string.Empty;
                }
                re = StaticClassReaderB.Inventory_G2(ref comAddr, AdrTID, LenTID, TIDFlag, InvData, ref Totallen, ref cards, portHandle);
            }
            if ((re == 1) || (re == 2) || (re == 3) || (re == 0xfb))
            {
                //询查结束
                if (cards < 1)
                {
                    return string.Empty;
                }
                int TIDClen = 0;
                TIDClen = InvData[0];
                byte[] tidBytes = new byte[TIDClen];
                Array.Copy(InvData, 1, tidBytes, 0, TIDClen);
                sTID = ByteArrayToHexString(tidBytes);
                tidStr = sTID;
            }
            return tidStr;*/
        }
        public string ReadStrData()
        {
            byte[] bytesData = ReadBytesData();
            if (bytesData == null)
            {
                return null;
            }
            return System.Text.Encoding.UTF8.GetString(bytesData);
        }
        #endregion
        private byte[] Inventory_EPC()
        {
            byte[] EPC = new byte[64];
            int Totallen = 0;
            int CardNum = 0;
            int fCmdRet = StaticClassReaderB.Inventory_G2(ref comAddr, 0, 0, 0, EPC, ref Totallen, ref CardNum, portHandle); 
            if ((fCmdRet == 1)| (fCmdRet == 2)| (fCmdRet == 3)| (fCmdRet == 4)|(fCmdRet == 0xFB) )//代表已查找结束
            {
                byte epcLen = EPC[0];
                byte[] validEPC = new byte[epcLen];
                Array.Copy(EPC, 1, validEPC, 0, epcLen);
                return validEPC;
            }
            else
            {
                return null;
            }
        }
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

    }
}
