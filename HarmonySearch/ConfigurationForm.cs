﻿using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HarmonySearch
{
    public partial class ConfigurationForm : Form
    {
        ClassicSearch classicHS;
        ImprovedSearch improvedHS;
        GlobalBestSearch globalHS;
        SelfAdaptiveSearch adaptiveHS;

        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            setHarmonySearch();
            
            GraphsForm graphs = new GraphsForm();
            graphs.classicHS = classicHS;
            graphs.improvedHS = improvedHS;
            graphs.globalHS = globalHS;
            graphs.adaptiveHS = adaptiveHS;
            graphs.showAll = showAllCheckBox.Checked;
            graphs.Show();

            this.Hide();
        }

        private void setHarmonySearch()
        {
            if (isInputOk())
            {
                if(ClassicRadioButton.Checked == true)
                {
                    classicHS = new ClassicSearch();
                    classicHS.NI = Convert.ToInt32(NITextBox.Text);
                    classicHS.Objective = new Expression(ObjectiveTextBox.Text);
                    classicHS.TotalNotes = Convert.ToInt32(TotalNotesTextBox.Text);
                    classicHS.MaximumValue = double.Parse(MaxValueTextBox.Text, CultureInfo.InvariantCulture);
                    classicHS.MinimumValue = double.Parse(MinValueTextBox.Text, CultureInfo.InvariantCulture);
                    classicHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                    classicHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                    classicHS.PAR = float.Parse(PARTextBox.Text, CultureInfo.InvariantCulture);
                    classicHS.BW = double.Parse(BWTextBox.Text, CultureInfo.InvariantCulture);
                }
                if (ImprovedRadioButton.Checked == true)
                {
                    improvedHS = new ImprovedSearch();
                    improvedHS.NI = Convert.ToInt32(NITextBox.Text);
                    improvedHS.TotalNotes = Convert.ToInt32(TotalNotesTextBox.Text);
                    improvedHS.MaximumValue = double.Parse(MaxValueTextBox.Text, CultureInfo.InvariantCulture);
                    improvedHS.MinimumValue = double.Parse(MinValueTextBox.Text, CultureInfo.InvariantCulture);
                    improvedHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                    improvedHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                    improvedHS.PARmin = float.Parse(PARMinTextBox.Text, CultureInfo.InvariantCulture);
                    improvedHS.PARmax = float.Parse(PARMaxTextBox.Text, CultureInfo.InvariantCulture);
                    improvedHS.BWmin = float.Parse(BWMinTextBox.Text, CultureInfo.InvariantCulture);
                    improvedHS.BWmax = float.Parse(BWMaxTextBox.Text, CultureInfo.InvariantCulture);
                }
                if (GlobalRadioButton.Checked == true)
                {
                    globalHS = new GlobalBestSearch();
                    globalHS.NI = Convert.ToInt32(NITextBox.Text);
                    globalHS.TotalNotes = Convert.ToInt32(TotalNotesTextBox.Text);
                    globalHS.MaximumValue = double.Parse(MaxValueTextBox.Text, CultureInfo.InvariantCulture);
                    globalHS.MinimumValue = double.Parse(MinValueTextBox.Text, CultureInfo.InvariantCulture);
                    globalHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                    globalHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                    globalHS.PAR = float.Parse(PARTextBox.Text, CultureInfo.InvariantCulture);
                }
                if (AdaptiveRadioButton.Checked == true)
                {
                    adaptiveHS = new SelfAdaptiveSearch();
                    adaptiveHS.NI = Convert.ToInt32(NITextBox.Text);
                    adaptiveHS.TotalNotes = Convert.ToInt32(TotalNotesTextBox.Text);
                    adaptiveHS.HMSize = Convert.ToInt32(HMSTextBox.Text);
                    adaptiveHS.MaximumValue = double.Parse(MaxValueTextBox.Text, CultureInfo.InvariantCulture);
                    adaptiveHS.MinimumValue = double.Parse(MinValueTextBox.Text, CultureInfo.InvariantCulture);
                    adaptiveHS.HMCR = float.Parse(HMCRTextBox.Text, CultureInfo.InvariantCulture);
                    adaptiveHS.PAR = float.Parse(PARTextBox.Text, CultureInfo.InvariantCulture);
                }
            }
            //int i = 1;
            //string strarg = "x" + i;
            //Argument arg1 = new Argument(strarg, Math.PI/2);
            
            //Argument arg2 = new Argument("x2", Math.PI / 2);
            //Expression expr = new Expression(ObjectiveTextBox.Text);
            //expr.addArguments(arg1, arg2);
            //expr.calculate();
            //double res;
            //res = expr.calculate();
            //res = 0;
        }

        private Boolean isInputOk()
        {
            if (!ConfigurationRules.isNIValid(NITextBox.Text))
                return false;
            if (!ConfigurationRules.areTotalVariablesValid(TotalNotesTextBox.Text))
                return false;
            if (!ConfigurationRules.isHMSValid(HMSTextBox.Text))
                return false;
            if (!ConfigurationRules.isHMCRValid(HMCRTextBox.Text))
                return false;
            if (!ConfigurationRules.isPARValid(PARTextBox.Text))
                return false;
            if (!ConfigurationRules.arePARExtremesValid(PARMinTextBox.Text, PARMaxTextBox.Text))
                return false;
            if (!ConfigurationRules.isBWValid(BWTextBox.Text))
                return false;
            if (!ConfigurationRules.isBWValid(BWMinTextBox.Text))
                return false;
            if (!ConfigurationRules.isBWValid(BWMaxTextBox.Text))
                return false;
            if (!ConfigurationRules.areExtremeValuesValid(MinValueTextBox.Text, MaxValueTextBox.Text))
                return false;

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
                parametersLabel.Text = "Parameters of the Classic Harmony Search:";
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
                parametersLabel.Text = "Parameters of the Improved Harmony Search:";
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
                parametersLabel.Text = "Parameters of the Global Best Harmony Search:";
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
                parametersLabel.Text = "Parameters of the Self Adaptive Harmony Search:";
            }
        }
    }
}
