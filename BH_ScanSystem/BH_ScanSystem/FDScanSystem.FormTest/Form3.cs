using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Automation.BDaq;

namespace FDScanSystem.FormTest
{
    public partial class Form3 : Form
    {
        #region fields

        SimpleGraph m_simpleGraph;
        ListViewItem m_firstItem = new ListViewItem();
        ListViewItem m_secondItem = new ListViewItem();

        public const int CHANNEL_COUNT_MAX = 16;
        static double[] m_dataScaled = new double[CHANNEL_COUNT_MAX];
        static int chanCountSet = 16;
        static int startChannel = 0;

        double YMax;
        double YMin;

        int DataInterval = 2 * 60 * 1000;
        DateTime now = DateTime.Now;

        static int IsAppend = 1;

        int IsStart = 1;
        System.Threading.Timer _dataTimer = null;
        System.Windows.Forms.Timer _chartTimer = null;
        BufferedAiCtrl bufferedAiCtrl = null;
        #endregion

        public Form3()
        {
            InitializeComponent();
            YMax = Convert.ToDouble(ConfigurationManager.AppSettings["YMax"]);
            YMin = Convert.ToDouble(ConfigurationManager.AppSettings["YMin"]);
            chanCountSet = Convert.ToInt32(ConfigurationManager.AppSettings["ChannelCount"]);
            startChannel = Convert.ToInt32(ConfigurationManager.AppSettings["StartChannel"]);
            string deviceConnection = ConfigurationManager.AppSettings["Device"];
            int scanTime = Convert.ToInt32(ConfigurationManager.AppSettings["ScanInterval"]);
            int scanLength = Convert.ToInt32(ConfigurationManager.AppSettings["ScanLength"]);
            int convertClkRate = scanTime;
            bufferedAiCtrl = new BufferedAiCtrl();
            bufferedAiCtrl.Streaming = true;
            bufferedAiCtrl.DataReady += new EventHandler<BfdAiEventArgs>(bufferedAiCtrl_DataReady);
            bufferedAiCtrl.Overrun += new EventHandler<BfdAiEventArgs>(bufferedAiCtrl_Overrun);
            bufferedAiCtrl.SelectedDevice = new DeviceInformation(deviceConnection);
            _dataTimer = new System.Threading.Timer(new System.Threading.TimerCallback(DataTimer_Callback));
            DataInterval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]) * 60 * 1000;

            this.Disposed += new EventHandler(Form1_Disposed);
            _dataTimer.Change(DataInterval, DataInterval);

            ThreadPool.QueueUserWorkItem(new WaitCallback(_chartTimer_Tick));


            ScanChannel scanChannel = bufferedAiCtrl.ScanChannel;
            scanChannel.ChannelStart = startChannel;
            scanChannel.ChannelCount = chanCountSet;
            scanChannel.IntervalCount = scanTime * scanLength;
            scanChannel.Samples = scanChannel.IntervalCount * 2;
            bufferedAiCtrl.ConvertClock.Rate = convertClkRate;

            // Step 5: prepare the buffered AI. 
            ErrorCode errorCode = bufferedAiCtrl.Prepare();
            if (BioFailed(errorCode))
            {
                throw new Exception();
            }

