﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Dsp;

namespace Entrada
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        WaveIn waveIn;

        public MainWindow()
        {
            InitializeComponent();
            LlenarComboDispositivos();
        }
        public void LlenarComboDispositivos()
        {
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                WaveInCapabilities capacidades = WaveIn.GetCapabilities(i);
                cmbDispositivo.Items.Add(capacidades.ProductName);
            }
            cmbDispositivo.SelectedIndex = 0;
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();

            //Formato de audio
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);

            //Biffer

            waveIn.BufferMilliseconds = 250;

            //Que hacer cuando hay mustras disponibles

            waveIn.DataAvailable += WaveIn_DataAvailable;

        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;
            float acumulador = 0.0f;

            for (int i = 0; i < bytesGrabados; i += 2)
            {
                short muestra = (short)(buffer[i + 1] << 8 | buffer[i]);
                float muestra32bits =
                    (float)muestra / 32768.0f;
                acumulador += Math.Abs(muestra32bits);

            }

            float promedio = acumulador /
                (bytesGrabados / 2.0f);
            sldMicrofono.Value = (double)promedio;
         }
    }
}
