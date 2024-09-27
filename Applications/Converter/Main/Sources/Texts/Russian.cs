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
/// RussianText
///
/// <summary>
/// Represents the Russian texts used by CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class RussianText() : Globalization.TextGroup(new()
{
    // Labels for General tab
    { nameof(Text.General_Tab), "Общие" },
    { nameof(Text.General_Source), "Источник" },
    { nameof(Text.General_Destination), "Путь сохранения" },
    { nameof(Text.General_Format), "Формат файла" },
    { nameof(Text.General_Color), "Цветной режим" },
    { nameof(Text.General_Resolution), "Разрешение" },
    { nameof(Text.General_Orientation), "Ориентация" },
    { nameof(Text.General_Options), "Дополнительно" },
    { nameof(Text.General_PostProcess), "После печати" },

    // Menus for General tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.General_Overwrite), "Перезаписать" },
    { nameof(Text.General_MergeHead), "Добавить страницы в начало документа" },
    { nameof(Text.General_MergeTail), "Добавить страницы в конец документа" },
    { nameof(Text.General_Rename), "Переименовать" },
    { nameof(Text.General_Auto), "Автоматический" },
    { nameof(Text.General_Rgb), "Цветной" },
    { nameof(Text.General_Grayscale), "Градации серого" },
    { nameof(Text.General_Monochrome), "Монохромный" },
    { nameof(Text.General_Portrait), "Книжная" },
    { nameof(Text.General_Landscape), "Альбомная" },
    { nameof(Text.General_Jpeg), "Сжимать изображения в PDF документе" },
    { nameof(Text.General_Linearization), "Оптимизировать PDF документ для веб-просмотра" },
    { nameof(Text.General_Open), "Открыть документ" },
    { nameof(Text.General_OpenDirectory), "Открыть папку" },
    { nameof(Text.General_None), "Ничего не делать" },
    { nameof(Text.General_UserProgram), "Запустить приложение" },

    // Labels for Metadata tab
    { nameof(Text.Metadata_Tab), "Метаданные" },
    { nameof(Text.Metadata_Title), "Заголовок" },
    { nameof(Text.Metadata_Author), "Автор" },
    { nameof(Text.Metadata_Subject), "Тема" },
    { nameof(Text.Metadata_Keyword), "Ключевые слова" },
    { nameof(Text.Metadata_Creator), "Приложение" },
    { nameof(Text.Metadata_Layout), "Режим просмотра" },

    // Menus for Metadata tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Metadata_SinglePage), "Одна страница" },
    { nameof(Text.Metadata_OneColumn), "Один столбец" },
    { nameof(Text.Metadata_TwoPageLeft), "Две страницы (слева)" },
    { nameof(Text.Metadata_TwoPageRight), "Две страницы (справа)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Два столбца (слева)" },
    { nameof(Text.Metadata_TwoColumnRight), "Два столбца (справа)" },

    // Labels for Security tab
    { nameof(Text.Security_Tab), "Защита" },
    { nameof(Text.Security_OwnerPassword), "Пароль" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "Пароль" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "Подтвердить" },
    { nameof(Text.Security_Operations), "Права доступа" },

    // Menus for Security tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Security_Enable), "Зашифровать PDF документ с помощью пароля" },
    { nameof(Text.Security_OpenWithPassword), "Запрашивать пароль для открытия документа" },
    { nameof(Text.Security_SharePassword), "Использовать пароль владельца" },
    { nameof(Text.Security_AllowPrint), "Разрешить печать" },
    { nameof(Text.Security_AllowCopy), "Разрешить копирование текста и изображений" },
    { nameof(Text.Security_AllowModify), "Разрешить вставку и удаление страниц" },
    { nameof(Text.Security_AllowAccessibility), "Разрешить доступ к тексту системам для слабовидящих" },
    { nameof(Text.Security_AllowForm), "Разрешить заполнение форм" },
    { nameof(Text.Security_AllowAnnotation), "Разрешить создание и редактирование комментариев" },

    // Labels for Misc tab
    { nameof(Text.Misc_Tab), "Прочее" },
    { nameof(Text.Misc_About), "О программе" },
    { nameof(Text.Misc_Language), "Язык приложения" },

    // Menus for Misc tab (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Misc_CheckUpdate), "Проверять обновления при запуске программы" },

    // Buttons
    { nameof(Text.Menu_Convert), "Печать" },
    { nameof(Text.Menu_Cancel), "Отмена" },
    { nameof(Text.Menu_Save), "Сохранить настройки" },

    // Titles for dialogs
    { nameof(Text.Window_Source), "Выберите исходный файл" },
    { nameof(Text.Window_Destination), "Сохранить файл как" },
    { nameof(Text.Window_UserProgram), "Выберите приложение" },

    // Error messages
    { nameof(Text.Error_Source), "Исходный файл не определён. Запустить CubePDF через печать файлов." },
    { nameof(Text.Error_Digest), "Хеш-сумма исходного файла не совпадает." },
    { nameof(Text.Error_Ghostscript), "Ошибка Ghostscript ({0:D})" },
    { nameof(Text.Error_InvalidChars), "Путь не может содержать следующие символы." },
    { nameof(Text.Error_OwnerPassword), "Пароль владельца не указан или не совпадает с подтверждением. Проверьте поля ещё раз." },
    { nameof(Text.Error_UserPassword), "Пароль пользователя не указан или не совпадает с подтверждением. Проверьте поля ещё раз или активируйте параметр \"Использовать пароль владельца\"." },
    { nameof(Text.Error_MergePassword), "Установка пароля владельца будет перенесена на вкладку \"Защита\"." },
    { nameof(Text.Error_PostProcess), "Не удалось выполнить действие после печати, однако печать прошла успешно. Проверьте ассоциации файлов или выбранное приложение." },

    // Warning/Confirm messages
    { nameof(Text.Warn_Exist), "Файл {0} уже существует." },
    { nameof(Text.Warn_Overwrite), "Желаете перезаписать файл?" },
    { nameof(Text.Warn_MergeHead), "Желаете добавить страницы в начало существующего документа?" },
    { nameof(Text.Warn_MergeTail), "Желаете добавить страницы в конец существующего документа?" },
    { nameof(Text.Warn_Metadata), "Заполнено одно из полей: Заголовок, Автор, Тема или Ключевые слова. Желаете сохранить настройки?" },

    // File filters
    { nameof(Text.Filter_All), "Все файлы" },
    { nameof(Text.Filter_Pdf), "Файлы PDF" },
    { nameof(Text.Filter_Ps), "Файлы PS" },
    { nameof(Text.Filter_Eps), "Файлы EPS" },
    { nameof(Text.Filter_Bmp), "BMP изображения" },
    { nameof(Text.Filter_Png), "PNG изображения" },
    { nameof(Text.Filter_Jpeg), "JPEG изображения" },
    { nameof(Text.Filter_Tiff), "TIFF изображения" },
    { nameof(Text.Filter_Exe), "Исполняемые файлы" },
});