using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace VecUnitsConverter
{
    public partial class Form1 : Form
    {
        List<UnitType> AllUnitsJson;
        string UserArgs;
        public Form1(string strFullArgs)
        {
            UserArgs=strFullArgs;
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSelText = listBox1.GetItemText(listBox1.SelectedItem);
            grpUnits.Text= strSelText;
            LoadUnitsIntoForm(strSelText);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            AllUnitsJson=LoadJsons();
            foreach (var Unit in AllUnitsJson)
            {
                string UnitTName=Unit.UnitTypeName;
                listBox1.Items.Add(UnitTName);
            }
            if (UserArgs != "")
            {
                SearchBox.Text = UserArgs;
                bool UnitPos = IsContainsUnit(UserArgs);
            }
            else
            {
                string FirstUnitTypeName = AllUnitsJson[0].UnitTypeName;
                LoadUnitsIntoForm(FirstUnitTypeName);
                grpUnits.Text = FirstUnitTypeName;
            }


        }
        private void LoadUnitsIntoForm(string UnitTypeIdxName)
        {
            foreach (var vUnit in AllUnitsJson)
            {
                string UnitTName = vUnit.UnitTypeName;
                if(UnitTName== UnitTypeIdxName)
                {
                    var UnitsInThisType = vUnit.Units;
                    int i = 0;
                    foreach (var sUnit in UnitsInThisType)
                    {
                        i =i + 1;
                        GroupBox gBox = this.grpUnits.Controls["groupBox" + i.ToString()] as GroupBox;
                        gBox.Text = sUnit.UnitFullName;
                        gBox.Visible = true;
                        Label lblCtl = gBox.Controls["label" + i.ToString()] as Label;
                        lblCtl.Text = sUnit.UnitShortName;
                        TextBox txtCtl = gBox.Controls["textBox" + i.ToString()] as TextBox;
                        txtCtl.Text = "";
                        gBox.ForeColor = System.Drawing.SystemColors.InfoText;
                        if (sUnit.IsFavorite) 
                        { 
                            gBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                        }
                    }
                    break;
                }
            }
        }

        private List<UnitType> LoadJsons()
        {
            string fileName = "UnitsConfig.json";
            string jsonString = File.ReadAllText(fileName);
            List<UnitType> AllUnits = JsonSerializer.Deserialize<List<UnitType>>(jsonString);
            return AllUnits;
        }

        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            TextBox ThisTextBox = (TextBox)sender;
            string InStr = ThisTextBox.Text;
            bool IsValidInput;
            if (InStr == "")
            {
                ClearInputBox();
                IsValidInput = false;
            }
            else
            {
                IsValidInput = IsInputNumber(ThisTextBox.Text);
            }
            if (IsValidInput)
            {
                double ThisInputValue = Input2Double(ThisTextBox.Text);
                double InputUnitToBasicUnit = 0;
                Control ParentCtl = ThisTextBox.Parent;
                Control GrandParentCtl = ParentCtl.Parent;
                string ThisUnitName = ParentCtl.Text;
                string ThisUnitTypeName = GrandParentCtl.Text;
                var ThisUnitItems = new List<UnitItem>();

                foreach (var vUnit in AllUnitsJson)
                {
                    string UnitTName = vUnit.UnitTypeName;
                    if (UnitTName == ThisUnitTypeName)
                    {
                        ThisUnitItems = vUnit.Units;
                        break;
                    }
                }
                foreach (var sUnit in ThisUnitItems)
                {
                    string UnitTName = sUnit.UnitFullName;
                    if (UnitTName == ThisUnitName)
                    {
                        InputUnitToBasicUnit = sUnit.EqualsToBasicUnit;
                        break;
                    }
                }
                if (ParentCtl != null)
                {
                    string ThisGroupBoxText = ParentCtl.Text;
                    for (int i = 1; i <= 20; i++)
                    {
                        GroupBox gBox = this.grpUnits.Controls["groupBox" + i.ToString()] as GroupBox;
                        if (!gBox.Visible) break;
                        gBox.Text = ThisUnitItems[i - 1].UnitFullName;
                        if (gBox.Text == ThisUnitTypeName) continue;
                        Label lblCtl = gBox.Controls["label" + i.ToString()] as Label;
                        lblCtl.Text = ThisUnitItems[i - 1].UnitShortName;
                        TextBox txtCtl = gBox.Controls["textBox" + i.ToString()] as TextBox;
                        double Temp = ThisInputValue * InputUnitToBasicUnit / ThisUnitItems[i - 1].EqualsToBasicUnit;
                        string TempStr = Temp.ToString();
                        txtCtl.TextChanged -= new System.EventHandler(this.txtBox_TextChanged);
                        txtCtl.Text = TempStr;
                        txtCtl.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
                    }

                }
            }
        }
        private void ClearInputBox()
        {
            for (int i = 1; i <= 20; i++)
            {
                GroupBox gBox = this.grpUnits.Controls["groupBox" + i.ToString()] as GroupBox;
                if (!gBox.Visible) break;
                TextBox txtCtl = gBox.Controls["textBox" + i.ToString()] as TextBox;
                txtCtl.Text = "";
            }
        }
        private bool IsInputNumber(string Instr)
        {
            string lastChar = Instr.Substring(Instr.Length - 1);
            if (lastChar == "." || lastChar=="E" || lastChar == "e" || lastChar == "-") return false;
            if (lastChar == "0" && Instr.Contains(".")) return false;
            bool IsNormDigi = IsMatched("^[0-9]+[.|,]?([0-9]+)?$",Instr);
            bool IsSciDigi = IsMatched("^[0-9]+[E|E-]?([0-9]+)?$", Instr);
            if(IsNormDigi || IsSciDigi)  return true;
            else return false;

        }

        private bool IsMatched(string pattern,string MString)
        {
            Regex regex1 = new Regex(pattern);
            bool IsNormDigi = regex1.IsMatch(MString);
            return IsNormDigi;
        }
        private double Input2Double(string InStr)
        {
            double floatResult;
            double.TryParse(InStr, out floatResult);
            return floatResult;
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox ThisTextBox = (TextBox)sender;
            string InStr = ThisTextBox.Text;
            // wait for {enter}, no {enter}, the program can NOT identify m and mm
            if (e.KeyCode == Keys.Enter)
            {
                bool UnitPos = IsContainsUnit(InStr);
            }
        }

        private bool IsContainsUnit(string InStr)
        {
            InStr=InStr.Trim().ToLower();
            foreach (var vUnitType in AllUnitsJson)
            {
                string UnitTypeIdxName = vUnitType.UnitTypeName;
                int iU = 0;
                foreach (var sUnit in vUnitType.Units)
                {
                    iU = iU + 1;
                    string UnitStName = sUnit.UnitShortName.ToLower();
                    int UnitStNameLen = UnitStName.Length;
                    int index = InStr.IndexOf(UnitStName, StringComparison.CurrentCultureIgnoreCase);
                    // distinguish short unit from long unit, like t o distinguish M from KM,
                    // by checking whether the last char is letter
                    // if last char is letter, it means [KM]
                    string InstrValue=InStr.Substring(0, index);
                    string lastChar = InstrValue.Substring(InstrValue.Length - 1).ToLower();
                    bool IsLastCharLetter = IsMatched("[a-z]$", lastChar);
                    if (index == -1 || IsLastCharLetter)    continue;
                    else if (index >= 0)
                    {
                        string MatchedUnitStName = UnitStName.ToLower();

                        // find unit and value, jump to textbox with value
                        LoadUnitsIntoForm(UnitTypeIdxName);
                        grpUnits.Text = UnitTypeIdxName;

                        if (InStr.Substring(index)== UnitStName)
                        {
                            for (int i = 1;i<=20;i++)
                            {
                                GroupBox gBox = this.grpUnits.Controls["groupBox" + i.ToString()] as GroupBox;
                                Label lblCtl = gBox.Controls["label" + i.ToString()] as Label;
                                string lblCtlText2Lower = lblCtl.Text.ToLower();
                                if (lblCtlText2Lower.Contains(MatchedUnitStName))
                                {
                                    TextBox txtCtl = gBox.Controls["textBox" + i.ToString()] as TextBox;
                                    if (index > 0) txtCtl.Text = InStr.Substring(0, index).Trim();
                                    txtCtl.Focus();
                                    break;
                                }

                            }
                            return true;
                        }
                        return false;
                    }
                }
            }
            return false;

        }
        private int GetUnitPos(string InStr)
        {
            int StrLen=InStr.Length;
            int pos = StrLen-1;
            while (!Char.IsLetter(InStr[pos]))
            {
                if(pos == 0) break;
                --pos;

            }
            //InStr = InStr.Substring(0, pos);
            return pos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string InStr= SearchBox.Text.Trim();
            bool UnitPos = IsContainsUnit(InStr);


        }

        private void lbl_Click(object sender, EventArgs e)
        {
            Label ThisLabel = (Label)sender;
            string ThisLabelName = ThisLabel.Name;
            string ThisLabelText = ThisLabel.Text;
            string ThisLabelID = ThisLabelName.Substring(5);
            Control ParentCtl = ThisLabel.Parent;
            TextBox txtCtl = ParentCtl.Controls["textBox" + ThisLabelID] as TextBox;
            string ConvertedValue=txtCtl.Text+" "+ ThisLabelText;
            Clipboard.SetText(ConvertedValue); 
        }
        private void txtBox_DbClick(object sender, EventArgs e)
        {
            TextBox ThisTextBox = (TextBox)sender;
            string ThisTextBoxText = ThisTextBox.Text;
            Clipboard.SetText(ThisTextBoxText);
        }

    }



}
