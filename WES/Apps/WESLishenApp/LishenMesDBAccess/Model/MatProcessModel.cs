using System;
namespace LishenMesDBAccess.Model
{
    /// <summary>
    /// MatProcessModel:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class MatProcessModel
    {
        public MatProcessModel()
        { }
        #region Model
        private string _shopname;
        private int _zhengjihongkao;
        private int _fujihongkao;
        private int _gemohongkao;
        private string _tag1;
        private string _tag2;
        private string _tag3;
        private string _tag4;
        private string _tag5;
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
        public int ZhengjiHongkao
        {
            set { _zhengjihongkao = value; }
            get { return _zhengjihongkao; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FujiHongkao
        {
            set { _fujihongkao = value; }
            get { return _fujihongkao; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GemoHongkao
        {
            set { _gemohongkao = value; }
            get { return _gemohongkao; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tag1
        {
            set { _tag1 = value; }
            get { return _tag1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tag2
        {
            set { _tag2 = value; }
            get { return _tag2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tag3
        {
            set { _tag3 = value; }
            get { return _tag3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tag4
        {
            set { _tag4 = value; }
            get { return _tag4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tag5
        {
            set { _tag5 = value; }
            get { return _tag5; }
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

