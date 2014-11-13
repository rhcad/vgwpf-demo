using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using touchvg.view;
using democmds.api;
using Newtonsoft.Json.Linq;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            this.Unloaded += new RoutedEventHandler(Window1_Unloaded);
        }
        string[] _commands = new string[] {
            "选择",     "select",
            "删除",      "erase",
            "随手画",    "splines",
            "直线段",    "line",
            "矩形",      "rect",
            "正方形",    "square",
            "椭圆",      "ellipse",
            "圆",        "circle",
            "三角形",    "triangle",
            "菱形",      "diamond",
            "多边形",    "polygon",
            "四边形",    "quadrangle",
            "折线",      "lines",
            "曲线",      "spline_mouse",
            "平行四边形", "parallel",
            "网格",      "grid",
            "三点圆弧",  "arc3p",
            "圆心圆弧",  "arc_cse",
            "切线圆弧",  "arc_tan",
            "点击测试(in DemoCmds)",  "hittest"
        };
        string[] _lineStyles = new string[] {
            "实线",     "0",
            "虚线",     "1",
            "点线",     "2",
            "点划线",   "3",
            "双点划线", "4",
        };

        private WPFGraphView _view;
        private WPFViewHelper _helper;
        private bool _updateLocked = false;

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            _view = new WPFGraphView(canvas1);
            _view.OnCommandChanged += new CommandChangedEventHandler(View_OnCommandChanged);
            _view.OnSelectionChanged += new touchvg.view.SelectionChangedEventHandler(View_OnSelectionChanged);
            _view.ShowMessageHandler = View_ShowMessage;

            List<KeyValuePair<string, string>> commandSource = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < _commands.Length; i += 2)
            {
                commandSource.Add(new KeyValuePair<string, string>(_commands[i], _commands[i + 1]));
            }
            this.cboCmd.DisplayMemberPath = "Key";
            this.cboCmd.SelectedValuePath = "Value";
            this.cboCmd.ItemsSource = commandSource;
            this.cboCmd.SelectedIndex = 0;

            List<KeyValuePair<string, string>> lineStyleSource = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < _lineStyles.Length; i += 2)
            {
                lineStyleSource.Add(new KeyValuePair<string, string>(_lineStyles[i], _lineStyles[i + 1]));
            }
            this.cboLineStyle.DisplayMemberPath = "Key";
            this.cboLineStyle.SelectedValuePath = "Value";
            this.cboLineStyle.ItemsSource = lineStyleSource;
            this.cboLineStyle.SelectedIndex = 0;

            _helper = new WPFViewHelper(_view);
            DemoCmds.registerCmds(_helper.CmdViewHandle());
            _helper.Load("C:\\Test\\page.vg");
            _helper.StartUndoRecord("C:\\Test\\undo");
            _helper.Command = "select";

            JObject dictForTest = OptionsHelper.GetOptions(_helper);
            OptionsHelper.SetOptions(_helper, dictForTest);
        }

        void Window1_Unloaded(object sender, RoutedEventArgs e)
        {
            _helper.Dispose();
        }

        private void cboCmd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_helper != null && !_updateLocked)
            {
                _helper.Command = cboCmd.SelectedValue.ToString();
            }
        }

        void View_OnCommandChanged(object sender, EventArgs e)
        {
            string cmdname = cboCmd.SelectedValue.ToString();
            if (!_helper.Command.Equals(cmdname))
            {
                _updateLocked = true;
                cboCmd.SelectedValue = _helper.Command;
                _updateLocked = false;
            }
            View_OnSelectionChanged(sender, e);
        }

        private void cboLineStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_helper != null && !_updateLocked)
            {
                _helper.LineStyle = int.Parse(cboLineStyle.SelectedValue.ToString());
            }
        }

        void View_OnSelectionChanged(object sender, EventArgs e)
        {
            if (!_updateLocked)
            {
                _updateLocked = true;
                alphaSlider.Value = (double)_helper.LineAlpha;
                widthSlider.Value = (double)_helper.LineWidth;
                cboLineStyle.SelectedValue = _helper.LineStyle.ToString();
                _updateLocked = false;
            }
        }

        private void alphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_helper != null && !_updateLocked)
                _helper.LineAlpha = (int)(sender as Slider).Value;
        }

        private void widthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_helper != null && !_updateLocked)
                _helper.LineWidth = (int)(sender as Slider).Value;
        }

        private void redBtn_Click(object sender, RoutedEventArgs e)
        {
            _helper.LineColor = Colors.Red;
        }

        private void blueBtn_Click(object sender, RoutedEventArgs e)
        {
            _helper.LineColor = Colors.Blue;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            _helper.Save("C:\\Test\\page.vg");
            _helper.ExportSVG("C:\\Test\\page.svg");
            _helper.ExportPNG("C:\\Test\\page.---");
        }

        private void undoBtn_Click(object sender, RoutedEventArgs e)
        {
            _helper.Undo();
        }

        private void redoBtn_Click(object sender, RoutedEventArgs e)
        {
            _helper.Redo();
        }

        private void zoomExtentBtn_Click(object sender, RoutedEventArgs e)
        {
            _helper.ZoomToExtent();
        }

        private class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;

            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        private void View_ShowMessage(string text)
        {
            AutoClosingMessageBox.Show(text, "Message", 400);
        }
    }
}
