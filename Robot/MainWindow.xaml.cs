using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using OPCAutomation;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Robot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private readonly OpenFileDialog _openFileDialog;    //打开文件
        private readonly SaveFileDialog _saveFileDialog1;   //保存文件
      
        public MainWindow()
        {
            InitializeComponent();
           InitializeCommand();
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.FileOk += OpenFileDialogFileOk;
            _saveFileDialog1 = new SaveFileDialog();
            _saveFileDialog1.FileOk += SaveFileDialog1FileOk;
           // this.Reset();
            Button3.IsEnabled = false;
        }
        OPCServer _kepServer;
        OPCGroups _kepGroups;
        OPCGroup _kepGroup;
        OPCItems _kepItems;
        OPCItem _kepItem; 
        string _strHostIp = "";//主机ip
        string _strHostName = "";//主机名称
        bool _opcConnected;//连接状态
        int _itmHandleClient;//客户端句柄
        int _itmHandleServer;//服务端句柄
      
        private void GetLocalServer()//枚举本地OPC服务器
        {
            //获取本地计算机IP，计算机名称
            //IPHostEntry IPHost = Dns.Resolve(Environment.MachineName);
            IPHostEntry ipHost = Dns.GetHostEntry(Environment.MachineName);
            
            if (ipHost.AddressList.Length > 0)
            {
                _strHostIp = ipHost.AddressList[0].ToString();
            }
            else
            {
                return;
            }
            //通过IP来获取计算机名称，可用在局域网内

            IPHostEntry ipHostEntry = Dns.GetHostEntry(_strHostIp);
            _strHostName = ipHostEntry.HostName;

            //获取本地计算机上的OPCServerName
            try
            {
                _kepServer = new OPCServer();
                object serverList = _kepServer.GetOPCServers(_strHostName);
                foreach (string turn in (Array)serverList)
                {
                    ComboBox1.Items.Add(turn);

                }
                ComboBox1.SelectedIndex = 0;
            
            }
            catch (Exception err)
            {
                MessageBox.Show("枚举本地OPC服务器出错：" + err.Message, "提示信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
      
        //创建组
        private bool CreatGroup()
        {
            try
            {
                _kepGroups = _kepServer.OPCGroups;
                _kepGroup = _kepGroups.Add("OPCDOTNETGROUP");
                String();
                _kepGroup.DataChange += KepGroup_DataChange;
                _kepItems = _kepGroup.OPCItems;
               
            }
            catch (Exception err)
            {
                MessageBox.Show("创建组出现错误：" + err.Message, "提示信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        public void String()                              //设置组属性
        {
            string a;
            string b;
            string c;
            // Window1 m = new Window1();
            var m = new Window1();
            a = m.TextBox1.Text;
            _kepServer.OPCGroups.DefaultGroupIsActive = Convert.ToBoolean(a);
            b = m.TextBox2.Text;
            _kepServer.OPCGroups.DefaultGroupDeadband = Convert.ToInt32(b);
            c = m.TextBox3.Text;
            _kepGroup.UpdateRate = Convert.ToInt32(c);
            var d = m.TextBox4.Text;
            _kepGroup.IsActive = Convert.ToBoolean(d);
            var e = m.TextBox5.Text;
            _kepGroup.IsSubscribed = Convert.ToBoolean(e);
        }

        public void KepGroup_DataChange(int transactionId, int numItems, ref Array clientHandles, ref Array itemValues, ref Array qualities, ref Array timeStamps)
        {
            for (int i = 1; i <= numItems; i++)
            {
                if (clientHandles.GetValue(i).Equals(1000))
                {
                    Label19.Content = itemValues.GetValue(i).ToString();
                    x.Angle = Convert.ToDouble(itemValues.GetValue(i).ToString());
                }
                if (clientHandles.GetValue(i).Equals(1001))
                {
                    Label29.Content = itemValues.GetValue(i).ToString();
                }
                if (clientHandles.GetValue(i).Equals(1002))
                {
                    Label21.Content = itemValues.GetValue(i).ToString();
                    y.Angle = Convert.ToDouble(itemValues.GetValue(i).ToString());
                }
                if (clientHandles.GetValue(i).Equals(1003))
                {
                    Label31.Content = itemValues.GetValue(i).ToString();
                }
                if (clientHandles.GetValue(i).Equals(1004))
                {
                    Label23.Content = itemValues.GetValue(i).ToString();
                    z.Angle = Convert.ToDouble(itemValues.GetValue(i).ToString());
                }
                if (clientHandles.GetValue(i).Equals(1005))
                {
                    Label33.Content = itemValues.GetValue(i).ToString();
                }
                if (clientHandles.GetValue(i).Equals(1006))
                {
                    Label6.Content = itemValues.GetValue(i).ToString();
                    a.Angle = Convert.ToDouble(itemValues.GetValue(i).ToString());
                }
                if (clientHandles.GetValue(i).Equals(1007))
                {
                    Label41.Content = itemValues.GetValue(i).ToString();
                  
                }
                if (clientHandles.GetValue(i).Equals(1008))
                {
                    Label27.Content = itemValues.GetValue(i).ToString();
                    b.Angle = Convert.ToDouble(itemValues.GetValue(i).ToString());
                }
                if (clientHandles.GetValue(i).Equals(1009))
                {
                    Label38.Content = itemValues.GetValue(i).ToString();
                }
                if (clientHandles.GetValue(i).Equals(1010))
                {
                    Label25.Content = itemValues.GetValue(i).ToString();
                    c.Angle = Convert.ToDouble(itemValues.GetValue(i).ToString());
                }
                if (clientHandles.GetValue(i).Equals(1011))
                {
                    Label36.Content = itemValues.GetValue(i).ToString();
                }
                if (clientHandles.GetValue(i).Equals(1012))
                {
                    Label44.Content = itemValues.GetValue(i).ToString();
                }
                if (clientHandles.GetValue(i).Equals(1013))
                {
                    Label46.Content = itemValues.GetValue(i).ToString();
                }
                if (clientHandles.GetValue(i).Equals(1014))
                {
                    Label48.Content = itemValues.GetValue(i).ToString();
                }
                if (!clientHandles.GetValue(i).Equals(1234)) continue;
                TextBox3.Text = TextBox2.Text == "" ? "" : itemValues.GetValue(i).ToString();
            }
        }
        void Read(string u1,int u2)                                            //读方法
        {
            _kepItem = _kepItems.AddItem(u1, u2);
            _itmHandleServer = _kepItem.ServerHandle;
            //OPCItem bItem = _kepItems.GetOPCItem(_itmHandleServer);
            var bItem = _kepItems.GetOPCItem(_itmHandleServer);
            var temp = new[] { 0, bItem.ServerHandle };
            //Array serverHandles = (Array)temp; 
            var serverHandles = (Array)temp;
            Array errors;
            int cancelId;
            _kepGroup.AsyncRead(1, ref serverHandles, out errors, 2009, out cancelId);
            GC.Collect();  
        }
        private bool ConnectRemoteServer(string remoteServerIp, string remoteServerName)
        {
            try
            {
                _kepServer.Connect(remoteServerName, remoteServerIp);
                if (_kepServer.ServerState == (int)OPCServerState.OPCRunning)
                {
                    TextBox1.Text = "已连接到-" + _kepServer.ServerName + "   ";
                }
                else
                {
                    TextBox1.Text = "状态：" + _kepServer.ServerState.ToString() + "   ";
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("连接远程服务器出现错误：" + err.Message, "提示信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)   //运行窗口时处理的事情
        {
            GetLocalServer();
        }

        private void Window_Closing(object sender, CancelEventArgs e)  //关闭窗体时处理的事情
        {

            MessageBoxResult key = MessageBox.Show(
             "确定退出吗",
            "确定",
            MessageBoxButton.YesNo,
             MessageBoxImage.Question,
             MessageBoxResult.No);
            e.Cancel = (key == MessageBoxResult.No);
            if (!_opcConnected)
            {
                return;
            }

            if (_kepGroup != null)
            {
                _kepGroup.DataChange -= KepGroup_DataChange;
            }

            if (_kepServer != null)
            {
                _kepServer.Disconnect();
                _kepServer = null;
            }
            
            _opcConnected = false;
         
            Application.Current.Shutdown();
        }
        private void RecurBrowse(OPCBrowser oPcBrowser)
        {
            //展开分支
            oPcBrowser.ShowBranches();
            //展开叶子
            oPcBrowser.ShowLeafs(true);
            foreach (object turn in oPcBrowser)
            {
                ListBox1.Items.Add(turn.ToString());
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)          //连接
        {
            Button1.Background = new SolidColorBrush(Color.FromRgb(0,204,0));
            try
            {
                if (!ConnectRemoteServer("" ,ComboBox1.Text))
               
                {
                    return;
                }
                _opcConnected = true;
                RecurBrowse(_kepServer.CreateBrowser());
                Button3.IsEnabled = true;
                if (!CreatGroup())
                {
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("初始化出错：" + err.Message, "提示信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
          
        }
       
        private void button3_Click(object sender, RoutedEventArgs e)              //设置
        {
            Hide();
            Window1 m = new Window1();
            m.ShowDialog();
        }
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)  //列表
        {
            try
            {
                if (_itmHandleClient != 0)
                {
                    TextBox3.Text = "";
                    Array errors;
                    OPCItem bItem = _kepItems.GetOPCItem(_itmHandleServer);
                    //注： OPC中以1为数组的基数
                    int[] temp = new int[] { 0, bItem.ServerHandle };
                    Array serverHandle = temp;
                    //移除上一次选择的项
                    _kepItems.Remove(_kepItems.Count, ref serverHandle, out errors);

                }
                _itmHandleClient = 1234;
                _kepItem = _kepItems.AddItem(ListBox1.SelectedItem.ToString(), _itmHandleClient);
                TextBox2.Text = Convert.ToString(ListBox1.SelectedItem);
                _itmHandleServer = _kepItem.ServerHandle;
            }
            catch (Exception err)
            {
                //没有任何权限的项，都是OPC服务器保留的系统项，此处可不做处理
                _itmHandleClient = 0;
                TextBox3.Text = "Error ox";
                MessageBox.Show("此项为系统保留项：" + err.Message, "提示信息");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)           //断开
        {
           
            _kepServer.Disconnect();
            TextBox1.Text = "断开";
            ListBox1.Items.Clear();
            Reset1();
            Button2.Background = new SolidColorBrush(Color.FromRgb(255,255,0));
           
        }
        public void Reset1()
        { 
            TextBox2.Text = string.Empty;
            TextBox3.Text = string.Empty;
            TextBox6.Text = string.Empty;
            TextBox7.Text = string.Empty;
            TextBox8.Text = string.Empty;
            TextBox9.Text = string.Empty;
            TextBox10.Text = string.Empty;
            TextBox11.Text = string.Empty;
            TextBox12.Text = string.Empty;
            TextBox13.Text = string.Empty;
            TextBox14.Text = string.Empty;
            TextBox15.Text = string.Empty;
            Label19.Content = string.Empty;
            Label21.Content = string.Empty;
            Label23.Content = string.Empty;
            Label25.Content = string.Empty;
            Label27.Content = string.Empty;
            Label29.Content = string.Empty;
            Label31.Content = string.Empty;
            Label33.Content = string.Empty;
            Label36.Content = string.Empty;
            Label38.Content = string.Empty;
            Label6.Content = string.Empty;
            Label41.Content = string.Empty;
            textBox4.Text = string.Empty;
            Label44.Content = string.Empty;
            Label46.Content = string.Empty;
            Label48.Content = string.Empty;
        }

        private void button5_Click(object sender, RoutedEventArgs e)                     //写值
        {
            Write(TextBox2.Text, TextBox6.Text);
        }

        private void button4_Click_1(object sender, RoutedEventArgs e)                   //读值
        {
            if (TextBox2.Text!=null)
            {
                Read(TextBox2.Text, 1234);
            }
        }
        public void Write(string m1,string m2)                                            // 写函数
        {
            _itmHandleClient = 1234;
            _kepItem = _kepItems.AddItem(m1, _itmHandleClient);
            _itmHandleServer = _kepItem.ServerHandle;
            OPCItem bItem = _kepItems.GetOPCItem(_itmHandleServer);
            int[] temp = new int[] { 0, bItem.ServerHandle };
            Array serverHandles = temp;
            object[] valueTemp = new object[] { "", m2 };
            Array values = valueTemp;
            Array errors;
            int cancelId;
            _kepGroup.AsyncWrite(1, ref serverHandles, ref values, out errors, 2009, out cancelId);
            GC.Collect();
        }
      
        private void button6_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)  //X+
        {
            
            Write("PLC1.Application.XAXIS.mcj1.Velocity", TextBox7.Text);
            Write("PLC1.Application.XAXIS.mcj1.JogForward", "true");
        }
       
        
        private void button6_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)   //
        {
            
            Write("PLC1.Application.XAXIS.mcj1.JogForward", "false"); 
        }

        private void button7_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)  //X-
        {
            Write("PLC1.Application.XAXIS.mcj1.Velocity", TextBox7.Text);
            Write("PLC1.Application.XAXIS.mcj1.JogBackward", "true");
        }

        private void button7_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
         
            Write("PLC1.Application.XAXIS.mcj1.JogBackward", "false");
        }

        private void button8_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Y+
        {
            Write("PLC1.Application.YAXIS.mcj2.Velocity", TextBox8.Text);
            Write("PLC1.Application.YAXIS.mcj2.JogForward", "true");
        }

        private void button8_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
         
            Write("PLC1.Application.YAXIS.mcj2.JogForward", "false");
        }

        private void button9_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Y-
        {
            Write("PLC1.Application.YAXIS.mcj2.Velocity", TextBox8.Text);
            Write("PLC1.Application.YAXIS.mcj2.JogBackward", "true");
        }

        private void button9_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            Write("PLC1.Application.YAXIS.mcj2.JogBackward", "false");
        }

        private void button10_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)// Z+
        {
            Write("PLC1.Application.ZAXIS.mcj3.Velocity", TextBox9.Text);
            Write("PLC1.Application.ZAXIS.mcj3.JogForward", "true");
        }

        private void button10_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
          
            Write("PLC1.Application.ZAXIS.mcj3.JogForward", "false");
        }

        private void button11_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)//Z-
        {
            Write("PLC1.Application.ZAXIS.mcj3.Velocity", TextBox9.Text);
            Write("PLC1.Application.ZAXIS.mcj3.JogBackward", "true");
        }

        private void button11_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
         
            Write("PLC1.Application.ZAXIS.mcj3.JogBackward", "false");
        }

        private void button12_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)//C+
        {
            Write("PLC1.Application.CAXIS.mcj4.Velocity", TextBox10.Text);
            Write("PLC1.Application.CAXIS.mcj4.JogForward", "true");
        }

        private void button12_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
          
            Write("PLC1.Application.CAXIS.mcj4.JogForward", "false");
        }

        private void button13_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)//C-
        {
            Write("PLC1.Application.CAXIS.mcj4.Velocity", TextBox10.Text);
            Write("PLC1.Application.CAXIS.mcj4.JogBackward", "true");
        }

        private void button13_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
          
            Write("PLC1.Application.CAXIS.mcj4.JogBackward", "false");
        }

        private void button14_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)//B+
        {
            Write("PLC1.Application.BAXIS.mcj5.Velocity", TextBox11.Text);
            Write("PLC1.Application.BAXIS.mcj5.JogForward", "true");
        }

        private void button14_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
         
            Write("PLC1.Application.BAXIS.mcj5.JogForward", "false");
        }

        private void button15_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)   //B-
        {
            Write("PLC1.Application.BAXIS.mcj5.Velocity", TextBox11.Text);
            Write("PLC1.Application.BAXIS.mcj5.JogBackward", "true");
        }

        private void button15_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            Write("PLC1.Application.BAXIS.mcj5.JogBackward", "false");
        }
        
        private void button16_Click(object sender, RoutedEventArgs e)                             //使能开
        {
            
            Write("PLC1.Application.XAXIS.mcp1.bRegulatorOn", "true");
            Write("PLC1.Application.YAXIS.mcp2.bRegulatorOn", "true");
            Write("PLC1.Application.ZAXIS.mcp3.bRegulatorOn", "true");
            Write("PLC1.Application.CAXIS.mcp4.bRegulatorOn", "true");
            Write("PLC1.Application.BAXIS.mcp5.bRegulatorOn", "true");
            Write("PLC1.Application.AAXIS.mcp6.bRegulatorOn", "true");
            Read("PLC1.Application.XAXIS.mcp01", 1000);
            Read("PLC1.Application.XAXIS.mcv01", 1001);
            Read("PLC1.Application.YAXIS.mcp02", 1002);
            Read("PLC1.Application.YAXIS.mcv02", 1003);
            Read("PLC1.Application.ZAXIS.mcp03", 1004);
            Read("PLC1.Application.ZAXIS.mcv03", 1005);
            Read("PLC1.Application.AAXIS.mcp06", 1006);
            Read("PLC1.Application.AAXIS.mcv06", 1007);
            Read("PLC1.Application.BAXIS.mcp05", 1008);
            Read("PLC1.Application.BAXIS.mcv05", 1009);
            Read("PLC1.Application.CAXIS.mcp04", 1010);
            Read("PLC1.Application.CAXIS.mcv04", 1011);
            Read("PLC1.Application.READGCODE.m1.dX", 1012);
            Read("PLC1.Application.READGCODE.m1.dY", 1013);
            Read("PLC1.Application.READGCODE.m1.dZ", 1014);
 
        }

        private void button17_Click(object sender, RoutedEventArgs e)                            //打开文件
        {
            _openFileDialog.ShowDialog();
        }

        private void OpenFileDialogFileOk(object sender, CancelEventArgs e)
        {
            string fullPathname = _openFileDialog.FileName;
            FileInfo src = new FileInfo(fullPathname);
            TextBox12.Text = src.FullName;
            TextReader reader = src.OpenText();
            DisplayData(reader);
        }
        private void DisplayData(TextReader reader)
        {
            TextBox14.Text = "";
            string line = reader.ReadLine();
            while (line != null)
            {
                TextBox14.Text += line + '\n';
                line = reader.ReadLine();
            }
            reader.Close();
        }
        
        private void button18_Click(object sender, RoutedEventArgs e)                              // 保存文件
        {
            _saveFileDialog1.ShowDialog();
        }
        private void SaveFileDialog1FileOk(object sender, CancelEventArgs e)
        {
            string fullPathname = _saveFileDialog1.FileName;
            FileInfo src = new FileInfo(fullPathname);
            TextBox13.Text = src.FullName;
            try
            {
                Stream stream = File.OpenWrite(TextBox13.Text);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(TextBox14.Text);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("不能保存");
            }
        }
        
        //private void button20_Click(object sender, RoutedEventArgs e)                                 //清屏
        //{
        //    this.Reset();
        //}
        //public void Reset()
        //{ textBox14.Text = string.Empty; }

        private readonly RoutedCommand _clearCmd = new RoutedCommand("Clear", typeof(MainWindow));        //清屏
        private void InitializeCommand()
        {
            button20.Command = _clearCmd;
            _clearCmd.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Alt));
            button20.CommandTarget = TextBox14;
            CommandBinding cb = new CommandBinding {Command = _clearCmd};
            cb.CanExecute += cb_CanExecute;
            cb.Executed += cb_Executed;
            Grid7.CommandBindings.Add(cb);

        }
        void cb_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(textBox14.Text))
            //{ e.CanExecute = false; }
            //else
            //{ e.CanExecute = true; }
            //e.Handled = true;
            e.CanExecute = !string.IsNullOrEmpty(TextBox14.Text);
            e.Handled = true;
        }

        void cb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox14.Clear();
            e.Handled = true;
        }

        private void button21_Click(object sender, RoutedEventArgs e)                                 //开始
        {
            if (TextBox15.Text == "")
            {
            }
            else
            {
                Write("PLC1.Application.READGCODE.smcr1.sFileName", TextBox15.Text);
                Write("PLC1.Application.READGCODE.smcr1.bExecute", "true");
            }
        }

        private void button22_Click(object sender, RoutedEventArgs e)                                 //使能关
        {
            Write("PLC1.Application.XAXIS.mcp1.bRegulatorOn", "false");
            Write("PLC1.Application.YAXIS.mcp2.bRegulatorOn", "false");
            Write("PLC1.Application.ZAXIS.mcp3.bRegulatorOn", "false");
            Write("PLC1.Application.CAXIS.mcp4.bRegulatorOn", "false");
            Write("PLC1.Application.BAXIS.mcp5.bRegulatorOn", "false");
            Reset2();
           
        }
        public void Reset2()
        {
            Label19.Content = string.Empty;
            Label21.Content = string.Empty;
            Label23.Content = string.Empty;
            Label25.Content = string.Empty;
            Label27.Content = string.Empty;
            Label29.Content = string.Empty;
            Label31.Content = string.Empty;
            Label33.Content = string.Empty;
            Label36.Content = string.Empty;
            Label38.Content = string.Empty;
            Label6.Content = string.Empty;
            Label41.Content = string.Empty;
            textBox4.Text = string.Empty;
            Label44.Content = string.Empty;
            Label46.Content = string.Empty;
            Label48.Content = string.Empty;
        }

        private void button23_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Write("PLC1.Application.AAXIS.mcj6.Velocity", textBox4.Text);
            Write("PLC1.Application.AAXIS.mcj6.JogForward", "true");
        }

        private void button23_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Write("PLC1.Application.AAXIS.mcj6.Velocity", textBox4.Text);
            Write("PLC1.Application.AAXIS.mcj6.JogForward", "false");
        }

        private void button24_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Write("PLC1.Application.AAXIS.mcj6.Velocity", textBox4.Text);
            Write("PLC1.Application.AAXIS.mcj6.JogBackward", "true");
        }

        private void button24_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Write("PLC1.Application.AAXIS.mcj6.Velocity", textBox4.Text);
            Write("PLC1.Application.AAXIS.mcj6.JogBackward", "false");
        }
       
     


        
      
        

    }
}
