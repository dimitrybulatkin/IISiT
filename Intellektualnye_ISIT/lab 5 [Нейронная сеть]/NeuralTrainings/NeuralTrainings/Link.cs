using System;

namespace NeuralTrainings
{
    // Класс связи нейронов
    class Link
    {
        // Генератор псевдослучайных значений
        private static Random rnd = new Random();

        // Базовый конструктор
        public Link(Neuron neuron1, Neuron neuron2)
        {
            Neuron1 = neuron1;
            Neuron2 = neuron2;
            WeightValue = Math.Round(rnd.NextDouble() * 0.05, 4);
        }

        // Геттер первого нейрона
        public Neuron Neuron1 { get; private set; }

        // Вес связи
        public double WeightValue { get; set; }

        // Геттер второго нейрона
        public Neuron Neuron2 { get; private set; }
    }
}
