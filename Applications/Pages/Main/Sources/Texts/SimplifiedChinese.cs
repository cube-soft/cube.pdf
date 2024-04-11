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
namespace Cube.Pdf.Pages;

/* ------------------------------------------------------------------------- */
///
/// SimplifiedChineseText
///
/// <summary>
/// Represents the Simplified Chinese texts used by CubePDF Page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class SimplifiedChineseText() : Globalization.TextGroup(new()
{
    // Menus
    { nameof(Text.Menu_Ok), "确定" },
    { nameof(Text.Menu_Cancel), "取消" },
    { nameof(Text.Menu_Exit), "退出" },
    { nameof(Text.Menu_Setting), "设置" },
    { nameof(Text.Menu_Metadata), "元数据" },
    { nameof(Text.Menu_Merge), "合并" },
    { nameof(Text.Menu_Split), "分割" },
    { nameof(Text.Menu_Add), "添加" },
    { nameof(Text.Menu_Up), "向上" },
    { nameof(Text.Menu_Down), "向下" },
    { nameof(Text.Menu_Remove), "移除" },
    { nameof(Text.Menu_Clear), "清除" },
    { nameof(Text.Menu_Preview), "预览" },

    // Columns for Main window
    { nameof(Text.Column_Filename), "文件名" },
    { nameof(Text.Column_Filetype), "类型" },
    { nameof(Text.Column_Filesize), "文件大小" },
    { nameof(Text.Column_Pages), "页面" },
    { nameof(Text.Column_Date), "最近更新时间" },

    // Labels for Setting window
    { nameof(Text.Setting_Window), "CubePDF Page 设置" },
    { nameof(Text.Setting_Tab), "设置" },
    { nameof(Text.Setting_Version), "版本" },
    { nameof(Text.Setting_Options), "选项" },
    { nameof(Text.Setting_Temp), "临时" },
    { nameof(Text.Setting_Language), "语言" },
    { nameof(Text.Setting_Others), "其它" },
    { nameof(Text.Setting_Shrink), "减少重复资源" },
    { nameof(Text.Setting_KeepOutline), "保留源 PDF 文件的书签" },
    { nameof(Text.Setting_CheckUpdate), "启动时检查更新" },

    // Labels for Metadata window
    { nameof(Text.Metadata_Window), "PDF 元数据" },
    { nameof(Text.Metadata_Tab), "摘要" },
    { nameof(Text.Metadata_Title), "标题" },
    { nameof(Text.Metadata_Author), "作者" },
    { nameof(Text.Metadata_Subject), "主题" },
    { nameof(Text.Metadata_Keyword), "关键字" },
    { nameof(Text.Metadata_Creator), "创作者" },
    { nameof(Text.Metadata_Version), "版本" },
    { nameof(Text.Metadata_Layout), "布局" },

    // Menus for Metadata window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Metadata_SinglePage), "单页" },
    { nameof(Text.Metadata_OneColumn), "一列" },
    { nameof(Text.Metadata_TwoPageLeft), "两页（左侧）" },
    { nameof(Text.Metadata_TwoPageRight), "两页（右侧）" },
    { nameof(Text.Metadata_TwoColumnLeft), "两列（左侧）" },
    { nameof(Text.Metadata_TwoColumnRight), "两列（右侧）" },

    // Labels for Security window
    { nameof(Text.Security_Tab), "安全" },
    { nameof(Text.Security_OwnerPassword), "密码" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "密码" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "确认" },
    { nameof(Text.Security_Operations), "操作" },

    // Menus for Security window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Security_Enable), "使用密码加密 PDF" },
    { nameof(Text.Security_OpenWithPassword), "使用密码打开" },
    { nameof(Text.Security_SharePassword), "使用所有者密码" },
    { nameof(Text.Security_AllowPrint), "允许打印" },
    { nameof(Text.Security_AllowCopy), "允许复制文本和图像" },
    { nameof(Text.Security_AllowModify), "允许插入和删除页面" },
    { nameof(Text.Security_AllowAccessibility), "允许使用内容进行辅助访问" },
    { nameof(Text.Security_AllowForm), "允许填写表格" },
    { nameof(Text.Security_AllowAnnotation), "允许创建和编辑注释" },

    // Labels for Password window
    { nameof(Text.Password_Window), "输入所有者密码" },
    { nameof(Text.Password_Show), "显示密码" },

    // Titles for other dialogs
    { nameof(Text.Window_Add), "选择来源文件" },
    { nameof(Text.Window_Merge), "合并文件另存为" },
    { nameof(Text.Window_Split), "选择一个文件夹来保存分割文件。" },
    { nameof(Text.Window_Temp), "选择临时文件夹。 如果不指定临时文件夹，将使用与源文件相同的文件夹。" },

    // Error messages
    { nameof(Text.Error_OwnerPassword), "所有者密码为空或与确认信息不匹配。 请再次检查您的密码并确认" },
    { nameof(Text.Error_UserPassword), "用户密码为空或与确认信息不符。 请检查用户密码或启用使用所有者密码复选框。" },

    // Warning messages
    { nameof(Text.Warn_Password), "{0} 受到保护。 输入所有者密码进行编辑。" },

    // File filters
    { nameof(Text.Filter_All), "所有文件" },
    { nameof(Text.Filter_Support), "所有支持的文件" },
    { nameof(Text.Filter_Pdf), "PDF 文件" },
});
