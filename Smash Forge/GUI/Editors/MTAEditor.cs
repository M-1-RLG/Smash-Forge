﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Smash_Forge
{
    public partial class MTAEditor : EditorBase
    {
        public MTA mta;

        public MTAEditor(MTA mta)
        {
            InitializeComponent();
            this.mta = mta;
        }

        private void MTAEditor_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = mta.Decompile();
        }

        public override void Save()
        {
            if (FilePath.Equals(""))
            {
                SaveAs();
                return;
            }
            FileOutput o = new FileOutput();
            mta.Compile(new List<string>(richTextBox1.Text.Split('\n')));
            byte[] n = mta.Rebuild();
            o.writeBytes(n);
            o.save(FilePath);
            Edited = false;
        }

        public override void SaveAs()
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Material Animation (.mta)|*.mta|" +
                             "All Files (*.*)|*.*";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FileName.EndsWith(".mta"))
                    {
                        FilePath = sfd.FileName;
                        Save();
                    }
                }
            }
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            mta.Compile(new List<string>(richTextBox1.Text.Split('\n')));
            richTextBox1.Text = mta.Decompile();
        }

        private void loadViewportButton_Click(object sender, EventArgs e)
        {
            // Compile the MTA just in case and load into the active viewport.
            mta = new MTA();
            mta.Compile(new List<string>(richTextBox1.Text.Split('\n')));
            ModelViewport modelViewport = (ModelViewport)MainForm.Instance.GetActiveModelViewport();
            if (modelViewport != null)
                modelViewport.CurrentMaterialAnimation = mta;
        }
    }
}
