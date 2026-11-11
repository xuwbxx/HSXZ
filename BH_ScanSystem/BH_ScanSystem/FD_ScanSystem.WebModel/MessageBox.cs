using System;
using System.Web.UI;
using System.Web;

/// <summary>
/// Summary description for MessageBox
/// </summary>
public class MessageBox {
    /// <summary>
    /// 显示具有指定文本的消息框
    /// </summary>
    /// <param name="text">要在消息框中显示的文本</param>
    public static void Show(string text) {
        Show(text, null, MessageBoxIcon.Default);
    }

    /// <summary>
    /// 显示具有指定文本和标题的消息框
    /// </summary>
    /// <param name="text">要在消息框中显示的文本</param>
    /// <param name="caption">要在消息框的标题栏中显示的文本</param>
    public static void Show(string text, string caption) {
        Show(text, caption, MessageBoxIcon.Default);
    }

    /// <summary>
    /// 显示具有指定文本和位图的消息框。
    /// </summary>
    /// <param name="text">要在消息框中显示的文本</param>
    /// <param name="image">要在消息框中显示的位图</param>
    public static void Show(string text, MessageBoxIcon image) {
        Show(text, null, image);
    }

    /// <summary>
    /// 显示具有指定文本的信息并跳转
    /// </summary>
    /// <param name="text">要在消息框中显示的文本</param>
    /// <param name="url">跳转的Url</param>
    public static void Redirect(string text, string url) {
        if (!string.IsNullOrEmpty(url)) {
            Page p = (System.Web.UI.Page)HttpContext.Current.Handler;
            string script = String.Format("<script language=\"javascript\">window.location=\"{0}\";</script>", url);

            p.ClientScript.RegisterStartupScript(p.GetType(), new Guid().ToString(), script);
        }

        MessageBox.Show(text);
    }

    /// <summary>
    /// 显示具有指定文本、标题和位图的消息框。
    /// </summary>
    /// <param name="text">要在消息框中显示的文本</param>
    /// <param name="caption">要在消息框的标题栏中显示的文本</param>
    /// <param name="image">要在消息框中显示的位图</param>
    public static void Show(string text, string caption, MessageBoxIcon icon) {
        Page p = (System.Web.UI.Page)HttpContext.Current.Handler;
        string script = string.Format("<script>alert_tips('{0}','{1}','{2}')</script>", text, MessageBoxIcon.Default, caption);
        ScriptManager.RegisterStartupScript(p, p.GetType(), new Guid().ToString(), script, false);
    }

    public static void Alert(string text) {
        Page p = (System.Web.UI.Page)HttpContext.Current.Handler;
        string script = string.Format("<script type='text/javascript'>alert('{0}')</script>", text);
        ScriptManager.RegisterStartupScript(p, p.GetType(), new Guid().ToString(), script, false);
    }

    public static void Alert(string text, string url) {
        Page p = (System.Web.UI.Page)HttpContext.Current.Handler;
        string script = string.Format("<script type='text/javascript'>alert('{0}');window.location.href='{1}';</script>", text, url);
        ScriptManager.RegisterStartupScript(p, p.GetType(), new Guid().ToString(), script, false);
    }

    /// <summary>
    /// 根据参数，调用指定函数
    /// </summary>
    /// <param name="function"></param>
    /// <param name="parameters"></param>
    public static void Invoke(string function, params object[] parameters) {
        string funStr = string.Format(function, parameters);
        funStr = string.Format("<script>{0}</script>", funStr);
        Page p = (System.Web.UI.Page)HttpContext.Current.Handler;
        ScriptManager.RegisterStartupScript(p, p.GetType(), new Guid().ToString(), funStr, false);
    }
}

public enum MessageBoxIcon {
    Default,
    Error,
    Sucess
}
