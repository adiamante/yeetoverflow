using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
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

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = ((string[])e.Data.GetData(DataFormats.FileDrop));
                foreach (var file in files)
                {
                    String filename = Path.GetFileName(file);
                    String ext = Path.GetExtension(file).TrimStart('.');

                    //YeetTableViewModel tbl = new YeetTableViewModel();
                    YeetTableViewModel tbl = new YeetTableViewModel(Guid.NewGuid(), filename);
                    tbl.Init();
                    Data.Root.AddChild(tbl);

                    switch (ext.ToLower())
                    {
                        case "csv":
                            var fieldDelim = ',';
                            var recordDelim = '\n';
                            var hasHeaders = true;
                            CsvConfiguration _conf = new CsvConfiguration(CultureInfo.InvariantCulture);
                            var fs = File.OpenRead(file);
                            var sr = new StreamReader(fs);
                            fs.Seek(0, SeekOrigin.Begin); // <-- missing line

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
                            break;
                    }

                    //Data.Root[filename] = tbl;
                    //Data.Root.AddChild(tbl);
                }

            }
        }
    }
}
