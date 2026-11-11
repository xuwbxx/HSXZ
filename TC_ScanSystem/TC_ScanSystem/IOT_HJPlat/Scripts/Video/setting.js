// ocxPanel.setting 配置预览工具栏
var settingPanel;
(function ($) {
    settingPanel = {
        errorPtzOpMsg: function (result, aname) {
            if (result.success) {
                if (result.success && result.ret == 1) {
                } else if (result.ret == -1) {
                    $.sticky(aname + language1['js.setting.js.operationfail'], {
                        type: "error"
                    });
                } else if (result.ret == -3) {
                    $.sticky(aname + language1['js.setting.js.notnet'], {
                        type: "attention"
                    });
                } else if (result.ret == 5023) {
                    $.sticky(aname + language1['js.setting.js.opfail'] + '该设备不支持此操作!', {
                        type: "attention"
                    });
                } else {
                    var msg = result.msg || language1['js.setting.js.unkownerror'];
                    $.sticky(aname + language1['js.setting.js.opfail'] + msg, {
                        type: "error"
                    });
                }

            } else {
                $.sticky(aname + language1['js.setting.js.sysinnererror'], {
                    type: "error"
                });
            }
        },
        openIoSwitch: function (camname, indexCode) {
            var url = pt.ctx + '/web/dialog/listIoSwitch.action';
            url = url + "?" + $.param({
                indexCode: indexCode
            }, $.ajaxSettings.traditional);
            $.dialog({
                width: 600,
                height: 500,
                id: 'list_ioSwitch_iframe',
                title: '[' + camname + language1['js.setting.js.output'],
                iframeSrc: url,
                buttons: [{
                    text: language1['close'],
                    click: function () {
                        this.close();
                    }
                }]
            });
        },

        uploadBreakdown: function (indexCode, isPlayback) {
            var url = pt.ctx + '/web/dialog/gotoBreakdown.action';
            url = url + "?" +
    		$.param({
    		    _top_iframe_name: 'mainFrame'
    		}, $.ajaxSettings.traditional);
            $.dialog({
                width: 800,
                height: 500,
                id: 'list_camera_iframe',
                title: language1['js.setting.js.error'],
                iframeSrc: url,
                buttons: [{
                    text: language1['streamMediaAllot.assign-group-edit.jsp.ok'],
                    'class': 'bPrimary',
                    click: function () {
                        var getInfos = top.frames["list_camera_iframe"].$('body').data('getInfos');
                        var infos = getInfos && getInfos(function (checkeds) { });
                        if (!infos) {
                            return;
                        } else {
                            this.close();
                            loader.show();
                            $.ajax({
                                url: pt.ctx + '/web/module/preview/sendMessageToEds.action',
                                type: 'POST',
                                data: {
                                    indexCode: indexCode,
                                    extendInfos: infos,
                                    isPlayback: isPlayback == true ? true : false
                                },
                                dataType: "json",
                                success: function (result) {
                                    jAlert(language1['js.setting.js.errorsuccess'], language1['js.monitor_dialog.js.prompt'], 'attention');
                                },
                                error: function (json) {
                                    jAlert(language1['js.setting.js.errorfail'], language1['js.monitor_dialog.js.prompt'], 'attention');
                                }
                            });
                            loader.hide();
                        }
                    }
                }, {
                    text: language1['close'],
                    click: function () {
                        this.close();
                    }
                }]
            });
        },
        openPoliceDialog: function (file, indexCode, type, DecodeTag, isPlayback) {
            var url = pt.ctx + '/web/dialog/gotoPoliceGathering.action';
            url = url + "?" + $.param({
                filePath: file,
                indexCode: indexCode,
                gatheringType: type,
                DecodeTag: DecodeTag == null ? '' : DecodeTag,
                _top_iframe_name: 'mainFrame'
            }, $.ajaxSettings.traditional);
            $.dialog({
                width: 800,
                height: 500,
                id: 'list_police_iframe',
                title: language1['js.setting.js.situation'],
                iframeSrc: url,
                buttons: [{
                    text: language1['streamMediaAllot.assign-group-edit.jsp.ok'],
                    'class': 'bPrimary',
                    click: function () {
                        var getInfos = top.frames["list_police_iframe"].$('body').data('getInfos');
                        var infos = getInfos && getInfos(function (checkeds) {
                        });
                        if (!infos) {
                            return;
                        } else {
                            loader.show();
                            $.ajax({
                                url: pt.ctx + '/web/module/preview/savePoliceGatheringInfos.action',
                                type: 'POST',
                                data: {
                                    indexCode: indexCode,
                                    extendInfos: infos,
                                    isPlayback: isPlayback == true ? true : false
                                },
                                dataType: "json",
                                success: function (result) {
                                    if (result.msg == "success") {
                                        jAlert(language1['js.setting.js.situationsuccess'], language1['js.monitor_dialog.js.prompt'], 'attention');
                                    } else if (result.msg == "upLoadFail") {
                                        jAlert(language1['js.setting.js.uploadsuccess'], language1['js.monitor_dialog.js.error'], 'error');
                                    } else {
                                        jAlert(language1['js.setting.js.situationfail'], language1['js.monitor_dialog.js.error'], 'error');
                                    }
                                }
                            });
                            loader.hide();
                            this.close();
                        }
                    }
                }, {
                    text: language1['close'],
                    click: function () {
                        this.close();
                    }
                }]
            });
        },
        saveUploadFile: function (file, indexCode, type, tauto, lWndId) {
            var ocxDiv = $('#uploadfile_ocx');
            if (!ocxDiv.length) {
                var ocxStr = '<div id="uploadfile_ocx" style="width:0%;height:0%;"></div>';
                $('body').append(ocxStr);
                ocxDiv = $('#uploadfile_ocx');
                var s = '\<script language="javascript" event="NotifyUploadResult(xml,code)" for="ocx_uploadfile_ocx"\> \n' + '$("#uploadfile_ocx").hikui().trigger("MsgNotify",xml,code); \n'
						+ '\<\/script\> \n';
                $('head')[0].appendChild($(s)[0]);
            }
            var hik = ocxDiv.hikui();
            function doRun(ocx) {
                ocx.toLocalFileUpLoad(file, indexCode, type, settingPanel.type, tauto);
            }
            if (hik == null) {
                var cameraId = null;
                if (settingPanel.type == 'monitor') {
                    var ocx = $('#preview_' + lWndId).hikui();
                    if (typeof ocx.getCameraIdByWinId != 'undefined') {
                        cameraId = ocx.getCameraIdByWinId(lWndId);
                    }
                }
                ocxDiv.hik().ocx_kmsfileupload({
                    listeners: {
                        create: function () {
                            doRun(this);
                        },
                        MsgNotify: function (xmlT, lErrorCode) {
                            var xml = $.parseXML(xmlT);
                            xml = $(xml);
                            var te = null;
                            if (xml.find('file_type').text() == '0') {
                                te = language1['localepicture']
                            } else {
                                te = language1['js.setting.js.locale']
                            }
                            var re = null;
                            var tobj = null;
                            if (lErrorCode == 0) {
                                re = language1['js.setting.js.uploadsuccess1'];
                            } else {
                                re = language1['js.setting.js.uploadfail'];
                                tobj = {
                                    type: "error"
                                };
                            }
                            $.sticky(te + re + ' <br/>[' + xml.find('camera_name').text() + ']', tobj);
                            var ocx = null;
                            if (settingPanel.type == 'monitor')
                                ocx = $('#preview_' + lWndId).hikui();
                            else {
                                ocx = playback_Obj != null ? playback_Obj.getOcx() : null;
                                ocx = $(ocx).parent().hikui();
                            }
                            if (!ocx) {
                                return;
                            }
                            if (cameraId) {
                                ocx.trigger('logByCameraId', cameraId, 'fileupload,' + te + re, lErrorCode == 0 ? 1 : 0);
                            } else {
                                ocx.trigger('log', lWndId, 'fileupload,' + te + re, lErrorCode == 0 ? 1 : 0);
                            }
                        }
                    }
                })
            } else {
                doRun(hik);
            }
        },
        video: {
            record: function (filename, folder, indexCode, lWndId) {
                folder = folder.replace(/&/g, '&amp;');
                var result, html = [];
                html.push('<div class="block block-msg">');
                html.push('<div class="bd">');
                html.push('<h6><a data-type="vedio" href="#" data-file="', folder, '\\', filename, '">', filename, '</a></h6>');
                html.push('<p><a data-type="folder" href="#" data-folder="', folder, '"><span class="ico i-folder-open"></span><span>' + language1['Opendirectory'] + '</span></a></p>');
                html.push('<p><a data-type="vedio"  href="#" data-file="', folder, '\\', filename, '"><span class="ico i-video-play"></span><span>' + language1['playtherecord'] + '</span></a></p>');
                // nihf-shan
                if (pt.type == 'sifa') {
                    html.push('<p><a data-type="upload"  data-indexcode = ' + indexCode + ' href="#" data-file="', folder, '\\', filename,
							'"><span class="ico i-video-play"></span><span>' + language1['js.setting.js.situation'] + '</span></a></p>');
                } else if (pt.type == 'nengyuan') {
                    var key = null;
                    if (settingPanel.type == 'monitor') {
                        key = 'ocx-setting-preview-isautoupload';
                    } else {
                        key = 'ocx-setting-video-isautoupload';
                    }
                    var isAutoUpload = $.commonCookie(key);
                    isAutoUpload = isAutoUpload == '0' ? '0' : '1';
                    if (isAutoUpload == '0') {
                        html.push('<p><a lWndId="' + lWndId + '" isAutoUpload="' + isAutoUpload + '" tatype="1" mtt="' + settingPanel.type + '" data-type="uploadFile" data-indexcode = ' + indexCode
								+ ' href="#" data-file="' + folder + '\\' + filename + '"><span class="ico i-video-play"></span><span>' + (isAutoUpload == '0' ? language1['manualdownload'] : language1['auto']) + '</span></a></p>');
                    } else {
                        settingPanel.saveUploadFile(folder + '\\' + filename, indexCode, '1', '1', lWndId);
                    }
                }
                // nihf-shan
                html.push('</div></div>');
                result = html.join('');
                jSticky({
                    message: result,
                    type: "st-msg st-video",
                    options: {
                        html: true,
                        position: "bottom-right",
                        autoclose: 5000
                    }
                });
            }
        },
        picture: {
            windowstatus: {
                status: [],
                data: []
            },
            capture: function (filename, folder, pfc, indexCode, lWndId, needUpload) {
                if (!filename || !folder) {
                    return;
                }
                folder = folder.replace(/&/g, '&amp;');
                var id = 'capture-info-' + pfc;
                // console.info('st' + id)
                var obj = settingPanel.picture.windowstatus.data[id];
                // console.info(obj)

                function setAutoupload(filename, folder, pfc, indexCode, lWndId) {
                    var key = 'ocx-setting-capture-isautoupload';
                    var isAutoUpload = $.commonCookie(key);
                    isAutoUpload = isAutoUpload == '0' ? '0' : '1';
                    if (isAutoUpload != '0') {
                        settingPanel.saveUploadFile(folder + '\\' + filename, indexCode, '0', '1', lWndId);
                    }
                    return isAutoUpload;
                }

                if (!obj) {
                    settingPanel.picture.windowstatus.data[id] = {
                        filenames: [],
                        filefolders: [],
                        total: 0,
                        current: 1
                    };
                    settingPanel.picture.windowstatus.data[id].filenames.push(filename);
                    settingPanel.picture.windowstatus.data[id].filefolders.push(folder);
                    settingPanel.picture.windowstatus.data[id].total = settingPanel.picture.windowstatus.data[id].total + 1;
                } else {
                    obj.filenames.push(filename);
                    obj.filefolders.push(folder);
                    obj.total = obj.total + 1;
                    var c = settingPanel.picture.windowstatus.data[id].current;
                    var t = settingPanel.picture.windowstatus.data[id].total;
                    top.$('#' + id + ' .bd .page-index').html(c + "/" + (parseInt(t)));
                    if (parseInt(t) != c) {
                        top.$('#' + id + ' .next a').removeClass('disabled');
                    }
                    var captureDiv = top.$('#' + id);
                    var filenames = settingPanel.picture.windowstatus.data[id].filenames;
                    var filefolders = settingPanel.picture.windowstatus.data[id].filefolders;
                    captureDiv.attr('data-total', parseInt(t));
                    captureDiv.attr('data-filename', filenames.join(','));
                    captureDiv.attr('data-folder', filefolders.join(','));
                    if (pt.type == 'nengyuan') {
                        setAutoupload(filename, folder, pfc, indexCode, lWndId);
                    }
                    return;
                }
                var captureDiv = $('#' + id);
                if (captureDiv && captureDiv.length > 0) {
                    var c = captureDiv.attr('data-current');
                    var t = captureDiv.attr('data-total');
                    var filenames = captureDiv.attr('data-filename').split(',');
                    var filefolders = captureDiv.attr('data-folder').split(',');
                    filenames.push(filename);
                    filefolders.push(folder);
                    captureDiv.attr('data-total', parseInt(t) + 1);
                    captureDiv.attr('data-filename', filenames.join(','));
                    captureDiv.attr('data-folder', filefolders.join(','));
                    $('#' + id + ' .bd .page-index').html(c + "/" + (parseInt(t) + 1));
                    $('#' + id + ' .next a').removeClass('disabled');
                    if (pt.type == 'nengyuan') {
                        setAutoupload(filename, folder, pfc, indexCode, lWndId);
                    }
                    return;
                }

                var nextClass = " disabled";
                var preClass = " disabled";
                var result, html = [];
                html.push('<div id="', id, '" data-current="1" data-total="1" data-filename="', filename, '" data-folder="', folder, '" class="block block-msg">');
                html.push('<div class="bd">');
                html.push('<h6><a open-type="0"  data-type="pic" href="#" data-file="', folder, '\\', filename, '">', filename, '</a></h6>');
                html.push('<p><a data-type="folder"  href="#" data-folder="', folder, '"><span class="ico i-folder-open"></span><span>' + language1['Opendirectory'] + '</span></a></p>');
                html.push('<p><a open-type="1"  data-type="pic" href="#" data-file="', folder, '\\', filename, '"><span class="ico i-video-play"></span><span>' + language1['editpicture'] + '</span></a></p>');
                // nihf-shan
                if (needUpload != null && !needUpload) {
                    //告警中心，无需处理上传
                } else {
                    if (pt.type == 'sifa') {
                        html.push('<p><a open-type="2"  data-type="uploadPic" data-indexcode = ' + indexCode + ' href="#" data-file="', folder, '\\', filename,
							'"><span class="ico i-video-play"></span><span>' + language1['js.setting.js.situation'] + '</span></a></p>');
                    } else if (pt.type == 'nengyuan') {
                        if (setAutoupload(filename, folder, pfc, indexCode, lWndId) == '0') {
                            var isAutoUpload = '0';
                            html.push('<p><a lWndId="' + lWndId + '" isAutoUpload="' + isAutoUpload + '" tatype="0" mtt="' + settingPanel.type + '" data-type="uploadFile" data-indexcode = ' + indexCode
								+ ' href="#" data-file="' + folder + '\\' + filename + '"><span class="ico i-video-play"></span><span>' + (isAutoUpload == '0' ? language1['manualdownload'] : language1['auto']) + '</span></a></p>');
                        }
                    }
                }
                // nihf-shan
                html.push('<div class="pagination"><ul class="pages"><li class="prev"><a data- href="#" class="', preClass, '"><span class="ico i-prev"></span></a></li><li class="page-index">', 1,
						'/', 1, '</li><li class="next"><a class="', nextClass, '" href="#"><span class="ico i-next"></span></a></li></ul></div>');
                html.push('</div></div>');
                result = html.join('');
                jSticky({
                    message: result,
                    type: "st-msg st-picture",
                    options: {
                        id: "sticky_" + id,
                        position: "bottom-right",
                        html: true,
                        autoclose: 5000
                    },
                    afterclose: function () {
                        settingPanel.picture.windowstatus.data[id] = null;
                    },
                    afteropen: function (options) {
                        console.info("afteropen")
                        var c = settingPanel.picture.windowstatus.data[id].current;
                        var t = settingPanel.picture.windowstatus.data[id].total;
                        console.info(c + "afteropen" + t)
                        top.$('#' + id + ' .bd .page-index').html(c + "/" + (parseInt(t)));
                        if (parseInt(t) != c) {
                            top.$('#' + id + ' .next a').removeClass('disabled');
                        }
                        var captureDiv = top.$('#' + id);
                        var filenames = settingPanel.picture.windowstatus.data[id].filenames;
                        var filefolders = settingPanel.picture.windowstatus.data[id].filefolders;
                        captureDiv.attr('data-total', parseInt(t));
                        captureDiv.attr('data-filename', filenames.join(','));
                        captureDiv.attr('data-folder', filefolders.join(','));

                        function seta(self, vv) {
                            if (top.$(self).hasClass("disabled")) {
                                return;
                            }
                            var c = top.$('#' + id).attr('data-current');
                            var t = top.$('#' + id).attr('data-total');
                            var filenames = top.$('#' + id).attr('data-filename').split(',');
                            var filefolders = top.$('#' + id).attr('data-folder').split(',');
                            if (vv == 1)
                                var current = parseInt(c) + 1;
                            else
                                var current = parseInt(c) - 1;
                            top.$('#' + id).attr('data-current', current);
                            if (current != parseInt(t)) {
                                top.$('#' + id + ' .next a').removeClass("disabled");
                            } else {
                                top.$('#' + id + ' .next a').addClass("disabled");
                            }
                            if (current > 1) {
                                top.$('#' + id + ' .prev a').removeClass("disabled");
                            } else {
                                top.$('#' + id + ' .prev a').addClass("disabled");
                            }
                            top.$('#' + id + ' .bd h6 a').html(filenames[current - 1]);
                            top.$('#' + id + ' .bd .page-index').html(current + "/" + t);

                            top.$('#' + id + ' .bd a[data-file]').attr("data-file", filefolders[current - 1] + '\\' + filenames[current - 1]);
                            top.$('#' + id + ' .bd a[data-folder]').attr("data-folder", filefolders[current - 1]);
                        }
                        top.$('#' + id + ' .prev a').click(function () {
                            seta(this, 0);
                        });
                        top.$('#' + id + ' .next a').click(function () {
                            seta(this, 1);
                        });
                    }
                });
            }
        },
        setting: {
            loaded: false,
            load: function (callback) {
                if (!settingPanel.setting.loaded) {
                    settingPanel.setting.loaded = true;
                    if (settingPanel.type == 'monitor') {
                        //$('#grids_div').hide();
                    } else if (settingPanel.type == 'playback') {
                        $('#replay-container').hide();
                    }
                    $.ajax({
                        url: pt.ctx + '/web/module/setting/settings.action',
                        error: function () {
                            settingPanel.setting.loaded = false;
                            if (settingPanel.type == 'monitor') {
                                $('#grids_div').show();
                            } else if (settingPanel.type == 'playback') {
                                $('#replay-container').show();
                            }
                        },
                        success: function (data) {
                            $('body').append(data);
                            // 系统设置
                            $('#settingDialog').dialog({
                                autoOpen: false,
                                modal: true,
                                width: 720,
                                height: 450,
                                buttons: {
                                    "restore": {
                                        text: language1['js.setting.js.recoverdefault'],
                                        'class': 'floatL',
                                        click: function () {
                                            var self = this;
                                            function aaa() {
                                                var result = settingPanel.setting.reset();
                                                if (result) {
                                                    callback();
                                                    self.close();
                                                    $(self).remove();
                                                    settingPanel.setting.loaded = false;
                                                    if (settingPanel.type == 'monitor') {
                                                        $('#grids_div').show();
                                                    } else if (settingPanel.type == 'playback') {
                                                        $('#replay-container').show();
                                                    }
                                                    if ($('[data-trigger="monitor.config"]').length)
                                                        $('[data-trigger="monitor.config"]').trigger("click");
                                                    if ($('#playback_ocx').hikui())
                                                        $('#playback_ocx').hikui().trigger("MsgNotify", 0x0200000D, null, null, null);
                                                }
                                            }
                                            function restoreClip() {

                                            }
                                            jConfirm(language1['js.setting.js.recoverdefault1'], language1['js.setting.js.recoverdefault'], function (r) {
                                                if (r) {
                                                    aaa();
                                                }
                                            });
                                        }
                                    },
                                    "ok": {
                                        text: language1['streamMediaAllot.assign-group-edit.jsp.ok'],
                                        'class': 'bPrimary',
                                        click: function () {
                                            //校验当启用大于N路自动转换为子码流的功能时，填写的路数是否是数字
                                            if ($('#btn-select-plan li[class="active"]').length != 0) {
                                                var type = $('#btn-select-plan li[class="active"]').get(0).value;
                                                if (type == '3') {
                                                    if ($('#toSubStreamEnable').attr('checked')) {
                                                        var winNum = $('#winNum').val().trim();
                                                        var num = parseInt(winNum);
                                                        if (winNum - num >= 0) {
                                                            //为数字不做任何操作
                                                        } else {    //表示填的内容是非数字  
                                                            jAlert('填的内容必须为数字，且值在[1,25]之间', '提示', 'attention');
                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                            var result = settingPanel.setting.save();
                                            if (result) {
                                                callback();
                                                this.close();
                                                $(this).remove();
                                                settingPanel.setting.loaded = false;
                                                if (settingPanel.type == 'monitor') {
                                                    $('#grids_div').show();
                                                } else if (settingPanel.type == 'playback') {
                                                    $('#replay-container').show();
                                                }
                                            }
                                        }
                                    },
                                    "cancel": {
                                        text: language1['js.monitor_expand.js.cancel'],
                                        click: function () {
                                            this.close();
                                            $(this).remove();
                                            settingPanel.setting.loaded = false;
                                            if (settingPanel.type == 'monitor') {
                                                $('#grids_div').show();
                                            } else if (settingPanel.type == 'playback') {
                                                $('#replay-container').show();
                                            }
                                        }
                                    }
                                },
                                close: function () {
                                    $(this).dialog("destroy");
                                    $(this).remove();
                                    settingPanel.setting.loaded = false;
                                    if (settingPanel.type == 'monitor') {
                                        $('#grids_div').show();
                                    } else if (settingPanel.type == 'playback') {
                                        $('#replay-container').show();
                                    }
                                }
                            });
                            $('#settingDialog').dialog('open');
                            if (settingPanel.type == 'monitor') {
                                $('#settingDialog>.tab-container>.tabs>dd:eq(1) a:eq(0)').trigger('click')
                            } else if (settingPanel.type == 'playback') {
                                $('#settingDialog>.tab-container>.tabs>dd:eq(1) a:eq(1)').trigger('click')
                            }
                        }
                    });
                }
            },
            setLocalConfig2Ocx: function () {
                // TODO:设置到已经初始化的ocx中
            },
            save: function () {
                settingPanel.setting.savePreviewSetting();
                settingPanel.setting.savePreviewSnapParam();
                settingPanel.setting.savePlaybackSetting();
                settingPanel.setting.savePlaybackDownload();
                settingPanel.setting.savePlaybackVideoclibs();
                settingPanel.setting.savePerformancePlan();
                return true;
            },
            reset: function () {
                var path = "C:", dpath = null;
                if (typeof getDefaultOcx === 'undefined') {
                    try {
                        var fso = new ActiveXObject("WScript.shell");
                        var pn = fso.Environment("PROCESS");
                        path = pn("WINDIR").toString().substring(0, 2);
                    } catch (e) {
                    }
                    dpath = path + "\\Program Data";
                } else {
                    // path = getDefaultOcx().GetSystemDrive();
                    dpath = getDefaultOcx().GetUserDocument();
                    //适配获取客户端安装路径，如果最后有斜杠，则删除之
                    if (dpath[dpath.length - 1] == '\\') {
                        dpath = dpath.slice(0, dpath.length - 1);
                    }
                }
                $.commonCookie("ocx-setting-preview-init", "");
                $.commonCookie("ocx-setting-playback-init", "");
                $.commonCookie("ocx-setting-download-path", "");
                $.commonCookie("ocx-setting-down-save-count", 10);
                $.commonCookie("ocx-setting-down-single-speed", 128);
                $.commonCookie("ocx-setting-down-run-count", 5);

                $.commonCookie("ocx-setting-playback-btn", '');
                $.commonCookie("ocx-setting-playback-bar-isshow", 2);
                $.commonCookie("ocx-setting-replay-decoding", 9);

                $.commonCookie("ocx-setting-btn", '');
                $.commonCookie("ocx-setting-bar-isshow", 0);
                $.commonCookie("ocx-setting-preview-video-path", dpath + "\\record\\");
                $.commonCookie("ocx-setting-limit-time", 0);
                $.commonCookie("ocx-setting-video-file", "");
                $.commonCookie("ocx-setting-replay-time", 30);
                $.commonCookie("ocx-setting-replay-type", 0);

                $.commonCookie("ocx-setting-capture-time", "captureframe");
                $.commonCookie("ocx-setting-capture-count", 3);
                $.commonCookie("ocx-setting-capture-file", "");
                $.commonCookie("ocx-setting-capture-format", "JPG");

                $.commonCookie("ocx-setting-capture-path", dpath + "\\capture\\");
                $.commonCookie("ocx-setting-split-package", "packagetime,1");

                $.commonCookie("ocx-setting-videoclibs-time", "videoclibsframe");
                $.commonCookie("ocx-setting-videoclibs-size", 256);
                $.commonCookie("ocx-setting-videoclibs-filename", "");
                $.commonCookie("ocx-setting-videoclibs-path", dpath + "\\clip\\");
                $.commonCookie("ocx-setting-videoclibs-time", "performanceplanframe");

                $.commonCookie("ocx-setting-performanceplan-type", 1);
                $.commonCookie("ocx-setting-performanceplan-to-subStream-num", 9);
                $.commonCookie("ocx-setting-performanceplan-autochg", 1);
                $.commonCookie("ocx-setting-performanceplan-decodeper", 5);
                $.commonCookie("ocx-setting-performanceplan-playPer", 0);

                $.commonCookie("ocx-setting-preview-isautoupload", 1);
                $.commonCookie("ocx-setting-video-isautoupload", 1);
                $.commonCookie("ocx-setting-capture-isautoupload", 1);
                return true;
            },
            savePlaybackDownload: function () {

                // 保存下载录像存放位置
                var downloadSavePathItem = $("#downloadSavePath");
                if (downloadSavePathItem && downloadSavePathItem.length > 0) {
                    var downloadSavePath = "";
                    if ($("#downloadSavePath").val()) {
                        downloadSavePath = $("#downloadSavePath").val();
                    }
                    $.commonCookie("ocx-setting-download-path", downloadSavePath);
                }

                var spliteSize = $("#downLoadSplitSize");
                if (spliteSize && spliteSize.length > 0) {
                    $.commonCookie("ocx-setting-down-split-size", spliteSize.val());
                }

                var downSaveCount = $("#downSaveCount");
                if (downSaveCount && downSaveCount.length > 0) {
                    $.commonCookie("ocx-setting-down-save-count", downSaveCount.val());
                }

                var downSingleCount = $("#downSingleCount");
                if (downSingleCount && downSingleCount.length > 0) {
                    $.commonCookie("ocx-setting-down-single-speed", downSingleCount.val());
                }

                var downRunCount = $("#downRunCount");
                if (downRunCount && downRunCount.length > 0) {
                    $.commonCookie("ocx-setting-down-run-count", downRunCount.val());
                }

                var downFileName = $('#downFileName');
                if (downFileName && downFileName.length > 0) {
                    $.commonCookie("ocx-setting-down-file", downFileName.val());
                }
            },
            savePlaybackSetting: function () {
                // 保存工具栏
                var bar = $("#ocxReplaySortable");
                if (bar && bar.length > 0) {
                    $.commonCookie("ocx-setting-playback-init", 1);
                    var barItems = $("#ocxReplaySortable li");
                    if (barItems.length > 0) {
                        var items = [];
                        for (var i = 0; i < barItems.length; i++) {
                            items.push($(barItems[i]).attr("data-index"));
                        }
                        $.commonCookie("ocx-setting-playback-btn", items.join(','));
                    } else {
                        $.commonCookie("ocx-setting-playback-btn", '');
                    }
                }

                // 保存是否显示工具栏
                var isShowItem = $('#isPlaybackBarShow');
                if (isShowItem && isShowItem.length > 0) {
                    var isShow = 2;
                    if (isShowItem.attr('checked')) {
                        isShow = 1;
                    }
                    $.commonCookie("ocx-setting-playback-bar-isshow", isShow);
                }

                $('input[name="replay.mode"]').each(function () {
                    if ($(this).attr('checked')) {
                        $.commonCookie("ocx-setting-replay-mode", $(this).attr('data-value'));
                    }
                });

                // 保存解码性能
                $('input[name="decoding.performance"]').each(function () {
                    if ($(this).attr('checked')) {
                        $.commonCookie("ocx-setting-replay-decoding", $(this).attr('data-value'));
                    }
                });
            },
            savePreviewSetting: function () {
                // 保存工具栏
                var bar = $("#ocxSortable");
                if (bar && bar.length > 0) {
                    $.commonCookie("ocx-setting-preview-init", 1);
                    var barItems = $("#ocxSortable li");
                    if (barItems.length > 0) {
                        var items = [];
                        for (var i = 0; i < barItems.length; i++) {
                            items.push($(barItems[i]).attr("data-index"));
                        }
                        $.commonCookie("ocx-setting-btn", items.join(','));
                    } else {
                        $.commonCookie("ocx-setting-btn", '');
                    }
                }

                // 保存是否显示工具栏
                var isShowItem = $('#isBarShow');
                if (isShowItem && isShowItem.length > 0) {
                    var isShow = 0;
                    if (isShowItem.attr('checked')) {
                        isShow = 1;
                    }
                    $.commonCookie("ocx-setting-bar-isshow", isShow);
                }
                // 保存即时回放时间
                $('input[name="replay.time"]').each(function () {
                    if ($(this).attr('checked')) {
                        $.commonCookie("ocx-setting-replay-time", $(this).attr('data-value'));
                    }
                });

                // 保存播放模式
                $('input[name="replay.type"]').each(function () {
                    if ($(this).attr('checked')) {
                        //						var oldReplayType = $.commonCookie("ocx-setting-replay-type")||0;
                        //						if(oldReplayType!=$(this).attr('data-value')){
                        //		    				jAlert('更改启动时默认播放模式后，需要重新刷新页面才生效','提示','attention');
                        //						}
                        $.commonCookie("ocx-setting-replay-type", $(this).attr('data-value'));
                    }
                });

                // 保存预览时紧急录像存放位置
                var previewVideoPathItem = $("#previewVideoSavePath");
                if (previewVideoPathItem && previewVideoPathItem.length > 0) {
                    var previewVideoPath = "";
                    if ($("#previewVideoSavePath").val()) {
                        previewVideoPath = $("#previewVideoSavePath").val();
                    }
                    $.commonCookie("ocx-setting-preview-video-path", previewVideoPath);
                }

                $('input[name="package.type"]').each(function () {
                    if ($(this).attr('checked')) {
                        var value = $(this).attr('value');
                        var subValue = $('#' + value + 'Select').val();
                        $.commonCookie("ocx-setting-split-package", value + "," + subValue);
                    }
                });
                var limitTimeBox = $('input[toggle-checkbox="video.limit.time"]');
                if (limitTimeBox && limitTimeBox.length > 0) {
                    if (limitTimeBox.attr('checked')) {
                        var value = limitTimeBox.attr('value');
                        var limitTime = $("#" + value + "Select").val();
                        $.commonCookie("ocx-setting-limit-time", limitTime);
                    } else {
                        $.commonCookie("ocx-setting-limit-time", 0);
                    }
                }

                var videoFileName = $('#videoFileName');
                if (videoFileName && videoFileName.length > 0) {
                    $.commonCookie("ocx-setting-video-file", videoFileName.attr("data-value"));
                }
                $.commonCookie("ocx-setting-preview-isautoupload", $('#chkpreview_isAutoUpload:checked').length);
            },
            savePreviewSnapParam: function () {
                $('input[name="capture.type"]').each(function () {
                    if ($(this).attr('checked')) {
                        var value = $(this).attr('value');
                        var cookieValue = "";
                        if ($('#' + value + 'Select').length > 0) {
                            var subValue = $('#' + value + 'Select').val();
                            cookieValue = value + "," + subValue;
                        } else {
                            cookieValue = value
                        }
                        $.commonCookie("ocx-setting-capture-time", cookieValue);
                    }
                });

                var captureCountItem = $("#captureCount");
                if (captureCountItem && captureCountItem.length > 0) {
                    $.commonCookie("ocx-setting-capture-count", captureCountItem.val());
                }

                var captureFileName = $('#captureFileName');
                if (captureFileName && captureFileName.length > 0) {
                    $.commonCookie("ocx-setting-capture-file", captureFileName.attr("data-value"));
                }

                $('input[name="capture.format"]').each(function () {
                    if ($(this).attr('checked')) {
                        $.commonCookie("ocx-setting-capture-format", $(this).attr('data-value'));
                    }
                });

                // 保存抓图存放位置
                var captureSavePathItem = $("#captureSavePath");
                if (captureSavePathItem && captureSavePathItem.length > 0) {
                    var captureSavePath = "";
                    if ($("#captureSavePath").val()) {
                        captureSavePath = $("#captureSavePath").val();
                    }
                    $.commonCookie("ocx-setting-capture-path", captureSavePath);
                }
                $.commonCookie("ocx-setting-capture-isautoupload", $('#chkcapture_isAutoUpload:checked').length);
            },
            savePlaybackVideoclibs: function () {
                // 分包大小
                var videoClibsItem = $("#videoClibsSize");
                if (videoClibsItem && videoClibsItem.length > 0) {
                    $.commonCookie("ocx-setting-videoclibs-size", videoClibsItem.val());
                }

                // 录像剪辑名称
                var videoClibsFileName = $('#videoClibsFileName');
                if (videoClibsFileName && videoClibsFileName.length > 0) {
                    $.commonCookie("ocx-setting-videoclibs-filename", videoClibsFileName.attr("data-value"));
                }
                // 保存剪辑录像存放位置
                var videoClibsSavePathItem = $("#videoClibsSavePath");
                if (videoClibsSavePathItem && videoClibsSavePathItem.length > 0) {
                    var videoClibsSavePath = "";
                    if ($("#videoClibsSavePath").val()) {
                        videoClibsSavePath = $("#videoClibsSavePath").val();
                    }
                    $.commonCookie("ocx-setting-videoclibs-path", videoClibsSavePath);
                }
                $.commonCookie("ocx-setting-video-isautoupload", $('#chkvideo_isAutoUpload:checked').length);
            },
            savePerformancePlan: function () {
                if ($('#btn-select-plan li[value=3]').get(0)) {
                    var type = $('#btn-select-plan li[value=3]').get(0).className;
                    if (type == "active") {
                        $.commonCookie("ocx-setting-performanceplan-type", 3);
                    } else if ($('#btn-select-plan li[value=2]').get(0).className == "active") {
                        $.commonCookie("ocx-setting-performanceplan-type", 2);
                    } else if ($('#btn-select-plan li[value=1]').get(0).className == "active") {
                        $.commonCookie("ocx-setting-performanceplan-type", 1);
                    } else if ($('#btn-select-plan li[value=0]').get(0).className == "active") {
                        $.commonCookie("ocx-setting-performanceplan-type", 0);
                    }
                }
                // 是否启用大于N路自动切换到主码流功能
                if ($('#toSubStreamEnable').length > 0) {
                    var toSubStream = $('#toSubStreamEnable').attr('checked');
                    if (!toSubStream) {
                        $.commonCookie("ocx-setting-performanceplan-to-subStream-num", -1);
                    } else {
                        var toSubStreamNum = $('#winNum').val();
                        if (toSubStreamNum == null || toSubStreamNum == '') {
                            $.commonCookie("ocx-setting-performanceplan-to-subStream-num", 9);
                        } else {
                            if (parseInt(toSubStreamNum) < 1) {
                                $.commonCookie("ocx-setting-performanceplan-to-subStream-num", 1);
                            } else if (parseInt(toSubStreamNum) > 25) {
                                $.commonCookie("ocx-setting-performanceplan-to-subStream-num", 25);
                            } else
                                $.commonCookie("ocx-setting-performanceplan-to-subStream-num", parseInt(toSubStreamNum));
                        }
                    }
                }

                // 单窗口切换
                if ($('#streamChange').length > 0) {
                    var autoChg = $('#streamChange').attr('checked');
                    if (!autoChg) {
                        $.commonCookie("ocx-setting-performanceplan-autochg", 0)
                    } else {
                        $.commonCookie("ocx-setting-performanceplan-autochg", 1)
                    }
                }

                // 解码性能
                if ($('#slideValue').get(0)) {
                    var decodePer = $('#slideValue').get(0).innerText; // $('#slide')
                    if (decodePer) {
                        $.commonCookie("ocx-setting-performanceplan-decodeper", decodePer)
                    } else {
                        $.commonCookie("ocx-setting-performanceplan-decodeper", 5)
                    }
                }

                var playPer = $('#playSelect').attr('value');
                if (playPer) {
                    $.commonCookie("ocx-setting-performanceplan-playPer", playPer)
                }
            }
        }
    }
    // var saveUploadFile = settingPanel.saveUploadFile;
    // settingPanel.saveUploadFile = function(file, indexCode, type, tauto,
    // lWndId) {
    // try {
    // return saveUploadFile.apply(this,arguments);
    // } catch (e) {
    // jAlert('视频上传失败');
    // }
    // }
})(jQuery);