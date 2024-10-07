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
namespace Cube.Pdf.Converter;

/* ------------------------------------------------------------------------- */
///
/// SimplifiedChineseText
///
/// <summary>
/// Represents the Simplified Chinese texts used by CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class SimplifiedChineseText() : Globalization.TextGroup(new()
{
    // Labels for General tab
    { nameof(Text.General_Tab), "常规" },
    { nameof(Text.General_Source), "来源" },
    { nameof(Text.General_Destination), "目标" },
    { nameof(Text.General_Format), "格式" },
    { nameof(Text.General_Color), "颜色" },
    { nameof(Text.General_Resolution), "分辨率" },
    { nameof(Text.General_Orientation), "方向" },
    { nameof(Text.General_Options), "选项" },
    { nameof(Text.General_PostProcess), "后期处理" },

    // Menus for General tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.General_Overwrite), "覆盖" },
    { nameof(Text.General_MergeHead), "在开头合并" },
    { nameof(Text.General_MergeTail), "在结尾合并" },
    { nameof(Text.General_Rename), "重命名" },
    { nameof(Text.General_Auto), "自动" },
    { nameof(Text.General_Rgb), "RGB" },
    { nameof(Text.General_Grayscale), "灰度" },
    { nameof(Text.General_Monochrome), "单色" },
    { nameof(Text.General_Portrait), "肖像" },
    { nameof(Text.General_Landscape), "风景" },
    { nameof(Text.General_Jpeg), "压缩 PDF 图像为 JPEG" },
    { nameof(Text.General_Open), "打开" },
    { nameof(Text.General_OpenDirectory), "打开目录" },
    { nameof(Text.General_None), "无" },
    { nameof(Text.General_UserProgram), "其它" },

    // Labels for Metadata tab
    { nameof(Text.Metadata_Tab), "元数据" },
    { nameof(Text.Metadata_Title), "标题" },
    { nameof(Text.Metadata_Author), "作者" },
    { nameof(Text.Metadata_Subject), "主题" },
    { nameof(Text.Metadata_Keyword), "关键词" },
    { nameof(Text.Metadata_Creator), "创作者" },
    { nameof(Text.Metadata_Layout), "布局" },

    // Menus for Metadata tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Metadata_SinglePage), "单页" },
    { nameof(Text.Metadata_OneColumn), "一列" },
    { nameof(Text.Metadata_TwoPageLeft), "两页（左侧）" },
    { nameof(Text.Metadata_TwoPageRight), "两页（右侧）" },
    { nameof(Text.Metadata_TwoColumnLeft), "两列（左侧）" },
    { nameof(Text.Metadata_TwoColumnRight), "两列（右侧）" },

    // Labels for Security tab
    { nameof(Text.Security_Tab), "安全" },
    { nameof(Text.Security_OwnerPassword), "密码" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "密码" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "确认" },
    { nameof(Text.Security_Operations), "操作" },

    // Menus for Security tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Security_Enable), "使用密码加密 PDF" },
    { nameof(Text.Security_OpenWithPassword), "使用密码打开" },
    { nameof(Text.Security_SharePassword), "使用所有者密码" },
    { nameof(Text.Security_AllowPrint), "允许打印" },
    { nameof(Text.Security_AllowCopy), "允许复制文本和图像" },
    { nameof(Text.Security_AllowModify), "允许插入和删除页面" },
    { nameof(Text.Security_AllowAccessibility), "允许使用内容进行辅助访问" },
    { nameof(Text.Security_AllowForm), "允许填写表格" },
    { nameof(Text.Security_AllowAnnotation), "允许创建和编辑注释" },

    // Labels for Misc tab
    { nameof(Text.Misc_Tab), "其它" },
    { nameof(Text.Misc_About), "关于" },
    { nameof(Text.Misc_Language), "语言" },

    // Menus for Misc tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Misc_CheckUpdate), "启动时检查更新" },

    // Buttons
    { nameof(Text.Menu_Convert), "转换" },
    { nameof(Text.Menu_Cancel), "取消" },
    { nameof(Text.Menu_Save), "保存设置" },

    // Titles for dialogs
    { nameof(Text.Window_Source), "选择来源文件" },
    { nameof(Text.Window_Destination), "另存为" },
    { nameof(Text.Window_UserProgram), "选择用户程序" },

    // Error messages
    { nameof(Text.Error_Source), "未指定输入文件。 请检查 CubePDF 是否按照正常程序执行。" },
    { nameof(Text.Error_Digest), "源文件的消息摘要不匹配。" },
    { nameof(Text.Error_Ghostscript), "Ghostscript 错误 ({0:D})" },
    { nameof(Text.Error_InvalidChars), "路径不能包含以下任何字符。" },
    { nameof(Text.Error_OwnerPassword), "所有者密码为空或与确认信息不匹配。 请再次检查您的密码并确认。" },
    { nameof(Text.Error_UserPassword), "用户密码为空或与确认信息不符。 请检查用户密码或启用“使用所有者密码”复选框。" },
    { nameof(Text.Error_MergePassword), "在“安全”选项卡中设置要合并的 PDF 文件的所有者密码。" },
    { nameof(Text.Error_PostProcess), "虽然转换已完成，但后处理失败。 检查文件关联设置或用户程序是否正确。" },

    // Warning/Confirm messages
    { nameof(Text.Warn_Exist), "{0} 已存在。" },
    { nameof(Text.Warn_Overwrite), "您想覆盖该文件吗？" },
    { nameof(Text.Warn_MergeHead), "您想在现有文件的开头合并吗？" },
    { nameof(Text.Warn_MergeTail), "您想在现有文件的末尾合并吗？" },
    { nameof(Text.Warn_Metadata), "在标题、作者、主题或关键字字段中输入一些值。 您想保存设置吗？" },

    // File filters
    { nameof(Text.Filter_All), "所有文件" },
    { nameof(Text.Filter_Pdf), "PDF 文件" },
    { nameof(Text.Filter_Ps), "PS 文件" },
    { nameof(Text.Filter_Eps), "EPS 文件" },
    { nameof(Text.Filter_Bmp), "BMP 文件" },
    { nameof(Text.Filter_Png), "PNG 文件" },
    { nameof(Text.Filter_Jpeg), "JPEG 文件" },
    { nameof(Text.Filter_Tiff), "TIFF 文件" },
    { nameof(Text.Filter_Exe), "可执行文件" },
});