using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YeetOverFlow.Data.Wpf.ViewModels;
using YeetOverFlow.Wpf.Controls;

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

                    switch (ext.ToLower())
                    {
                        case "csv":
                            Data.Root[filename] = YeetDataConverter.CsvFileToData(filePath);
                            break;
                        case "tsv":
                            Data.Root[filename] = YeetDataConverter.CsvFileToData(filePath, new YeetDataConverterOptions() { FieldDelim = '\t' });
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

                switch (tabItem.Header.ToString().ToLower())
                {
                    case "csv":
                        Data.Root[name] = YeetDataConverter.CsvStringToData(text);
                        break;
                    case "tsv":
                        Data.Root[name] = YeetDataConverter.CsvStringToData(text, new YeetDataConverterOptions() { FieldDelim = '\t' });
                        break;
                }
                
                e.Handled = true;
            }
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
            CsvDataReader dataReader = new CsvDataReader(csvReader);

            if (hasHeaders)
            {
                csvReader.ReadHeader();
            }

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

            return tbl;
        }
    }
}
