/*
 * Copyrigth © 2008 DreamFly All rigths reserved
 * File Name: ConfigManager.cs
 * Created By:    Benny Xu
 * Created Date:   2010-08-08 15:08:45
 * Description:    2010-08-08 15:08:45
 * Change History:  ID     ModifiedBy    ModifiedDate    Desciption   
 * 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for QueryStringManager
/// </summary>
public class QueryStringManager
{


    #region "fields"
    private HttpRequest _request;
    #endregion

    #region "public properties"

    public string BizCode
    {
        get
        {
            return GetVal(QueryString.PARAM_BIZCODE);
        }
    }

    public string ActionCode
    {
        get
        {
            return GetVal(QueryString.PARAM_ACTION_CODE);
        }
    }


    #endregion

    public QueryStringManager(HttpRequest request)
    {
        _request = request;
    }

    #region "private methods

    private string GetVal(string paramName)
    {
        return _request.QueryString[paramName] == null ? string.Empty : HttpUtility.UrlDecode(_request.QueryString[paramName]);
    }

    #endregion

}


public sealed class QueryString
{
    public const string PARAM_BIZCODE = "BizCode";

    public const string PARAM_ACTION_CODE = "ActionCode";
}
