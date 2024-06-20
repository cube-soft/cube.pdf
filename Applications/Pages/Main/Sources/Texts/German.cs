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
/// GermanText
///
/// <summary>
/// Represents the German texts used by CubePDF Page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class GermanText() : Globalization.TextGroup(new()
{
    // Menus
    { nameof(Text.Menu_Ok), "OK" },
    { nameof(Text.Menu_Cancel), "Abbrechen" },
    { nameof(Text.Menu_Exit), "Beenden" },
    { nameof(Text.Menu_Setting), "Einstellungen" },
    { nameof(Text.Menu_Metadata), "Metadaten" },
    { nameof(Text.Menu_Merge), "Zusammenführen" },
    { nameof(Text.Menu_Split), "Splitten" },
    { nameof(Text.Menu_Add), "Hinzufügen" },
    { nameof(Text.Menu_Up), "Nach oben" },
    { nameof(Text.Menu_Down), "Nach unten" },
    { nameof(Text.Menu_Remove), "Entfernen" },
    { nameof(Text.Menu_Clear), "Löschen" },
    { nameof(Text.Menu_Preview), "Vorschau" },

    // Columns for Main window
    { nameof(Text.Column_Filename), "Dateiname" },
    { nameof(Text.Column_Filetype), "Typ" },
    { nameof(Text.Column_Filesize), "Dateigröße" },
    { nameof(Text.Column_Pages), "Seiten" },
    { nameof(Text.Column_Date), "Zuletzt aktualisiert" },

    // Labels for Setting window
    { nameof(Text.Setting_Window), "CubePDF Page-Einstellungen" },
    { nameof(Text.Setting_Tab), "Einstellungen" },
    { nameof(Text.Setting_Version), "Version" },
    { nameof(Text.Setting_Options), "Optionen" },
    { nameof(Text.Setting_Temp), "Temp" },
    { nameof(Text.Setting_Language), "Sprache" },
    { nameof(Text.Setting_Others), "Sonstiges" },
    { nameof(Text.Setting_Shrink), "Duplizierte Ressourcen reduzieren" },
    { nameof(Text.Setting_KeepOutline), "Bookmarks der Quell-PDF-Dateien behalten" },
    { nameof(Text.Setting_CheckUpdate), "Beim Start nach Updates suchen" },

    // Labels for Metadata window
    { nameof(Text.Metadata_Window), "PDF-Metadaten" },
    { nameof(Text.Metadata_Tab), "Metadaten" },
    { nameof(Text.Metadata_Title), "Titel" },
    { nameof(Text.Metadata_Author), "Autor" },
    { nameof(Text.Metadata_Subject), "Betreff" },
    { nameof(Text.Metadata_Keyword), "Schlagwörter" },
    { nameof(Text.Metadata_Creator), "Creator" },
    { nameof(Text.Metadata_Version), "Version" },
    { nameof(Text.Metadata_Layout), "Layout" },

    // Menus for Metadata window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Metadata_SinglePage), "Einzelne Seite" },
    { nameof(Text.Metadata_OneColumn), "Eine Spalte" },
    { nameof(Text.Metadata_TwoPageLeft), "Zwei Seiten (links)" },
    { nameof(Text.Metadata_TwoPageRight), "Zwei Seiten (rechts)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Zwei Spalten (links)" },
    { nameof(Text.Metadata_TwoColumnRight), "Zwei Spalten (rechts)" },

    // Labels for Security window
    { nameof(Text.Security_Tab), "Sicherheit" },
    { nameof(Text.Security_OwnerPassword), "Passwort" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "Passwort" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "Bestätigen" },
    { nameof(Text.Security_Operations), "Operationen" },

    // Menus for Security window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Security_Enable), "Das PDF mit Passwort verschlüsseln" },
    { nameof(Text.Security_OpenWithPassword), "Mit Passwort öffnen" },
    { nameof(Text.Security_SharePassword), "Eigentümer-Passwort verwenden" },
    { nameof(Text.Security_AllowPrint), "Drucken erlauben" },
    { nameof(Text.Security_AllowCopy), "Kopieren von Text und Bildern erlauben" },
    { nameof(Text.Security_AllowModify), "Einfügen und Entfernen von Seiten erlauben" },
    { nameof(Text.Security_AllowAccessibility), "Verwendung von Inhalten für Barrierefreiheit erlauben" },
    { nameof(Text.Security_AllowForm), "Ausfüllen von Formularfeldern erlauben" },
    { nameof(Text.Security_AllowAnnotation), "Erstellen und Editieren von Annotationen erlauben" },

    // Labels for Password window
    { nameof(Text.Password_Window), "Geben Sie das Eigentümer-Passwort ein" },
    { nameof(Text.Password_Show), "Passwort zeigen" },

    // Titles for other dialogs
    { nameof(Text.Window_Add), "Dateien hinzufügen" },
    { nameof(Text.Window_Merge), "Zusammengeführte Datei speichern als" },
    { nameof(Text.Window_Split), "Wählen Sie einen Ordner aus, um die Split-Dateien zu speichern." },
    { nameof(Text.Window_Temp), "Wählen Sie einen temporären Ordner aus. Wenn Sie keinen Temp-Ordner spezifizieren, wird derselbe Ordner wie für die Quelldateien verwendet." },

    // Error messages
    { nameof(Text.Error_OwnerPassword), "Das Eigentümer-Passwort ist leer oder stimmt nicht mit der Bestätigung überein. Bitte überprüfen Sie Ihr Passwort und die Bestätigung noch einmal." },
    { nameof(Text.Error_UserPassword), "Das Benutzer-Passwort ist leer oder stimmt nicht mit der Bestätigung überein. Bitte überprüfen Sie das Benutzer-Passwort oder aktivieren Sie das Kontrollkästchen Eigentümer-Passwort verwenden." },

    // Warning messages
    { nameof(Text.Warn_Password), "{0} ist geschützt. Geben Sie das Eigentümer-Passwort zum Bearbeiten ein." },

    // File filters
    { nameof(Text.Filter_All), "Alle Dateien" },
    { nameof(Text.Filter_Support), "Alle unterstützten Dateien" },
    { nameof(Text.Filter_Pdf), "PDF-Dateien" },
});
