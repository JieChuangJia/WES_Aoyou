using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Web.Services.Description;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SysCfg;
namespace PrcsCtlModelsAoyou
{
   
   public class MesBatteryItem
   {
       public int channel = 0;
       public string batteryID = "";
   }
      
    public class WShelper
    {
        public static string url = "http://192.168.72.1:8092/MesFrameWork.asmx";

        /// < summary>           
        /// 动态调用web服务         
        /// < /summary>          
        /// < param name="url">WSDL服务地址< /param> 
        /// < param name="methodname">方法名< /param>           
        /// < param name="args">参数< /param>           
        /// < returns>< /returns>          
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return WShelper.InvokeWebService(url, null, methodname, args);
        }

        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = WShelper.GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient wc = new WebClient();
               
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CodeDomProvider provider = new CSharpCodeProvider();//设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = provider.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
        public static bool GetNGBatterys(string containerID,IList<MesBatteryItem> NGBatterys,ref string reStr)
        {
            try
            {
                NGBatterys = new List<MesBatteryItem>();
                JObject jsonParam = new JObject();
                jsonParam.Add("ContainerSN", containerID);
                string jsonStr = jsonParam.ToString();
                object[] addParams = new object[] { jsonStr };
                object result = WShelper.InvokeWebService(url, "GetContainerBadBattery", addParams);
                string strRES = result.ToString();
               
                if (strRES.ToUpper().Contains("NG") == true)
                {
                    reStr = strRES;
                    return false;
                }
                else
                {
                    if(string.IsNullOrWhiteSpace(strRES))
                    {
                        return true;
                    }
                    string[] strArray = strRES.Split(new string[] { "|"}, StringSplitOptions.RemoveEmptyEntries );
                    foreach(string str in strArray)
                    {
                        string[] batteryStrArray = str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if(batteryStrArray != null && batteryStrArray.Count()>1)
                        {
                            MesBatteryItem battery = new MesBatteryItem();
                            battery.batteryID = batteryStrArray[0];
                            int channel = 0;
                            if(!int.TryParse(batteryStrArray[1],out channel))
                            {
                                reStr = string.Format("不可解析的NG电芯位置号:{0},MES返回原始数据：{1}",batteryStrArray[1],strRES);
                                return false;
                            }
                            battery.channel = channel;
                            NGBatterys.Add(battery);
                        }
                    }
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                reStr = ex.ToString();
                return false ;
            }
        }
        #region 返回JSON字符串
        /// <summary>
        /// 返回JSON字符串
        /// </summary>
        /// <param name="RES">返回执行结果</param>
        /// <param name="CDetail">JSON数据类</param>
        /// <returns></returns>
        //public static string ReturnJsonData(string RES, string CONTROL_TYPE, List<ContentDetail> CDetail)
        //{
        //    JArray l_jarray = new JArray();
        //    JObject l_jobject = new JObject();
        //    JObject l_total = new JObject();

        //    for (int i = 0; i < CDetail.Count; i++)
        //    {
        //        l_jobject = new JObject();
        //        l_jobject.Add("M_FLAG", CDetail[i].M_FLAG);
        //        l_jobject.Add("M_DEVICE_SN", CDetail[i].M_DEVICE_SN);
        //        l_jobject.Add("M_WORKSTATION_SN", CDetail[i].M_WORKSTATION_SN);
        //        l_jobject.Add("M_EMP_NO", CDetail[i].M_EMP_NO);
        //        l_jobject.Add("M_AREA", CDetail[i].M_AREA);
        //        l_jobject.Add("M_MO", CDetail[i].M_MO);
        //        l_jobject.Add("M_MODEL", CDetail[i].M_MODEL);
        //        l_jobject.Add("M_CONTAINER_SN", CDetail[i].M_CONTAINER_SN);
        //        l_jobject.Add("M_SN", CDetail[i].M_SN);

        //        l_jobject.Add("M_UNION_SN", CDetail[i].M_UNION_SN);
        //        l_jobject.Add("M_LEVEL", CDetail[i].M_LEVEL);
        //        l_jobject.Add("M_EC_FLAG", CDetail[i].M_EC_FLAG);
        //        l_jobject.Add("M_ITEMVALUE", CDetail[i].M_ITEMVALUE);
        //        l_jobject.Add("M_TEST_TIME", CDetail[i].M_TEST_TIME);
        //        l_jobject.Add("M_DECRIPTION", CDetail[i].M_DECRIPTION);

        //        l_jobject.Add("M_ROUTE", CDetail[i].M_ROUTE);
        //        l_jobject.Add("M_GROUP", CDetail[i].M_GROUP);
        //        l_jobject.Add("M_ERROR_CODE", CDetail[i].M_ERROR_CODE);
        //        l_jobject.Add("M_ERROR_LEVEL", CDetail[i].M_ERROR_LEVEL);
        //        l_jobject.Add("M_ERROR_STATUS", CDetail[i].M_ERROR_STATUS);
        //        l_jobject.Add("M_ITEM_TYPE", CDetail[i].M_ITEM_TYPE);
        //        l_jobject.Add("M_POLAR", CDetail[i].M_POLAR);
        //        l_jobject.Add("M_LOG_ID", CDetail[i].M_LOG_ID);

        //        l_jobject.Add("M_MARK1", CDetail[i].M_MARK1);
        //        l_jobject.Add("M_MARK2", CDetail[i].M_MARK2);
        //        l_jobject.Add("M_MARK3", CDetail[i].M_MARK3);
        //        l_jobject.Add("M_MARK4", CDetail[i].M_MARK4);

        //        l_jarray.Add(l_jobject);
        //    }

        //    l_total.Add("RES", RES);
        //    l_total.Add("CONTROL_TYPE", CONTROL_TYPE);
        //    l_total.Add("M_COMENT", l_jarray);

        //    return l_total.ToString();
        //}
        #endregion

        ///// <summary>
        ///// 设备数据上传
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static RootObject DevDataUpload(int M_FLAG, string M_DEVICE_SN, string M_WORKSTATION_SN, string M_SN, string M_UNION_SN, string M_CONTAINER_SN, string M_LEVEL, string M_ITEMVALUE, ref string strJson, string CONTROL_TYPE)
        //{
        //    RootObject rObj = null;
        //    if(SysCfgModel.SimMode)
        //    {
        //        rObj = new RootObject();
        //        rObj.RES="OK";

        //        return rObj;
        //    }
        //    List<ContentDetail> CList = new List<ContentDetail>();
        //    ContentDetail tail = new ContentDetail();
        //    tail.M_FLAG = M_FLAG;
        //    tail.M_DEVICE_SN = M_DEVICE_SN;
        //    tail.M_WORKSTATION_SN = M_WORKSTATION_SN;
        //    tail.M_SN = M_SN;
        //    tail.M_UNION_SN = M_UNION_SN;
        //    tail.M_CONTAINER_SN = M_CONTAINER_SN;
        //    tail.M_LEVEL = M_LEVEL;
        //    tail.M_ITEMVALUE = M_ITEMVALUE;
        //    tail.CONTROL_TYPE = CONTROL_TYPE;
        //    CList.Add(tail);
        //    //上传参数
        //    strJson = WShelper.ReturnJsonData("OK", CONTROL_TYPE, CList);
        //    object objJson = strJson;
        //    object[] addParams = new object[] { objJson };
            
        //    object result = WShelper.InvokeWebService(url, "DxDataUploadJson", addParams);
        //    string strRES = result.ToString();
        //    rObj = new RootObject();
        //    rObj = JsonConvert.DeserializeObject<RootObject>(strRES);
        //    return rObj;
           
         
        //}

        ///// </summary>
        ///// <param name="jsonStr">格式化的json串</param>
        ///// <param name="restr"></param>
        ///// <returns>0上传成功，1上传成功，单返回NG，2传输失败</returns>
        //public static int UploadDataToMes(string jsonStr,ref string restr)
        //{
        //    RootObject rObj = new RootObject();

        //    object objJson = jsonStr;
        //    object[] addParams = new object[] { objJson };

        //    object result = WShelper.InvokeWebService(url, "DxDataUploadJson", addParams);
        //    string strRES = result.ToString();
        //    rObj = new RootObject();
        //    rObj = JsonConvert.DeserializeObject<RootObject>(strRES);
        //    restr = rObj.RES;
        //    if(rObj.RES.ToUpper().Contains("OK")== true)
        //    {
        //        return 0;
        //    }
        //    else if(rObj.RES.ToUpper().Contains("NG")== true)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return 2;
        //    }
        //}
       
    }
}
