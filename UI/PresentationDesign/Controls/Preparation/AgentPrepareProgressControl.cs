using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace UI.PresentationDesign.DesignUI.Controls.Preparation
{
    /// <summary>
    /// Элемент управления для отображения прогресса копирования источников для агента.
    /// </summary>
    public partial class AgentPrepareProgressControl : UserControl
    {

        public enum AgentCopyState
        {
            InProgress,
            Paused,
            Stopped,
            Complete
        };

        /// <summary>
        /// Статус копирования.
        /// </summary>
        AgentCopyState state = AgentCopyState.InProgress;
        /// <summary>
        /// Статус копирования.
        /// </summary>
        public AgentCopyState State
        {
            get
            {
                return state;
            }
        }
        /// <summary>
        /// Название агента.
        /// </summary>
        public string AgentName
        {
            get { return agentLabel.Text; }
        }

        public int Progress
        {
            set
            {
                progressBar.Value = value;
            }
        }

        public double Speed
        {
            set
            {
                if (value < 0) speedLabel.Text = "";
                else speedLabel.Text = string.Format("{0} Мб/с", value.ToString("0.00"));
            }
        }

        private string _currentFile = "";
        public string CurrentFile
        {
            set
            {
                _currentFile = value;
                UpdateProgressLabel();
            }
        }

        DisplayType agent;

        public delegate void LoadCommandGainedDelegate(Domain.PresentationShow.ShowCommon.TerminateLoadCommand command, DisplayType display);
        public event LoadCommandGainedDelegate LoadCommandGained;
        public AgentPrepareProgressControl()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Создать контрол для определенного агента.
        /// </summary>
        /// <param name="agentName">Название агента.</param>
        /// <param name="resourceCount">Чисто ресурсов для агента.</param>
        public AgentPrepareProgressControl(DisplayType agentName, int resourceCount)
        {
            InitializeComponent();
            this.agentLabel.Text = agentName.Name;
            agent = agentName;
            this.progressBar.Maximum = resourceCount * 2; // Количество ресурсов плюс файлы-описатели
            UpdateProgressLabel();
            speedLabel.Text = "";
            progressLabel.Text = "";
            CurrentFile = "";
            if (resourceCount == 0)
            {
                this.progressBar.Maximum = 1;
                this.progressBar.Value = 1;
                SetCompleted();
            }
        }

        /// <summary>
        /// Обновить текстовое описание прогресса по загрузке.
        /// </summary>
        private void UpdateProgressLabel()
        {
            string info;
            switch (state)
            {
                case AgentCopyState.InProgress: info = "Скопировано источников"; break;
                case AgentCopyState.Paused: info = "Копирование источников приостановлено"; break;
                case AgentCopyState.Stopped: info = "Копирование прервано"; break;
                case AgentCopyState.Complete: info = "Копирование источников завершено"; break;
                default: throw new Exception("Некорректный статус загрузки.");
            }
            if (state != AgentCopyState.Stopped)
            {
                

                if(
                    (
                    (this.state == AgentCopyState.InProgress) 
                    || (this.state == AgentCopyState.Paused)
                    ) && _currentFile.Length > 0) //Наименование копируемого источника
                {
                    this.progressLabel.Text = string.Format("{2}: {0} из {1}. ({3})",
                                                    (progressBar.Value/2),
                                                    progressBar.Maximum/2,
                                                    info,
                                                    _currentFile);
                }
                else
                {
                 this.progressLabel.Text = string.Format("{2}: {0} из {1}.",
                                                    (progressBar.Value/2),
                                                    progressBar.Maximum/2,
                                                    info);
                }
            }
            else
            {
                this.progressLabel.Text = info;
            }

            //"Приостановить"
            pauseButton.Visible = (this.state == AgentCopyState.InProgress);
            //"Продолжить"
            playButton.Visible = (this.state == AgentCopyState.Paused) || (this.state == AgentCopyState.Stopped);
            // "Прервать"
            stopButton.Visible = (this.state == AgentCopyState.InProgress) || (this.state == AgentCopyState.Paused);
            //"Очистить"
            clearButton.Visible = (this.state == AgentCopyState.Paused);
                
            UpdateButtonPositions();
            this.Refresh();
        }

        /// <summary>
        /// Переразместить управляющие кнопки, чтобы не было пустых мест.
        /// </summary>
        private void UpdateButtonPositions()
        {
            Button[] buttons = new Button[]
            {
                pauseButton,
                playButton,
                stopButton,
                clearButton
            };
            int j=0;
            for(int i=0; i<buttons.Length; i++)
            {
                if(buttons[i].Visible==true)
                {
                    buttons[i].Left=buttons[0].Left+j*29;
                    j++;
                }
            }
            this.Refresh();
        }

        /// <summary>
        /// Двинуть дальше индикатор скачанного.
        /// </summary>
        public void Step()
        {
            this.progressBar.Value++;
            UpdateProgressLabel();
        }

        private void commandButton_Click(object sender, EventArgs e)
        {
            Domain.PresentationShow.ShowCommon.TerminateLoadCommand command = Domain.PresentationShow.ShowCommon.TerminateLoadCommand.StopAll;

            if (sender == pauseButton)
            {
                command = Domain.PresentationShow.ShowCommon.TerminateLoadCommand.PauseLoad;
                SetPaused();
            }
            else if (sender == playButton)
            {
                command = Domain.PresentationShow.ShowCommon.TerminateLoadCommand.ResumeLoad;
                this.progressBar.Value = 0;
                SetPlaying();
            }
            else if (sender == stopButton)
            {
                command = Domain.PresentationShow.ShowCommon.TerminateLoadCommand.StopLoad;
                SetStopped();
            }
            else if (sender == clearButton)
            {
                command = Domain.PresentationShow.ShowCommon.TerminateLoadCommand.ClearSpace;
                this.progressBar.Value = 0;
            }
            if (LoadCommandGained != null) LoadCommandGained(command, this.agent);
        }

        /// <summary>
        /// Остановить копирование.
        /// </summary>
        public void SetStopped()
        {
            this.state = AgentCopyState.Stopped;
            this.progressBar.Value = 0;
            UpdateProgressLabel();
        }

        /// <summary>
        /// Установить режим загрузки.
        /// </summary>
        public void SetPlaying()
        {
            state = AgentCopyState.InProgress;
            UpdateProgressLabel();
        }

        /// <summary>
        /// Включить режим паузы.
        /// </summary>
        public void SetPaused()
        {
            state = AgentCopyState.Paused;
            UpdateProgressLabel();
        }

        public void NeedMoreSpace()
        {
            state = AgentCopyState.Paused;
            UpdateProgressLabel();
        }

        public void SetCompleted()
        {
            this.CurrentFile = "";
            this.Speed = -1;
            state = AgentCopyState.Complete;
            UpdateProgressLabel();
        }
    }
}
