﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class SubjectEntry : Form
    {
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\Server2\second semester 2023-2024\LAB802\79286_CC_APPSDEV22_1030_1230_PM_MW\79286-23220726\Desktop\FINAL\Saguisa.accdb";
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\arjay\Documents\Github\dbaccesstrial\Saguisa.accdb";
        public SubjectEntry()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            OleDbConnection thisConnection = new OleDbConnection(connectionString);
            string Ole = "Select * From SUBJECTFILE";
            OleDbDataAdapter thisAdapter = new OleDbDataAdapter(Ole, thisConnection);
            OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);
            DataSet thisDataSet = new DataSet();
            thisAdapter.Fill(thisDataSet, "SubjectFile");

            DataRow thisRow = thisDataSet.Tables["SubjectFile"].NewRow();
            thisRow["SFSUBJCODE"] = SubjectCodeTextBox.Text;
            thisRow["SFSUBJDESC"] = DescriptionTextBox.Text;
            thisRow["SFSUBJUNITS"] = UnitsTextBox.Text;
            thisRow["SFSUBJREGOFRNG"] = OfferingComboBox.SelectedIndex + 1;
            thisRow["SFSUBJCATEGORY"] = CategoryComboBox.Text.Substring(0, 3).ToUpper();
            thisRow["SFSUBJSTATUS"] = "AC";
            thisRow["SFSUBJCOURSECODE"] = CourseCodeComboBox.Text;
            thisRow["SFSUBJCURRCODE"] = CurriculumYearTextBox.Text;

            thisDataSet.Tables["SubjectFile"].Rows.Add(thisRow);
            thisAdapter.Update(thisDataSet, "SubjectFile");

            //REQUISITE
            if(RequisiteTextBox.Text != string.Empty)
            {
                Ole = "Select * From SUBJECTPREQFILE";
                OleDbDataAdapter requisiteAdapter = new OleDbDataAdapter(Ole, thisConnection);
                OleDbCommandBuilder requisiteBuilder = new OleDbCommandBuilder(requisiteAdapter);
                DataSet requisiteDataSet = new DataSet();
                requisiteAdapter.Fill(requisiteDataSet, "SubjectPreqFile");

                DataRow requisiteRow = requisiteDataSet.Tables["SubjectPreqFile"].NewRow();
                requisiteRow["SFSUBJCODE"] = SubjectCodeTextBox.Text;
                requisiteRow["SUBJPRECODE"] = RequisiteTextBox.Text;
                if (PreRequisiteRadioButton.Checked)
                    requisiteRow["SUBJCATEGORY"] = "PR";
                else if (CoRequisiteRadioButton.Checked)
                    requisiteRow["SUBJCATEGORY"] = "CR";

                requisiteDataSet.Tables["SubjectPreqFile"].Rows.Add(requisiteRow);
                requisiteAdapter.Update(requisiteDataSet, "SubjectPreqFile");
            }
            PreRequisiteRadioButton.Checked = false;
            CoRequisiteRadioButton.Checked = false;
            MessageBox.Show("Recorded");
        }

        private void RequisiteTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                OleDbConnection thisConnection = new OleDbConnection(connectionString);
                thisConnection.Open();
                OleDbCommand thisCommand = thisConnection.CreateCommand();

                string sql = "SELECT * FROM SUBJECTFILE";
                thisCommand.CommandText = sql;

                OleDbDataReader thisDataReader = thisCommand.ExecuteReader();
                //
                bool found = false;
                string subjectCode = "";
                string description = "";
                string units = "";

                while (thisDataReader.Read())
                {
                    // MessageBox.Show(thisDataReader["SFSUBJCODE"].ToString());
                    if (thisDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == RequisiteTextBox.Text.Trim().ToUpper())
                    {
                        found = true;
                        subjectCode = thisDataReader["SFSUBJCODE"].ToString();
                        description = thisDataReader["SFSUBJDESC"].ToString();
                        units = thisDataReader["SFSUBJUNITS"].ToString();
                        break;
                    //
                    }
                }

                if (!found)
                    MessageBox.Show("Subject Code Not Found");
                else
                {
                    SubjectDataGridView.Rows[0].Cells[0].Value = subjectCode;
                    SubjectDataGridView.Rows[0].Cells[1].Value = description;
                    SubjectDataGridView.Rows[0].Cells[2].Value = units;
                }

                //REQUISITE
                OleDbConnection requisiteConnection = new OleDbConnection(connectionString);
                requisiteConnection.Open();
                OleDbCommand requisiteCommand = requisiteConnection.CreateCommand();

                string requisitesql = "SELECT * FROM SUBJECTPREQFILE";
                requisiteCommand.CommandText = requisitesql;

                OleDbDataReader requisiteDataReader = requisiteCommand.ExecuteReader();
                while (requisiteDataReader.Read())
                {
                    if (requisiteDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == RequisiteTextBox.Text.Trim().ToUpper())
                    {
                        SubjectDataGridView.Rows[0].Cells[3].Value = requisiteDataReader["SUBJPRECODE"].ToString().Trim().ToUpper();
                        break;
                    }
                    else
                        SubjectDataGridView.Rows[0].Cells[3].Value = string.Empty;
                }
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
        }
    }
}
