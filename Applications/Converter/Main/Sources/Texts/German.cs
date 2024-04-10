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
/// GermanText
///
/// <summary>
/// Represents the German texts used by CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class GermanText() : Globalization.TextGroup(new()
{
    // Labels for General tab
    { nameof(Text.General_Tab), "Allgemein" },
    { nameof(Text.General_Source), "Quelle" },
    { nameof(Text.General_Destination), "Ziel" },
    { nameof(Text.General_Format), "Format" },
    { nameof(Text.General_Color), "Farbe" },
    { nameof(Text.General_Resolution), "Auflösung" },
    { nameof(Text.General_Orientation), "Ausrichtung" },
    { nameof(Text.General_Options), "Optionen" },
    { nameof(Text.General_PostProcess), "Post-Prozess" },

    // Menus for General tab (ComboBox, CheckBox, ...)
    { nameof(Text.General_Overwrite), "Überschreiben" },
    { nameof(Text.General_MergeHead), "Am Anfang zusammenführen" },
    { nameof(Text.General_MergeTail), "Am Ende zusammenführen" },
    { nameof(Text.General_Rename), "Umbenennen" },
    { nameof(Text.General_Auto), "Auto" },
    { nameof(Text.General_Rgb), "RGB" },
    { nameof(Text.General_Grayscale), "Graustufen" },
    { nameof(Text.General_Monochrome), "Monochrom" },
    { nameof(Text.General_Portrait), "Hochformat" },
    { nameof(Text.General_Landscape), "Querformat" },
    { nameof(Text.General_Jpeg), "PDF-Bilder als JPEG komprimieren" },
    { nameof(Text.General_Linearization), "PDF für schnelle Webansicht optimieren" },
    { nameof(Text.General_Open), "Öffnen" },
    { nameof(Text.General_OpenDirectory), "Verzeichnis öffnen" },
    { nameof(Text.General_None), "Keine" },
    { nameof(Text.General_UserProgram), "Sonstiges" },

    // Menus for Metadata tab (ComboBox, CheckBox, ...)
    { nameof(Text.Metadata_SinglePage), "Einzelne Seite" },
    { nameof(Text.Metadata_OneColumn), "Eine Spalte" },
    { nameof(Text.Metadata_TwoPageLeft), "Zwei Seiten (links)" },
    { nameof(Text.Metadata_TwoPageRight), "Zwei Seiten (rechts)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Zwei Spalten (links)" },
    { nameof(Text.Metadata_TwoColumnRight), "Zwei Spalten (rechts)" },

    // Labels for Metadata tab
    { nameof(Text.Metadata_Tab), "Metadaten" },
    { nameof(Text.Metadata_Title), "Titel" },
    { nameof(Text.Metadata_Author), "Autor" },
    { nameof(Text.Metadata_Subject), "Betreff" },
    { nameof(Text.Metadata_Keyword), "Schlagwörter" },
    { nameof(Text.Metadata_Creator), "Creator" },
    { nameof(Text.Metadata_Layout), "Layout" },

    // Labels for Security tab
    { nameof(Text.Security_Tab), "Sicherheit" },
    { nameof(Text.Security_OwnerPassword), "Passwort" },
    { nameof(Text.Security_UserPassword), "Passwort" },
    { nameof(Text.Security_ConfirmPassword), "Bestätigen" },
    { nameof(Text.Security_Operations), "Operationen" },

    // Menus for Security tab (ComboBox, CheckBox, ...)
    { nameof(Text.Security_Enable), "Das PDF mit Passwort verschlüsseln" },
    { nameof(Text.Security_OpenWithPassword), "Mit Passwort öffnen" },
    { nameof(Text.Security_SharePassword), "Eigentümer-Passwort verwenden" },
    { nameof(Text.Security_AllowPrint), "Drucken erlauben" },
    { nameof(Text.Security_AllowCopy), "Kopieren von Text und Bildern erlauben" },
    { nameof(Text.Security_AllowModify), "Einfügen und Entfernen von Seiten erlauben" },
    { nameof(Text.Security_AllowAccessibility), "Verwendung von Inhalten für Barrierefreiheit erlauben" },
    { nameof(Text.Security_AllowForm), "Ausfüllen von Formularen erlauben" },
    { nameof(Text.Security_AllowAnnotation), "Erstellen und Editieren von Annotationen erlauben" },

    // Labels for Misc tab
    { nameof(Text.Misc_Tab), "Sonstiges" },
    { nameof(Text.Misc_About), "Über" },
    { nameof(Text.Misc_Language), "Sprache" },

    // Menus for Misc tab (ComboBox, CheckBox, ...)
    { nameof(Text.Misc_CheckUpdate), "Beim Start nach Updates suchen" },

    // Buttons
    { nameof(Text.Menu_Convert), "Konvertieren" },
    { nameof(Text.Menu_Cancel), "Abbrechen" },
    { nameof(Text.Menu_Save), "Einst. speichern" },

    // Titles for dialogs
    { nameof(Text.Window_Source), "Quelldatei auswählen" },
    { nameof(Text.Window_Destination), "Speichern unter" },
    { nameof(Text.Window_UserProgram), "Anwenderprogramm auswählen" },

    // Error messages
    { nameof(Text.Error_Source), "Eingabedatei nicht spezifiziert. Bitte prüfen Sie, ob CubePDF mittels der normalen Vorgehensweise ausgeführt wurde." },
    { nameof(Text.Error_Digest), "Message-Digest der Quelldatei stimmt nicht überein." },
    { nameof(Text.Error_Ghostscript), "Ghostscript-Fehler ({0:D})" },
    { nameof(Text.Error_InvalidChars), "Der Pfad darf keines der folgenden Zeichen enthalten." },
    { nameof(Text.Error_OwnerPassword), "Das Eigentümer-Passwort ist leer oder stimmt nicht mit der Bestätigung überein. Bitte überprüfen Sie Ihr Passwort und die Bestätigung noch einmal." },
    { nameof(Text.Error_UserPassword), "Das Benutzer-Passwort ist leer oder stimmt nicht mit der Bestätigung überein. Bitte überprüfen Sie das Benutzer-Passwort oder aktivieren Sie das Kontrollkästchen \"Eigentümer-Passwort verwenden\"." },
    { nameof(Text.Error_MergePassword), "Legen Sie in der Registerkarte Sicherheit das Eigentümer-Passwort der zusammenzuführenden PDF-Datei fest." },
    { nameof(Text.Error_PostProcess), "Post-Prozess ist fehlgeschlagen, obwohl die Konvertierung abgeschlossen wurde. Prüfen Sie, ob die Einstellungen für die Dateizuordnung oder das Benutzerprogramm korrekt sind." },

    // Warning/Confirm messages
    { nameof(Text.Warn_Exist), "{0} ist bereits vorhanden." },
    { nameof(Text.Warn_Overwrite), "Möchten Sie die Datei überschreiben?" },
    { nameof(Text.Warn_MergeHead), "Möchten Sie am Anfang der vorhandenen Datei zusammenführen?" },
    { nameof(Text.Warn_MergeTail), "Möchten Sie am Ende der vorhandenen Datei zusammenführen?" },
    { nameof(Text.Warn_Metadata), "In das Feld Titel, Autor, Betreff oder Schlagwörter wird ein gewisser Wert eingegeben. Möchten Sie die Einstellungen speichern?" },

    // File filters
    { nameof(Text.Filter_All), "Alle Dateien" },
    { nameof(Text.Filter_Pdf), "PDF-Dateien" },
    { nameof(Text.Filter_Ps), "PS-Dateien" },
    { nameof(Text.Filter_Eps), "EPS-Dateien" },
    { nameof(Text.Filter_Bmp), "BMP-Dateien" },
    { nameof(Text.Filter_Png), "PNG-Dateien" },
    { nameof(Text.Filter_Jpeg), "JPEG-Dateien" },
    { nameof(Text.Filter_Tiff), "TIFF-Dateien" },
    { nameof(Text.Filter_Exe), "Ausführbare Dateien" },
});