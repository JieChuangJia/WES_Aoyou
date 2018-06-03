using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace AsrsModel
{
    /// <summary>
    /// 货位坐标描述
    /// </summary>
    [DataContract]
    public class CellCoordModel
    {
        private int row = 0; //排
        private int col = 0; //列
        private int layer = 0;//层

        [DataMember]
        public int Row { get { return row; } set { this.row = value; } }

        [DataMember]
        public int Col { get { return col; } set { this.col = value; } }

        [DataMember]
        public int Layer { get { return layer; } set { this.layer = value; } }
        /// <summary>
        /// 货位扩展属性
        /// </summary>
        public string ExtProp1 { get; set; }
        public CellCoordModel(int row,int col,int layer)
        {
            this.row = row;
            this.col = col;
            this.layer = layer;
        }
        public bool Parse(string strPos)
        {
            try
            {
                string[] strArray = strPos.Split(new string[] { "-", ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray == null || strArray.Count() < 3)
                {
                    return false;
                }
                this.row = int.Parse(strArray[0]);
                this.col = int.Parse(strArray[1]);
                this.layer = int.Parse(strArray[2]);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
           

        }
        public string GetStr()
        {
            string str = string.Format("{0}-{1}-{2}",row,col,layer);
            return str;
        }
    }
}
