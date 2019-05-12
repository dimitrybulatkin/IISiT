using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralTrainings
{
    // Класс с логикой нейроной сети
    class NeuralNetwork
    {
        private List<Layer> network; // Список слоев сети
        private double[,] weights; // Вес связей
        private int inputNeuronsCount, outputNeuronsCount; // Число входящих и исходящих нейронов

        // Конструктор класса
        public NeuralNetwork(int matrixSide, double[,] weights)
        {
            network = new List<Layer>();
            inputNeuronsCount = matrixSide * matrixSide;
            outputNeuronsCount = 10;
            network.Add(new Layer(inputNeuronsCount, outputNeuronsCount)); 
            Threshold = 0.6;

            if (weights != null)
            {
                this.weights = weights;
                WriteWeightsFromFileToNeurons();
            }
        }

        // Установка веса нейронов загрузкой из внешнего файла
        private void WriteWeightsFromFileToNeurons()
        {
            // Цикл для прохода по строкам и столбцам с весами связей
            for (int i = 0; i < weights.GetLength(0); i++)
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    int index = (i + 1) * weights.GetLength(1) - (weights.GetLength(1) - j);
                    double weightValue = weights[i, j];
                    network[0].SetWeight(index, weightValue);
                }
        }

        // Обнаружение столкновение между нейронами для их активации
        public Tuple<int, int> GetConclusion(int[,] imageOnes)
        {
            // Деактивация первого слоя
            network[0].SignalsReset();

            // Активация нужных нейронов согласно образу и увеличение их весов
            for (int i = 0; i < imageOnes.GetLength(0); i++) 
            {
                for (int j = 0; j < imageOnes.GetLength(1); j++)
                    if (imageOnes[i, j] == 1)
                    {
                        int index = (i + 1) * imageOnes.GetLength(1) - (imageOnes.GetLength(1) - j);
                        network[0].SetNeuronSignal(index, true);
                    }
            }

            // Выбор наиболее подходящей цифры переданному образу
            double[] multiples = network[0].GetMultiples();

            int[] outputNeuronsSignals = new int[outputNeuronsCount];

            for (int i = 0; i < multiples.Length; i++)
            {
                double y = 1 / (1 + Math.Exp(-multiples[i]));
                outputNeuronsSignals[i] = y > Threshold ? 1 : 0;
            }

            int resultIndex = multiples.ToList().IndexOf(multiples.Max());
            int resultNeuronSignal = outputNeuronsSignals[resultIndex];

            return Tuple.Create(resultNeuronSignal, resultIndex);
        }

        // Геттер и ссетер порогового значения
        private double Threshold
        {
            get;
            set;
        }

        // Исправление весов связей
        public void WeightsFix(int neuronValue, int weightIndex)
        {
            network[0].WeightsFix(neuronValue, weightIndex);
        }

        // Получение весов связей нейронов
        public double[,] GetWeights()
        {
            return network[0].GetWeights();
        }
    }
}
