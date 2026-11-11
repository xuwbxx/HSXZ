using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace TrainingsApp.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class FormBinding
    {
        /// <summary>
        /// 业务对象绑定到窗体或者控件容器
        /// </summary>
        /// <param name="obj">业务对象</param>
        /// <param name="container">窗体或者控件容器</param>
        public static void BindObjectToControls(object obj, Control container, bool isEnabled = true, params object[] paras)
        {
            if (obj == null)
                return;

            //判断是否具有模板页
            Page page = container as Page;
            if (page != null && page.Master != null)
            {
                container = page.Master.FindControl("ContentPlaceHolder1");
            }

            Type objType = obj.GetType();
            PropertyInfo[] objPropertiesArray = objType.GetProperties();

            //遍历业务对象属性
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                //根据业务对象属性名称，找到对象控件
                Control control = container.FindControl(objProperty.Name); 

                if (control == null)
                    continue;


                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    string propertyValue = objProperty.GetValue(obj, null) == null ? "" : objProperty.GetValue(obj, null).ToString();

                    listControl.SelectedIndex = listControl.Items.IndexOf(listControl.Items.FindByValue(propertyValue));

                    if (!isEnabled)
                    {
                        listControl.Enabled = listControl.Enabled ? isEnabled : listControl.Enabled;
                    }
                }
                else
                {
                    //通过反向查找，查找控件中是否包含Checked，Text等属性，
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

                    bool success = false;
                    success = FindAndSetControlProperty(obj, objProperty, control, controlPropertiesArray, "Checked", typeof(bool));

                    if (!success)
                        success = FindAndSetControlProperty(obj, objProperty, control, controlPropertiesArray, "Value", typeof(String));

                    if (!success)
                        success = FindAndSetControlProperty(obj, objProperty, control, controlPropertiesArray, "Text", typeof(String));

                    //设置Enable属性
                    if (!isEnabled)
                    {
                        FindAndSetControlProperty(isEnabled, control, controlPropertiesArray, "Enabled", typeof(bool));
                    }
                    else
                    {
                        if (paras.Length > 0)
                        {
                            if (Array.IndexOf(paras, control.ID) >= 0)
                            {
                                FindAndSetControlProperty(false, control, controlPropertiesArray, "Enabled", typeof(bool));
                            }
                        }
                    }

                }
            }
        }

        private static void FindAndSetControlProperty(bool isEnabled, Control control, PropertyInfo[] controlPropertiesArray, string propertyName, Type type)
        {
            if (control is Label || control is LiteralControl || control is IButtonControl || control is GridView)
                return;

            foreach (PropertyInfo controlProperty in controlPropertiesArray)
            {
                if (controlProperty.Name == propertyName && controlProperty.PropertyType == type &&
                    Convert.ToBoolean(controlProperty.GetValue(control, null)))
                {
                    controlProperty.SetValue(control, isEnabled, null);
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">业务对象</param>
        /// <param name="objProperty">业务对象属性</param>
        /// <param name="control">控件，与属性相对应</param>
        /// <param name="controlPropertiesArray">控件属性列表</param>
        /// <param name="propertyName">查找的属性名称</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static bool FindAndSetControlProperty(object obj, PropertyInfo objProperty, Control control, PropertyInfo[] controlPropertiesArray, string propertyName, Type type)
        {
            foreach (PropertyInfo controlProperty in controlPropertiesArray)
            {
                if (controlProperty.Name == propertyName && controlProperty.PropertyType == type)
                {
                    try
                    {
                        controlProperty.SetValue(control, Convert.ChangeType(objProperty.GetValue(obj, null), type), null);
                    }
                    catch
                    {
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 窗体控件值绑定到业务对象
        /// </summary>
        /// <param name="obj">业务对象</param>
        /// <param name="container">控件容器</param>
        public static void BindControlsToObject(object obj, Control container)
        {
            if (obj == null)
                return;

            //判断是否具有模板页
            Page page = container as Page;
            if (page != null && page.Master != null)
            {
                container = page.Master.FindControl("ContentPlaceHolder1");
            }

            Type objType = obj.GetType();
            PropertyInfo[] objPropertiesArray = objType.GetProperties();

            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                Control control = container.FindControl(objProperty.Name);

                if (control == null)
                    continue;

                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    if (listControl.SelectedItem != null)
                    {
                        if (objProperty.PropertyType.IsEnum)
                        {
                            object newVal = Enum.Parse(objProperty.PropertyType, listControl.SelectedItem.Value);
                            objProperty.SetValue(obj, Convert.ChangeType(newVal, objProperty.PropertyType), null);
                        }
                        else
                        {
                            try
                            {
                                objProperty.SetValue(obj, Convert.ChangeType(listControl.SelectedItem.Value, objProperty.PropertyType), null);

                            }
                            catch
                            {
                            }

                        }
                    }


                }
                else
                {
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

                    bool success = false;
                    success = FindAndGetControlProperty(obj, objProperty, control, controlPropertiesArray, "Checked", typeof(bool));

                    if (!success)
                        success = FindAndGetControlProperty(obj, objProperty, control, controlPropertiesArray, "Value", typeof(String));

                    if (!success)
                        success = FindAndGetControlProperty(obj, objProperty, control, controlPropertiesArray, "Text", typeof(String));
                }
            }
        }


        public static void BindControlsToObject(object obj, Control container, String tag)
        {
            if (obj == null)
                return;

            //判断是否具有模板页
            Page page = container as Page;
            if (page != null && page.Master != null)
            {
                container = page.Master.FindControl("ContentPlaceHolder1");
            }

            Type objType = obj.GetType();
            PropertyInfo[] objPropertiesArray = objType.GetProperties();

            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                Control control = container.FindControl(objProperty.Name);

                if (control == null)
                    continue;

                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    if (listControl.SelectedItem != null)
                    {
                        try
                        {
                            objProperty.SetValue(obj, Convert.ChangeType(listControl.SelectedItem.Value, objProperty.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            string ss = ex.Message;
                        }
                    }
                }
                else
                {
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

                    bool success = false;
                    success = FindAndGetControlProperty(obj, objProperty, control, controlPropertiesArray, "Checked", typeof(bool));

                    if (!success)
                        success = FindAndGetControlProperty(obj, objProperty, control, controlPropertiesArray, "SelectedDate", typeof(DateTime));

                    if (!success)
                        success = FindAndGetControlProperty(obj, objProperty, control, controlPropertiesArray, "Value", typeof(String));

                    if (!success)
                        success = FindAndGetControlProperty(obj, objProperty, control, controlPropertiesArray, "Text", typeof(String));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <param name="objProperty">实体属性</param>
        /// <param name="control"></param>
        /// <param name="controlPropertiesArray"></param>
        /// <param name="propertyName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool FindAndGetControlProperty(object obj, PropertyInfo objProperty, Control control, PropertyInfo[] controlPropertiesArray, string propertyName, Type type)
        {
            foreach (PropertyInfo controlProperty in controlPropertiesArray)
            {
                if (controlProperty.Name == propertyName && controlProperty.PropertyType == type)
                {
                    try
                    {
                        objProperty.SetValue(obj, Convert.ChangeType(controlProperty.GetValue(control, null), objProperty.PropertyType), null);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public static void BindDictionaryToControls<TKey, TVal>(IDictionary<TKey, TVal> dic, string targetProperty, Control container, bool isEnabled = true)
        {
            if (dic == null)
                return;
            //判断是否具有模板页
            Page page = container as Page;
            if (page != null && page.Master != null)
            {
                container = page.Master.FindControl("ContentPlaceHolder1");
            }
            foreach (TKey key in dic.Keys)
            {
                Control control = container.FindControl(key.ToString());
                TVal obj = dic[key];
                if (control == null || obj == null)
                    continue;
                Type tType = obj.GetType();
                PropertyInfo pInfo = tType.GetProperty(targetProperty);

                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    string targetPropertyVal = pInfo.GetValue(obj, null) == null ? "" : pInfo.GetValue(obj, null).ToString();
                    listControl.SelectedIndex = listControl.Items.IndexOf(listControl.Items.FindByValue(targetPropertyVal));
                    listControl.Enabled = listControl.Enabled ? isEnabled : listControl.Enabled;
                }
                else
                {
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();
                    bool success = false;
                    success = FindAndSetControlProperty(obj, pInfo, control, controlPropertiesArray, "Checked", typeof(bool));

                    if (!success)
                        success = FindAndSetControlProperty(obj, pInfo, control, controlPropertiesArray, "Value", typeof(String));
                    if (!success)
                        success = FindAndSetControlProperty(obj, pInfo, control, controlPropertiesArray, "Text", typeof(String));
                    if (!success)
                        success = FindAndSetControlProperty(obj, pInfo, control, controlPropertiesArray, "InnerHtml", typeof(String));
                    if (!success)
                        success = FindAndSetControlProperty(obj, pInfo, control, controlPropertiesArray, "Color", typeof(String));
                    //设置Enable属性
                    FindAndSetControlProperty(isEnabled, control, controlPropertiesArray, "Enabled", typeof(bool));

                }

            }
        }

        public static void BindControlToDictionary(Dictionary<string, object> dic, Control container)
        {
            if (dic == null)
                return;
            //判断是否具有模板页
            Page page = container as Page;
            if (page != null && page.Master != null)
            {
                container = page.Master.FindControl("ContentPlaceHolder1");
            }
            List<string> keys = new List<string>(dic.Keys);
            foreach (string key in keys)
            {
                Control control = container.FindControl(key);
                if (control == null)
                    continue;
                if (control is ListControl)
                {
                    ListControl lc = control as ListControl;
                    dic[key] = lc.SelectedValue;
                }
                else if (control is HtmlContainerControl)
                {
                    HtmlContainerControl hc = control as HtmlContainerControl;
                    dic[key] = hc.InnerHtml;
                }
                else
                {
                    //Checked,SelectedDate,Value,Text
                    bool success = false;
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

                    success = FindAndGetControlPropertyForDictionary(dic, control, "Checked", typeof(bool));
                    if (!success)
                        success = FindAndGetControlPropertyForDictionary(dic, control, "Value", typeof(string));
                    if (!success)
                        success = FindAndGetControlPropertyForDictionary(dic, control, "Text", typeof(string));
                    if (!success)
                        success = FindAndGetControlPropertyForDictionary(dic, control, "Color", typeof(string));
                }
            }
        }

        private static bool FindAndGetControlPropertyForDictionary(Dictionary<string, object> dic, Control control, string propertyName, Type type)
        {
            string id = control.ID;

            PropertyInfo p = control.GetType().GetProperty(propertyName);
            if (p != null && p.PropertyType == type)
            {
                dic[id] = p.GetValue(control, null);
                return true;
            }
            return false;
        }




        public static void BindXmlToControls(string xml, Control ctrIn, bool isEnabled = true)
        {
            if (string.IsNullOrEmpty(xml))
                return;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;
            foreach (Control ctr in (ctrIn as Page).Controls)
            {
                getControlValue(ctr, isEnabled, topM);
            }
        }

        private static void getControlValue(Control ctrIn, bool isEnabled, XmlNodeList topM)
        {
            foreach (Control ctr in ctrIn.Controls)
            {
                Type controlType = ctr.GetType();

                switch (controlType.ToString())
                {

                    case "SunGard.Shisan.Web.UI.Controls.TextControlExt.TextControlExt":
                        TextBox controlTextBoxObj = (TextBox)ctr;
                        foreach (XmlElement element in topM)
                        {
                            if (controlTextBoxObj.ID == element.Name)
                                controlTextBoxObj.Text = element.InnerText;
                        }
                        controlTextBoxObj.Enabled = isEnabled;
                        break;
                    default:
                        break;
                }
            }
        }


    }
}