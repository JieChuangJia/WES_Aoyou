using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LishenMesDBAccess.DBUtility;//Please add references
namespace LishenMesDBAccess.DAL
{
    /// <summary>
    /// 数据访问类:ShopMatprsCfgViewModel
    /// </summary>
    public partial class ShopMatprsCfgViewDal
    {
        public ShopMatprsCfgViewDal()
        { }
        #region  Method



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LishenMesDBAccess.Model.ShopMatprsCfgViewModel DataRowToModel(DataRow row)
        {
            LishenMesDBAccess.Model.ShopMatprsCfgViewModel model = new LishenMesDBAccess.Model.ShopMatprsCfgViewModel();
            if (row != null)
            {
                if (row["ShopName"] != null)
                {
                    model.ShopName = row["ShopName"].ToString();
                }
                if (row["MatPrsName"] != null)
                {
                    model.MatPrsName = row["MatPrsName"].ToString();
                }
                if (row["ZhengjiHongkao"] != null && row["ZhengjiHongkao"].ToString() != "")
                {
                    if ((row["ZhengjiHongkao"].ToString() == "1") || (row["ZhengjiHongkao"].ToString().ToLower() == "true"))
                    {
                        model.ZhengjiHongkao = true;
                    }
                    else
                    {
                        model.ZhengjiHongkao = false;
                    }
                }
                if (row["FujiHongkao"] != null && row["FujiHongkao"].ToString() != "")
                {
                    if ((row["FujiHongkao"].ToString() == "1") || (row["FujiHongkao"].ToString().ToLower() == "true"))
                    {
                        model.FujiHongkao = true;
                    }
                    else
                    {
                        model.FujiHongkao = false;
                    }
                }
                if (row["GemoHongkao"] != null && row["GemoHongkao"].ToString() != "")
                {
                    if ((row["GemoHongkao"].ToString() == "1") || (row["GemoHongkao"].ToString().ToLower() == "true"))
                    {
                        model.GemoHongkao = true;
                    }
                    else
                    {
                        model.GemoHongkao = false;
                    }
                }
                if (row["mark"] != null)
                {
                    model.mark = row["mark"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ShopName,MatPrsName,ZhengjiHongkao,FujiHongkao,GemoHongkao,mark ");
            strSql.Append(" FROM ShopMatprsCfgView ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" ShopName,MatPrsName,ZhengjiHongkao,FujiHongkao,GemoHongkao,mark ");
            strSql.Append(" FROM ShopMatprsCfgView ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ShopMatprsCfgView ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.mark desc");
            }
            strSql.Append(")AS Row, T.*  from ShopMatprsCfgView T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        */

        #endregion  Method
        #region  MethodEx

        #endregion  MethodEx
    }
}

