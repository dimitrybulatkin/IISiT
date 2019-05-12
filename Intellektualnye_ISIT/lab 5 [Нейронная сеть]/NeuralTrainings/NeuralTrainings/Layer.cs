using System.Collections.Generic;

namespace NeuralTrainings
{
    // Класс одного слоя
    class Layer
    {
        private List<Link> layer; // Список связаных со слоем нейронов
        private int inputNeuronsCount, outputNeuronsCount; // Число входящих и исходящих нейронов 
        private const double errorFixValue = 0.15; // Ошибка сигнала нейронов

        // Конструктор класса
        public Layer(int inputNeuronsCount, int outputNeuronsCount)
        {
            this.inputNeuronsCount = inputNeuronsCount;
            this.outputNeuronsCount = outputNeuronsCount;
            layer = new List<Link>();

            for (int i = 0; i < inputNeuronsCount; i++)
            {
                Neuron firstLayerNeuron = new Neuron();

                for (int j = 0; j < outputNeuronsCount; j++)
                    layer.Add(new Link(firstLayerNeuron, new Neuron()));
            }
        }

        // Получение сигнала определенного нейрона данного слоя
        public bool GetNeuronSignal(int linkIndex)
        {
            return layer[linkIndex].Neuron1.Signal;
        }

        // Установка сигнала определенного нейрона данного слоя
        public void SetNeuronSignal(int linkIndex, bool signalValue)
        {
            layer[linkIndex].Neuron1.Signal = signalValue;
        }

        public double GetWeight(int linkIndex)
        {
            return layer[linkIndex].WeightValue;
        }

        public void SetWeight(int linkIndex, double weightValue)
        {
            layer[linkIndex].WeightValue = weightValue;
        }

        // Исправление веса
        public void WeightsFix(int neuronSignal, int neuronIndex)
        {
            // Цикл прохождение по части слоя
            for (int i = neuronIndex; i < layer.Count; i += outputNeuronsCount)
            {
                // Если нейрон активный
                if (layer[i].Neuron1.Signal) 
                {
                    // Изменяется значения с ошибкой нейронов
                    if (neuronSignal == 0)
                        layer[i].WeightValue += errorFixValue;
                    else
                        layer[i].WeightValue -= errorFixValue;
                }
            }
        }

        // Получение весов связей нейронов
        public double[,] GetWeights()
        {
            double[,] weights = new double[inputNeuronsCount, outputNeuronsCount];

            int i = 0, j = 0;

            for (int k = 0; k < layer.Count; k++)
            {
                weights[i, j++] = layer[k].WeightValue;

                if ((k + 1) % outputNeuronsCount == 0)
                {
                    i++;
                    j = 0;
                }
            }

            return weights;
        }

        // Деактивация нейронов в слое
        public void SignalsReset()
        {
            foreach (var link in layer)
            {
                link.Neuron1.Signal = false;
                link.Neuron2.Signal = false;
            }
        }

        // Получение результатов распознания нейронами и соответствие к одному из 9 цифр
        public double[] GetMultiples()
        {
            double[] multiples = new double[outputNeuronsCount];

            for (int i = 0; i < outputNeuronsCount; i++)
                for (int j = i; j < layer.Count; j += outputNeuronsCount)
                {
                    if (layer[j].Neuron1.Signal)
                        multiples[i] += layer[j].WeightValue;
                }

            return multiples;
        }
    }
}
