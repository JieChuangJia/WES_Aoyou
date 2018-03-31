﻿using System;
namespace MesDBAccess.Model
{
    /// <summary>
    /// DevLineBatteryCfgModel:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class DevLineBatteryCfgModel
    {
        public DevLineBatteryCfgModel()
        { }
        #region Model
        private string _devbatterycfgid;
        private string _shopsection;
        private string _lineid;
        private string _batterycatacode;
        private string _mark;
        private string _tag1;
        private string _tag2;
        private string _tag3;
        private string _tag4;
        private string _tag5;
        /// <summary>
        /// 产线ID跟电池型号配置关系ID
        /// </summary>
        public string DevBatteryCfgID
        {
            set { _devbatterycfgid = value; }
            get { return _devbatterycfgid; }
        }
        /// <summary>
        /// 工段名称，注液，二封
        /// </summary>
        public string ShopSection
        {
            set { _shopsection = value; }
            get { return _shopsection; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LineID
        {
            set { _lineid = value; }
            get { return _lineid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string batteryCataCode
        {
            set { _batterycatacode = value; }
            get { return _batterycatacode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mark
        {
            set { _mark = value; }
            get { return _mark; }
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
        #endregion Model

    }
}


