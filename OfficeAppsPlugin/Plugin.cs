using Mobilize;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace OfficeAppsPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        public string ImgSource
        {
            get
            {
                return null;
            }
        }

        public string Name
        {
            get
            {
                return "OfficeApps";
            }
        }

        static Dictionary<Guid, Excel.Application> excelApplicationReferences = new Dictionary<Guid, Excel.Application>();
        static Dictionary<Guid, Word.Application> wordApplicationReferences = new Dictionary<Guid, Word.Application>();

        public string InvokeAction(IPlugin Plugin, string Action, string ActionParams)
        {
            switch (Action)
            {
                case "OpenExcel":
                    {
                        var excelApp = new Excel.Application();
                        // Make the object visible.
                        excelApp.Visible = true;
                        var guid = new Guid();
                        excelApplicationReferences.Add(guid, excelApp);
                        return new JObject(new JProperty("ExcelID", guid.ToString())).ToString();
                    }
                case "SetCell":
                    {
                        var parameters = JObject.Parse(ActionParams);
                        if (parameters["ExcelID"] == null ) throw new ArgumentException("Missing arguments excel ID");
                        var excelID = parameters["ExcelID"].Value<string>();
                        var excelApp = excelApplicationReferences[new Guid(excelID)];
                        if (excelApp == null) throw new InvalidOperationException(string.Format("ExcelAPP for ID {0} was not found", excelID));
                        if (parameters["row"] == null || parameters["column"] == null || parameters["value"] == null) throw new ArgumentException("Missing arguments, either row or column or value is missing");
                        var row = parameters["row"].Value<int>();
                        var column = parameters["column"].Value<string>();
                        Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
                        workSheet.Cells[row, column] = parameters["value"].Value<string>();
                        return new JObject().ToString();
                    }
                case "GetCell":
                    {
                        var parameters = JObject.Parse(ActionParams);
                        if (parameters["ExcelID"] == null) throw new ArgumentException("Missing arguments excel ID");
                        var excelID = parameters["ExcelID"].Value<string>();
                        var excelApp = excelApplicationReferences[new Guid(excelID)];
                        if (excelApp == null) throw new InvalidOperationException(string.Format("ExcelAPP for ID {0} was not found", excelID));
                        if (parameters["row"] == null || parameters["column"] == null) throw new ArgumentException("Missing arguments, either row or column or value is missing");
                        var row = parameters["row"].Value<int>();
                        var column = parameters["column"].Value<string>();
                        Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
                        var value = workSheet.Cells[row, column];
                        return new JObject(new JProperty("Value",value)).ToString();
                    }
                case "OpenWord":
                    {
                        var wordApp = new Word.Application();
                        // Make the object visible.
                        wordApp.Visible = true;
                        wordApp.Documents.Add();
                        var guid = new Guid();
                        wordApplicationReferences.Add(guid, wordApp);
                        return new JObject(new JProperty("WORDID", guid.ToString())).ToString();
                    }
                case "PasteToWord":
                    {
                        var parameters = JObject.Parse(ActionParams);
                        if (parameters["WordID"] == null) throw new ArgumentException("Missing arguments word ID");
                        var wordID = parameters["WordID"].Value<string>();
                        var wordApp = wordApplicationReferences[new Guid(wordID)];
                        if (wordApp == null) throw new InvalidOperationException(string.Format("WordAPP for ID {0} was not found", wordID));
                        if (parameters["value"] == null) throw new ArgumentException("Missing argument value is missing");
                        Word.Range rng = wordApp.ActiveDocument.Range(0, 0);
                        rng.Text = parameters["value"].Value<string>();
                        return new JObject().ToString();
                    }

                default:
                    throw new InvalidPluginAction(Action);
            }

        }
    }
}
