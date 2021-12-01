using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;
using YeetOverFlow.Data.Wpf.ViewModels;
using YeetOverFlow.Wpf.Controls;
using YeetOverFlow.Wpf.Ui;

namespace YeetOverFlow.Data.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for YeetDataSetControl.xaml
    /// </summary>
    public partial class YeetDataControl : YeetControlBase
    {
        #region Data
        private static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(YeetDataLibraryViewModel), typeof(YeetDataControl));

        public YeetDataLibraryViewModel Data
        {
            get { return (YeetDataLibraryViewModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        #endregion Data

        public YeetDataControl()
        {
            InitializeComponent();
        }

        private void MainPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = ((string[])e.Data.GetData(DataFormats.FileDrop));
                foreach (var filePath in files)
                {
                    String filename = Path.GetFileName(filePath);
                    String ext = Path.GetExtension(filePath).TrimStart('.');
                    var opts = new YeetDataConverterOptions();

                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        opts.HasHeaders = false;
                    }

                    switch (ext.ToLower())
                    {
                        case "csv":
                            Data.Root[filename] = YeetDataConverter.CsvFileToData(filePath, opts);
                            break;
                        case "tsv":
                            opts.FieldDelim = '\t';
                            Data.Root[filename] = YeetDataConverter.CsvFileToData(filePath, opts);
                            break;
                        case "json":
                            Data.Root[filename] = YeetDataConverter.JsonFileToData(filePath, opts);
                            break;
                        case "xml":
                            Data.Root[filename] = YeetDataConverter.XmlFileToData(filePath, opts);
                            break;
                    }
                }
            }
        }

        private void PasteTextBox_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                var tabItem = (TabItem)((FrameworkElement)sender).Parent;
                String text = Clipboard.GetText();
                String name = $"Tbl {Data.Root.Count + 1}";
                var opts = new YeetDataConverterOptions();

                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    opts.HasHeaders = false;
                }

                switch (tabItem.Header.ToString().ToLower())
                {
                    case "csv":
                        Data.Root[name] = YeetDataConverter.CsvStringToData(text, opts);
                        break;
                    case "tsv":
                        opts.FieldDelim = '\t';
                        Data.Root[name] = YeetDataConverter.CsvStringToData(text, opts);
                        break;
                    case "json":
                        Data.Root[name] = YeetDataConverter.JsonStringToData(text, opts);
                        break;
                    case "xml":
                        Data.Root[name] = YeetDataConverter.XmlStringToData(text, opts);
                        break;
                }
                
                e.Handled = true;
            }
        }

        private void TabHeader_TextBoxLoaded(object sender, RoutedEventArgs e)
        {
            TextBox txtText = (TextBox)sender;
            txtText.SelectAll();
            txtText.Focus();
        }

        private void TabHeader_TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox txtText = (TextBox)sender;
                MenuItem menuItem = (MenuItem)DependencyObjectHelper.TryFindParent<MenuItem>(txtText);
                Button btn = menuItem.FindLogicalChild<Button>();
                btn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void TabHeader_RenameClick(object sender, RoutedEventArgs e)
        {
            Button btnRename = (Button)sender;
            ContextMenu contextMenu = DependencyObjectHelper.TryFindParent<ContextMenu>(btnRename);
            contextMenu.IsOpen = false;
            var args = (object[])btnRename.Tag;
            Data.RenameByGuid((Guid)args[0], (string)args[1]);
        }
    }

    internal class YeetDataConverterOptions
    {
        public string Name { get; set; }
        public char RecordDelim { get; set; } = '\n';
        public char FieldDelim { get; set; } = ',';
        public bool HasHeaders { get; set; } = true;
    }

    internal static class YeetDataConverter
    {
        public static YeetDataViewModel CsvStringToData(string csv, YeetDataConverterOptions opts = null)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            return YeetDataConverter.CsvStreamToData(stream, opts);
        }

        public static YeetDataViewModel CsvFileToData(string filePath, YeetDataConverterOptions opts = null)
        {
            var fileStream = File.OpenRead(filePath);
            fileStream.Seek(0, SeekOrigin.Begin);
            return YeetDataConverter.CsvStreamToData(fileStream, opts);
        }

        public static YeetDataViewModel CsvStreamToData(Stream stream, YeetDataConverterOptions opts = null)
        {
            if (opts == null)
            {
                opts = new YeetDataConverterOptions();
            }

            var fieldDelim = opts.FieldDelim;
            var recordDelim = opts.RecordDelim;
            var hasHeaders = opts.HasHeaders;
            CsvConfiguration _conf = new CsvConfiguration(CultureInfo.InvariantCulture);

            YeetTableViewModel tbl = new YeetTableViewModel();
            var sr = new StreamReader(stream);

            if (recordDelim != '\n')
            {
                #region If Record Delimiter is overriden, itterate through all characters and replace them with new line
                //https://stackoverflow.com/questions/1232443/writing-to-then-reading-from-a-memorystream
                MemoryStream ms = new MemoryStream();

                StreamWriter sw = new StreamWriter(ms);
                while (sr.Peek() >= 0)
                {
                    Char c = (Char)sr.Read();
                    if (c == recordDelim)
                    {
                        sw.Write('\n');
                    }
                    else
                    {
                        sw.Write(c);
                    }
                }

                sw.Flush();
                ms.Position = 0;

                sr = new StreamReader(ms, Encoding.UTF8);
                #endregion
            }

            _conf.BadDataFound = null;
            //_conf.BadDataFound = cxt =>
            //{
            //    //For debugging(put breakpoints here)
            //};

            _conf.Delimiter = fieldDelim.ToString();
            if (_conf.Delimiter != ",")
            {
                _conf.Mode = CsvHelper.CsvMode.NoEscape;
            }
            _conf.HasHeaderRecord = hasHeaders;
            _conf.MissingFieldFound = null;
            CsvReader csvReader = new CsvReader(sr, _conf);

            if (hasHeaders)
            {
                csvReader.ReadHeader();

                foreach (var header in csvReader.HeaderRecord)
                {
                    var col = new YeetStringColumnViewModel();
                    tbl.Columns[header.Trim().Trim('"')] = col;
                }

                while (csvReader.Read())
                {
                    var row = new YeetRowViewModel();
                    foreach (var header in csvReader.HeaderRecord)
                    {
                        if (csvReader.TryGetField<String>(header, out string field))
                        {
                            var cell = new YeetStringCellViewModel();
                            cell.Value = field;
                            row[header.Trim().Trim('"')] = cell;
                        }
                    }
                    tbl.Rows.AddChild(row);
                }
            }
            else
            {
                while (csvReader.Read())
                {
                    var row = new YeetRowViewModel();

                    for (int f = 0; f < csvReader.Parser.Count; f++)
                    {
                        var field = csvReader.Parser.Record[f];
                        var colName = $"Col{f}";

                        if (!tbl.Columns.ContainsKey(colName))
                        {
                            tbl.Columns[colName] = new YeetStringColumnViewModel();
                        }

                        var cell = new YeetStringCellViewModel();
                        cell.Value = field;
                        row[colName] = cell;
                    }

                    tbl.Rows.AddChild(row);
                }
            }
            
            return tbl;
        }

        public static YeetDataViewModel JsonFileToData(string filePath, YeetDataConverterOptions opts = null)
        {
            var fileStream = File.OpenRead(filePath);
            fileStream.Seek(0, SeekOrigin.Begin);
            return YeetDataConverter.JsonStreamToData(fileStream, opts);
        }

        public static YeetDataViewModel JsonStreamToData(Stream stream, YeetDataConverterOptions opts = null)
        {
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            return JsonStringToData(json, opts);
        }

        public static YeetDataViewModel JsonStringToData(string json, YeetDataConverterOptions opts = null)
        {
            DataSet ds = new DataSet();
            //Conversion to JObject is to prevent automatic DateTime columns because they break when the value is an empty string
            JObject jInput = JsonConvert.DeserializeObject<JObject>(json, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

            //Handle Array with one value
            if (jInput.Count == 1 && jInput["root"] is JArray && ((JArray)jInput["root"]).Count == 1 && ((JArray)jInput["root"])[0] is JValue)
            {
                DataTable dt = new DataTable("root");
                dt.Columns.Add("root_Text");
                dt.Rows.Add(new object[] { ((JArray)jInput["root"])[0] });
                ds.Tables.Add(dt);
            }
            else
            {
                XmlDocument xmlDocument = (XmlDocument)JsonConvert.DeserializeXmlNode(jInput.ToString(), "root");
                //We will add try catch with handling later
                ds.ReadXml(new XmlNodeReader(xmlDocument));
            }

            var yeetDataSet = new YeetDataSetViewModel();
            
            foreach (DataTable dtbl in ds.Tables)
            {
                var yeetTable = new YeetTableViewModel();
                yeetDataSet[dtbl.TableName] = yeetTable;

                foreach (DataColumn dtc in dtbl.Columns)
                {
                    var typeCode = Type.GetTypeCode(dtc.DataType);
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            yeetTable.Columns[dtc.ColumnName] = new YeetBooleanColumnViewModel();
                            break;
                        case TypeCode.String:
                            yeetTable.Columns[dtc.ColumnName] = new YeetStringColumnViewModel();
                            break;
                        case TypeCode.Int32:
                            yeetTable.Columns[dtc.ColumnName] = new YeetIntColumnViewModel();
                            break;
                        case TypeCode.Decimal:
                        case TypeCode.Double:
                            yeetTable.Columns[dtc.ColumnName] = new YeetDoubleColumnViewModel();
                            break;
                        case TypeCode.DateTime:
                            yeetTable.Columns[dtc.ColumnName] = new YeetDateTimeColumnViewModel();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                foreach (DataRow dtr in dtbl.Rows)
                {
                    var yeetRow = new YeetRowViewModel();
                    yeetTable.Rows.AddChild(yeetRow);
                    foreach (DataColumn dtc in dtbl.Columns)
                    {
                        var typeCode = Type.GetTypeCode(dtc.DataType);
                        switch (typeCode)
                        {
                            case TypeCode.Boolean:
                                yeetRow[dtc.ColumnName] = new YeetBooleanCellViewModel(){ Value = (bool)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.String:
                                yeetRow[dtc.ColumnName] = new YeetStringCellViewModel(){ Value = (string)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.Int32:
                                yeetRow[dtc.ColumnName] = new YeetIntCellViewModel(){ Value = (int)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                                yeetRow[dtc.ColumnName] = new YeetDoubleCellViewModel(){ Value = (double)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.DateTime:
                                yeetRow[dtc.ColumnName] = new YeetDateTimeCellViewModel() { Value = (DateTimeOffset)dtr[dtc.ColumnName] };
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }

            return yeetDataSet;
        }

        public static YeetDataViewModel XmlFileToData(string filePath, YeetDataConverterOptions opts = null)
        {
            var fileStream = File.OpenRead(filePath);
            fileStream.Seek(0, SeekOrigin.Begin);
            return YeetDataConverter.XmlStreamToData(fileStream, opts);
        }

        public static YeetDataViewModel XmlStringToData(string xml, YeetDataConverterOptions opts = null)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            return YeetDataConverter.XmlStreamToData(stream, opts);
        }

        public static YeetDataViewModel XmlStreamToData(Stream stream, YeetDataConverterOptions opts = null)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(stream);
            return DataSetToYeetDataSet(ds);
        }

        private static YeetDataSetViewModel DataSetToYeetDataSet(DataSet ds)
        {
            var yeetDataSet = new YeetDataSetViewModel();

            foreach (DataTable dtbl in ds.Tables)
            {
                var yeetTable = new YeetTableViewModel();
                yeetDataSet[dtbl.TableName] = yeetTable;

                foreach (DataColumn dtc in dtbl.Columns)
                {
                    var typeCode = Type.GetTypeCode(dtc.DataType);
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            yeetTable.Columns[dtc.ColumnName] = new YeetBooleanColumnViewModel();
                            break;
                        case TypeCode.String:
                            yeetTable.Columns[dtc.ColumnName] = new YeetStringColumnViewModel();
                            break;
                        case TypeCode.Int32:
                            yeetTable.Columns[dtc.ColumnName] = new YeetIntColumnViewModel();
                            break;
                        case TypeCode.Decimal:
                        case TypeCode.Double:
                            yeetTable.Columns[dtc.ColumnName] = new YeetDoubleColumnViewModel();
                            break;
                        case TypeCode.DateTime:
                            yeetTable.Columns[dtc.ColumnName] = new YeetDateTimeColumnViewModel();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                foreach (DataRow dtr in dtbl.Rows)
                {
                    var yeetRow = new YeetRowViewModel();
                    yeetTable.Rows.AddChild(yeetRow);
                    foreach (DataColumn dtc in dtbl.Columns)
                    {
                        var typeCode = Type.GetTypeCode(dtc.DataType);
                        switch (typeCode)
                        {
                            case TypeCode.Boolean:
                                yeetRow[dtc.ColumnName] = new YeetBooleanCellViewModel() { Value = (bool)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.String:
                                yeetRow[dtc.ColumnName] = new YeetStringCellViewModel() { Value = (string)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.Int32:
                                yeetRow[dtc.ColumnName] = new YeetIntCellViewModel() { Value = (int)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                                yeetRow[dtc.ColumnName] = new YeetDoubleCellViewModel() { Value = (double)dtr[dtc.ColumnName] };
                                break;
                            case TypeCode.DateTime:
                                yeetRow[dtc.ColumnName] = new YeetDateTimeCellViewModel() { Value = (DateTimeOffset)dtr[dtc.ColumnName] };
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }

            return yeetDataSet;
        }

    }
}
