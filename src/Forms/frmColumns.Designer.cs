namespace sMkTaskManager.Forms {
    partial class frmColumns {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            lblHelp = new Label();
            lv = new ListView();
            columnHeader1 = new ColumnHeader();
            btnMoveUp = new Button();
            btnMoveDown = new Button();
            btnShow = new Button();
            btnHide = new Button();
            btnDefaults = new Button();
            Label2 = new Label();
            txtWidth = new TextBox();
            btnCancel = new Button();
            btnOk = new Button();
            chkSmallIcons = new CheckBox();
            SuspendLayout();
            // 
            // lblHelp
            // 
            lblHelp.Location = new Point(3, 7);
            lblHelp.Margin = new Padding(0);
            lblHelp.Name = "lblHelp";
            lblHelp.Size = new Size(360, 32);
            lblHelp.TabIndex = 0;
            lblHelp.Text = "Check the columns that you would like to make visible.\r\nUse the Move Up and Move Down buttons to reorder the columns.";
            // 
            // lv
            // 
            lv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lv.BorderStyle = BorderStyle.FixedSingle;
            lv.CheckBoxes = true;
            lv.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            lv.FullRowSelect = true;
            lv.HeaderStyle = ColumnHeaderStyle.None;
            lv.LabelWrap = false;
            lv.Location = new Point(6, 44);
            lv.MultiSelect = false;
            lv.Name = "lv";
            lv.Size = new Size(267, 360);
            lv.TabIndex = 1;
            lv.UseCompatibleStateImageBehavior = false;
            lv.View = View.Details;
            lv.ItemCheck += lv_ItemCheck;
            lv.ItemChecked += lv_ItemChecked;
            lv.SelectedIndexChanged += lv_SelectedIndexChanged;
            lv.SizeChanged += lv_SizeChanged;
            lv.KeyDown += lv_KeyDown;
            // 
            // columnHeader1
            // 
            columnHeader1.Width = 220;
            // 
            // btnMoveUp
            // 
            btnMoveUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMoveUp.Location = new Point(279, 44);
            btnMoveUp.Margin = new Padding(3, 3, 3, 0);
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(80, 23);
            btnMoveUp.TabIndex = 2;
            btnMoveUp.Text = "Move Up";
            btnMoveUp.UseVisualStyleBackColor = true;
            btnMoveUp.Click += btnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMoveDown.Location = new Point(279, 70);
            btnMoveDown.Margin = new Padding(3, 3, 3, 0);
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(80, 23);
            btnMoveDown.TabIndex = 3;
            btnMoveDown.Text = "Move Down";
            btnMoveDown.UseVisualStyleBackColor = true;
            btnMoveDown.Click += btnMoveDown_Click;
            // 
            // btnShow
            // 
            btnShow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnShow.Location = new Point(279, 96);
            btnShow.Margin = new Padding(3, 3, 3, 0);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(80, 23);
            btnShow.TabIndex = 4;
            btnShow.Text = "Show";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += btnShow_Click;
            // 
            // btnHide
            // 
            btnHide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHide.Location = new Point(279, 122);
            btnHide.Margin = new Padding(3, 3, 3, 0);
            btnHide.Name = "btnHide";
            btnHide.Size = new Size(80, 23);
            btnHide.TabIndex = 5;
            btnHide.Text = "Hide";
            btnHide.UseVisualStyleBackColor = true;
            btnHide.Click += btnHide_Click;
            // 
            // btnDefaults
            // 
            btnDefaults.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDefaults.Location = new Point(279, 156);
            btnDefaults.Margin = new Padding(3, 3, 3, 0);
            btnDefaults.Name = "btnDefaults";
            btnDefaults.Size = new Size(80, 23);
            btnDefaults.TabIndex = 6;
            btnDefaults.Text = "Defaults";
            btnDefaults.UseVisualStyleBackColor = true;
            btnDefaults.Click += btnDefaults_Click;
            // 
            // Label2
            // 
            Label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Label2.Location = new Point(278, 205);
            Label2.Name = "Label2";
            Label2.Size = new Size(85, 17);
            Label2.TabIndex = 7;
            Label2.Text = "Column Width";
            Label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // txtWidth
            // 
            txtWidth.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtWidth.BorderStyle = BorderStyle.FixedSingle;
            txtWidth.Location = new Point(279, 222);
            txtWidth.MaxLength = 5;
            txtWidth.Name = "txtWidth";
            txtWidth.Size = new Size(80, 23);
            txtWidth.TabIndex = 8;
            txtWidth.Text = "32";
            txtWidth.TextAlign = HorizontalAlignment.Center;
            txtWidth.WordWrap = false;
            txtWidth.KeyPress += txtWidth_KeyPress;
            txtWidth.LostFocus += txtWidth_LostFocus;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(279, 381);
            btnCancel.Margin = new Padding(3, 3, 3, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 23);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Location = new Point(279, 355);
            btnOk.Margin = new Padding(3, 3, 3, 0);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(80, 23);
            btnOk.TabIndex = 10;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // chkSmallIcons
            // 
            chkSmallIcons.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkSmallIcons.AutoSize = true;
            chkSmallIcons.Location = new Point(282, 330);
            chkSmallIcons.Name = "chkSmallIcons";
            chkSmallIcons.Size = new Size(78, 19);
            chkSmallIcons.TabIndex = 9;
            chkSmallIcons.Text = "Wider List";
            chkSmallIcons.TextAlign = ContentAlignment.MiddleCenter;
            chkSmallIcons.UseVisualStyleBackColor = true;
            chkSmallIcons.CheckedChanged += chkSmallIcons_CheckedChanged;
            // 
            // frmColumns
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(364, 410);
            Controls.Add(lv);
            Controls.Add(chkSmallIcons);
            Controls.Add(txtWidth);
            Controls.Add(Label2);
            Controls.Add(btnDefaults);
            Controls.Add(btnHide);
            Controls.Add(btnShow);
            Controls.Add(btnMoveDown);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            Controls.Add(btnMoveUp);
            Controls.Add(lblHelp);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmColumns";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Columns Settings";
            Load += frmColumns_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblHelp;
        private ListView lv;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnShow;
        private Button btnHide;
        private Button btnDefaults;
        private Label Label2;
        private TextBox txtWidth;
        private Button btnCancel;
        private Button btnOk;
        private CheckBox chkSmallIcons;
        private ColumnHeader columnHeader1;
    }
}