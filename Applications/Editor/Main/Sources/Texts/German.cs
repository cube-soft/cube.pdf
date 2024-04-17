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
/// GermanText
///
/// <summary>
/// Represents the German texts used by CubePDF Utility.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class GermanText() : Globalization.TextGroup(new()
{
    // Menus. Note that Menu_*_Long values are used for tooltips.
    { nameof(Text.Menu_Ok), "OK" },
    { nameof(Text.Menu_Cancel), "Abbrechen" },
    { nameof(Text.Menu_Exit), "Beenden" },
    { nameof(Text.Menu_File), "Datei" },
    { nameof(Text.Menu_Edit), "Bearbeiten" },
    { nameof(Text.Menu_Misc), "Sonstiges" },
    { nameof(Text.Menu_Help), "Hilfe" },
    { nameof(Text.Menu_Setting), "Einstellungen" },
    { nameof(Text.Menu_Preview), "Vorschau" },
    { nameof(Text.Menu_Metadata), "Metadaten" },
    { nameof(Text.Menu_Metadata_Long), "PDF-Dokument-Metadaten" },
    { nameof(Text.Menu_Security), "Sicherheit" },
    { nameof(Text.Menu_Open), "Öffnen" },
    { nameof(Text.Menu_Close), "Schließen" },
    { nameof(Text.Menu_Save), "Speichern" },
    { nameof(Text.Menu_Save_Long), "Speichern" },
    { nameof(Text.Menu_Save_As), "Speichern unter" },
    { nameof(Text.Menu_Redraw), "Refresh" },
    { nameof(Text.Menu_Undo), "Undo" },
    { nameof(Text.Menu_Redo), "Redo" },
    { nameof(Text.Menu_Select), "Auswählen" },
    { nameof(Text.Menu_Select_All), "Alle auswählen" },
    { nameof(Text.Menu_Select_Flip), "Auswahl umkehren" },
    { nameof(Text.Menu_Select_Clear), "Alle abwählen" },
    { nameof(Text.Menu_Insert), "Einfügen" },
    { nameof(Text.Menu_Insert_Long), "Hinter ausgewählter Position einfügen" },
    { nameof(Text.Menu_Insert_Head), "Am Anfang einfügen" },
    { nameof(Text.Menu_Insert_Tail), "Am Ende einfügen" },
    { nameof(Text.Menu_Insert_Custom), "An anderer Position einfügen" },
    { nameof(Text.Menu_Extract), "Extrahieren" },
    { nameof(Text.Menu_Extract_Long), "Die ausgewählten Seiten extrahieren" },
    { nameof(Text.Menu_Extract_Custom), "Mit anderen Einstellungen extrahieren" },
    { nameof(Text.Menu_Remove), "Entfernen" },
    { nameof(Text.Menu_Remove_Long), "Die ausgewählten Seiten entfernen" },
    { nameof(Text.Menu_Remove_Custom), "Andere Seiten entfernen" },
    { nameof(Text.Menu_Move_Back), "Zurück" },
    { nameof(Text.Menu_Move_Forth), "Vor" },
    { nameof(Text.Menu_Rotate_Left), "Links" },
    { nameof(Text.Menu_Rotate_Right), "Rechts" },
    { nameof(Text.Menu_Zoom_In), "Heranzoomen" },
    { nameof(Text.Menu_Zoom_Out), "Herauszoomen" },
    { nameof(Text.Menu_Frame), "Nur Frame" },
    { nameof(Text.Menu_Recent), "Zuletzt verwendete Dateien" },

    // Setting window
    { nameof(Text.Setting_Window), "CubePDF Utility-Einstellungen" },
    { nameof(Text.Setting_Tab), "Einstellungen" },
    { nameof(Text.Setting_Version), "Version" },
    { nameof(Text.Setting_Options), "Speicheroptionen" },
    { nameof(Text.Setting_Backup), "Backup" },
    { nameof(Text.Setting_Backup_Enable), "Backup aktivieren" },
    { nameof(Text.Setting_Backup_Clean), "Delete old backup files automatically" },
    { nameof(Text.Setting_Temp), "Temp" },
    { nameof(Text.Setting_Language), "Sprache" },
    { nameof(Text.Setting_Others), "Sonstiges" },
    { nameof(Text.Setting_Shrink), "Duplizierte Ressourcen reduzieren" },
    { nameof(Text.Setting_KeepOutline), "Bookmarks der Quell-PDF-Datei behalten" },
    { nameof(Text.Setting_Recent), "Kürzlich verwendete Dateien zeigen" },
    { nameof(Text.Setting_CheckUpdate), "Beim Start nach Updates suchen" },

    // Metadata window
    { nameof(Text.Metadata_Window), "PDF-Metadaten" },
    { nameof(Text.Metadata_Summary), "Zusammenfassung" },
    { nameof(Text.Metadata_Detail), "Details" },
    { nameof(Text.Metadata_Title), "Titel" },
    { nameof(Text.Metadata_Author), "Autor" },
    { nameof(Text.Metadata_Subject), "Betreff" },
    { nameof(Text.Metadata_Keyword), "Schlagwörter" },
    { nameof(Text.Metadata_Version), "PDF-Version" },
    { nameof(Text.Metadata_Layout), "Layout" },
    { nameof(Text.Metadata_Creator), "Creator" },
    { nameof(Text.Metadata_Producer), "Producer" },
    { nameof(Text.Metadata_Filename), "Dateiname" },
    { nameof(Text.Metadata_Filesize), "Dateigröße" },
    { nameof(Text.Metadata_CreationTime), "Erstellung" },
    { nameof(Text.Metadata_LastWriteTime), "Zuletzt aktualisiert" },
    { nameof(Text.Metadata_SinglePage), "Einzelne Seite" },
    { nameof(Text.Metadata_OneColumn), "Eine Spalte" },
    { nameof(Text.Metadata_TwoPageLeft), "Zwei Seiten (links)" },
    { nameof(Text.Metadata_TwoPageRight), "Zwei Seiten (rechts)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Zwei Spalten (links)" },
    { nameof(Text.Metadata_TwoColumnRight), "Zwei Spalten (rechts)" },

    // Security window
    { nameof(Text.Security_Window), "Sicherheit" },
    { nameof(Text.Security_OwnerPassword), "Passwort" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "Passwort" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "Bestätigen" },
    { nameof(Text.Security_Method), "Methode" },
    { nameof(Text.Security_Operations), "Operationen" },
    { nameof(Text.Security_Enable), "Das PDF mit Passwort verschlüsseln" },
    { nameof(Text.Security_OpenWithPassword), "Mit Passwort öffnen" },
    { nameof(Text.Security_SharePassword), "Eigentümer-Passwort verwenden" },
    { nameof(Text.Security_AllowPrint), "Drucken erlauben" },
    { nameof(Text.Security_AllowCopy), "Kopieren von Text und Bildern erlauben" },
    { nameof(Text.Security_AllowModify), "Einfügen und Entfernen von Seiten erlauben" },
    { nameof(Text.Security_AllowAccessibility), "Verwendung von Inhalten für Barrierefreiheit erlauben" },
    { nameof(Text.Security_AllowForm), "Ausfüllen von Formularfeldern erlauben" },
    { nameof(Text.Security_AllowAnnotation), "Erstellen und Editieren von Annotationen erlauben" },

    // Insert window
    { nameof(Text.Insert_Window), "Details zur Einfügung" },
    { nameof(Text.Insert_Menu_Add), "Hinzufügen" },
    { nameof(Text.Insert_Menu_Up), "Nach oben" },
    { nameof(Text.Insert_Menu_Down), "Nach unten" },
    { nameof(Text.Insert_Menu_Remove), "Remove" },
    { nameof(Text.Insert_Menu_Clear), "Löschen" },
    { nameof(Text.Insert_Menu_Preview), "Vorschau" },
    { nameof(Text.Insert_Position), "Einfügeposition" },
    { nameof(Text.Insert_Position_Select), "Ausgewählte Position" },
    { nameof(Text.Insert_Position_Head), "Anfang" },
    { nameof(Text.Insert_Position_Tail), "Ende" },
    { nameof(Text.Insert_Position_Custom), "Hinter der Anzahl von" },
    { nameof(Text.Insert_Column_Filename), "Dateiname" },
    { nameof(Text.Insert_Column_Filetype), "Typ" },
    { nameof(Text.Insert_Column_Filesize), "Dateigröße" },
    { nameof(Text.Insert_Column_LastWriteTime), "Zuletzt aktualisiert" },

    // Extract window
    { nameof(Text.Extract_Window), "Details zur Extraktion" },
    { nameof(Text.Extract_Destination), "Speicherpfad" },
    { nameof(Text.Extract_Format), "Format" },
    { nameof(Text.Extract_Page), "Seitenzahl" },
    { nameof(Text.Extract_Target), "Zielseiten" },
    { nameof(Text.Extract_Target_Select), "Ausgewählte Seiten" },
    { nameof(Text.Extract_Target_All), "Alle Seiten" },
    { nameof(Text.Extract_Target_Custom), "Spezifizierter Bereich" },
    { nameof(Text.Extract_Options), "Optionen" },
    { nameof(Text.Extract_Split), "Als separate Datei pro Seite speichern" },

    // Remove window
    { nameof(Text.Remove_Window), "Details zur Entfernung" },
    { nameof(Text.Remove_Page), "Seitenzahl" },
    { nameof(Text.Remove_Target), "Zielseiten" },

    // Password window

    // Titles for other dialogs
    { nameof(Text.Window_Open), "Datei öffnen" },
    { nameof(Text.Window_Save), "Speichern unter" },
    { nameof(Text.Window_Backup), "Wählen Sie einen Backup-Ordner aus." },
    { nameof(Text.Window_Temp), "Wählen Sie einen temporären Ordner aus. Wenn Sie keinen Temp-Ordner spezifizieren, wird derselbe Ordner wie für die Quelldatei verwendet." },
    { nameof(Text.Window_Preview), "{0} ({1}/{2} Seite)" },
    { nameof(Text.Window_Password), "Passwort eingeben" },

    // Error messages
    { nameof(Text.Error_Open), "Datei ist nicht im PDF-Format oder ist beschädigt." },
    { nameof(Text.Error_Metadata), "PDF-Metadaten konnten nicht abgerufen werden." },
    { nameof(Text.Error_Range), "Der Entfernungsbereich konnte nicht geparst werden." },

    // Warning messages
    { nameof(Text.Warn_Password), "Bitte geben Sie das Eigentümer-Passwort zum Öffnen und Editieren ein - {0}." },
    { nameof(Text.Warn_Overwrite), "PDF wird modifiziert. Möchten Sie überschreiben?" },

    // Other messages
    { nameof(Text.Message_Loading), "Geladen wird {0} ..." },
    { nameof(Text.Message_Saving), "Speicherung als {0} ..." },
    { nameof(Text.Message_Saved), "Gespeichert als {0}" },
    { nameof(Text.Message_Pages), "{0} Seite(n)" },
    { nameof(Text.Message_Total), "{0} Seite(n)" },
    { nameof(Text.Message_Selection), "{0} Seite(n) ausgewählt" },
    { nameof(Text.Message_Range), "Zum Beispiel: 1,2,4-7,9" },
    { nameof(Text.Message_Byte), "Bytes" },
    { nameof(Text.Message_Dpi), "dpi" },

    // File filters
    { nameof(Text.Filter_All), "Alle Dateien" },
    { nameof(Text.Filter_Insertable), "Einfügbare Dateien" },
    { nameof(Text.Filter_Extractable), "Unterstützte Dateien" },
    { nameof(Text.Filter_Pdf), "PDF-Dateien" },
});
