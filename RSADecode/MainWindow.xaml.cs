﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace RSAExample
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Debugger.Instance.IsLogging = true;
        }

        private void Baton_Click(object sender, RoutedEventArgs e)
        {
            string N = NTB.Text;
            string E = ETB.Text;
            string C = CTB.Text;
            string A;
            try
            {
                A = RSADecipher.Instance.DecipherRSA(N, E, C);
            }
            catch (Exception strin)
            {
                A = strin.Message;
            }

            RTB.Text = A;
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            string N = NTB.Text;
            string E = ETB.Text;
            string S = ITB.Text;
            string A;
            try
            {
                A = RSAEncode.Instance.EncodeRSA(N, E, S);
            }
            catch (Exception strin)
            {
                A = strin.Message;
            }

            CiTB.Text = A;
        }

        private void CiTB_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            CTB.Text = CiTB.Text;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var nl = NumericLogics.Instance;
            try
            {
                var pqn = nl.GeneratePrimesAndN(UpperLimit.Text, LowerLimit.Text);
                pTB.Text = pqn[0].ToString();
                qTB.Text = pqn[1].ToString();
                NTB.Text = pqn[2].ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
