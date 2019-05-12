using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NeuralTrainings
{
    enum CurrentMouseStates { MouseIsUp, MouseIsDown }; // Перечисление состояний мыши
    enum WorkMode { Study, Recognition }; // Перечислние режима работы распознавания

    public partial class Form1 : Form
    {
        NeuralNetwork neuralNetwork; // Экземпляр нейроной сети
        Graphics graphics; // Контекст для доступа к инструментам для рисования
        Pen pen; // Кисть для рисования на форме
        Bitmap bitmap; // Растровое изображение для рисования
        int pbWidth, pbHeight; // Размеры поля для рисования (ширина, высота)
        float penWidth = 6.5F; // 0.5F - тонкое 2.0F - 2.5F - толстое   
        double[,] weights; // Вес связей
        string path = Directory.GetCurrentDirectory() + "/weights.txt"; // Путь до файла с информацией о весах связей
        int ratio = 4; // Размер ячейки 
        int size = 0; // Размер стороны матрицы с нейронами

        // Базовый класс
        public Form1()
        {
            InitializeComponent();

            // Инициализация переменных
            pen = new Pen(Color.Blue, penWidth);
            CurrentMouseState = CurrentMouseStates.MouseIsUp;
            pbWidth = pictureBox1.Width;
            pbHeight = pictureBox1.Height;
            bitmap = new Bitmap(pbWidth, pbHeight);
            pictureBox1.Image = bitmap;
            weights = WeightsRead();
            size = pbWidth / ratio;
            neuralNetwork = new NeuralNetwork(size, weights);
            StudyModeRB.Checked = true;
        }

        // Геттер и сеттер состояния мыши
        CurrentMouseStates CurrentMouseState
        {
            get;
            set;
        }

        // Геттер и сеттер режима распознания
        WorkMode WorkMode
        {
            get;
            set;
        }

        // Обработка зажимания кнопки мыши
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            CurrentMouseState = CurrentMouseStates.MouseIsDown; // Изменение состояния на зажатую
        }

        // Обработка перемещения мыши
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // Если кнопка мыши зажата, то рисуется полоска в позиции курсора
            if (CurrentMouseState == CurrentMouseStates.MouseIsDown)
            {
                graphics = Graphics.FromImage(bitmap);
                graphics.DrawLine(pen, e.X + penWidth, e.Y + penWidth, e.X, e.Y);
                pictureBox1.Invalidate();
            }
        }

        // Обработка отпускания кнопки мыши
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            CurrentMouseState = CurrentMouseStates.MouseIsUp; // Изменение состояния на отпущенную
        }

        // Обработка нажатия для распознания изображения
        private void RecognitionBtn_Click(object sender, EventArgs e)
        {
            // Подсчет попавших закрашеных пикселей в ячейку
            int[,] imageOnes = new int[size, size];

            for (int i = 0; i < pbWidth; i++)
                for (int j = 0; j < pbHeight; j++)
                    imageOnes[i / ratio, j / ratio] = bitmap.GetPixel(j, i).B == 255 ? 1 : 0;

            // Передача нейросети получившегося образа
            Tuple<int, int> result = neuralNetwork.GetConclusion(imageOnes);

            // Изъятие результатов
            int neuronSignal = result.Item1;
            int number = result.Item2;

            DialogResult dialogResult = DialogResult.None;

            // Вывод результата в зависимости от режима распознания
            if (WorkMode == WorkMode.Study)
                dialogResult = MessageBox.Show("Это " + number + " ?", "Помоги...", MessageBoxButtons.YesNo);
            else
                MessageBox.Show("Это " + number + " !");

            // Изменение веса нейронов в случае режима обучения и неверного распознания
            if (dialogResult == DialogResult.No && WorkMode == WorkMode.Study)
                neuralNetwork.WeightsFix(neuronSignal, number);

            // Очистка поля
            PictureBoxClear();
        }

        // Обработка события закрытия программы
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Сохраняются весы связей в файл
            weights = neuralNetwork.GetWeights();
            ArrayWrite(weights, path);
        }

        // Сохранение весов связей в файл
        private void ArrayWrite(double[,] arr, string path)
        {
            File.WriteAllText(path, "");

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    File.AppendAllText(path, arr[i, j] + " ");
                }
                File.AppendAllText(path, "\r\n");
            }
        }

        // Обработка события нажатия кнопки для очистки поля для рисования
        private void PbClearBtn_Click(object sender, EventArgs e)
        {
            PictureBoxClear();
        }

        // Обработка события изменения режима распознания на обучение
        private void StudyModeRB_CheckedChanged(object sender, EventArgs e)
        {
            WorkMode = WorkMode.Study;
        }

        // Обработка события изменения режима распознания без обучения
        private void RecognitionModeRB_CheckedChanged(object sender, EventArgs e)
        {
            WorkMode = WorkMode.Recognition;
        }

        // Очистка поля с изображением
        private void PictureBoxClear()
        {
            bitmap = new Bitmap(pbWidth, pbHeight);
            pictureBox1.Image = bitmap;
        }

        // Чтение весов связей из файла
        private double[,] WeightsRead()
        {
            string[] strArray = null;

            if (File.Exists(path))
                strArray = File.ReadAllLines(path);
            else
                return null;

            int columnsCount = strArray[0].Split(' ').Length - 1;

            double[,] weights = new double[strArray.Length, columnsCount];

            for (int i = 0; i < strArray.Length; i++)
            {
                string[] str = strArray[i].Split(' ');

                for (int j = 0; j < columnsCount; j++)
                {
                    weights[i, j] = Convert.ToDouble(str[j]);
                }
            }

            return weights;
        }
    }
}
