using System;
namespace LishenMesDBAccess.Model
{
    /// <summary>
    /// ShopMatprsCfgViewModel:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ShopMatprsCfgViewModel
    {
        public ShopMatprsCfgViewModel()
        { }
        #region Model
        private string _shopname;
        private string _matprsname;
        private bool _zhengjihongkao;
        private bool _fujihongkao;
        private bool _gemohongkao;
        private string _mark;
        /// <summary>
        /// 
        /// </summary>
        public string ShopName
        {
            set { _shopname = value; }
            get { return _shopname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MatPrsName
        {
            set { _matprsname = value; }
            get { return _matprsname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ZhengjiHongkao
        {
            set { _zhengjihongkao = value; }
            get { return _zhengjihongkao; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool FujiHongkao
        {
            set { _fujihongkao = value; }
            get { return _fujihongkao; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool GemoHongkao
        {
            set { _gemohongkao = value; }
            get { return _gemohongkao; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mark
        {
            set { _mark = value; }
            get { return _mark; }
        }
        #endregion Model

    }
}

