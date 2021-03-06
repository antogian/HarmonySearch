﻿using NCalc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HarmonySearch
{
    public partial class ConfigurationForm : Form
    {
        private ClassicSearch classicHS;
        private ImprovedSearch improvedHS;
        private GlobalBestSearch globalHS;
        private SelfAdaptiveSearch adaptiveHS;

        private Expression objective;
        private HarmonySearchVariant currentVariant;

        private int totalNotesControls = 2;

        public ConfigurationForm()
        {
            InitializeComponent();
            ObjectiveRichTextBox.SelectAll();
            ObjectiveRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
            ObjectiveRichTextBox.DeselectAll();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (isInputOk())
                setHarmonySearch();
            else
                return;

            GraphsForm graphs = new GraphsForm();
            graphs.classicHS = classicHS;
            graphs.improvedHS = improvedHS;
            graphs.globalHS = globalHS;
            graphs.adaptiveHS = adaptiveHS;
            graphs.ShowAll = showAllCheckBox.Checked;
            graphs.activationFlag = true;
            graphs.Configuration = this;

            this.Hide();
            graphs.Show();
        }

        private void setHarmonySearch()
        {
            if (currentVariant == HarmonySearchVariant.Classic)
            {
                classicHS = ClassicSearch.Instance;
                classicHS.Objective = objective;
                if (MaxRadioBtn.Checked)
                    classicHS.Optimum = OptimizationGoal.Max;
                if (MinRadioBtn.Checked)
                    classicHS.Optimum = OptimizationGoal.Min;
                if (MinAbsRadioBtn.Checked)
                    classicHS.Optimum = OptimizationGoal.MinAbs;
                classicHS.NI = Convert.ToInt32(NITextBox.Text);
                classicHS.TotalNotes = countDecisionVariables();
                classicHS.MinimumValues = new double[classicHS.TotalNotes];
                classicHS.MaximumValues = new double[classicHS.TotalNotes];
                for (int i = 0; i < classicHS.TotalNotes; i++)
                {
                    TextBox minTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MinTextBox", true)[0];
                    classicHS.MinimumValues[i] = double.Parse(minTextBox.Text, CultureInfo.InvariantCulture);
                    TextBox maxTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MaxTextBox", true)[0];
                    classicHS.MaximumValues[i] = double.Parse(maxTextBox.Text, CultureInfo.InvariantCulture);
                }
                classicHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                classicHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                classicHS.PAR = float.Parse(PARTextBox.Text, CultureInfo.InvariantCulture);
                classicHS.BW = double.Parse(BWTextBox.Text, CultureInfo.InvariantCulture);
            }
            if (currentVariant == HarmonySearchVariant.Improved)
            {
                improvedHS = ImprovedSearch.Instance;
                improvedHS.Objective = objective;
                if (MaxRadioBtn.Checked)
                    improvedHS.Optimum = OptimizationGoal.Max;
                if (MinRadioBtn.Checked)
                    improvedHS.Optimum = OptimizationGoal.Min;
                if (MinAbsRadioBtn.Checked)
                    improvedHS.Optimum = OptimizationGoal.MinAbs;
                improvedHS.NI = Convert.ToInt32(NITextBox.Text);
                improvedHS.TotalNotes = countDecisionVariables();
                improvedHS.MinimumValues = new double[improvedHS.TotalNotes];
                improvedHS.MaximumValues = new double[improvedHS.TotalNotes];
                for (int i = 0; i < improvedHS.TotalNotes; i++)
                {
                    TextBox minTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MinTextBox", true)[0];
                    improvedHS.MinimumValues[i] = double.Parse(minTextBox.Text, CultureInfo.InvariantCulture);
                    TextBox maxTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MaxTextBox", true)[0];
                    improvedHS.MaximumValues[i] = double.Parse(maxTextBox.Text, CultureInfo.InvariantCulture);
                }
                improvedHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                improvedHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                improvedHS.PARmin = float.Parse(PARMinTextBox.Text, CultureInfo.InvariantCulture);
                improvedHS.PARmax = float.Parse(PARMaxTextBox.Text, CultureInfo.InvariantCulture);
                improvedHS.BWmin = float.Parse(BWMinTextBox.Text, CultureInfo.InvariantCulture);
                improvedHS.BWmax = float.Parse(BWMaxTextBox.Text, CultureInfo.InvariantCulture);
            }
            if (currentVariant == HarmonySearchVariant.GlobalBest)
            {
                globalHS = GlobalBestSearch.Instance;
                globalHS.Objective = objective;
                if (MaxRadioBtn.Checked)
                    globalHS.Optimum = OptimizationGoal.Max;
                if (MinRadioBtn.Checked)
                    globalHS.Optimum = OptimizationGoal.Min;
                if (MinAbsRadioBtn.Checked)
                    globalHS.Optimum = OptimizationGoal.MinAbs;
                globalHS.NI = Convert.ToInt32(NITextBox.Text);
                globalHS.TotalNotes = countDecisionVariables();
                globalHS.MinimumValues = new double[globalHS.TotalNotes];
                globalHS.MaximumValues = new double[globalHS.TotalNotes];
                for (int i = 0; i < globalHS.TotalNotes; i++)
                {
                    TextBox minTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MinTextBox", true)[0];
                    globalHS.MinimumValues[i] = double.Parse(minTextBox.Text, CultureInfo.InvariantCulture);
                    TextBox maxTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MaxTextBox", true)[0];
                    globalHS.MaximumValues[i] = double.Parse(maxTextBox.Text, CultureInfo.InvariantCulture);
                }
                globalHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                globalHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                globalHS.PAR = float.Parse(PARTextBox.Text, CultureInfo.InvariantCulture);
            }
            if (currentVariant == HarmonySearchVariant.SelfAdaptive)
            {
                adaptiveHS = SelfAdaptiveSearch.Instance;
                adaptiveHS.Objective = objective;
                if (MaxRadioBtn.Checked)
                    adaptiveHS.Optimum = OptimizationGoal.Max;
                if (MinRadioBtn.Checked)
                    adaptiveHS.Optimum = OptimizationGoal.Min;
                if (MinAbsRadioBtn.Checked)
                    adaptiveHS.Optimum = OptimizationGoal.MinAbs;
                adaptiveHS.NI = Convert.ToInt32(NITextBox.Text);
                adaptiveHS.TotalNotes = countDecisionVariables();
                adaptiveHS.MinimumValues = new double[adaptiveHS.TotalNotes];
                adaptiveHS.MaximumValues = new double[adaptiveHS.TotalNotes];
                for (int i = 0; i < adaptiveHS.TotalNotes; i++)
                {
                    TextBox minTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MinTextBox", true)[0];
                    adaptiveHS.MinimumValues[i] = double.Parse(minTextBox.Text, CultureInfo.InvariantCulture);
                    TextBox maxTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MaxTextBox", true)[0];
                    adaptiveHS.MaximumValues[i] = double.Parse(maxTextBox.Text, CultureInfo.InvariantCulture);
                }
                adaptiveHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                adaptiveHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                adaptiveHS.PAR = float.Parse(PARTextBox.Text, CultureInfo.InvariantCulture);
            }
        }

        private Boolean isInputOk()
        {
            //if(ObjectiveRichTextBox.Text.Equals("") || ObjectiveRichTextBox.Text == null)
            //{
            //    ControlStyle.MessageBoxStyle("The objective function is null or empty.");
            //    return false;
            //}
            objective = new Expression(ObjectiveRichTextBox.Text);
            //if (objective.HasErrors())
            //{
            //    ControlStyle.MessageBoxStyle("The objective function is not valid. \n\n" + objective.Error);
            //    return false;
            //}
            //if (objective == null)
            //{
            //    ControlStyle.MessageBoxStyle("The objective function is not valid. Please try again.");
            //    return false;
            //}
            if (totalNotesControls < 2)
            {
                //ControlStyle.MessageBoxStyle("The algorithm requires at least 2 decision variables.");
                ControlStyle.MessageBoxStyle("Ο αλγόριθμος απαιτεί τουλάχιστον 2 μεταβλητές απόφασης.");
                return false;
            }
            try
            {
                for (int k=1; k <= totalNotesControls; k++)
                {
                    objective.Parameters["x" + k] = 5;
                }
                object res = objective.Evaluate();
            }
            catch(Exception e)
            {
                //ControlStyle.MessageBoxStyle("The objective function is not valid. Please try again.");
                ControlStyle.MessageBoxStyle("Η αντικειμενική συνάρτηση δεν συντάχθηκε σωστά.");
                return false;
            }

            for (int i = 0; i < totalNotesControls; i++)
            {
                TextBox minTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MinTextBox", true)[0];
                TextBox maxTextBox = (TextBox)this.Controls.Find("x" + (i + 1) + "MaxTextBox", true)[0];
                if (!ConfigurationRules.areExtremeValuesValid(minTextBox.Text, maxTextBox.Text))
                {
                    //ControlStyle.MessageBoxStyle("Decision variable " + "X" + (i + 1) + " bounds are not valid.");
                    ControlStyle.MessageBoxStyle("Τα όρια της μεταβλητής απόφασης " + "X" + (i + 1) + " δεν είναι σωστά.");
                    return false;
                }
            }
            if (!ConfigurationRules.isNIValid(NITextBox.Text))
            {
                //ControlStyle.MessageBoxStyle("NI(Number of Improvisations) field is not valid. Please try again.");
                ControlStyle.MessageBoxStyle("Το πεδίο NI(Number of Improvisations) δεν είναι σωστό.");
                return false;
            }
            if (!ConfigurationRules.isHMSValid(HMSTextBox.Text))
            {
                //ControlStyle.MessageBoxStyle("HMS(Harmony Memory Size) field is not valid. Please try again.");
                ControlStyle.MessageBoxStyle("Το πεδίο HMS(Harmony Memory Size) δεν είναι σωστό.");
                return false;
            }
            if (!ConfigurationRules.isHMCRValid(HMCRTextBox.Text))
            {
                //ControlStyle.MessageBoxStyle("HMCR(Harmony Memory Consideration Rate) field is not valid. Please try again.");
                ControlStyle.MessageBoxStyle("Το πεδίο HMCR(Harmony Memory Consideration Rate) δεν είναι σωστό.");
                return false;
            }
            if (!currentVariant.Equals(HarmonySearchVariant.Improved))
            {
                if (!ConfigurationRules.isPARValid(PARTextBox.Text))
                {
                    //ControlStyle.MessageBoxStyle("PAR(Pitch Adjustment Rate) field is not valid.");
                    ControlStyle.MessageBoxStyle("Το πεδίο PAR(Pitch Adjustment Rate) δεν είναι σωστό.");
                    return false;
                }
                if(currentVariant.Equals(HarmonySearchVariant.Classic))
                {
                    if (!ConfigurationRules.isBWValid(BWTextBox.Text))
                    {
                        //ControlStyle.MessageBoxStyle("BW(Bandwidth) field is not valid.");
                        ControlStyle.MessageBoxStyle("Το πεδίο BW(Bandwidth) δεν είναι σωστό.");
                        return false;
                    }
                }
            }
            else
            {
                if (!ConfigurationRules.arePARExtremesValid(PARMinTextBox.Text, PARMaxTextBox.Text))
                {
                    //ControlStyle.MessageBoxStyle("PAR(Pitch Adjustment Rate) bounds are not valid.");
                    ControlStyle.MessageBoxStyle("Τα όρια του πεδίου PAR(Pitch Adjustment Rate) δεν είναι σωστά.");
                    return false;
                }
                if (!ConfigurationRules.areΒWExtremesValid(BWMinTextBox.Text, BWMaxTextBox.Text))
                {
                    //ControlStyle.MessageBoxStyle("BW(Bandwidth) bounds are not not valid.");
                    ControlStyle.MessageBoxStyle("Τα όρια του πεδίου BW(Bandwidth) δεν είναι σωστά.");
                    return false;
                }
            }

            return true;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton senderRadioButton = sender as RadioButton;
            if(senderRadioButton.Tag.Equals("ClassicHS"))
            {
                PARTextBox.Visible = true;
                PARMinTextBox.Visible = false;
                PARMaxTextBox.Visible = false;
                BWTextBox.Visible = true;
                BWMaxTextBox.Visible = false;
                BWMinTextBox.Visible = false;
                PARLabel.Visible = true;
                PARMinLabel.Visible = false;
                PARMaxLabel.Visible = false;
                BWLabel.Visible = true;
                BWMaxLabel.Visible = false;
                BWMinLabel.Visible = false;
                //ParametersLabel.Text = "Parameters of the Classic Harmony Search:";
                ParametersLabel.Text = "Παράμετροι της Classic Harmony Search:";
                currentVariant = HarmonySearchVariant.Classic;
            }
            if (senderRadioButton.Tag.Equals("ImprovedHS"))
            {
                PARTextBox.Visible = false;
                PARMinTextBox.Visible = true;
                PARMaxTextBox.Visible = true;
                BWTextBox.Visible = false;
                BWMaxTextBox.Visible = true;
                BWMinTextBox.Visible = true;
                PARLabel.Visible = false;
                PARMinLabel.Visible = true;
                PARMaxLabel.Visible = true;
                BWLabel.Visible = false;
                BWMaxLabel.Visible = true;
                BWMinLabel.Visible = true;
                //ParametersLabel.Text = "Parameters of the Improved Harmony Search:";
                ParametersLabel.Text = "Παράμετροι της Improved Harmony Search:";
                currentVariant = HarmonySearchVariant.Improved;
            }
            if (senderRadioButton.Tag.Equals("GlobalBestHS"))
            {
                PARTextBox.Visible = true;
                PARMinTextBox.Visible = false;
                PARMaxTextBox.Visible = false;
                BWTextBox.Visible = false;
                BWMaxTextBox.Visible = false;
                BWMinTextBox.Visible = false;
                PARLabel.Visible = true;
                PARMinLabel.Visible = false;
                PARMaxLabel.Visible = false;
                BWLabel.Visible = false;
                BWMaxLabel.Visible = false;
                BWMinLabel.Visible = false;
                //ParametersLabel.Text = "Parameters of the Global Best Harmony Search:";
                ParametersLabel.Text = "Παράμετροι της Global Best Harmony Search:";
                currentVariant = HarmonySearchVariant.GlobalBest;
            }
            if (senderRadioButton.Tag.Equals("SelfAdaptiveHS"))
            {
                PARTextBox.Visible = true;
                PARMinTextBox.Visible = false;
                PARMaxTextBox.Visible = false;
                BWTextBox.Visible = false;
                BWMaxTextBox.Visible = false;
                BWMinTextBox.Visible = false;
                PARLabel.Visible = true;
                PARMinLabel.Visible = false;
                PARMaxLabel.Visible = false;
                BWLabel.Visible = false;
                BWMaxLabel.Visible = false;
                BWMinLabel.Visible = false;
                //ParametersLabel.Text = "Parameters of the Self Adaptive Harmony Search:";
                ParametersLabel.Text = "Παράμετροι της Self Adaptive Harmony Search:";
                currentVariant = HarmonySearchVariant.SelfAdaptive;
            }
        }

        private int countDecisionVariables()
        {
            int counter = 1;
            while(true)
            {
                if (ObjectiveRichTextBox.Text.Contains("x" + counter))
                    counter++;
                else
                    return --counter;
            }
        }

        private void createNewTextbox(int totalNotes)
        {
            for(int i = totalNotesControls + 1; i <= totalNotes; i++)
            {
                TextBox minTextBox = new TextBox();
                minTextBox.Name = "x" + i + "MinTextBox";
                TextBox maxTextBox = new TextBox();
                maxTextBox.Name = "x" + i + "MaxTextBox";
                Label label = new Label();
                label.Name = "x" + i + "Label";

                ControlStyle.TextBoxStyle(minTextBox, i);
                ControlStyle.TextBoxStyle(maxTextBox, i);
                ControlStyle.ConfigurationLabelStyle(label, i);
                variablesTab.Controls.Add(minTextBox);
                variablesTab.Controls.Add(maxTextBox);
                variablesTab.Controls.Add(label);
            }
        }

        private void deleteTextbox(int totalNotes)
        {
            for (int i = totalNotesControls; i > totalNotes; i--)
            {
                variablesTab.Controls.RemoveByKey("x" + i + "MinTextBox");
                variablesTab.Controls.RemoveByKey("x" + i + "MaxTextBox");
                variablesTab.Controls.RemoveByKey("x" + i + "Label");
            }
        }

        private void ConfigurationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url;
            if (e.Link.LinkData != null)
                url = e.Link.LinkData.ToString();
            else
                url = linkLabel1.Text.Substring(e.Link.Start, e.Link.Length);

            if (!url.Contains("://"))
                url = "http://" + url;

            var si = new ProcessStartInfo(url);
            Process.Start(si);
            linkLabel1.LinkVisited = true;
        }

        public void initializeComponents()
        {
            classicHS = null;
            improvedHS = null;
            globalHS = null;
            adaptiveHS = null;
        }

        private void ObjectiveRichTextBox_Enter(object sender, EventArgs e)
        {
            ObjectiveRichTextBox.SelectAll();
            ObjectiveRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
            ObjectiveRichTextBox.DeselectAll();
        }

        private void ObjectiveRichTextBox_Leave(object sender, EventArgs e)
        {
            checkDecisionVariables();
        }

        private void checkDecisionVariables()
        {
            int totalNotes = countDecisionVariables();
            if (totalNotes > totalNotesControls)
            {
                createNewTextbox(totalNotes);
            }
            else if (totalNotes < totalNotesControls)
            {
                deleteTextbox(totalNotes);
            }
            totalNotesControls = totalNotes;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            Parameters parameters = new Parameters();
            string path = pathTextBox.Text;
            if (String.IsNullOrEmpty(path) || String.IsNullOrWhiteSpace(path) || !path.EndsWith(".txt") || !path.Contains("\\"))
            {
                ControlStyle.MessageBoxStyle("Η τοποθεσία(URI) του αρχείου δεν είναι έγκυρη");
                return;
            }
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                parameters = JsonConvert.DeserializeObject<Parameters>(json);
            }
            ObjectiveRichTextBox.Text = parameters.Objective;
            if (parameters.Optimum == OptimizationGoal.Max)
                MaxRadioBtn.Checked = true;
            if (parameters.Optimum == OptimizationGoal.Min)
                MinRadioBtn.Checked = true;
            if (parameters.Optimum == OptimizationGoal.MinAbs)
                MinAbsRadioBtn.Checked = true;
            showAllCheckBox.Checked = parameters.ShowAll;
            checkDecisionVariables();
            for (int j=0; j<totalNotesControls; j++)
            {
                Controls.Find("x" + (j + 1) + "MinTextBox", true)[0].Text = parameters.MinValues[j].ToString();
                Controls.Find("x" + (j + 1) + "MaxTextBox", true)[0].Text = parameters.MaxValues[j].ToString();
            }
            NITextBox.Text = parameters.NI.ToString();
            HMSTextBox.Text = parameters.HMS.ToString();
            HMCRLabel.Text = parameters.HMCR.ToString();
            PARTextBox.Text = parameters.PAR.ToString();
            BWTextBox.Text = parameters.BW.ToString();
            PARMinTextBox.Text = parameters.PARmin.ToString();
            PARMaxTextBox.Text = parameters.PARmax.ToString();
            BWMinTextBox.Text = parameters.PARmin.ToString();
            BWMaxTextBox.Text = parameters.PARmax.ToString();
            if (parameters.Variant == HarmonySearchVariant.Classic)
                ClassicRadioButton.Checked = true;
            if (parameters.Variant == HarmonySearchVariant.Improved)
                ImprovedRadioButton.Checked = true;
            if (parameters.Variant == HarmonySearchVariant.GlobalBest)
                GlobalRadioButton.Checked = true;
            if (parameters.Variant == HarmonySearchVariant.SelfAdaptive)
                AdaptiveRadioButton.Checked = true;

            ControlStyle.MessageBoxSuccessStyle("Οι παράμετροι του αλγορίθμου φορτώθηκαν επιτυχώς από το αρχείο.");
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if(!isInputOk())
            {
                //ControlStyle.MessageBoxStyle("Parameters are not valid.");
                ControlStyle.MessageBoxStyle("Οι παράμετροι δεν είναι σωστοί.");
                return;
            }

            string path = pathTextBox.Text;
            if (String.IsNullOrEmpty(path) || String.IsNullOrWhiteSpace(path) || !path.EndsWith(".txt") || !path.Contains("\\"))
            {
                ControlStyle.MessageBoxStyle("Η τοποθεσία(URI) του αρχείου δεν είναι έγκυρη");
                return;
            }

            Parameters parameters = new Parameters();

            parameters.Objective = ObjectiveRichTextBox.Text;
            parameters.ShowAll = showAllCheckBox.Checked;
            if (MaxRadioBtn.Checked == true)
                parameters.Optimum = OptimizationGoal.Max;
            if (MinRadioBtn.Checked == true)
                parameters.Optimum = OptimizationGoal.Min;
            if (MinAbsRadioBtn.Checked == true)
                parameters.Optimum = OptimizationGoal.MinAbs;
            parameters.MaxValues = new double[totalNotesControls];
            parameters.MinValues = new double[totalNotesControls];
            for (int i=0; i<totalNotesControls; i++)
            {
                TextBox maxTextbox = (TextBox) this.Controls.Find("x" + (i + 1) + "MaxTextBox", true)[0];
                parameters.MaxValues[i] = double.Parse(maxTextbox.Text, CultureInfo.InvariantCulture);
                TextBox minTextbox = (TextBox) this.Controls.Find("x" + (i + 1) + "MinTextBox", true)[0];
                parameters.MinValues[i] = double.Parse(minTextbox.Text, CultureInfo.InvariantCulture);
            }
            parameters.NI = Int32.Parse(NITextBox.Text);
            parameters.HMS = Int32.Parse(HMSTextBox.Text);
            parameters.HMCR = Convert.ToDouble(HMCRTextBox.Text);
            parameters.PAR = Convert.ToDouble(PARTextBox.Text);
            parameters.BW = Convert.ToDouble(BWTextBox.Text);
            parameters.PARmin = Convert.ToDouble(PARMinTextBox.Text);
            parameters.PARmax = Convert.ToDouble(PARMaxTextBox.Text);
            parameters.BWmin = Convert.ToDouble(BWMinTextBox.Text);
            parameters.BWmax = Convert.ToDouble(BWMaxTextBox.Text);
            if (ClassicRadioButton.Checked == true)
                parameters.Variant = HarmonySearchVariant.Classic;
            if (ImprovedRadioButton.Checked == true)
                parameters.Variant = HarmonySearchVariant.Improved;
            if (GlobalRadioButton.Checked == true)
                parameters.Variant = HarmonySearchVariant.GlobalBest;
            if (AdaptiveRadioButton.Checked == true)
                parameters.Variant = HarmonySearchVariant.SelfAdaptive;

            string json = JsonConvert.SerializeObject(parameters);
            File.WriteAllText(@path, json);

            ControlStyle.MessageBoxSuccessStyle("Οι παράμετροι του αλγορίθμου αποθηκεύτηκαν επιτυχώς σε αρχείο.");
        }
    }
}
