/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Editor;

/* ------------------------------------------------------------------------- */
///
/// SimplifiedChineseText
///
/// <summary>
/// Represents the Simplified Chinese texts used by CubePDF Utility.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class SimplifiedChineseText() : Globalization.TextGroup(new()
{
    // Menus. Note that Menu_*_Long values are used for tooltips.
    { nameof(Text.Menu_Ok), "确定" },
    { nameof(Text.Menu_Cancel), "取消" },
    { nameof(Text.Menu_Exit), "退出" },
    { nameof(Text.Menu_File), "文件" },
    { nameof(Text.Menu_Edit), "编辑" },
    { nameof(Text.Menu_Misc), "其它" },
    { nameof(Text.Menu_Help), "帮助" },
    { nameof(Text.Menu_Setting), "设置" },
    { nameof(Text.Menu_Preview), "预览" },
    { nameof(Text.Menu_Metadata), "元数据" },
    { nameof(Text.Menu_Metadata_Long), "PDF 文档元数据" },
    { nameof(Text.Menu_Security), "安全" },
    { nameof(Text.Menu_Open), "打开" },
    { nameof(Text.Menu_Close), "关闭" },
    { nameof(Text.Menu_Save), "保存" },
    { nameof(Text.Menu_Save_Long), "保存" },
    { nameof(Text.Menu_Save_As), "另存为" },
    { nameof(Text.Menu_Redraw), "刷新" },
    { nameof(Text.Menu_Undo), "撤销" },
    { nameof(Text.Menu_Redo), "重做" },
    { nameof(Text.Menu_Select), "选择" },
    { nameof(Text.Menu_Select_All), "全选" },
    { nameof(Text.Menu_Select_Flip), "反选" },
    { nameof(Text.Menu_Select_Clear), "取消全选" },
    { nameof(Text.Menu_Insert), "插入" },
    { nameof(Text.Menu_Insert_Long), "插入到所选位置后面" },
    { nameof(Text.Menu_Insert_Head), "插入到开头" },
    { nameof(Text.Menu_Insert_Tail), "插入到末尾" },
    { nameof(Text.Menu_Insert_Custom), "插入其他位置" },
    { nameof(Text.Menu_Extract), "提取" },
    { nameof(Text.Menu_Extract_Long), "提取所选页面" },
    { nameof(Text.Menu_Extract_Custom), "使用其他设置提取" },
    { nameof(Text.Menu_Remove), "移除" },
    { nameof(Text.Menu_Remove_Long), "移除所选页面" },
    { nameof(Text.Menu_Remove_Custom), "移除其它页面" },
    { nameof(Text.Menu_Move_Back), "返回" },
    { nameof(Text.Menu_Move_Forth), "向前" },
    { nameof(Text.Menu_Rotate_Left), "左侧" },
    { nameof(Text.Menu_Rotate_Right), "右侧" },
    { nameof(Text.Menu_Zoom_In), "放大" },
    { nameof(Text.Menu_Zoom_Out), "缩小" },
    { nameof(Text.Menu_Frame), "仅边框" },
    { nameof(Text.Menu_Recent), "最近的文件" },

    // Setting window
    { nameof(Text.Setting_Window), "CubePDF Utility 设置" },
    { nameof(Text.Setting_Tab), "设置" },
    { nameof(Text.Setting_Version), "版本" },
    { nameof(Text.Setting_Options), "保存选项" },
    { nameof(Text.Setting_Backup), "备份" },
    { nameof(Text.Setting_Backup_Enable), "启用备份" },
    { nameof(Text.Setting_Backup_Clean), "自动删除旧的备份文件" },
    { nameof(Text.Setting_Temp), "临时" },
    { nameof(Text.Setting_Language), "语言" },
    { nameof(Text.Setting_Others), "其它" },
    { nameof(Text.Setting_Shrink), "减少重复资源" },
    { nameof(Text.Setting_KeepOutline), "保留源 PDF 文件的书签" },
    { nameof(Text.Setting_Recent), "显示最近使用的文件" },
    { nameof(Text.Setting_CheckUpdate), "启动时检查更新" },

    // Metadata window
    { nameof(Text.Metadata_Window), "PDF 元数据" },
    { nameof(Text.Metadata_Summary), "摘要" },
    { nameof(Text.Metadata_Detail), "详细信息" },
    { nameof(Text.Metadata_Title), "标题" },
    { nameof(Text.Metadata_Author), "作者" },
    { nameof(Text.Metadata_Subject), "主题" },
    { nameof(Text.Metadata_Keyword), "关键字" },
    { nameof(Text.Metadata_Version), "版本" },
    { nameof(Text.Metadata_Layout), "布局" },
    { nameof(Text.Metadata_Creator), "创作者" },
    { nameof(Text.Metadata_Producer), "制作者" },
    { nameof(Text.Metadata_Filename), "文件名" },
    { nameof(Text.Metadata_Filesize), "文件大小" },
    { nameof(Text.Metadata_CreationTime), "创建" },
    { nameof(Text.Metadata_LastWriteTime), "最后更新" },
    { nameof(Text.Metadata_SinglePage), "单页" },
    { nameof(Text.Metadata_OneColumn), "一列" },
    { nameof(Text.Metadata_TwoPageLeft), "两页 (左侧)" },
    { nameof(Text.Metadata_TwoPageRight), "两页 (右侧)" },
    { nameof(Text.Metadata_TwoColumnLeft), "两列 (左侧)" },
    { nameof(Text.Metadata_TwoColumnRight), "两列 (右侧)" },

    // Security window
    { nameof(Text.Security_Window), "安全" },
    { nameof(Text.Security_OwnerPassword), "密码" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "密码" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "确认" },
    { nameof(Text.Security_Method), "方法" },
    { nameof(Text.Security_Operations), "行动" },
    { nameof(Text.Security_Enable), "使用密码加密 PDF" },
    { nameof(Text.Security_OpenWithPassword), "使用密码打开" },
    { nameof(Text.Security_SharePassword), "使用所有者密码" },
    { nameof(Text.Security_AllowPrint), "允许打印" },
    { nameof(Text.Security_AllowCopy), "允许复制文本和图像" },
    { nameof(Text.Security_AllowModify), "允许插入和删除页面" },
    { nameof(Text.Security_AllowAccessibility), "允许使用内容进行辅助访问" },
    { nameof(Text.Security_AllowForm), "允许填写表格" },
    { nameof(Text.Security_AllowAnnotation), "允许创建和编辑注释" },

    // Insert window
    { nameof(Text.Insert_Window), "插入详细信息" },
    { nameof(Text.Insert_Menu_Add), "添加" },
    { nameof(Text.Insert_Menu_Up), "向上" },
    { nameof(Text.Insert_Menu_Down), "向下" },
    { nameof(Text.Insert_Menu_Remove), "移除" },
    { nameof(Text.Insert_Menu_Clear), "清除" },
    { nameof(Text.Insert_Menu_Preview), "预览" },
    { nameof(Text.Insert_Position), "插入位置" },
    { nameof(Text.Insert_Position_Select), "所选位置" },
    { nameof(Text.Insert_Position_Head), "开始" },
    { nameof(Text.Insert_Position_Tail), "结束" },
    { nameof(Text.Insert_Position_Custom), "后面的数字" },
    { nameof(Text.Insert_Column_Filename), "文件名" },
    { nameof(Text.Insert_Column_Filetype), "类型" },
    { nameof(Text.Insert_Column_Filesize), "文件大小" },
    { nameof(Text.Insert_Column_LastWriteTime), "最后更新" },

    // Extract window
    { nameof(Text.Extract_Window), "提取详细信息" },
    { nameof(Text.Extract_Destination), "保存路径" },
    { nameof(Text.Extract_Format), "格式" },
    { nameof(Text.Extract_Page), "页数" },
    { nameof(Text.Extract_Target), "目标页面" },
    { nameof(Text.Extract_Target_Select), "所选页面" },
    { nameof(Text.Extract_Target_All), "全部页面" },
    { nameof(Text.Extract_Target_Custom), "指定范围" },
    { nameof(Text.Extract_Options), "选项" },
    { nameof(Text.Extract_Split), "每页另存为一个单独的文件" },

    // Remove window
    { nameof(Text.Remove_Window), "移除详细信息" },
    { nameof(Text.Remove_Page), "页数" },
    { nameof(Text.Remove_Target), "目标页面" },

    // Password window

    // Titles for other dialogs
    { nameof(Text.Window_Open), "打开文件" },
    { nameof(Text.Window_Save), "另存为" },
    { nameof(Text.Window_Backup), "选择一个备份文件夹。" },
    { nameof(Text.Window_Temp), "选择临时文件夹。 如果不指定临时文件夹，将使用与源文件相同的文件夹。" },
    { nameof(Text.Window_Preview), "{0} ({1}/{2} 页)" },
    { nameof(Text.Window_Password), "输入所有者密码" },

    // Error messages
    { nameof(Text.Error_Open), "文件不是 PDF 格式或已损坏。" },
    { nameof(Text.Error_Metadata), "获取 PDF 元数据失败。" },
    { nameof(Text.Error_Range), "无法解析删除范围。" },

    // Warning messages
    { nameof(Text.Warn_Password), "请输入所有者密码以打开并编辑 {0}。" },
    { nameof(Text.Warn_Overwrite), "PDF 已修改。 您想覆盖吗？" },

    // Other messages
    { nameof(Text.Message_Loading), "正在加载 {0} ..." },
    { nameof(Text.Message_Saving), "正在另存为 {0} ..." },
    { nameof(Text.Message_Saved), "已保存为 {0}" },
    { nameof(Text.Message_Pages), "{0} 页" },
    { nameof(Text.Message_Total), "{0} 页" },
    { nameof(Text.Message_Selection), "已选择 {0} 页" },
    { nameof(Text.Message_Range), "例如：1,2,4-7,9" },
    { nameof(Text.Message_Byte), "字节" },
    { nameof(Text.Message_Dpi), "分辨率" },

    // File filters
    { nameof(Text.Filter_All), "所有文件" },
    { nameof(Text.Filter_Insertable), "所有可插入文件" },
    { nameof(Text.Filter_Extractable), "所有支持的文件" },
    { nameof(Text.Filter_Pdf), "PDF 文件" },
});