            errorCode = bufferedAiCtrl.Start();
            if (BioFailed(errorCode))
            {
                throw new Exception();
            }

        }

        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }

        void bufferedAiCtrl_Overrun(object sender, BfdAiEventArgs e)
        {
            Console.WriteLine("Over");
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bufferedAiCtrl_DataReady(object sender, BfdAiEventArgs e)
        {
            if (IsStart == 1)
            {
                BufferedAiCtrl bufferedAiCtrl = (BufferedAiCtrl)sender;
                ScanChannel scanChannel = bufferedAiCtrl.ScanChannel;
                int channelCountMax = bufferedAiCtrl.Features.ChannelCountMax;
                int startChan = scanChannel.ChannelStart;
                int channelCount = scanChannel.ChannelCount;
                int scanTime = Convert.ToInt32(ConfigurationManager.AppSettings["ScanInterval"]);
                if (e.Count > channelCount)
                {
                    var dataTemp = new double[e.Count];
                    m_dataScaled = new double[channelCount];
                    bufferedAiCtrl.GetData(e.Count, dataTemp);
                    Array.Copy(dataTemp, 0, m_dataScaled, 0, 8);

                    //Array.Copy(m_dataScaled, temp, m_dataScaled.Length);
                    DateTime now2 = DateTime.Now;
                    if (IsAppend == 1)
                    {
                        bool isInsert = true;
                        for (int i = 0; i < e.Count; )
                        {
                            double[] temp = new double[m_dataScaled.Length];
                            for (int j = 0; j < channelCount; j++)
                            {

                                temp[j] = dataTemp[i];

                                i++;
                            }

                            if (isInsert)
                            {
                                DataManager.Append(new QueueData
                                {
                                    Data = temp,
                                    Time = now
                                });
                                isInsert = false;
                            }
                            else
                            {
                                DataManager.Append(new QueueData
                                {
                                    Data = temp,
                                    Time = now2
                                });
                            }
                        }

                    }

                    bufferedAiCtrl.Stop();
                }

                DataManager.Callback(null);
            }
        }

        void _chartTimer_Tick(object sender)
        {


        }

        void Form1_Disposed(object sender, EventArgs e)
        {
            IsStart = 0;
            bufferedAiCtrl.Stop();
            bufferedAiCtrl.Dispose();
        }

        public Form3(int deviceNumber)
        {

            InitializeComponent();

        }

        private void DataTimer_Callback(object statue)
        {
            try
            {
                int scanLength = Convert.ToInt32(ConfigurationManager.AppSettings["ScanLength"]);
                System.Threading.Interlocked.Exchange(ref IsAppend, 1);
                bufferedAiCtrl.Stop();
                bufferedAiCtrl.Start();
                now = DateTime.Now;
                while (scanLength > 0)
                {

                    Thread.Sleep(1000);
                    scanLength--;
                }

                System.Threading.Interlocked.Exchange(ref IsAppend, 0);

            }
            catch
            {

            }
            finally
            {
                _dataTimer.Change(DataInterval, DataInterval);
            }
        }

        private void InstantAiForm_Load(object sender, EventArgs e)
        {
            //The default device of project is demo device, users can choose other devices according to their needs. 
            //if (!instantAiCtrl1.Initialized)
            //{
            //    MessageBox.Show("No device be selected or device open failed!", "AI_InstantAI");
            //    this.Close();
            //    return;
            //}

            //set title of the form.

        }

        private void ConfigureGraph()
        {
            m_simpleGraph.XCordTimeDiv = 1000;
            string[] X_rangeLabels = new string[2];
            Helpers.GetXCordRangeLabels(X_rangeLabels, 10, 0, TimeUnit.Second);
            label_XCoordinateMax.Text = X_rangeLabels[0];
            label_XCoordinateMin.Text = X_rangeLabels[1];

            ValueUnit unit = (ValueUnit)(-1); // Don't show unit in the label.
            string[] Y_CordLables = new string[3];
            Helpers.GetYCordRangeLabels(Y_CordLables, YMax, YMin, unit);
            label_YCoordinateMax.Text = Y_CordLables[0];
            label_YCoordinateMin.Text = Y_CordLables[1];
            label_YCoordinateMiddle.Text = Y_CordLables[2];

            m_simpleGraph.YCordRangeMax = YMax;// Convert.ToDouble(YMax);
            m_simpleGraph.YCordRangeMin = YMin;// Convert.ToDouble(this.txtYMin.Text);
            m_simpleGraph.Clear();
        }

        private void timer_getData_Tick(object sender, EventArgs e)
        {
            int scanTime = Convert.ToInt32(ConfigurationManager.AppSettings["ScanInterval"]);
            m_simpleGraph.Chart(m_dataScaled, chanCountSet, 1, 1.0 * scanTime / 1000);
            RefreshListView();
            timer_getData.Interval = scanTime;

        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            //int scanTime = Convert.ToInt32(ConfigurationManager.AppSettings["ScanInterval"]);
            //m_simpleGraph.Clear();
            //timer_getData.Interval = trackBar.Value;
            //textBox.Text = trackBar.Value.ToString();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            //instantAiCtrl1.SelectedDevice = new DeviceInformation(this.txtName.Text);
            //timer_getData.Start();
            //// chanCountSet = chanCountSet; Convert.ToInt32(this.txtChannel.Text);
            //DataManager.Start();
            //button_start.Enabled = false;
            //button_pause.Enabled = true;
            //button_stop.Enabled = true;
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            //timer_getData.Stop();
            //DataManager.Stop();
            //button_start.Enabled = true;
            //button_pause.Enabled = false;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            //timer_getData.Stop();
            //button_start.Enabled = true;
            //button_stop.Enabled = false;
            //button_pause.Enabled = false;
            //Array.Clear(m_dataScaled, 0, chanCountSet);
            //m_simpleGraph.Clear();
        }

        private void InitListView()
        {
            //control list view ,one grid indicates a channel which specials with color and value.
            listView.Clear();
            listView.FullRowSelect = false;
            listView.Width = 578;
            listView.Height = 53;
            listView.View = View.Details;// Set the view to show details.
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.GridLines = true;
            // there are 8 columns for every item.
            for (int i = 0; i < 8; i++)
            {
                listView.Columns.Add("", 71);
            }

            // modify the grid's height with image Indirectly.
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 21);//width and height.
            listView.SmallImageList = imgList; //use imgList to modify the height of listView grids.

            // create two ListViewItem objects,so there are 16 grids for listView.
            m_firstItem = new ListViewItem();
            m_firstItem.SubItems.Clear();
            m_firstItem.UseItemStyleForSubItems = false;
            m_firstItem.Font = new Font("SimSun", 10);
            m_firstItem.SubItems[0].Text = String.Format("{0:0.0000}", m_dataScaled[0]);
            m_firstItem.SubItems[0].BackColor = m_simpleGraph.Pens[0].Color;
            for (int i = 1; i < 8; i++)
            {
                if (i < chanCountSet)
                {
                    m_firstItem.SubItems.Add((String.Format("{0:0.0000}", m_dataScaled[i])), Color.Black, Color.Honeydew, new Font("SimSun", 10));
                    m_firstItem.SubItems[i].BackColor = m_simpleGraph.Pens[i].Color;

                }
                else
                {
                    m_firstItem.SubItems.Add("");
                    m_firstItem.SubItems[i].BackColor = Color.White;
                }
            }

            m_secondItem = new ListViewItem();
            m_secondItem.SubItems.Clear();
            m_secondItem.Font = new Font("SimSun", 10);
            m_secondItem.UseItemStyleForSubItems = false;
            if (8 < chanCountSet)
            {
                m_secondItem.SubItems[0].Text = String.Format("{0:0.0000}", m_dataScaled[8]);
                m_secondItem.SubItems[0].BackColor = m_simpleGraph.Pens[8].Color;
            }
            else
            {
                m_secondItem.SubItems[0].Text = "";
                m_secondItem.SubItems[0].BackColor = Color.White;
            }
            for (int i = 9; i < 16; i++)
            {
                if (i < chanCountSet)
                {
                    m_secondItem.SubItems.Add((String.Format("{0:0.0000}", m_dataScaled[i])), Color.Black, Color.Honeydew, new Font("SimSun", 10));
                    m_secondItem.SubItems[i - 8].BackColor = m_simpleGraph.Pens[i].Color;
                }
                else
                {
                    m_secondItem.SubItems.Add("");
                    m_secondItem.SubItems[i - 8].BackColor = Color.White;
                }
            }

            ListViewItem[] list = new ListViewItem[] { m_firstItem, m_secondItem };
            listView.Items.AddRange(list);
        }

        private void RefreshListView()
        {
            for (int i = 0; i < chanCountSet; i++)
            {
                if (i < 8)
                {
                    m_firstItem.SubItems[i].Text = String.Format("{0:0.0000}", m_dataScaled[i]);
                }
                else
                {
                    m_secondItem.SubItems[i % 8].Text = String.Format("{0:0.0000}", m_dataScaled[i]);
                }
            }
        }

        private void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! some errors happened, the error code is: " + err.ToString(), "AI_InstantAI");
            }
        }

        private void comboBox_chanCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            chanCountSet = 16;//comboBox_chanCount.SelectedIndex + 1;
            InitListView();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer_getData_Tick_1(object sender, EventArgs e)
        {
            int scanTime = Convert.ToInt32(ConfigurationManager.AppSettings["ScanInterval"]);
            m_simpleGraph.Chart(m_dataScaled, chanCountSet, 1, 1.0 * scanTime / 1000);
            RefreshListView();
            timer_getData.Interval = scanTime;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.Text = "Instant AI(" + this.bufferedAiCtrl.SelectedDevice.Description + ")";

            //button_start.Enabled = true;
            //button_stop.Enabled = false;
            //button_pause.Enabled = false;

            //initialize a graph with a picture box control to draw Ai data. 
            m_simpleGraph = new SimpleGraph(pictureBox.Size, pictureBox);
            //initialize a timer which drive the data acquisition.
            timer_getData.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["ScanInterval"]); //trackBar.Value;

            //textBox.ReadOnly = true;
            //  textBox.Text = trackBar.Value.ToString();

            //Add combo Box  to select start channel and channel count
            int chanCount = (bufferedAiCtrl.ChannelCount <= CHANNEL_COUNT_MAX) ? bufferedAiCtrl.ChannelCount : CHANNEL_COUNT_MAX;


            //int count = 0;
            //for (int i = 0; i < chanCount; ++i)
            //{
            //   comboBox_chanStart.Items.Add(i.ToString());
            //   count = i + 1;
            //   comboBox_chanCount.Items.Add(count.ToString());
            //}

            //comboBox_chanStart.SelectedIndex = 0;
            //comboBox_chanCount.SelectedIndex = 2;

            ConfigureGraph();
            InitListView();


            timer_getData.Start();
            DataManager.Start();
        }


    }
}