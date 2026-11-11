using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ClientConstSet
{

    /// <summary>
    /// 新增
    /// </summary>
    public const string Add = "Action00001";

    /// <summary>
    /// 修改
    /// </summary>
    public const string Update = "Action00002";

    /// <summary>
    /// 查看
    /// </summary>
    public const string Select = "Action00003";

    /// <summary>
    /// 删除
    /// </summary>
    public const string Delete = "Action00004";

    /// <summary>
    /// 查看(模版)
    /// </summary>
    public const string SelectTemplate = "Action00005";

    /// <summary>
    /// 生成(模版生成监控表)
    /// </summary>
    public const string Release = "Action00006";

    /// <summary>
    /// 发布(监控表发布)
    /// </summary>
    public const string Publish = "Action00007";

    /// <summary>
    /// 新增(模版)
    /// </summary>
    public const string AddTemplate = "Action00008";

    /// <summary>
    /// 新增模版(从模版List跳转)
    /// </summary>
    public const string AddTemplateFromList = "Action00009";

    /// <summary>
    /// 监控表发布状态(已经发布)
    /// </summary>
    public const string IsRelease = "1";

    /// <summary>
    /// 监控表发布状态(未发布)
    /// </summary>
    public const string NotRelease = "0";

    /// <summary>
    /// 允许上传的附件格式
    /// </summary>
    public const string Attachment_AllowedType = "*.jpg;*.bmp;*.gif;*.jpeg;*.png;*.pdf;*.doc;*.docx;*.xls;*.xlst;*.ppt;*.pps;*.txt";

    /// <summary>
    /// 允许上传的附件大小
    /// </summary>
    public const int Attachment_MaxSize = 4;


    /// <summary>
    /// 动态监测 - 设备采集间隔 默认为200毫秒
    /// </summary>
    public const int Default_Dynamic_Period = 200;


    /// <summary>
    /// 动态监测 - 设备采集时间长度(毫秒)
    /// </summary>
    public const int Default_Dynamic_HowLong = 10 * 60 * 1000;

    /// <summary>
    /// 通带阻截频率
    /// </summary>
    public const int Default_Dynamic_TDZJPL = 2;

    /// <summary>
    /// 阻带阻截频率
    /// </summary>
    public const int Default_Dynamic_ZDZJPL = 1;
}