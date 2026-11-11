using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;

using OfficeShop.Model;
using System.IO;
using OfficeShop.WebModel;
using OfficeShop.WebModel.Storage;

/// <summary>
///PageBase 的摘要说明
/// </summary>
public abstract class PageBase : Page
{
    public PageBase()
    {
        //this.Error += new EventHandler(PageBase_Error);
    }





    public virtual void RedirectTo(string url)
    {
        Response.Redirect(url);
    }

    public virtual void RedirectTo(string url, string message)
    {
        MessageBox.Redirect(message, url);
    }

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
    }


    //protected void PageBase_Error(object sender, EventArgs e)
    //{
    //    Exception ex = Server.GetLastError();
    //    ProcessUnhandledException(ex);
    //}

    //protected virtual void ProcessUnhandledException(string message)
    //{
    //    Exception ex = new Exception(message);
    //    ProcessUnhandledException(ex);
    //}

    //protected virtual void ProcessUnhandledException(Exception ex)
    //{
    //    //Response.Redirect()
    //    //if (ex == null)
    //    //    return;
    //    // Response.Redirect("~/Error.aspx");
    //}

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!IsPostBack)
        {
            UrlReferrer = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
        }
    }

    protected virtual string UrlReferrer
    {
        get
        {
            object url = ViewState["UrlReferrer"];
            return url == null ? "" : ViewState["UrlReferrer"].ToString();
        }
        set
        {
            ViewState["UrlReferrer"] = value;
        }
    }

    public virtual bool IsAuthenticated
    {
        get
        {
            return PassPort.IsAuthenticated;
        }
    }

    public UserContext CurrentUser
    {
        get
        {

            UserContext user = PassPort.CurrentUser;
            //if (user == null && IsAuthenticated) {
            //    Response.Redirect("~/UserLogout.ashx", true);
            //}

            return user;
        }
    }

    public bool IsLogined { get { return this.CurrentUser != null; } }

    #region Helper

    protected void Alert(string msg, string url = "")
    {
        if (string.IsNullOrEmpty(url))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("layer.msg('{0}',2,8);", msg), true);
        else
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("layer.msg('{0}',2,8,{1});", msg, "function (){window.location='" + url + "';}"), true);
    }



    protected void Successs(string msg, string url = "")
    {
        if (string.IsNullOrEmpty(url))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("layer.msg('{0}',2,1);", msg), true);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("layer.msg('{0}',2,1,{1});", msg, "function (){window.location='" + url + "';}"), true);
        }
    }

    //protected void Error(string msg)
    //{
    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("layer.msg('{0}',2,3);", msg), true);
    //}

    protected void Error(string msg, string url = "")
    {
        if (string.IsNullOrEmpty(url))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("layer.msg('{0}',2,3);", msg), true);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("layer.msg('{0}',2,3,{1});", msg, "function (){window.location='" + url + "';}"), true);
        }
    }

    public void SystemAlert(string msg)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ALERT_MESSAGE", string.Format("window.alert('{0}');", msg), true);
    }

    public string FormatDate(object date)
    {
        return FormatDate((DateTime)date);
    }

    public string FormatDate(DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    public string Truncate(string str, int maxLength)
    {
        return ZJFramework10.Utility.StringHelper.Truncate(ZJFramework10.Utility.StringHelper.NoHTML(str), maxLength);
    }

    public string Truncate(object str, int maxLength)
    {
        if (str == null) { return string.Empty; }
        return Truncate(str.ToString(), maxLength);
    }

    #endregion Helper


    internal class FormFixerHtml32TextWriter : System.Web.UI.Html32TextWriter
    {
        private string _url; // 伪装的URL
        internal FormFixerHtml32TextWriter(TextWriter writer)
            : base(writer)
        {
            _url = HttpContext.Current.Request.Path + HttpContext.Current.Request.Url.Query;
        }
        public override void WriteAttribute(string name, string value, bool encode)
        {
            if (_url != null && string.Compare(name, "action", true) == 0)
            {
                value = _url;
            }
            base.WriteAttribute(name, value, encode);
        }
    }

    internal class FormFixerHtmlTextWriter : System.Web.UI.HtmlTextWriter
    {
        private string _url;
        internal FormFixerHtmlTextWriter(TextWriter writer)
            : base(writer)
        {
            _url = HttpContext.Current.Request.Path + HttpContext.Current.Request.Url.Query;
        }
        public override void WriteAttribute(string name, string value, bool encode)
        {
            if (_url != null && string.Compare(name, "action", true) == 0)
            {
                value = _url;
            }
            base.WriteAttribute(name, value, encode);
        }
    }

}
