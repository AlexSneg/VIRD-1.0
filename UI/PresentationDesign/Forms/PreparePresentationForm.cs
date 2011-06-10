using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.PresentationShow.ShowClient;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using UI.PresentationDesign.DesignUI.Controllers;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Controls.Preparation;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class PreparePresentationForm : Office2007Form
    {
        private PreparePresentationController m_Controller = null;
        private List<string> m_NotEnoughSpaceList = new List<string>();

        public PreparePresentationForm(PreparePresentationController controller)
        {
            m_Controller = controller;
            InitializeComponent();
            m_Controller.OnWorkFinished += new WorkFinished(m_Controller_OnWorkFinished);
            m_Controller.OnProgressChanged += new WorkProgressChanged(m_Controller_OnProgressChanged);
            m_Controller.OnNotEnoughSpace += new Action<DisplayType>(m_Controller_OnNotEnoughSpace);
            m_Controller.OnReceiveAgentResourcesList += new Action<DisplayType[], int[]>(m_Controller_OnReceiveAgentResourcesList);
            m_Controller.OnUploadSpeed += new Action<double, string, string>(m_Controller_OnUploadSpeed);
            m_Controller.OnPreparationForDisplayEnded += new Action<string, bool>(m_Controller_OnPreparationForDisplayEnded);
            m_Controller.OnLogMessage += new Action<string>(m_Controller_OnLogMessage);
        }

        void m_Controller_OnLogMessage(string obj)
        {
            // Добавляем запись в лог
            this.Invoke(new MethodInvoker(()=>
                {
                    detailsText.AppendText(obj);
                    detailsText.AppendText(Environment.NewLine);
                }));
        }

        void m_Controller_OnPreparationForDisplayEnded(string obj, bool allOk)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    var control = SearcAgentControl(obj);
                    if (control != null)
                    {
                        if (control.State == AgentPrepareProgressControl.AgentCopyState.InProgress)
                        {
                            if (allOk) control.SetCompleted();
                            else control.SetStopped();
                        }
                    }
                    EnsureAllComplete();
                }
                    ));
            }
        }

        /// <summary>
        /// Проверка, есть ли копирующиеся агенты.
        /// </summary>
        private void EnsureAllComplete()
        {
            // Проверим, закончилось ли копирование.
            bool totalComplete = true;
            foreach (var a in agentsProgressPanel.Controls)
            {
                var state = ((AgentPrepareProgressControl)a).State;
                if (state != AgentPrepareProgressControl.AgentCopyState.Stopped
                    && state != AgentPrepareProgressControl.AgentCopyState.Complete)
                {
                    totalComplete = false;
                    break;
                }
            }
            if (totalComplete)
            {
                // Копирование закончено, надо показать кнопку OK.
                okButton.Visible = true;
                cancelButton.Visible = false;
            }
        }

        void m_Controller_OnUploadSpeed(double arg1, string arg2, string arg3)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(() =>
                    {
                        var control = SearcAgentControl(arg2);
                        if (control != null)
                        {
                            control.Speed = arg1;
                            control.CurrentFile = arg3;
                        }
                    }));
            }
        }

        void m_Controller_OnReceiveAgentResourcesList(DisplayType[] arg1, int[] arg2)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(
                                delegate
                                    {
                                        AddAgentControls(arg1, arg2);
                                    }
                                )
                    );
            }
            else
            {

                AddAgentControls(arg1, arg2);
            }

        }

        /// <summary>
        /// Добавить на форму контролы, показывающие прогресс загрузки.
        /// </summary>
        /// <param name="arg1">Список агентов.</param>
        /// <param name="arg2">Число ресурсов для агентов.</param>
        private void AddAgentControls(DisplayType[] arg1, int[] arg2)
        {
            this.groupBox.Text = "Выполняется копирование источников";
            int topOffset = 0;
            for (int i = 0; i < arg1.Length; i++)
            {
                var c = new AgentPrepareProgressControl(arg1[i], arg2[i]);
                c.Top = topOffset;
                topOffset+=c.Height;
                agentsProgressPanel.Controls.Add(c);
                c.LoadCommandGained += new AgentPrepareProgressControl.LoadCommandGainedDelegate(c_LoadCommandGained);
                //Если на этом контроллере достаточно места
                if (arg2[i] > 0) // Есть, что копировать.
                {
                    if (!m_NotEnoughSpaceList.Contains(arg1[i].Name))
                    {
                        m_Controller.TerminateLoad(Domain.PresentationShow.ShowCommon.TerminateLoadCommand.ResumeLoad, arg1[i].Name); // Сразу запустим копирование
                    }
                }
            }

            // Приостановим копирование на все дисплеи с закончившимся местом
            foreach (var a in m_NotEnoughSpaceList)
            {
                AgentPrepareProgressControl c = this.SearcAgentControl(a);
                if (c != null) c.NeedMoreSpace();
            }
            m_NotEnoughSpaceList.Clear();
            EnsureAllComplete();
        }

        /// <summary>
        /// Найти индикатор прогресса загрузки по имени агента.
        /// </summary>
        /// <param name="name">Агент.</param>
        /// <returns>Соответсвующий элемент управления.</returns>
        private AgentPrepareProgressControl SearcAgentControl(string name)
        {
            AgentPrepareProgressControl control = null;
            foreach (var a in agentsProgressPanel.Controls)
            {
                if (((AgentPrepareProgressControl)a).AgentName == name)
                {
                    control = (AgentPrepareProgressControl)a;
                    break;
                }
            }
            return control;
        }

        void c_LoadCommandGained(Domain.PresentationShow.ShowCommon.TerminateLoadCommand command, DisplayType display)
        {
            if (command == Domain.PresentationShow.ShowCommon.TerminateLoadCommand.ClearSpace)
            {
                m_Controller.ResponseForSpace(display, false);
                AgentPrepareProgressControl control = SearcAgentControl(display.Name);
            }
            else
            {
                if (cancelButton.Visible == false && command == Domain.PresentationShow.ShowCommon.TerminateLoadCommand.ResumeLoad)
                {
                    // Какой-то остановленный дисплей запустили. Надо изменить видимость кнопок Ок/Отмена.
                    cancelButton.Visible = true;
                    okButton.Visible = false;
                }
                else
                {
                    bool okVisible = true;
                    foreach (var c in agentsProgressPanel.Controls.Cast<AgentPrepareProgressControl>())
                    {
                        if (c.State == AgentPrepareProgressControl.AgentCopyState.InProgress
                            || c.State == AgentPrepareProgressControl.AgentCopyState.Paused)
                        {
                            okVisible = false;
                            break;
                        }
                    }
                    if (okVisible)
                    {
                        cancelButton.Visible = false;
                        okButton.Visible = true;
                    }
                }
                m_Controller.TerminateLoad(command, display.Name);
            }
        }

        void m_Controller_OnNotEnoughSpace(DisplayType obj)
        {
            // Закончилось место на дисплее
            this.BeginInvoke(new MethodInvoker(delegate
            {
                //if (MessageBoxExt.Show(
                //    "На контроллере дисплея «"
                //    + obj.Name +
                //    "» нет места для копирования источников. Очистить дисковое пространство контроллера?", "Подготовка",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new[] { "Очистить", "Продолжить" }) == DialogResult.OK)
                //    m_Controller.ResponseForSpace(obj, false);
                //else
                //    m_Controller.ResponseForSpace(obj, true);
                m_NotEnoughSpaceList.Add(obj.Name);
                AgentPrepareProgressControl c = this.SearcAgentControl(obj.Name);
                if (c != null) c.NeedMoreSpace();
            }));
        }

        void m_Controller_OnProgressChanged(int processed, int total, string displayName)
        {
            if (this.InvokeRequired)
            {
                if(this.IsHandleCreated)
                {
                    this.Invoke(new Action
                (
                () => UpdateProgress(displayName)
                ));
                }
                else
                {
                    UpdateProgress(displayName);
                }
                //this.Invoke(new WorkProgressChanged(m_Controller_OnProgressChanged), processed, total);
                return;
            }
        }

        private void UpdateProgress(string displayName)
        {
            var control = SearcAgentControl(displayName);
            if (control != null) control.Step();
            this.Refresh();
        }

        void m_Controller_OnWorkFinished(ShowClient.PreparationStatus preparationStatus, string error, string warning)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new WorkFinished(m_Controller_OnWorkFinished), preparationStatus, error, warning);
                return;
            }
            m_Controller.CheckPrepareResult();

            cancelButton.Visible = false;
            //detailsButton.Visible = preparationStatus != ShowClient.PreparationStatus.Ok;
            if (preparationStatus == ShowClient.PreparationStatus.Error)
                detailsText.Text = error;
            else if (preparationStatus == ShowClient.PreparationStatus.Warning)
                detailsText.Text = warning;
            groupBox.Text = m_Controller.Status;
            okButton.Visible = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Пошлем команду на завершение процесса загрузки
            m_Controller.TerminateLoad(Domain.PresentationShow.ShowCommon.TerminateLoadCommand.EndSourceUpload, null);
            // Дождемся ответа
            while (!m_Controller.CanClose)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            Close();
        }

        private void buttonAdv1_Click(object sender, EventArgs e)
        {
            int space = 50;
            int minimalHeight=330;
            Size newSize=this.Size;
            if (this.detailsText.Visible)
            {
                this.detailsText.Visible = false;
                newSize = new Size(this.Width, this.Height - this.detailsText.Height - space);
                okButton.Location = new Point(okButton.Location.X, okButton.Location.Y + this.detailsText.Height);
                detailsButton.Location = new Point(detailsButton.Location.X, detailsButton.Location.Y + this.detailsText.Height);
                cancelButton.Location = new Point(cancelButton.Location.X, cancelButton.Location.Y + this.detailsText.Height);
                groupBox.Size = new Size(groupBox.Width, groupBox.Size.Height + this.detailsText.Height);
                this.detailsButton.Image = global::UI.PresentationDesign.DesignUI.Properties.Resources.down;
                minimalHeight=200;
            }
            else
            {
                this.detailsText.Visible = true;
                newSize = new Size(this.Width, this.Height + this.detailsText.Height + space);
                okButton.Location = new Point(okButton.Location.X, okButton.Location.Y - this.detailsText.Height);
                detailsButton.Location = new Point(detailsButton.Location.X, detailsButton.Location.Y - this.detailsText.Height);
                cancelButton.Location = new Point(cancelButton.Location.X, cancelButton.Location.Y - this.detailsText.Height);
                groupBox.Size = new Size(groupBox.Width, groupBox.Height - this.detailsText.Height);
                this.detailsText.Top = detailsButton.Top + space;
                this.detailsButton.Image = global::UI.PresentationDesign.DesignUI.Properties.Resources.up;
                minimalHeight=330;
            }
            this.MinimumSize = new Size(this.MinimumSize.Width, minimalHeight);
            this.Size = newSize;
        }

        private void PreparePresentationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_Controller.CanClose)
            {
                e.Cancel = true;
                return;
            }
            m_Controller.OnWorkFinished -= new WorkFinished(m_Controller_OnWorkFinished);
            m_Controller.OnProgressChanged -= new WorkProgressChanged(m_Controller_OnProgressChanged);
            m_Controller.OnNotEnoughSpace -= new Action<DisplayType>(m_Controller_OnNotEnoughSpace);
            m_Controller.OnReceiveAgentResourcesList -= new Action<DisplayType[], int[]>(m_Controller_OnReceiveAgentResourcesList);
            m_Controller.OnUploadSpeed -= new Action<double, string, string>(m_Controller_OnUploadSpeed);
            m_Controller.OnPreparationForDisplayEnded -= new Action<string, bool>(m_Controller_OnPreparationForDisplayEnded);
            m_Controller.OnLogMessage -= new Action<string>(m_Controller_OnLogMessage);
        }

        private void PreparePresentationForm_Load(object sender, EventArgs e)
        {
            m_Controller.StartPrepare();
            buttonAdv1_Click(null, EventArgs.Empty);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            m_Controller.TerminateLoad(Domain.PresentationShow.ShowCommon.TerminateLoadCommand.StopAll, null);
            groupBox.Enabled = false;
            okButton.Visible = true;
            cancelButton.Visible = false;
            foreach (var a in agentsProgressPanel.Controls)
            {
             ((AgentPrepareProgressControl)a).SetStopped();
            }
        }

        private void okButton_VisibleChanged(object sender, EventArgs e)
        {
            if (okButton.Visible) groupBox.Text = "Загрузка завершена";
            else groupBox.Text = "Выполняется копирование источников";
        }
    }
}
