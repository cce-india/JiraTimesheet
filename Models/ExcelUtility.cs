﻿using System;
using System.IO;
using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace JiraTimesheet.Models
{
    public class ExcelUtility 
    {
        // Get the excel column letter by index
        public string ColumnLetter(int intCol)
        {
            int intFirstLetter = ((intCol) / 676) + 64;
            int intSecondLetter = ((intCol % 676) / 26) + 64;
            int intThirdLetter = (intCol % 26) + 65;

            char firstLetter = (intFirstLetter > 64) ? (char)intFirstLetter : ' ';
            char secondLetter = (intSecondLetter > 64) ? (char)intSecondLetter : ' ';
            char thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter, thirdLetter).Trim();
        }

        // Create a text cell
        private Cell CreateTextCell(string header, UInt32 index, string text)
        {
            var cell = new Cell();
            cell.CellReference = header + index;
            cell.DataType = CellValues.InlineString;
            var istring = new InlineString();
            var cellText = new Text { Text = text };
            istring.Append(cellText);
            cell.Append(istring);
            return cell;
        }

        public MemoryStream GetExcel(DataTable data)
        {
            string[] fieldsToExpose = new string[data.Columns.Count];
            for (int i = 0; i < data.Columns.Count; i++)
            {
                fieldsToExpose[i] = data.Columns[i].ColumnName;
            }

            return GetExcel(fieldsToExpose, data);
        }

        public MemoryStream GetExcel(string[] fieldsToExpose, DataTable data)
        {
            MemoryStream stream = new MemoryStream();
            UInt32 rowcount = 0;

            // Create the Excel document
            var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var relId = workbookPart.GetIdOfPart(worksheetPart);

            var workbook = new Workbook();
            var fileVersion = new FileVersion { ApplicationName = "Microsoft Office Excel" };
            var worksheet = new Worksheet();
            var sheetData = new SheetData();
            worksheet.Append(sheetData);
            worksheetPart.Worksheet = worksheet;

            var sheets = new Sheets();
            var sheet = new Sheet { Name = "Sheet1", SheetId = 1, Id = relId };
            sheets.Append(sheet);
            workbook.Append(fileVersion);
            workbook.Append(sheets);
            document.WorkbookPart.Workbook = workbook;
            document.WorkbookPart.Workbook.Save();

            // Add header to the sheet
            var row = new Row { RowIndex = ++rowcount };
            for (int i = 0; i < fieldsToExpose.Length; i++)
            {
                row.Append(CreateTextCell(ColumnLetter(i), rowcount, fieldsToExpose[i]));
            }
            sheetData.AppendChild(row);
            worksheetPart.Worksheet.Save();

            // Add data to the sheet
            foreach (DataRow dataRow in data.Rows)
            {
                row = new Row { RowIndex = ++rowcount };
                for (int i = 0; i < fieldsToExpose.Length; i++)
                {
                    row.Append(CreateTextCell(ColumnLetter(i), rowcount, dataRow[fieldsToExpose[i]].ToString()));
                }
                sheetData.AppendChild(row);
            }
            worksheetPart.Worksheet.Save();
            document.Close();         
            return stream;
        }        
    }
}