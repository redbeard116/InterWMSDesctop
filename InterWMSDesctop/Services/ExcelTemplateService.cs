using ApiApp.Models;
using ClosedXML.Excel;
using ApiApp.Extensions;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace InterWMSDesctop.Services
{
    class ExcelTemplateService
    {
        #region Public methods
        public void SaveReport(ProductsReport productsReport)
        {
            var headers = GetHeaders(productsReport);
            var values = GetValues(productsReport.ReportProducts);

            var excel = GenerateReport(headers, values, productsReport.ReportName);

            var sfd = new SaveFileDialog();
            sfd.FileName = "report";
            sfd.Filter = "Excel documents(.xlsx) | *.xlsx";
            if (sfd.ShowDialog() == true)
            {
                excel.SaveAs(sfd.FileName);
                var path = Path.GetDirectoryName(sfd.FileName);
                Process.Start(path);
            }
        }

        public void SaveContract(Contract contract)
        {

            var excel = GenerateContract(contract);

            var sfd = new SaveFileDialog();
            sfd.FileName = "contract";
            sfd.Filter = "Excel documents(.xlsx) | *.xlsx";
            if (sfd.ShowDialog() == true)
            {
                excel.SaveAs(sfd.FileName);
                var path = Path.GetDirectoryName(sfd.FileName);
                Process.Start(path);
            }
        }
        #endregion

        #region Private methods
        private List<List<string>> GetValues(IEnumerable<ReportProduct> reportProducts)
        {
            var list = new List<List<string>>();
            foreach (var product in reportProducts)
            {
                var values = new List<string>();
                values.Add(product.Name);
                foreach (var sort in product.Infos)
                {
                    values.Add(sort.Count.ToString());
                }
                list.Add(values);
            }
            return list;
        }

        private List<string> GetHeaders(ProductsReport productsReport)
        {
            var list = new List<string>();
            list.Add("Продукт");
            foreach (var product in productsReport.ReportProducts)
            {
                foreach (var info in product.Infos)
                {
                    list.Add(info.Date);
                }
                break;
            }

            return list;
        }

        private XLWorkbook GenerateReport(List<string> headers, List<List<string>> values, string title)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Отчет");

            var insertedReportTitle = ws.Cell(1, 1).InsertData(new List<string> { title });
            insertedReportTitle.Transpose(XLTransposeOptions.ReplaceCells);

            var insertedHeaders = ws.Cell(2, 1).InsertData(headers);
            insertedHeaders.Transpose(XLTransposeOptions.MoveCells);
            int row = 3;
            foreach (var item in values)
            {
                var insertedData = ws.Cell(row++, 1).InsertData(item);
                insertedData.Transpose(XLTransposeOptions.MoveCells);
            }


            var style = wb.Style;
            style.Font.Bold = true;
            style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            insertedHeaders.Style = style;
            insertedReportTitle.Style = style;

            ws.Columns().AdjustToContents();

            return wb;
        }

        private XLWorkbook GenerateContract(Contract contract)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Договор");
            var date = contract.Date.GetNormalTime().ToString("dd.MM.yyy HH:mm");

            var insertedTitle = ws.Cell(1, 2).InsertData(new List<string> { $"Договор от {date}" });
            insertedTitle.Transpose(XLTransposeOptions.MoveCells);

            var insertedCounterParty = ws.Cell(2, 2).InsertData(new List<string> { "Контрагент:", contract.Counterparty.FullName });
            insertedCounterParty.Transpose(XLTransposeOptions.MoveCells);

            var insertedHeaders = ws.Cell(4, 1).InsertData(new List<string> { "Товар", "Количество", "Сумма" });
            insertedHeaders.Transpose(XLTransposeOptions.MoveCells);
            int row = 5;
            foreach (var item in contract.Products)
            {
                var insertedData = ws.Cell(row++, 1).InsertData(new List<string> { item.Product.Name, item.Count.ToString(), item.Sum.ToString() });
                insertedData.Transpose(XLTransposeOptions.MoveCells);
            }

            row++;

            var insertedAvg = ws.Cell(row++, 1).InsertData(new List<string> { "Итого:", contract.Products.Sum(w => w.Count).ToString(), contract.Sum.ToString() });
            insertedAvg.Transpose(XLTransposeOptions.MoveCells);

            var insertedDate = ws.Cell(row++, 2).InsertData(new List<string> { "Дата:", date });
            insertedDate.Transpose(XLTransposeOptions.MoveCells);

            row++;
            var inserted = ws.Cell(row++, 2).InsertData(new List<string> { "Подпись" });
            inserted.Transpose(XLTransposeOptions.MoveCells);

            var style = wb.Style;
            style.Font.Bold = true;
            style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            insertedHeaders.Style = style;
            insertedTitle.Style = style;
            insertedCounterParty.Style = style;
            insertedAvg.Style = style;
            insertedDate.Style = style;
            inserted.Style = style;

            ws.Columns().AdjustToContents();

            return wb;
        }
        #endregion
    }
}
