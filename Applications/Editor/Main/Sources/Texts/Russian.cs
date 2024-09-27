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
/// RussianText
///
/// <summary>
/// Represents the Russian texts used by CubePDF Utility.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class RussianText() : Globalization.TextGroup(new()
{
    // Menus. Note that Menu_*_Long values are used for tooltips.
    { nameof(Text.Menu_Ok), "ОК" },
    { nameof(Text.Menu_Cancel), "Отмена" },
    { nameof(Text.Menu_Exit), "Выход" },
    { nameof(Text.Menu_File), "Файл" },
    { nameof(Text.Menu_Edit), "Редактирование" },
    { nameof(Text.Menu_Misc), "Прочее" },
    { nameof(Text.Menu_Help), "Справка" },
    { nameof(Text.Menu_Setting), "Настройки" },
    { nameof(Text.Menu_Preview), "Предпросмотр" },
    { nameof(Text.Menu_Metadata), "Метаданные" },
    { nameof(Text.Menu_Metadata_Long), "Метаданные PDF документа" },
    { nameof(Text.Menu_Security), "Защитить" },
    { nameof(Text.Menu_Open), "Открыть" },
    { nameof(Text.Menu_Close), "Закрыть" },
    { nameof(Text.Menu_Save), "Сохранить" },
    { nameof(Text.Menu_Save_Long), "Сохранить" },
    { nameof(Text.Menu_Save_As), "Сохранить как" },
    { nameof(Text.Menu_Redraw), "Обновить" },
    { nameof(Text.Menu_Undo), "Отменить" },
    { nameof(Text.Menu_Redo), "Повторить" },
    { nameof(Text.Menu_Select), "Выделить" },
    { nameof(Text.Menu_Select_All), "Выделить всё" },
    { nameof(Text.Menu_Select_Flip), "Инвертировать выделение" },
    { nameof(Text.Menu_Select_Clear), "Снять выделение" },
    { nameof(Text.Menu_Insert), "Вставить" },
    { nameof(Text.Menu_Insert_Long), "Вставить после выделенных страниц" },
    { nameof(Text.Menu_Insert_Head), "Вставить в начало документа" },
    { nameof(Text.Menu_Insert_Tail), "Вставить в конец документа" },
    { nameof(Text.Menu_Insert_Custom), "Вставить в другое место" },
    { nameof(Text.Menu_Extract), "Извлечь" },
    { nameof(Text.Menu_Extract_Long), "Извлечь выделенные страницы" },
    { nameof(Text.Menu_Extract_Custom), "Извлечь другие страницы" },
    { nameof(Text.Menu_Remove), "Удалить" },
    { nameof(Text.Menu_Remove_Long), "Удалить выделенные страницы" },
    { nameof(Text.Menu_Remove_Custom), "Удалить другие страницы" },
    { nameof(Text.Menu_Move_Back), "Назад" },
    { nameof(Text.Menu_Move_Forth), "Вперёд" },
    { nameof(Text.Menu_Rotate_Left), "Налево" },
    { nameof(Text.Menu_Rotate_Right), "Направо" },
    { nameof(Text.Menu_Zoom_In), "Увеличить" },
    { nameof(Text.Menu_Zoom_Out), "Уменьшить" },
    { nameof(Text.Menu_Frame), "Только рамка" },
    { nameof(Text.Menu_Recent), "Последние файлы" },

    // Setting window
    { nameof(Text.Setting_Window), "Настройки программы CubePDF Utility" },
    { nameof(Text.Setting_Tab), "Настройки" },
    { nameof(Text.Setting_Version), "Версия" },
    { nameof(Text.Setting_Options), "При сохранении" },
    { nameof(Text.Setting_Backup), "Резервирование" },
    { nameof(Text.Setting_Backup_Enable), "Включить резервное копирование" },
    { nameof(Text.Setting_Backup_Clean), "Удалять старые копии автоматически" },
    { nameof(Text.Setting_Temp), "Временные файлы" },
    { nameof(Text.Setting_Language), "Язык приложения" },
    { nameof(Text.Setting_Others), "Прочие настройки" },
    { nameof(Text.Setting_Shrink), "Уменьшать дублирование ресурсов" },
    { nameof(Text.Setting_KeepOutline), "Сохранять закладки исходного PDF документа" },
    { nameof(Text.Setting_Recent), "Отображать последние файлы" },
    { nameof(Text.Setting_AutoSort), "Сортировать выбранные файлы автоматически" },
    { nameof(Text.Setting_CheckUpdate), "Проверять обновления при запуске программы" },

    // Metadata window
    { nameof(Text.Metadata_Window), "Метаданные PDF документа" },
    { nameof(Text.Metadata_Summary), "Описание" },
    { nameof(Text.Metadata_Detail), "Дополнительно" },
    { nameof(Text.Metadata_Title), "Заголовок" },
    { nameof(Text.Metadata_Author), "Автор" },
    { nameof(Text.Metadata_Subject), "Тема" },
    { nameof(Text.Metadata_Keyword), "Ключевые слова" },
    { nameof(Text.Metadata_Version), "Версия PDF" },
    { nameof(Text.Metadata_Layout), "Режим просмотра" },
    { nameof(Text.Metadata_Creator), "Приложение" },
    { nameof(Text.Metadata_Producer), "Производитель PDF" },
    { nameof(Text.Metadata_Filename), "Имя файла" },
    { nameof(Text.Metadata_Filesize), "Размер файла" },
    { nameof(Text.Metadata_CreationTime), "Создан" },
    { nameof(Text.Metadata_LastWriteTime), "Изменён" },
    { nameof(Text.Metadata_SinglePage), "Одна страница" },
    { nameof(Text.Metadata_OneColumn), "Один столбец" },
    { nameof(Text.Metadata_TwoPageLeft), "Две страницы (слева)" },
    { nameof(Text.Metadata_TwoPageRight), "Две страницы (справа)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Два столбца (слева)" },
    { nameof(Text.Metadata_TwoColumnRight), "Два столбца (справа)" },

    // Security window
    { nameof(Text.Security_Window), "Защита PDF документа" },
    { nameof(Text.Security_OwnerPassword), "Пароль" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "Пароль" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "Подтвердить" },
    { nameof(Text.Security_Method), "Метод" },
    { nameof(Text.Security_Operations), "Права доступа" },
    { nameof(Text.Security_Enable), "Зашифровать PDF документ с помощью пароля" },
    { nameof(Text.Security_OpenWithPassword), "Запрашивать пароль для открытия документа" },
    { nameof(Text.Security_SharePassword), "Использовать пароль владельца" },
    { nameof(Text.Security_AllowPrint), "Разрешить печать" },
    { nameof(Text.Security_AllowCopy), "Разрешить копирование текста и изображений" },
    { nameof(Text.Security_AllowModify), "Разрешить вставку и удаление страниц" },
    { nameof(Text.Security_AllowAccessibility), "Разрешить доступ к тексту системам для слабовидящих" },
    { nameof(Text.Security_AllowForm), "Разрешить заполнение форм" },
    { nameof(Text.Security_AllowAnnotation), "Разрешить создание и редактирование комментариев" },

    // Insert window
    { nameof(Text.Insert_Window), "Параметры вставки" },
    { nameof(Text.Insert_Menu_Add), "Добавить" },
    { nameof(Text.Insert_Menu_Up), "Вверх" },
    { nameof(Text.Insert_Menu_Down), "Вниз" },
    { nameof(Text.Insert_Menu_Remove), "Удалить" },
    { nameof(Text.Insert_Menu_Clear), "Очистить" },
    { nameof(Text.Insert_Menu_Preview), "Предпросмотр" },
    { nameof(Text.Insert_Position), "Место вставки" },
    { nameof(Text.Insert_Position_Select), "После выделения" },
    { nameof(Text.Insert_Position_Head), "В начало" },
    { nameof(Text.Insert_Position_Tail), "В конец" },
    { nameof(Text.Insert_Position_Custom), "После страницы" },
    { nameof(Text.Insert_Column_Filename), "Имя файла" },
    { nameof(Text.Insert_Column_Filetype), "Тип" },
    { nameof(Text.Insert_Column_Filesize), "Размер" },
    { nameof(Text.Insert_Column_LastWriteTime), "Дата изменения" },

    // Extract window
    { nameof(Text.Extract_Window), "Параметры извлечения" },
    { nameof(Text.Extract_Destination), "Путь сохранения" },
    { nameof(Text.Extract_Format), "Формат файла" },
    { nameof(Text.Extract_Page), "Количество" },
    { nameof(Text.Extract_Target), "Извлечь" },
    { nameof(Text.Extract_Target_Select), "Выделенные" },
    { nameof(Text.Extract_Target_All), "Все" },
    { nameof(Text.Extract_Target_Custom), "Указанные" },
    { nameof(Text.Extract_Options), "Дополнительно" },
    { nameof(Text.Extract_Split), "Извлечь страницы как отдельные файлы" },

    // Remove window
    { nameof(Text.Remove_Window), "Параметры удаления" },
    { nameof(Text.Remove_Page), "Количество" },
    { nameof(Text.Remove_Target), "Удалить" },

    // Titles for other dialogs
    { nameof(Text.Window_Open), "Открыть файл" },
    { nameof(Text.Window_Save), "Сохранить файл как" },
    { nameof(Text.Window_Backup), "Выберите папку для хранения резервных копий документов." },
    { nameof(Text.Window_Temp), "Выберите папку для хранения временных файлов. Если папка не будет указана, то временные файлы будут сохраняться в папку документа." },
    { nameof(Text.Window_Preview), "{0} (Страница {1} из {2})" },
    { nameof(Text.Window_Password), "Введите пароль владельца" },

    // Error messages
    { nameof(Text.Error_Open), "Файл не является PDF документом либо повреждён." },
    { nameof(Text.Error_Backup), "Резервное копирование не удалось." },
    { nameof(Text.Error_Metadata), "Не удалось получить метаданные PDF документа." },
    { nameof(Text.Error_Range), "Не удалось распознать диапазон удаляемых страниц." },

    // Warning messages
    { nameof(Text.Warn_Password), "Введите, пожалуйста, пароль владельца для открытия и редактирования PDF документа: {0}." },
    { nameof(Text.Warn_Overwrite), "PDF документ изменён. Перезаписать?" },

    // Other messages
    { nameof(Text.Message_Loading), "Загружается {0} ..." },
    { nameof(Text.Message_Saving), "Сохраняется как {0} ..." },
    { nameof(Text.Message_Saved), "Сохранён как {0}" },
    { nameof(Text.Message_Pages), "{0} стр." },
    { nameof(Text.Message_Total), "Всего {0} стр." },
    { nameof(Text.Message_Selection), "Выделено {0} стр." },
    { nameof(Text.Message_Range), "например: 1,2,4-7,9" },
    { nameof(Text.Message_Byte), "байт" },
    { nameof(Text.Message_Dpi), "точки/дюйм" },

    // File filters
    { nameof(Text.Filter_All), "Все файлы" },
    { nameof(Text.Filter_Insertable), "Все поддерживаемые" },
    { nameof(Text.Filter_Extractable), "Все поддерживаемые" },
    { nameof(Text.Filter_Pdf), "Файлы PDF" },
});
