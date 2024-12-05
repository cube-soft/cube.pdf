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
/// RussianText
///
/// <summary>
/// Represents the Russian texts used by CubePDF Page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class RussianText() : Globalization.TextGroup(new()
{
    // Menus
    { nameof(Text.Menu_Ok), "ОК" },
    { nameof(Text.Menu_Cancel), "Отмена" },
    { nameof(Text.Menu_Exit), "Выход" },
    { nameof(Text.Menu_Setting), "Настройки" },
    { nameof(Text.Menu_Metadata), "Метаданные" },
    { nameof(Text.Menu_Merge), "Объединить" },
    { nameof(Text.Menu_Split), "Разделить" },
    { nameof(Text.Menu_Add), "Добавить" },
    { nameof(Text.Menu_Up), "Вверх" },
    { nameof(Text.Menu_Down), "Вниз" },
    { nameof(Text.Menu_Remove), "Удалить" },
    { nameof(Text.Menu_Clear), "Очистить" },
    { nameof(Text.Menu_Preview), "Предпросмотр" },

    // Columns for Main window
    { nameof(Text.Column_Filename), "Имя файла" },
    { nameof(Text.Column_Filetype), "Тип" },
    { nameof(Text.Column_Filesize), "Размер" },
    { nameof(Text.Column_Pages), "Количество страниц" },
    { nameof(Text.Column_Date), "Дата изменения" },

    // Labels for Setting window
    { nameof(Text.Setting_Window), "Настройки программы CubePDF Page" },
    { nameof(Text.Setting_Tab), "Настройки" },
    { nameof(Text.Setting_Version), "Версия" },
    { nameof(Text.Setting_Options), "При сохранении" },
    { nameof(Text.Setting_Temp), "Временные файлы" },
    { nameof(Text.Setting_Language), "Язык приложения" },
    { nameof(Text.Setting_Others), "Прочие настройки" },
    { nameof(Text.Setting_Shrink), "Уменьшать дублирование ресурсов" },
    { nameof(Text.Setting_KeepOutline), "Сохранять закладки исходного PDF документа" },
    { nameof(Text.Setting_AutoSort), "Сортировать выбранные файлы автоматически" },
    { nameof(Text.Setting_CheckUpdate), "Проверять обновления при запуске программы" },

    // Labels for Metadata window
    { nameof(Text.Metadata_Window), "Метаданные PDF документа" },
    { nameof(Text.Metadata_Tab), "Описание" },
    { nameof(Text.Metadata_Title), "Заголовок" },
    { nameof(Text.Metadata_Author), "Автор" },
    { nameof(Text.Metadata_Subject), "Тема" },
    { nameof(Text.Metadata_Keyword), "Ключевые слова" },
    { nameof(Text.Metadata_Creator), "Приложение" },
    { nameof(Text.Metadata_Version), "Версия PDF" },
    { nameof(Text.Metadata_Layout), "Режим просмотра" },

    // Menus for Metadata window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Metadata_SinglePage), "Одна страница" },
    { nameof(Text.Metadata_OneColumn), "Один столбец" },
    { nameof(Text.Metadata_TwoPageLeft), "Две страницы (слева)" },
    { nameof(Text.Metadata_TwoPageRight), "Две страницы (справа)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Два столбца (слева)" },
    { nameof(Text.Metadata_TwoColumnRight), "Два столбца (справа)" },

    // Labels for Security window
    { nameof(Text.Security_Tab), "Защита" },
    { nameof(Text.Security_OwnerPassword), "Пароль" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "Пароль" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "Подтвердить" },
    { nameof(Text.Security_Operations), "Права доступа" },

    // Menus for Security window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Security_Enable), "Зашифровать PDF документ с помощью пароля" },
    { nameof(Text.Security_OpenWithPassword), "Запрашивать пароль для открытия документа" },
    { nameof(Text.Security_SharePassword), "Использовать пароль владельца" },
    { nameof(Text.Security_AllowPrint), "Разрешить печать" },
    { nameof(Text.Security_AllowCopy), "Разрешить копирование текста и изображений" },
    { nameof(Text.Security_AllowModify), "Разрешить вставку и удаление страниц" },
    { nameof(Text.Security_AllowAccessibility), "Разрешить доступ к тексту системам для слабовидящих" },
    { nameof(Text.Security_AllowForm), "Разрешить заполнение форм" },
    { nameof(Text.Security_AllowAnnotation), "Разрешить создание и редактирование комментариев" },

    // Labels for Password window
    { nameof(Text.Password_Window), "Введите пароль владельца" },
    { nameof(Text.Password_Show), "Показать пароль" },

    // Titles for other dialogs
    { nameof(Text.Window_Add), "Выберите исходный файл" },
    { nameof(Text.Window_Merge), "Сохранить объединённый файл как" },
    { nameof(Text.Window_Split), "Выберите папку для сохранения разделённых файлов." },
    { nameof(Text.Window_Temp), "Выберите папку для хранения временных файлов. Если папка не будет указана, то временные файлы будут сохраняться в папку документа." },

    // Error messages
    { nameof(Text.Error_OwnerPassword), "Пароль владельца не указан или не совпадает с подтверждением. Проверьте поля ещё раз." },
    { nameof(Text.Error_UserPassword), "Пароль пользователя не указан или не совпадает с подтверждением. Проверьте поля ещё раз или активируйте параметр \"Использовать пароль владельца\"." },

    // Warning messages
    { nameof(Text.Warn_Password), "Введите, пожалуйста, пароль владельца для открытия и редактирования PDF документа: {0}." },

    // File filters
    { nameof(Text.Filter_All), "Все файлы" },
    { nameof(Text.Filter_Support), "Все поддерживаемые" },
    { nameof(Text.Filter_Pdf), "Файлы PDF" },
});
